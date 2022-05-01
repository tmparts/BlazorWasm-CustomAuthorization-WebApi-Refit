////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Группа пользователя
    /// </summary>
    public class UserGroupModelDB : EntryDescriptionModel
    {
        /// <summary>
        /// Конструткор
        /// </summary>
        public UserGroupModelDB() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Название группы</param>
        public UserGroupModelDB(string name) : base(name) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Название группы</param>
        /// <param name="descripton">Описание группы</param>
        public UserGroupModelDB(string name, string descripton) : base(name, descripton) { }

        /// <summary>
        /// Пользователи, входящие в группу
        /// </summary>
        public IEnumerable<UserModelDB>? Users { get; set; }
    }
}