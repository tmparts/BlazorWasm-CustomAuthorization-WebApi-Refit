////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SrvMetaApp;

namespace MetaLib.MemCash
{
    public class RedisMemoryCasheService : IMemoryCashe, IDisposable
    {
        public string RedisServerAddress => _config?.Value?.RedisConfig?.EndPoint ?? "localhost:6379";
        private readonly ILogger<RedisMemoryCasheService> _logger;
        private readonly RedisUtil _redis;
        private readonly IOptions<ServerConfigModel> _config;

        public RedisMemoryCasheService(IOptions<ServerConfigModel> set_config, ILogger<RedisMemoryCasheService> set_logger)
        {
            _config = set_config;
            _logger = set_logger;
            _redis = new RedisUtil(_config.Value.RedisConfig);
        }

        public void Dispose()
        {
            _redis.Dispose();
        }

        public List<string> FindKeys(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                return null;

            try
            {
                return _redis.FindKeys(pattern).Select(x => x.ToString()).ToList();
            }
            catch (Exception ex)
            {
                string msg = $"error '{nameof(FindKeys)}' by string pattern:{pattern}";
                _logger.LogError(ex, msg);
                return null;
            }
        }

        public List<string> FindKeys(MemCashePrefixModel pref) => FindKeys(pref.ToString());

        #region get
        public async Task<string?> GetStringValueAsync(MemCasheComplexKeyModel mem_key)
        {
            return await _redis.GetStringValueAsync(mem_key);
        }

        public string? GetStringValue(MemCasheComplexKeyModel mem_key)
        {
            return _redis.GetStringValue(mem_key);
        }

        public async Task<string?> GetStringValueAsync(MemCashePrefixModel pref, string id = "")
        {
            return await _redis.GetStringValueAsync(pref, id);
        }

        public string? GetStringValue(MemCashePrefixModel pref, string id = "")
        {
            return _redis.GetStringValue(pref, id);
        }

        public async Task<string?> GetStringValueAsync(string mem_key)
        {
            return await _redis.GetStringValueAsync(mem_key);
        }

        public string? GetStringValue(string mem_key)
        {
            return _redis.GetStringValue(mem_key);
        }

        #endregion

        #region set/update
        public async Task<bool> UpdateValueAsync(string key, string value, TimeSpan? expiry = null)
        {
            return await _redis.UpdateValueAsync(key, value, expiry);
        }

        public bool UpdateValue(string key, string value, TimeSpan? expiry = null)
        {
            return _redis.UpdateValue(key, value, expiry);
        }

        public async Task<bool> UpdateValueAsync(MemCasheComplexKeyModel key, string value, TimeSpan? expiry = null)
        {
            return await _redis.UpdateValueAsync(key, value, expiry);
        }

        public bool UpdateValue(MemCasheComplexKeyModel key, string value, TimeSpan? expiry = null)
        {
            return _redis.UpdateValue(key, value, expiry);
        }

        public async Task<bool> UpdateValueAsync(MemCashePrefixModel pref, string id, string value, TimeSpan? expiry = null)
        {
            return await _redis.UpdateValueAsync(pref, id, value, expiry);
        }

        public bool UpdateValue(MemCashePrefixModel pref, string id, string value, TimeSpan? expiry = null)
        {
            return _redis.UpdateValue(pref, id, value, expiry);
        }
        #endregion

        #region remove        
        public async Task<bool> RemoveKeyAsync(string key)
        {
            return await _redis.RemoveKeyAsync(key);
        }

        public bool RemoveKey(string key)
        {
            return _redis.RemoveKey(key);
        }

        public async Task<bool> RemoveKeyAsync(MemCasheComplexKeyModel key)
        {
            return await _redis.RemoveKeyAsync(key);
        }

        public bool RemoveKey(MemCasheComplexKeyModel key)
        {
            return _redis.RemoveKey(key);
        }

        public async Task<bool> RemoveKeyAsync(MemCashePrefixModel pref, string id)
        {
            return await _redis.RemoveKeyAsync(pref, id);
        }

        public bool RemoveKey(MemCashePrefixModel pref, string id)
        {
            return _redis.RemoveKey(pref, id);
        }

        #endregion        
    }
}
