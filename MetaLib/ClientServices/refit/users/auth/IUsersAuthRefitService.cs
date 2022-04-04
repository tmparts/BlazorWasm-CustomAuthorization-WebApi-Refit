using MetaLib.Models;

namespace MetaLib.ClientServices.refit
{
    public interface IUsersAuthRefitService
    {
        public SessionReadResponseModel GetUserSession();

        public Task<AuthUserResponseModel> LoginUser(UserAuthorizationModel user);

        public Task<AuthUserResponseModel> RegistrationNewUser(UserRegistrationModel user);

        public Task<ResponseBaseModel> LogOutUser();

        public Task<ResponseBaseModel> RestoreUser(UserRestoreModel user);
    }
}