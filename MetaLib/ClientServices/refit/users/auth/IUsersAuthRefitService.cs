using MetaLib.ClientServices.refit.users.auth.models;
using MetaLib.Models;

namespace MetaLib.ClientServices.refit
{
    public interface IUsersAuthRefitService
    {
        public SessionReadResponseRefitModel GetUserSession();

        public Task<AuthUserResponseRefitModel> LoginUser(UserAuthorizationModel user);

        public Task<AuthUserResponseRefitModel> RegistrationNewUser(UserRegistrationModel user);

        public Task<ResponseBaseRefitModel> LogOutUser();

        public Task<ResponseBaseRefitModel> RestoreUser(UserRestoreModel user);
    }
}