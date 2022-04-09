using MetaLib.Models;

namespace MetaLib.ClientServices.refit
{
    public interface IUsersAuthRefitService
    {
        public SessionReadResponseModel GetUserSession();

        public Task<AuthUserResponseModel> LoginUserAsync(UserAuthorizationModel user);

        public Task<AuthUserResponseModel> RegistrationNewUserAsync(UserRegistrationModel user);

        public Task<ResponseBaseModel> LogOutUserAsync();

        public Task<ResponseBaseModel> RestoreUserAsync(UserRestoreModel user);
    }
}