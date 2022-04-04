////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib
{
    public class MemCasheComplexKeyModel
    {
        public string Id { get; }
        public MemCashePrefixModel Pref { get; }

        public MemCasheComplexKeyModel(string set_id, MemCashePrefixModel set_pref)
        {
            Id = set_id;
            Pref = set_pref;
        }

        public override string ToString()
        {
            string itemId = Id == string.Empty ? "*" : Id;
            if (string.IsNullOrWhiteSpace(Pref.Dictionary))
                return string.Format("{0}:{1}", Pref.Namespace, itemId);
            else
                return string.Format("{0}:{1}", Pref.ToString(), itemId);
        }
    }
}
