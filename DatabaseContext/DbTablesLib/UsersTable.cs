////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcLib;
using SharedLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharedLib;

namespace DbcMetaSqliteLib.Users
{
    /// <summary>
    /// Доступ к таблице пользователей базы данных
    /// </summary>
    public class UsersTable : IUsersTable
    {
        readonly DbAppContext _db_context;
        readonly ILogger<UsersTable> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_db_context"></param>
        /// <param name="set_logger"></param>
        public UsersTable(DbAppContext set_db_context, ILogger<UsersTable> set_logger)
        {
            _db_context = set_db_context;
            _logger = set_logger;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db_context.SaveChangesAsync();
        }

        public async Task AddAsync(UserModelDB user, bool auto_save = true)
        {
            await _db_context.AddAsync(user);
            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserModelDB user, bool auto_save = true)
        {
            _db_context.Update(user);
            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        public async Task<bool> AnyByLoginOrEmailAsync(string login, string email)
        {
            return await _db_context.Users.AnyAsync(x => x.Login == login || x.Email == email);
        }

        public async Task<UserModelDB?> FirstOrDefaultByLoginAsync(string login)
        {
            return await _db_context.Users.FirstOrDefaultAsync(x => x.Login == login);
        }

        public async Task<UserModelDB?> FirstOrDefaultByEmailAsync(string email)
        {
            return await _db_context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<FindUsersProfilesResponseModel> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter)
        {
            FindUsersProfilesResponseModel res = new FindUsersProfilesResponseModel();

            if (filter is null)
            {
                res.IsSuccess = false;
                res.Message = "Запрос не можеть быть NULL.";
                _logger.LogError(res.Message);
                return res;
            }

            IQueryable<UserModelDB>? query = _db_context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(filter?.FindLogin?.Text))
            {
                switch (filter.FindLogin.Mode)
                {
                    case FindTextModesEnum.Contains:
                        query = query.Where(x => x.Login.ToLower().Contains(filter.FindLogin.Text.ToLower()));
                        break;
                    case FindTextModesEnum.NotContains:
                        query = query.Where(x => !x.Login.ToLower().Contains(filter.FindLogin.Text.ToLower()));
                        break;
                    case FindTextModesEnum.Equal:
                        query = query.Where(x => x.Login.ToLower() == filter.FindLogin.Text.ToLower());
                        break;
                    case FindTextModesEnum.NotEqual:
                        query = query.Where(x => x.Login.ToLower() != filter.FindLogin.Text.ToLower());
                        break;
                    default:
                        res.IsSuccess = false;
                        res.Message = $"Ошибка определения режима '{filter.FindLogin.Mode}' поиска пользователя по логину.";
                        _logger.LogError(res.Message);
                        return res;
                }
            }

            if (filter.AccessLevelsUsers?.Any() == true)
                query = query.Where(x => filter.AccessLevelsUsers.Contains(x.AccessLevelUser));

            if (filter.ConfirmationUsersTypes?.Any() == true)
                query = query.Where(x => filter.ConfirmationUsersTypes.Contains(x.ConfirmationType));


            if (filter.Projects?.Any() == true)
                query = query.Where(x => _db_context.UsersToProjectsLinks.Any(y => filter.Projects.Contains(y.ProjectId) && y.UserId == x.Id));

            if (filter.Groups?.Any() == true)
                query = query.Where(x => _db_context.UsersToGroupsLinks.Any(y => filter.Groups.Contains(y.GroupId) && y.UserId == x.Id));

            res.Users = await query.Select(x => new UserLiteModel()
            {
                Id = x.Id,
                Login = x.Login,
                Email = x.Email,
                Name = x.Name,
                AccessLevelUser = x.AccessLevelUser,
                ConfirmationType = x.ConfirmationType,
                CreatedAt = x.CreatedAt,
            }).ToArrayAsync();

            return res;
        }

        public async Task<GetUserProfileResponseModel> GetUserProfileAsync(int id)
        {
            GetUserProfileResponseModel res = new GetUserProfileResponseModel() { IsSuccess = id > 0 };
            if (!res.IsSuccess)
            {
                res.Message = "Требуется корректный Id.";
                return res;
            }

            res.User = await _db_context.Users.FirstOrDefaultAsync(x => x.Id == id);
            res.IsSuccess = res.User is not null;

            if (!res.IsSuccess)
            {
                res.Message = "Пользователь не найден.";
            }

            return res;
        }

        public async Task<GetUserProfileResponseModel> GetUserProfileAsync(string login)
        {
            GetUserProfileResponseModel res = new GetUserProfileResponseModel() { IsSuccess = !string.IsNullOrEmpty(login) };
            if (!res.IsSuccess)
            {
                res.Message = "Требуется корректный 'login'.";
                return res;
            }

            res.User = await _db_context.Users.FirstOrDefaultAsync(x => x.Login == login);

            res.IsSuccess = res.User is not null;
            if (!res.IsSuccess)
            {
                res.Message = $"Пользователь не найден (login:{login})";
                return res;
            }

            res.IsSuccess = res.User is not null;

            if (!res.IsSuccess)
            {
                res.Message = "Пользователь не найден.";
            }

            return res;
        }

        public async Task<bool> PasswordEqualByUserIdAsync(int user_id, string password_hash)
        {
            return await _db_context.Users.AnyAsync(x => x.Id == user_id && x.PasswordHash == password_hash);
        }

        public async Task PasswordUpdateByUserIdAsync(int user_id, string password_hash)
        {
            UserModelDB upd_user = new UserModelDB() { Id = user_id, PasswordHash = password_hash };
            _db_context.Users.Attach(upd_user);
            _db_context.Entry(upd_user).Property(x => x.PasswordHash).IsModified = true;

            await _db_context.SaveChangesAsync();
        }
    }
}