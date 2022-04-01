////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

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
}
