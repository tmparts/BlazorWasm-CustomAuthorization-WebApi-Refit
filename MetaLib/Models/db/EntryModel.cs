////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibMetaApp.Models
{
    public abstract class IdModel
    {
        [Key]
        public int Id { get; set; }
    }

    public class EntryModel : IdModel
    {
        public EntryModel() { }

        public EntryModel(string name) { Name = name; }

        public string Name { get; set; } = string.Empty;
    }

    public class EntryDescriptionModel : EntryModel
    {
        public EntryDescriptionModel() { }

        public EntryDescriptionModel(string name) : base(name) { }

        public EntryDescriptionModel(string name, string description) : base(name) { Description = description; }

        public string Description { get; set; } = string.Empty;
    }

    public class EntryTagModel : EntryModel
    {
        public EntryTagModel() { }

        protected EntryTagModel(string name, string tag) : base(name)
        {
            Tag = tag;
        }

        public string Tag { get; set; } = string.Empty;
    }

    [Index(nameof(CreatedAt))]
    public class EntryCreatedModel : EntryModel
    {
        public EntryCreatedModel() { }

        public EntryCreatedModel(string name) : base(name) { }

        /// <summary>
        /// Дата/время создания
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
