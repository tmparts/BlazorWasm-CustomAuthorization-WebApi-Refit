////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp;
using LibMetaApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Reflection;
using System.Runtime.Serialization;

namespace SrvMetaApp
{
    public static class RedisExt
    {
        public static string HashValueByName(this HashEntry[] arr, string name)
        {
            return arr.FirstOrDefault(_ => _.Name == name).Value;
        }

        public static bool HashArrEquals(this HashEntry[] xArr, HashEntry[] yArr)
        {
            return !xArr.Any(x => x.Value != yArr.HashValueByName(x.Name));
        }

        public static bool HashArrChanged(this HashEntry[] xArr, HashEntry[] yArr)
        {
            return xArr.HashValueByName("Id") != yArr.HashValueByName("Id")
                ? false
                : !xArr.HashArrEquals(yArr);
        }


        //Deserialize from Redis format
        public static T ConvertFromRedis<T>(this HashEntry[] hashEntries)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            T obj = (T)FormatterServices.GetUninitializedObject(typeof(T));
            foreach (var property in properties)
            {
                HashEntry entry = hashEntries.FirstOrDefault(g => g.Name.ToString().Equals(property.Name));
                if (entry.Equals(new HashEntry()))
                {
                    continue;
                }

                property.SetValue(obj, Convert.ChangeType(entry.Value.ToString(), property.PropertyType));
            }
            return (T)obj;
        }
    }

    public class RedisUtil : IDisposable
    {
        static RedisConfigModel? _config;
        protected readonly ILogger<RedisUtil> _logger;
        public static string RedisServerAddress => _config?.EndPoint ?? "localhost:6379";

        ConnectionMultiplexer connectionMultiplexer = null;
        private Lazy<ConnectionMultiplexer> lazyConnection => new Lazy<ConnectionMultiplexer>(() =>
        {
            ConfigurationOptions co = new ConfigurationOptions()
            {
                SyncTimeout = _config?.SyncTimeout ?? 500000,
                EndPoints = { { RedisServerAddress } },
                AbortOnConnectFail = _config?.AbortOnConnectFail ?? false,
                ConnectTimeout = _config?.ConnectTimeout ?? 10000,
                AllowAdmin = _config?.AllowAdmin ?? true,
                ConnectRetry = _config?.ConnectRetry ?? 5,
                ResolveDns = _config?.ResolveDns ?? true,
                User = _config?.User ?? string.Empty,
                Password = _config?.Password ?? string.Empty,
                KeepAlive = _config?.KeepAlive ?? 5,
                HighPrioritySocketThreads = _config?.HighPrioritySocketThreads ?? true,
                ConfigurationChannel = _config?.ConfigurationChannel ?? string.Empty,
                ClientName = _config?.ClientName ?? string.Empty,
                Ssl = _config?.Ssl ?? true,
                SslHost = _config?.SslHost ?? string.Empty
            };
            if (connectionMultiplexer is null)
                connectionMultiplexer = ConnectionMultiplexer.Connect(co);

            return connectionMultiplexer;
        });
        public ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
        public RedisUtil(IOptions<ServerConfigModel> set_config, ILogger<RedisUtil> set_logger)
        {
            _config = set_config.Value.RedisConfig;
            _logger = set_logger;
        }
        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
        }

        private string GetRedisKey(RedisPrefixExternModel pref, string id = "")
        {
            string itemId = id == string.Empty ? "*" : id;
            if (string.IsNullOrWhiteSpace(pref.Dict))
                return string.Format("{0}:{2}", pref.Namespace, pref.Dict, itemId);
            else
                return string.Format("{0}:{1}:{2}", pref.Namespace, pref.Dict, itemId);
        }

        public Tuple<int, HashEntry[]>[] ObjectsToHashes<T>(T[] arr)
        {
            if (arr?.Any() != true)
            {
                return Array.Empty<Tuple<int, HashEntry[]>>();
            }

            Type t = arr.GetType().GetElementType();
            PropertyInfo[] props = t.GetProperties();
            PropertyInfo propId = props.Where(_ => _.Name == "Id").FirstOrDefault();
            Type propIdType = Type.GetType(propId.PropertyType.FullName);
            Type testType = propId.PropertyType;

            Tuple<int, HashEntry[]>[] res = arr.Where(_ => propId.GetValue(_) != null).Select(i => Tuple.Create(
                (int)propId.GetValue(i),
                ObjectToHashes(props, i)
                )).ToArray();

            return res;
        }

        private HashEntry[] ObjectToHashes<T>(PropertyInfo[] props, T item)
        {
            return props
                .Where(_ => _.GetValue(item) != null)
                .Select(p =>
                {
                    string val = "";
                    object raw = p.GetValue(item);
                    if (p.PropertyType.BaseType.Name != "Array")
                        try
                        {
                            val = raw.ToString();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error - {nameof(ObjectToHashes)}");
                            throw ex;
                        }
                    else
                        val = JsonConvert.SerializeObject((Array)raw);
                    return new HashEntry(p.Name, val);
                }).ToArray();
        }

        public RedisKey[] Keys(RedisPrefixExternModel pref, string id = "")
        {
            ConnectionMultiplexer rc = Connection;
            IServer server = rc.GetServer(RedisServerAddress);
            _ = rc.GetDatabase();
            return server.Keys(pattern: GetRedisKey(pref, id)).ToArray();
        }

        public NodeModel[] Nodes(RedisPrefixExternModel pref, string id = "")
        {
            ConnectionMultiplexer rc = Connection;
            IServer server = rc.GetServer(RedisServerAddress);
            IDatabase db = rc.GetDatabase();

            return server.Keys(pattern: GetRedisKey(pref, id)).Select(_ =>
                new NodeModel
                (
                    GetRealId(_.ToString(), pref),
                    db.StringGet(_).ToString()
                )
            ).ToArray();
        }

        private static int GetRealId(string redisKey, RedisPrefixExternModel pref)
        {
            string idStr = redisKey.Replace(string.Format("{0}:{1}:", pref.Namespace, pref.Dict), string.Empty);
            return Convert.ToInt32(idStr);
        }

        private string GetRealStringKey(string redisKey, RedisPrefixExternModel pref)
        {
            string idStr = redisKey.Replace(string.Format("{0}:{1}:", pref.Namespace, pref.Dict), string.Empty);
            return idStr;
        }

        private Tuple<int, HashEntry[]>[] HashesDiff(Tuple<int, HashEntry[]>[] first, Tuple<int, HashEntry[]>[] second)
        {
            Tuple<int, HashEntry[]>[] diff = first.Where(r => !second.Any(a => a.Item2.HashArrEquals(r.Item2))).ToArray();
            return diff;
        }

        private Tuple<int, HashEntry[]>[] HashesChanged(Tuple<int, HashEntry[]>[] first, Tuple<int, HashEntry[]>[] second)
        {
            Tuple<int, HashEntry[]>[] changed = first.Where(r => second.Any(a => a.Item2.HashArrChanged(r.Item2))).ToArray();
            return changed;
        }

        public void MergeHashes<T>(T[] provObjects, RedisPrefixExternModel pref)
        {
            Tuple<int, HashEntry[]>[] redisHashes = Values(pref, KeyToHashEntry);
            Tuple<int, HashEntry[]>[] provHashes = ObjectsToHashes(provObjects);

            Tuple<int, HashEntry[]>[] expiredItems = HashesDiff(redisHashes, provHashes);
            Tuple<int, HashEntry[]>[] newItems = HashesDiff(provHashes, redisHashes);

            Tuple<int, HashEntry[]>[] changedItems = HashesChanged(provHashes, redisHashes);

            RemoveKeys(expiredItems.Select(_ => _.Item1).ToArray(), pref);
            UpdateHashes(newItems.Concat(changedItems).ToArray(), pref);
        }

        public void UpdateHashes(Tuple<int, HashEntry[]>[] hashes, RedisPrefixExternModel pref)
        {
            foreach (Tuple<int, HashEntry[]> hash in hashes)
            {
                UpdateHash(hash, pref);
            }
        }

        public async Task UpdateHashesAsync(Tuple<int, HashEntry[]>[] hashes, RedisPrefixExternModel pref)
        {
            foreach (Tuple<int, HashEntry[]> hash in hashes)
            {
                await UpdateHashAsync(hash, pref);
            }
        }

        public void UpdateHash(Tuple<int, HashEntry[]> hash, RedisPrefixExternModel pref)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            RedisKey key = GetRedisKey(pref, hash.Item1.ToString());
            try
            {
                db.HashSet(key, hash.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error - {nameof(UpdateHash)}");
                return;
            }
        }

        /// <summary>
        /// Пакетная загрузка значений в Redis
        /// </summary>
        /// <param name="prov_nodes">Загружаемые данные</param>
        /// <param name="pref">Префикс (шаблон ключей) загружаемых данных</param>
        /// <param name="auto_remove_expires_nodes">Автоматически удалять данные, которых нет (по ключу) в загружаемых</param>
        public void MergeNodes(NodeModel[] prov_nodes, RedisPrefixExternModel pref, bool auto_remove_expires_nodes = true)
        {
            if (prov_nodes == null)
            {
                return;
            }

            NodeModel[] redis_nodes = Values(pref, KeyToNode);
            if (redis_nodes == null)
            {
                return;
            }

            if (auto_remove_expires_nodes)
            {
                NodeModel[] expired_nodes = GlobalUtils.ExpiredNodes(prov_nodes, redis_nodes);
                RemoveKeys(expired_nodes.Select(_ => _.Id).ToArray(), pref);
            }

            NodeModel[] new_nodes = GlobalUtils.NewNodes(prov_nodes, redis_nodes);

            Dictionary<int, string> provDict = prov_nodes.ToDictionary(_ => _.Id, _ => _.Name);
            Dictionary<int, string> redisDict = redis_nodes.ToDictionary(_ => _.Id, _ => _.Name);

            NodeModel[] changedNodes = GlobalUtils.ChangedDictItems<int, string>(provDict, redisDict)
                .Select(_ => new NodeModel(_.Key, _.Value))
                .ToArray();

            KeyValuePair<string, string>[] pairsToUpdate = new_nodes.Concat(changedNodes)
                .Select(_ => new KeyValuePair<string, string>(_.Id.ToString(), _.Name))
                .ToArray();

            UpdateKeys(pairsToUpdate, pref);
        }

        public static NodeModel KeyToNode(IDatabase db, RedisKey _, RedisPrefixExternModel pref)
        {
            return
                new NodeModel
                (
                    GetRealId(_.ToString(), pref),
                    db.StringGet(_).ToString()
                );
        }

        public void UpdateKeys(KeyValuePair<string, string>[] pairs, RedisPrefixExternModel pref)
        {
            foreach (KeyValuePair<string, string> pair in pairs)
            {
                UpdateKey(pair, pref);
            }
        }

        public async Task UpdateKeysAsync(KeyValuePair<string, string>[] pairs, RedisPrefixExternModel pref)
        {
            foreach (KeyValuePair<string, string> pair in pairs)
            {
                await UpdateKeyAsync(pair, pref);
            }
        }

        public void UpdateKey(KeyValuePair<string, string> pair, RedisPrefixExternModel pref, TimeSpan? expiry = null)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            RedisKey key = GetRedisKey(pref, pair.Key);
            db.StringSet(key, pair.Value, expiry);
        }

        public async Task UpdateKeyAsync(KeyValuePair<string, string> pair, RedisPrefixExternModel pref, TimeSpan? expiry = null)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            RedisKey key = GetRedisKey(pref, pair.Key);
            await db.StringSetAsync(key, pair.Value, expiry);
        }

        public void UpdateKey(RedisKey redisKey, string value, TimeSpan? expiry = null)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            TimeSpan? expiryCurrent = db.KeyTimeToLive(redisKey);
            db.StringSet(redisKey, value, expiryCurrent ?? expiry);
        }

        public async Task UpdateKeyAsync(RedisKey redisKey, string value, TimeSpan? expiry = null)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            TimeSpan? expiryCurrent = db.KeyTimeToLive(redisKey);
            await db.StringSetAsync(redisKey, value, expiryCurrent ?? expiry);
        }

        public void MergeNodesNotRemove(NodeModel[] provNodes, RedisPrefixExternModel pref)
        {
            if (provNodes == null)
            {
                return;
            }
            List<NodeModel> redisNodes = new List<NodeModel>();
            try
            {
                ConnectionMultiplexer rc = Connection;
                IServer server = rc.GetServer(RedisServerAddress);
                IDatabase db = rc.GetDatabase();
                IEnumerable<RedisKey> dict = server.Keys(pattern: GetRedisKey(pref));

                RedisCompKeyExternModel red_comp_key = new RedisCompKeyExternModel(string.Empty, pref);
                string obj_id_str;
                int obj_id;
                RedisValue res;
                foreach (RedisKey k in dict)
                {
                    obj_id_str = k.ToString().Substring(13);
                    obj_id = int.Parse(obj_id_str);
                    red_comp_key = new RedisCompKeyExternModel(obj_id_str, pref);  // red_comp_key.Id = k.ToString();
                    res = db.StringGet(k);

                    redisNodes.Add(new NodeModel(obj_id, res.IsNull ? null : res.ToString()));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка Values(pref, KeyToNode)\n\n{JsonConvert.SerializeObject(pref)}\n");
                return;
            }

            if (redisNodes == null)
            {
                return;
            }

            NodeModel[] newNodes = GlobalUtils.NewNodes(provNodes, redisNodes.ToArray());

            Dictionary<int, string> provDict = provNodes.ToDictionary(_ => _.Id, _ => _.Name);
            Dictionary<int, string> redisDict = redisNodes.ToDictionary(_ => _.Id, _ => _.Name);

            redisNodes = default;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            NodeModel[] changedNodes = GlobalUtils.ChangedDictItems<int, string>(provDict, redisDict)
                .Select(_ => new NodeModel(_.Key, _.Value))
                .ToArray();

            KeyValuePair<string, string>[] pairsToUpdate = newNodes.Concat(changedNodes)
                .Select(_ => new KeyValuePair<string, string>(_.Id.ToString(), _.Name))
                .ToArray();

            UpdateKeys(pairsToUpdate, pref);

        }

        public async Task MergeNodesNotRemoveAsync(NodeModel[] provNodes, RedisPrefixExternModel pref)
        {
            if (provNodes == null)
            {
                return;
            }

            NodeModel[] redisNodes = Values(pref, KeyToNode);
            if (redisNodes == null)
            {
                return;
            }

            NodeModel[] newNodes = GlobalUtils.NewNodes(provNodes, redisNodes);

            Dictionary<int, string> provDict = provNodes.ToDictionary(_ => _.Id, _ => _.Name);
            Dictionary<int, string> redisDict = redisNodes.ToDictionary(_ => _.Id, _ => _.Name);

            NodeModel[] changedNodes = GlobalUtils.ChangedDictItems<int, string>(provDict, redisDict)
                .Select(_ => new NodeModel(_.Key, _.Value))
                .ToArray();

            KeyValuePair<string, string>[] pairsToUpdate = newNodes.Concat(changedNodes)
                .Select(_ => new KeyValuePair<string, string>(_.Id.ToString(), _.Name))
                .ToArray();

            await UpdateKeysAsync(pairsToUpdate, pref);
        }

        public async Task UpdateHashAsync(Tuple<int, HashEntry[]> hash, RedisPrefixExternModel pref)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            RedisKey key = GetRedisKey(pref, hash.Item1.ToString());
            try
            {
                await db.HashSetAsync(key, hash.Item2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error - {nameof(UpdateHashAsync)}");
            }
        }

        public T[] Values<T>(RedisPrefixExternModel pref, Func<IDatabase, RedisKey, RedisPrefixExternModel, T> conv)
        {
            ConnectionMultiplexer rc = Connection;
            IServer server = rc.GetServer(RedisServerAddress);
            IDatabase db = rc.GetDatabase();
            IEnumerable<RedisKey> dict = server.Keys(pattern: GetRedisKey(pref));
            T[] res = server.Keys(pattern: GetRedisKey(pref)).Select(_ => conv(db, _, pref)).ToArray();

            return res;
        }

        public void RemoveKeys<T>(T[] ids, RedisPrefixExternModel pref)
        {
            foreach (var id in ids)
            {
                RemoveKey(new RedisCompKeyExternModel(id.ToString(), pref));
            }
        }

        public async Task RemoveKeysAsync<T>(T[] ids, RedisPrefixExternModel pref)
        {
            foreach (var id in ids)
            {
                await RemoveKeyAsync(new RedisCompKeyExternModel(id.ToString(), pref));
            }
        }

        public void RemoveKey(RedisCompKeyExternModel compKey)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            RedisKey key = GetRedisKey(compKey.RedisPref, compKey.Id);
            db.KeyDelete(key);
        }

        public async Task<bool> RemoveKeyAsync(RedisCompKeyExternModel compKey)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            RedisKey key = GetRedisKey(compKey.RedisPref, compKey.Id);
            return await db.KeyDeleteAsync(key);
        }

        public bool RemoveKey(RedisKey redisKey)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            return db.KeyDelete(redisKey);
        }

        public async Task<bool> RemoveKeyAsync(RedisKey redisKey)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            return await db.KeyDeleteAsync(redisKey);
        }

        public string? Value(RedisCompKeyExternModel key)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            string r_key = GetRedisKey(key.RedisPref, key.Id.ToString());
            RedisValue res = db.StringGet(r_key);

            return res.IsNull ? null : res.ToString();
        }

        public async Task<string?> ValueAsync(RedisCompKeyExternModel key)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            string r_key = GetRedisKey(key.RedisPref, key.Id?.ToString() ?? string.Empty);
            RedisValue res = await db.StringGetAsync(r_key);
            return res.IsNull ? null : res.ToString();
        }

        public KeyValuePair<string, string> KeyToStringPair(IDatabase db, RedisKey _, RedisPrefixExternModel pref)
        {
            return
                new KeyValuePair<string, string>
                (
                    GetRealStringKey(_.ToString(), pref),
                    db.StringGet(_).ToString()
                );
        }

        public Tuple<int, HashEntry[]> KeyToHashEntry(IDatabase db, RedisKey r_key, RedisPrefixExternModel pref)
        {
            return
                Tuple.Create(GetRealId(r_key.ToString(), pref),
                    db.HashGetAll(r_key).ToArray());
        }
    }
}
