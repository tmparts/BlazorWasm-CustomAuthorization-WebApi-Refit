////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class ProjectModelDB : EntryDescriptionModel
    {
        public ProjectModelDB() { }

        public ProjectModelDB(int id, string name) : base(id, name) { }

        public ProjectModelDB(int id, string name, string descripton) : base(id, name, descripton) { }

        public IEnumerable<UserModelDB>? Users { get; set; }
    }
}