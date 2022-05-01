////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.Extensions.Logging;
using Refit;
using SharedLib.Models;
using SharedLib.Models.enums;

namespace SharedLib.Services
{
    public class UsersProfilesRefitProvider : IUsersProfilesRefitProvider
    {
        private readonly IUsersProfilesRefitService _api;
        private readonly ILogger<UsersProfilesRefitProvider> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_api"></param>
        /// <param name="set_logger"></param>
        public UsersProfilesRefitProvider(IUsersProfilesRefitService set_api, ILogger<UsersProfilesRefitProvider> set_logger)
        {
            _api = set_api;
            _logger = set_logger;
        }

        /// <summary>
        /// Изменить параметр профиля пользователя
        /// </summary>
        /// <param name="area">Область изменения профиля пользователя</param>
        /// <param name="user_options">Параметр для изменения</param>
        /// <returns>бновление профиля пользователя (результат запроса)</returns>
        public async Task<ApiResponse<UpdateUserProfileResponseModel>> ChangeUserProfileAsync(UserProfileAreasEnum area, ChangeUserProfileOptionsModel user_options)
        {
            return await _api.ChangeUserProfileAsync(area, user_options);
        }

        public async Task<ApiResponse<FindUsersProfilesResponseModel>> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter)
        {
            return await _api.FindUsersProfilesAsync(filter);
        }

        public async Task<ApiResponse<GetUserProfileResponseModel>> GetUserProfileAsync(int id)
        {
            return await _api.GetUserProfileAsync(id);
        }

        public async Task<ApiResponse<UpdateUserProfileResponseModel>> UpdateUserProfileAsync(UserLiteModel user)
        {
            return await _api.UpdateUserProfileAsync(user);
        }
    }
}
