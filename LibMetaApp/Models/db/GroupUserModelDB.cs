////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class GroupUserModelDB : EntryDescriptionModel
    {
        public GroupUserModelDB() { }

        public GroupUserModelDB(int id, string name) : base(id, name) { }

        public GroupUserModelDB(int id, string name, string descripton) : base(id, name, descripton) { }

        public IEnumerable<UserModelDB>? Users { get; set; }
    }
}