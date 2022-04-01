////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Models
{
    public class UserGroupModelDB : EntryDescriptionModel
    {
        public UserGroupModelDB() { }

        public UserGroupModelDB(string name) : base(name) { }

        public UserGroupModelDB(string name, string descripton) : base(name, descripton) { }

        public IEnumerable<UserModelDB>? Users { get; set; }
    }
}