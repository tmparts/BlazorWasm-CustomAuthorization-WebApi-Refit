////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class GroupUserModelDB : EntryDescriptionModel
    {
        public GroupUserModelDB() { }

        public GroupUserModelDB(string name) : base(name) { }

        public GroupUserModelDB(string name, string descripton) : base(name, descripton) { }

        public IEnumerable<UserModelDB>? Users { get; set; }
    }
}