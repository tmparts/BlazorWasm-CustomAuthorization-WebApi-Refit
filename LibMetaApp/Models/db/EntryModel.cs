////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public abstract class IdModel
    {
        public int Id { get; set; }
    }

    public class EntryModel : IdModel
    {
        public EntryModel() { }

        public EntryModel(int id, string name) { Id = id; Name = name; }

        public string Name { get; set; } = string.Empty;
    }

    public class EntryDescriptionModel : EntryModel
    {
        public EntryDescriptionModel() { }

        public EntryDescriptionModel(int id, string name) : base(id, name) { }

        public EntryDescriptionModel(int id, string name, string description) : base(id, name) { Description = description; }

        public string Description { get; set; } = string.Empty;
    }

    public class EntryTagModel : EntryModel
    {
        protected EntryTagModel() { }

        protected EntryTagModel(int id, string name, string tag) : base(id, name)
        {
            Tag = tag;
        }

        public string Tag { get; set; } = string.Empty;
    }

    public class EntryCreatedModel : EntryModel
    {
        protected EntryCreatedModel() { }

        protected EntryCreatedModel(int id, string name) : base(id, name) { }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
