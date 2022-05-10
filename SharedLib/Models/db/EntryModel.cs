////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SharedLib.Models
{
    /// <summary>
    /// Базовая DB модель с поддержкой -> int:Id
    /// </summary>
    public abstract class IdModel
    {
        /// <summary>
        /// Идентификатор/Key
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Объект удалён
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }

    /// <summary>
    /// Базовая DB модель объекта с поддржкой -> int:Id +string:Name
    /// </summary>
    public class EntryModel : IdModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public EntryModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя объекта</param>
        public EntryModel(string name) { Name = name; }

        /// <summary>
        /// Имя объекта
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Базовая DB модель объекта с поддржкой -> int:Id +string:Name +string:Description
    /// </summary>
    public class EntryDescriptionModel : EntryModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public EntryDescriptionModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя объекта</param>
        public EntryDescriptionModel(string name) : base(name) { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя объекта</param>
        /// <param name="description">Описание/примечание для объекта</param>
        public EntryDescriptionModel(string name, string description) : base(name) { Description = description; }

        /// <summary>
        /// Описание/примечание для объекта
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Базовая DB модель объекта с поддржкой -> int:Id +string:Name +string:Tag
    /// </summary>
    public class EntryTagModel : EntryModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public EntryTagModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя объекта</param>
        /// <param name="tag">Tag/Признак объекта</param>
        protected EntryTagModel(string name, string tag) : base(name)
        {
            Tag = tag;
        }

        /// <summary>
        /// Tag/Признак объекта
        /// </summary>
        public string Tag { get; set; } = string.Empty;
    }

    /// <summary>
    /// Базовая DB модель объекта с поддржкой -> int:Id +string:Name +DateTime:CreatedAt
    /// </summary>
    [Index(nameof(CreatedAt))]
    public class EntryCreatedModel : EntryModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public EntryCreatedModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя объекта</param>
        public EntryCreatedModel(string name) : base(name) { }

        /// <summary>
        /// Дата/время создания
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
