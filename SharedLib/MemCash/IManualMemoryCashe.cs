////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.MemCash
{
    /// <summary>
    /// Интерфейс мемкеша
    /// </summary>
    public interface IManualMemoryCashe
    {
        /// <summary>
        /// Найти ключи доступа к данным по шаблону/строке
        /// </summary>
        /// <param name="pattern">шаблон/строка для поиска ключей</param>
        /// <returns>Найденные полные имена ключей доступа к данным мемкеша</returns>
        public List<string> FindKeys(string pattern);

        /// <summary>
        /// Найти ключи доступа к данным по шаблону/объекту
        /// </summary>
        /// <param name="pref">шаблон/обхект для поиска ключей</param>
        /// <returns>Найденные полные имена ключей доступа к данным мемкеша</returns>
        public List<string> FindKeys(MemCashePrefixModel pref);

        #region get

        /// <summary>
        /// Прочитать (асинхронно) из мемкеша данные в виде строки
        /// </summary>
        /// <param name="mem_key">Полное имя ключа доступа мемкеш данным</param>
        /// <returns>Данные, прочитанные из мемкеша по полному имени ключа</returns>
        public Task<string?> GetStringValueAsync(string mem_key);

        /// <summary>
        /// Прочитать из мемкеша данные в виде строки
        /// </summary>
        /// <param name="mem_key">Полное имя ключа доступа мемкеш данных</param>
        /// <returns>Данные, прочитанные из мемкеша по полному имени ключа</returns>
        public string? GetStringValue(string mem_key);

        /// <summary>
        /// Прочитать (асинхронно) из мемкеша данные в виде строки
        /// </summary>
        /// <param name="mem_key">Комплексный/полный ключ доступа мемкеш данным</param>
        /// <returns>Данные, прочитанные из мемкеша по комплексному/полному имени ключа</returns>
        public Task<string?> GetStringValueAsync(MemCasheComplexKeyModel mem_key);

        /// <summary>
        /// Прочитать из мемкеша данные в виде строки
        /// </summary>
        /// <param name="mem_key">Комплексный/полный ключ доступа мемкеш данным</param>
        /// <returns>Данные, прочитанные из мемкеша по комплексному/полному имени ключа</returns>
        public string? GetStringValue(MemCasheComplexKeyModel mem_key);

        /// <summary>
        /// Прочитать (асинхронно) из мемкеша данные в виде строки
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным</param>
        /// <param name="id">Имя/идентификатор (конечный) данных</param>
        /// <returns>Данные, прочитанные из мемкеша</returns>
        public Task<string?> GetStringValueAsync(MemCashePrefixModel pref, string id = "");

        /// <summary>
        /// Прочитать из мемкеша данные в виде строки
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным</param>
        /// <param name="id">Имя/идентификатор (конечный) данных</param>
        /// <returns>Данные, прочитанные из мемкеша</returns>
        public string? GetStringValue(MemCashePrefixModel pref, string id = "");
        #endregion

        #region set/upd
        /// <summary>
        /// Обновить/записать (асинхронно) данные в мемкеш
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public Task<bool> UpdateValueAsync(string key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// Обновить/записать данные в мемкеш
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public bool UpdateValue(string key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// Обновить/записать (асинхронно) данные в мемкеш
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public Task<bool> UpdateValueAsync(MemCasheComplexKeyModel key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// Обновить/записать данные в мемкеш
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public bool UpdateValue(MemCasheComplexKeyModel key, string value, TimeSpan? expiry = null);

        /// <summary>
        /// Обновить/записать (асинхронно) данные в мемкеш
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным мекеша</param>
        /// <param name="id">Имя/идентификатор (конечный) доступа к данным мемкеша</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public Task<bool> UpdateValueAsync(MemCashePrefixModel pref, string id, string value, TimeSpan? expiry = null);

        /// <summary>
        /// Обновить/записать данные в мемкеш
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным мекеша</param>
        /// <param name="id">Имя/идентификатор (конечный) доступа к данным мемкеша</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public bool UpdateValue(MemCashePrefixModel pref, string id, string value, TimeSpan? expiry = null);
        #endregion

        #region remove
        /// <summary>
        /// Удалить (асинхронно) данные из мемкеша
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <returns>Результат операции</returns>
        public Task<bool> RemoveKeyAsync(string key);
        /// <summary>
        /// Удалить данные из мемкеша
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <returns>Результат операции</returns>
        public bool RemoveKey(string key);
        /// <summary>
        /// Удалить (асинхронно) данные из мемкеша
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <returns>Результат операции</returns>
        public Task<bool> RemoveKeyAsync(MemCasheComplexKeyModel key);
        /// <summary>
        /// Удалить данные из мемкеша
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <returns>Результат операции</returns>
        public bool RemoveKey(MemCasheComplexKeyModel key);
        /// <summary>
        /// Удалить (асинхронно) данные из мемкеша
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным мекеша</param>
        /// <param name="id">Имя/идентификатор (конечный) доступа к данным мемкеша</param>
        /// <returns>Результат операции</returns>
        public Task<bool> RemoveKeyAsync(MemCashePrefixModel pref, string id);
        /// <summary>
        /// Удалить данные из мемкеша
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным мекеша</param>
        /// <param name="id">Имя/идентификатор (конечный) доступа к данным мемкеша</param>
        /// <returns>Результат операции</returns>
        public bool RemoveKey(MemCashePrefixModel pref, string id);
        #endregion
    }

}