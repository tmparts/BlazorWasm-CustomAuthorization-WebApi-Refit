////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;
using StackExchange.Redis;

namespace SharedLib
{
    /// <summary>
    /// Redis утилита
    /// </summary>
    public class RedisUtil : IDisposable
    {
        private static string GetRedisKey(MemCashePrefixModel pref, string id = "") => new MemCasheComplexKeyModel(id, pref).ToString();
        static RedisConfigModel? _config;
        /// <summary>
        /// Адрес сервера Redis
        /// </summary>
        public string RedisServerAddress => _config?.EndPoint ?? "localhost:6379";
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

        /// <summary>
        /// Представляет взаимосвязанную группу подключений к серверам Redis. Ссылку на это следует сохранить и использовать повторно.
        /// </summary>
        public ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_config"></param>
        public RedisUtil(RedisConfigModel set_config)
        {
            _config = set_config;
        }

        /// <summary>
        /// Утилизировать объект
        /// </summary>
        public void Dispose()
        {
            Connection?.Close();
            Connection?.Dispose();
        }

        public List<RedisKey> FindKeys(string pattern)
        {
            List<RedisKey> res = new List<RedisKey>();
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            foreach (System.Net.EndPoint? server in rc.GetEndPoints())
            {
                res.AddRange(rc.GetServer(server).Keys(db.Database, pattern));
            }

            return res;
        }
        public List<RedisKey> FindKeys(MemCashePrefixModel pref) => FindKeys(pref.ToString());

        #region get
        public async Task<string?> GetStringValueAsync(RedisKey key)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();

            RedisValue res = await db.StringGetAsync(key);
            return res.IsNull ? null : res.ToString();
        }
        public string? GetStringValue(RedisKey key)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();

            RedisValue res = db.StringGet(key);

            return res.IsNull ? null : res.ToString();
        }

        ///////////////////////////////////
        public async Task<string?> GetStringValueAsync(string key) => await GetStringValueAsync(new RedisKey(key));
        public string? GetStringValue(string key) => GetStringValue(new RedisKey(key));
        //
        public async Task<string?> GetStringValueAsync(MemCasheComplexKeyModel key) => await GetStringValueAsync(GetRedisKey(key.Pref, key.Id?.ToString() ?? string.Empty));
        public string? GetStringValue(MemCasheComplexKeyModel key) => GetStringValue(GetRedisKey(key.Pref, key.Id?.ToString() ?? string.Empty));
        //
        public async Task<string?> GetStringValueAsync(MemCashePrefixModel pref, string id = "") => await GetStringValueAsync(new MemCasheComplexKeyModel(id, pref));
        public string? GetStringValue(MemCashePrefixModel pref, string id = "") => GetStringValue(new MemCasheComplexKeyModel(id, pref));
        #endregion

        #region set/update
        public async Task<bool> UpdateValueAsync(RedisKey key, string value, TimeSpan? expiry = null)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            return await db.StringSetAsync(key, value, expiry);
        }
        public bool UpdateValue(RedisKey key, string value, TimeSpan? expiry = null)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            return db.StringSet(key, value, expiry);
        }

        ///////////////////////////////////
        public async Task<bool> UpdateValueAsync(string key, string value, TimeSpan? expiry = null) => await UpdateValueAsync(new RedisKey(key), value, expiry);
        public bool UpdateValue(string key, string value, TimeSpan? expiry = null) => UpdateValue(new RedisKey(key), value, expiry);
        //
        public async Task<bool> UpdateValueAsync(MemCasheComplexKeyModel key, string value, TimeSpan? expiry = null) => await UpdateValueAsync(GetRedisKey(key.Pref, key.Id?.ToString() ?? string.Empty), value, expiry);
        public bool UpdateValue(MemCasheComplexKeyModel key, string value, TimeSpan? expiry = null) => UpdateValue(GetRedisKey(key.Pref, key.Id?.ToString() ?? string.Empty), value, expiry);
        //
        public async Task<bool> UpdateValueAsync(MemCashePrefixModel pref, string id, string value, TimeSpan? expiry = null) => await UpdateValueAsync(GetRedisKey(pref, id), value, expiry);
        public bool UpdateValue(MemCashePrefixModel pref, string id, string value, TimeSpan? expiry = null) => UpdateValue(GetRedisKey(pref, id), value, expiry);
        #endregion

        #region remove
        public async Task<bool> RemoveKeyAsync(RedisKey key)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            return await db.KeyDeleteAsync(key);
        }
        public bool RemoveKey(RedisKey key)
        {
            ConnectionMultiplexer rc = Connection;
            IDatabase db = rc.GetDatabase();
            return db.KeyDelete(key);
        }

        ///////////////////////////////////
        public async Task<bool> RemoveKeyAsync(string key) => await RemoveKeyAsync(new RedisKey(key));
        public bool RemoveKey(string key) => RemoveKey(new RedisKey(key));
        //
        public async Task<bool> RemoveKeyAsync(MemCasheComplexKeyModel key) => await RemoveKeyAsync(GetRedisKey(key.Pref, key.Id));
        public bool RemoveKey(MemCasheComplexKeyModel key) => RemoveKey(GetRedisKey(key.Pref, key.Id));
        //
        public async Task<bool> RemoveKeyAsync(MemCashePrefixModel pref, string id) => await RemoveKeyAsync(GetRedisKey(pref, id));
        public bool RemoveKey(MemCashePrefixModel pref, string id) => RemoveKey(GetRedisKey(pref, id));
        #endregion
    }
}
