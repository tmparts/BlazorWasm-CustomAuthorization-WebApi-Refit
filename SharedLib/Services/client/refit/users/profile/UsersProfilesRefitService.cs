﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Services;
using Microsoft.Extensions.Logging;
using Refit;
using SharedLib.Models;
using SharedLib.Models.enums;

namespace SharedLib.ClientServices.refit
{
    /// <summary>
    /// Refit служба раоты с API профилей пользователя
    /// </summary>
    public class UsersProfilesRefitService : IUsersProfilesRestService
    {
        private readonly IUsersProfilesRefitService _users_profile_service;
        private readonly ILogger<UsersProfilesRefitService> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_users_profile_service"></param>
        /// <param name="set_logger"></param>
        public UsersProfilesRefitService(IUsersProfilesRefitService set_users_profile_service, ILogger<UsersProfilesRefitService> set_logger)
        {
            _users_profile_service = set_users_profile_service;
            _logger = set_logger;
        }

        public async Task<GetUserProfileResponseModel> GetUserProfileAsync(int client_id)
        {
            GetUserProfileResponseModel result = new GetUserProfileResponseModel();

            try
            {
                ApiResponse<GetUserProfileResponseModel> rest = await _users_profile_service.GetUserProfileAsync(client_id);

                if (rest.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;

                    result.Message = $"HTTP error: [code={rest.StatusCode}] {rest?.Error?.Content}";
                    _logger.LogError(result.Message);

                    return result;
                }
                result.IsSuccess = rest.Content.IsSuccess;
                result.User = rest.Content.User;
                result.Sessions = rest.Content.Sessions;
                result.Message = rest.Content.Message;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Exception {nameof(_users_profile_service.GetUserProfileAsync)}";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }

        public async Task<FindUsersProfilesResponseModel> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter)
        {
            FindUsersProfilesResponseModel result = new FindUsersProfilesResponseModel();

            try
            {
                ApiResponse<FindUsersProfilesResponseModel> rest = await _users_profile_service.FindUsersProfilesAsync(filter);

                if (rest.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;
                    result.Message = $"HTTP error: [code={rest.StatusCode}] {rest?.Error?.Content}";
                    _logger.LogError(result.Message);

                    return result;
                }
                result.IsSuccess = rest.Content.IsSuccess;
                result = rest.Content;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Exception {nameof(_users_profile_service.FindUsersProfilesAsync)}";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }

        public async Task<UpdateUserProfileResponseModel> UpdateUserProfileAsync(UserLiteModel user)
        {
            UpdateUserProfileResponseModel result = new UpdateUserProfileResponseModel();

            try
            {
                ApiResponse<UpdateUserProfileResponseModel> rest = await _users_profile_service.UpdateUserProfileAsync(user);

                if (rest.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;
                    result.Message = $"HTTP error: [code={rest.StatusCode}] {rest?.Error?.Content}";
                    _logger.LogError(result.Message);

                    return result;
                }
                result.IsSuccess = rest.Content.IsSuccess;
                result = rest.Content;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Exception {nameof(_users_profile_service.UpdateUserProfileAsync)}";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }

        public async Task<UpdateUserProfileResponseModel> ChangeUserProfileAsync(UserProfileAreasEnum area, ChangeUserProfileOptionsModel user_options)
        {
            UpdateUserProfileResponseModel result = new UpdateUserProfileResponseModel();

            try
            {
                ApiResponse<UpdateUserProfileResponseModel>? rest = await _users_profile_service.ChangeUserProfileAsync(area, user_options);

                if (rest.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;
                    result.Message = $"HTTP error: [code={rest.StatusCode}] {rest?.Error?.Content}";
                    _logger.LogError(result.Message);

                    return result;
                }
                result.IsSuccess = rest.Content.IsSuccess;
                result = rest.Content;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Exception {nameof(_users_profile_service.ChangeUserProfileAsync)}";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }
    }
}