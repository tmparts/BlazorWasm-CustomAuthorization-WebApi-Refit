////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.MemCash
{
    public interface IMemoryCashe
    {
        public List<string> FindKeys(string pattern);
        public List<string> FindKeys(MemCashePrefixModel pref);

        #region get
        public Task<string?> GetStringValueAsync(string mem_key);
        public string? GetStringValue(string mem_key);

        public Task<string?> GetStringValueAsync(MemCasheComplexKeyModel mem_key);
        public string? GetStringValue(MemCasheComplexKeyModel mem_key);

        public Task<string?> GetStringValueAsync(MemCashePrefixModel pref, string id = "");
        public string? GetStringValue(MemCashePrefixModel pref, string id = "");
        #endregion

        #region set/upd        
        public Task<bool> UpdateValueAsync(string key, string value, TimeSpan? expiry = null);
        public bool UpdateValue(string key, string value, TimeSpan? expiry = null);
        //
        public Task<bool> UpdateValueAsync(MemCasheComplexKeyModel key, string value, TimeSpan? expiry = null);
        public bool UpdateValue(MemCasheComplexKeyModel key, string value, TimeSpan? expiry = null);
        //
        public Task<bool> UpdateValueAsync(MemCashePrefixModel pref, string id, string value, TimeSpan? expiry = null);
        public bool UpdateValue(MemCashePrefixModel pref, string id, string value, TimeSpan? expiry = null);
        #endregion

        #region remove        
        public Task<bool> RemoveKeyAsync(string key);
        public bool RemoveKey(string key);
        //
        public Task<bool> RemoveKeyAsync(MemCasheComplexKeyModel key);
        public bool RemoveKey(MemCasheComplexKeyModel key);
        //
        public Task<bool> RemoveKeyAsync(MemCashePrefixModel pref, string id);
        public bool RemoveKey(MemCashePrefixModel pref, string id);
        #endregion
    }

}