////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Проект пользовательский
    /// </summary>
    public class ProjectModelDB : EntryDescriptionModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public ProjectModelDB() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя проекта</param>
        public ProjectModelDB(string name) : base(name) { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Название проекта</param>
        /// <param name="descripton">Описание проекта</param>
        public ProjectModelDB(string name, string descripton) : base(name, descripton) { }

        /// <summary>
        /// Пользователи, связанные с проектом
        /// </summary>
        public IEnumerable<UserToProjectLinkModelDb>? UsersLinks { get; set; }
    }
}