////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp
{
    public class RedisCompKeyExternModel
    {
        public string Id { get; }
        public RedisPrefixExternModel RedisPref { get; }

        public RedisCompKeyExternModel(string set_id, RedisPrefixExternModel set_redis_pref)
        {
            Id = set_id;
            RedisPref = set_redis_pref;
        }

        public RedisCompKeyExternModel(int set_id, RedisPrefixExternModel set_redis_pref)
        {
            Id = set_id.ToString();
            RedisPref = set_redis_pref;
        }
    }
}
