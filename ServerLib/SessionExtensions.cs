﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace SrvMetaApp
{
    /// <summary>
    /// Расширение клиентской ссейссий
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Установить значение параметра
        /// </summary>
        /// <typeparam name="T">Тип хранимого параметра сессии</typeparam>
        /// <param name="session">Сессия</param>
        /// <param name="key">Имя параметра</param>
        /// <param name="value">Значение параметра</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Получить значение параметра сессии
        /// </summary>
        /// <typeparam name="T">ISession</typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
