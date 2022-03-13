////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp
{
    public class RedisPrefixExternModel
    {
        public string Namespace { get; }
        public string Dict { get; }

        public RedisPrefixExternModel(string set_namespace, string set_dict)
        {
            Namespace = set_namespace;
            Dict = set_dict;
        }
    }
}
