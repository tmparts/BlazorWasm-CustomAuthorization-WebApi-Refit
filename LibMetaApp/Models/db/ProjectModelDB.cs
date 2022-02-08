////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class ProjectModelDB : EntryDescriptionModel
    {
        public ProjectModelDB() { }

        public ProjectModelDB(string name) : base(name) { }

        public ProjectModelDB(string name, string descripton) : base(name, descripton) { }

        public IEnumerable<UserModelDB>? Users { get; set; }
    }
}