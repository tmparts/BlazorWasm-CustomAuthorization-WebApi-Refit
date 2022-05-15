﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.Collections;
using System.Globalization;
using System.Text;

namespace SharedLib.Models
{
    /// <summary>
    /// Глобальные утилиты
    /// </summary>
    public static class GlobalUtils
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Русская (ru-RU) CultureInfo
        /// </summary>
        public static CultureInfo RU => CultureInfo.GetCultureInfo("ru-RU");

        #region
        /// <summary>
        /// Получить отсутсвующие в [actual](новые) ноды по сравнению с [current](из redis)
        /// </summary>
        /// <param name="actual">Новые ноды</param>
        /// <param name="current">Текущие (из кэша) ноды</param>
        /// <returns>Ноды, которх нет в новом наборе нод (current), но есть в текущем наборе (actual)</returns>
        public static NodeModel[] ExpiredNodes(NodeModel[] actual, NodeModel[] current)
        {
            return current.Where(cur => !actual.Any(act => act.Id == cur.Id)).ToArray();
        }

        /// <summary>
        /// Получить вновь появившиеся в [actual](новые) ноды по сравнению с [current](из redis)
        /// </summary>
        /// <param name="actual">Новые ноды</param>
        /// <param name="current">Текущие (из кэша) ноды</param>
        /// <returns>Ноды, которых нет в текущем наборе нод (current), но есть в новом наборе (actual)</returns>
        public static NodeModel[] NewNodes(NodeModel[] actual, NodeModel[] current)
        {
            return actual.Where(act => !current.Any(cur => cur.Id == act.Id)).ToArray();
        }

        /// <summary>
        /// Получить отсутсвуюшие в [actual](новые) объекты, по сравнению с [current].
        /// Для сравнения значений массивов используется: компаратор проверки на равенство по умолчанию.
        /// </summary>
        /// <typeparam name="T">Тип данных (с поддержкой сравнения)</typeparam>
        /// <param name="actual">Новая колекция</param>
        /// <param name="current">Текущая (устареваемая) колекция</param>
        /// <returns>Объекты, которых нет в текущем наборе нод (current), но есть в новом наборе (actual)</returns>
        public static T[] ExpiredItems<T>(T[] actual, T[] current)
        {
            try
            {
                return current.Except(actual).ToArray();
            }
            catch
            {
                return Array.Empty<T>();
            }
        }

        /// <summary>
        /// Получить вновь появившиеся в [actual](новые) объекты по сравнению с [current]
        /// Для сравнения значений массивов используется: компаратор проверки на равенство по умолчанию.
        /// </summary>
        /// <typeparam name="T">Тип данных (с поддержкой сравнения)</typeparam>
        /// <param name="actual">Новая колекция</param>
        /// <param name="current">Текущая (устареваемая) колекция</param>
        /// <returns>Ноды, которых нет в текущем наборе объектов (current), но есть в новом наборе (actual)</returns>
        public static T[] NewItems<T>(T[] actual, T[] current)
        {
            return actual.Except(current).ToArray();
        }

        /// <summary>
        /// Получить изменившиеся значения словаря
        /// </summary>
        /// <typeparam name="I">Тип данных для ключа словаря</typeparam>
        /// <typeparam name="T">Тип данных для данных значения словаря</typeparam>
        /// <param name="actual">Ведущий/исходный словарь данных (его набор ключей истинный)</param>
        /// <param name="current">Ведомый словарь данных для сравнения (проверка выполняется только если в данном словаре есть искомый ключ из ведущего слоавря)</param>
        /// <returns>Словарь с различными данными (но с одним ключём)</returns>
        public static Dictionary<I, T> ChangedDictItems<I, T>(IDictionary actual, IDictionary current)
        {
            Dictionary<I, T> d = new Dictionary<I, T>();
            foreach (DictionaryEntry c in current)
            {
                dynamic a = actual[c.Key];
                if (!(a == null) && !a.Equals(c.Value))
                {
                    d.Add((I)c.Key, (T)a);
                }
            }
            return d;
        }
        #endregion

        #region
        /// <summary>
        /// Вычислить Хеш строки
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CalculateHashString(string password)
        {
            var bytes = new UTF8Encoding().GetBytes(password);
            var hashBytes = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Генерация пароля
        /// </summary>
        /// <param name="length">Длинна пароля</param>
        /// <returns></returns>
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        #endregion
    }
}
