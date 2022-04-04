////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib
{
    public class MemCashePrefixModel
    {
        public string Namespace { get; }
        public string Dictionary { get; }

        public MemCashePrefixModel(string set_namespace, string set_dict)
        {
            Namespace = set_namespace;
            Dictionary = set_dict;
        }

        public override string ToString() => $"{Namespace}:{Dictionary}";
    }
}
