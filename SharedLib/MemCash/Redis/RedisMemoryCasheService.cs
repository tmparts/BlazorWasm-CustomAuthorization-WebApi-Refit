////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace SharedLib.MemCash
{
    /// <summary>
    /// Сервис мемкеша Redis
    /// </summary>
    public class RedisMemoryCasheService : IManualMemoryCashe, IDisposable
    {
        /// <summary>
        /// Адрес сервреа Redis
        /// </summary>
        public string RedisServerAddress => _config?.Value?.RedisConfig?.EndPoint ?? "localhost:6379";
        private readonly ILogger<RedisMemoryCasheService> _logger;
        private readonly RedisUtil _redis;
        private readonly IOptions<ServerConfigModel> _config;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_config"></param>
        /// <param name="set_logger"></param>
        public RedisMemoryCasheService(IOptions<ServerConfigModel> set_config, ILogger<RedisMemoryCasheService> set_logger)
        {
            _config = set_config;
            _logger = set_logger;
            _redis = new RedisUtil(_config.Value.RedisConfig);
        }

        /// <summary>
        /// Утилизировать
        /// </summary>
        public void Dispose()
        {
            _redis.Dispose();
        }

        /// <summary>
        /// Найти ключи доступа к данным по шаблону/строке
        /// </summary>
        /// <param name="pattern">шаблон/строка для поиска ключей</param>
        /// <returns>Найденные полные имена ключей доступа к данным мемкеша</returns>
        public List<RedisKey>? FindKeys(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                return null;

            try
            {
                return _redis.FindKeys(pattern).ToList();
            }
            catch (Exception ex)
            {
                string msg = $"error '{nameof(FindKeys)}' by string pattern:{pattern}";
                _logger.LogError(ex, msg);
                return null;
            }
        }

        /// <summary>
        /// Найти ключи доступа к данным по шаблону/объекту
        /// </summary>
        /// <param name="pref">шаблон/обхект для поиска ключей</param>
        /// <returns>Найденные полные имена ключей доступа к данным мемкеша</returns>
        public List<RedisKey>? FindKeys(MemCashePrefixModel pref) => FindKeys(pref.ToString());

        /// <summary>
        /// Проверка существоания ключа
        /// </summary>
        /// <param name="mem_key">ключ для проверки</param>
        /// <returns>true, если ключ существует. false, если ключ не существует.</returns>
        public async Task<bool> KeyExistsAsync(MemCasheComplexKeyModel mem_key)
        {
            return await _redis.KeyExistsAsync(mem_key);
        }

        /// <summary>
        /// Установить тайм-аут на ключ. По истечении тайм-аута ключ будет автоматически удален. В терминологии Redis ключ с соответствующим тайм-аутом называется изменчивым.
        /// </summary>
        /// <param name="mem_key">Ключ, для которого устанавливается срок действия.</param>
        /// <param name="expiry">Тайм-аут, который нужно установить.</param>
        /// <returns>true, если тайм-аут был установлен. false, если ключ не существует или не удалось установить время ожидания.</returns>
        public async Task<bool> KeyExpireAsync(MemCasheComplexKeyModel mem_key, TimeSpan? expiry)
        {
            return await _redis.KeyExpireAsync(mem_key, expiry);
        }

        /// <summary>
        /// Переименовывает ключ в newKey. Он возвращает ошибку, если имена источника и назначения совпадают или если ключ не существует.
        /// </summary>
        /// <param name="mem_key">Ключ для переименования.</param>
        /// <param name="new_mem_key">Новый ключ, в который требуется переименовать</param>
        /// <returns>true, если ключ был переименован, в противном случае — false.</returns>
        public async Task<bool> KeyRenameAsync(MemCasheComplexKeyModel mem_key, MemCasheComplexKeyModel new_mem_key)
        {
            return await _redis.KeyRenameAsync(mem_key, new_mem_key);
        }

        /// <summary>
        /// Возвращает оставшееся время жизни ключа. Эта возможность позволяет клиенту Redis проверять, сколько секунд данный ключ будет оставаться частью набора данных.
        /// </summary>
        /// <param name="mem_key">Ключ для проверки</param>
        /// <returns>TTL или ноль, если ключ не существует или не имеет тайм-аута.</returns>
        public async Task<TimeSpan?> KeyTimeToLiveAsync(MemCasheComplexKeyModel mem_key)
        {
            return await _redis.KeyTimeToLiveAsync(mem_key);
        }

        #region get
        /// <summary>
        /// Прочитать из мемкеша данные в виде строки
        /// </summary>
        /// <param name="mem_key">Комплексный/полный ключ доступа мемкеш данным</param>
        /// <returns>Данные, прочитанные из мемкеша по комплексному/полному имени ключа</returns>
        public async Task<string?> GetStringValueAsync(MemCasheComplexKeyModel mem_key)
        {
            return await _redis.GetStringValueAsync(mem_key);
        }

        /// <summary>
        /// Прочитать из мемкеша данные в виде строки
        /// </summary>
        /// <param name="mem_key">Комплексный/полный ключ доступа мемкеш данным</param>
        /// <returns>Данные, прочитанные из мемкеша по комплексному/полному имени ключа</returns>
        public string? GetStringValue(MemCasheComplexKeyModel mem_key)
        {
            return _redis.GetStringValue(mem_key);
        }

        /// <summary>
        /// Прочитать (асинхронно) из мемкеша данные в виде строки
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным</param>
        /// <param name="id">Имя/идентификатор (конечный) данных</param>
        /// <returns>Данные, прочитанные из мемкеша</returns>
        public async Task<string?> GetStringValueAsync(MemCashePrefixModel pref, string id = "")
        {
            return await _redis.GetStringValueAsync(pref, id);
        }

        /// <summary>
        /// Прочитать из мемкеша данные в виде строки
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным</param>
        /// <param name="id">Имя/идентификатор (конечный) данных</param>
        /// <returns>Данные, прочитанные из мемкеша</returns>
        public string? GetStringValue(MemCashePrefixModel pref, string id = "")
        {
            return _redis.GetStringValue(pref, id);
        }

        /// <summary>
        /// Прочитать (асинхронно) из мемкеша данные в виде строки
        /// </summary>
        /// <param name="mem_key">Полное имя ключа доступа мемкеш данным</param>
        /// <returns>Данные, прочитанные из мемкеша по полному имени ключа</returns>
        public async Task<string?> GetStringValueAsync(string mem_key)
        {
            return await _redis.GetStringValueAsync(mem_key);
        }

        /// <summary>
        /// Прочитать из мемкеша данные в виде строки
        /// </summary>
        /// <param name="mem_key">Полное имя ключа доступа мемкеш данных</param>
        /// <returns>Данные, прочитанные из мемкеша по полному имени ключа</returns>
        public string? GetStringValue(string mem_key)
        {
            return _redis.GetStringValue(mem_key);
        }

        #endregion

        #region set/update
        /// <summary>
        /// Обновить/записать (асинхронно) данные в мемкеш
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public async Task<bool> UpdateValueAsync(string key, string value, TimeSpan? expiry = null)
        {
            return await _redis.UpdateValueAsync(key, value, expiry);
        }

        /// <summary>
        /// Обновить/записать данные в мемкеш
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public bool UpdateValue(string key, string value, TimeSpan? expiry = null)
        {
            return _redis.UpdateValue(key, value, expiry);
        }

        /// <summary>
        /// Обновить/записать (асинхронно) данные в мемкеш
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public async Task<bool> UpdateValueAsync(MemCasheComplexKeyModel key, string value, TimeSpan? expiry = null)
        {
            return await _redis.UpdateValueAsync(key, value, expiry);
        }

        /// <summary>
        /// Обновить/записать данные в мемкеш
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public bool UpdateValue(MemCasheComplexKeyModel key, string value, TimeSpan? expiry = null)
        {
            return _redis.UpdateValue(key, value, expiry);
        }

        /// <summary>
        /// Обновить/записать (асинхронно) данные в мемкеш
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным мекеша</param>
        /// <param name="id">Имя/идентификатор (конечный) доступа к данным мемкеша</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public async Task<bool> UpdateValueAsync(MemCashePrefixModel pref, string id, string value, TimeSpan? expiry = null)
        {
            return await _redis.UpdateValueAsync(pref, id, value, expiry);
        }

        /// <summary>
        /// Обновить/записать данные в мемкеш
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным мекеша</param>
        /// <param name="id">Имя/идентификатор (конечный) доступа к данным мемкеша</param>
        /// <param name="value">Значение для записи в мемкеш</param>
        /// <param name="expiry">Срок годности/хранения данных в мемкеше (null - по умолчанию = бессрочно)</param>
        /// <returns>Результат операции</returns>
        public bool UpdateValue(MemCashePrefixModel pref, string id, string value, TimeSpan? expiry = null)
        {
            return _redis.UpdateValue(pref, id, value, expiry);
        }
        #endregion

        #region remove
        /// <summary>
        /// Удалить (асинхронно) данные из мемкеша
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <returns>Результат операции</returns>
        public async Task<bool> RemoveKeyAsync(string key)
        {
            return await _redis.RemoveKeyAsync(key);
        }

        /// <summary>
        /// Удалить данные из мемкеша
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <returns>Результат операции</returns>
        public bool RemoveKey(string key)
        {
            return _redis.RemoveKey(key);
        }

        /// <summary>
        /// Удалить (асинхронно) данные из мемкеша
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <returns>Результат операции</returns>
        public async Task<bool> RemoveKeyAsync(MemCasheComplexKeyModel key)
        {
            return await _redis.RemoveKeyAsync(key);
        }

        /// <summary>
        /// Удалить данные из мемкеша
        /// </summary>
        /// <param name="key">Ключ/указатель на данные в мемкеше</param>
        /// <returns>Результат операции</returns>
        public bool RemoveKey(MemCasheComplexKeyModel key)
        {
            return _redis.RemoveKey(key);
        }

        /// <summary>
        /// Удалить (асинхронно) данные из мемкеша
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным мекеша</param>
        /// <param name="id">Имя/идентификатор (конечный) доступа к данным мемкеша</param>
        /// <returns>Результат операции</returns>
        public async Task<bool> RemoveKeyAsync(MemCashePrefixModel pref, string id)
        {
            return await _redis.RemoveKeyAsync(pref, id);
        }

        /// <summary>
        /// Удалить данные из мемкеша
        /// </summary>
        /// <param name="pref">Префикс ключа доступа к данным мекеша</param>
        /// <param name="id">Имя/идентификатор (конечный) доступа к данным мемкеша</param>
        /// <returns>Результат операции</returns>
        public bool RemoveKey(MemCashePrefixModel pref, string id)
        {
            return _redis.RemoveKey(pref, id);
        }
        #endregion
    }
}
