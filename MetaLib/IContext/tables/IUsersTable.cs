////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.Confirmations;
using MetaLib.Models;

namespace DbcMetaLib.Users
{
    /// <summary>
    /// Интерфейс доступа к данным пользователей
    /// </summary>
    public interface IUsersTable : SavingChanges
    {
        public Task<bool> AnyByLoginOrEmailAsync(string login, string email);

        public Task<UserModelDB?> FirstOrDefaultByLoginAsync(string login);

        public Task<UserModelDB?> FirstOrDefaultByEmailAsync(string email);

        public Task AddAsync(UserModelDB user, bool auto_save = true);

        public Task UpdateAsync(UserModelDB user, bool auto_save = true);

        //public Task<GetUserProfileResponseModel> UpdateMediumAsync(UserMediumModel user, bool auto_save = true);

        /// <summary>
        /// Поиск профилей пользовтелей
        /// </summary>
        public Task<FindUsersProfilesResponseModel> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter);

        public Task<GetUserProfileResponseModel> GetUserProfileAsync(int id);

        public Task<GetUserProfileResponseModel> GetUserProfileAsync(string login);
    }
}