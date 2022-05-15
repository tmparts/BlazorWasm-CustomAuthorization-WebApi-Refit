////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DbLayerLib
{
    /// <summary>
    /// Промежуточный/общий слой контекста базы данных
    /// </summary>
    public class LayerContext : DbContext
    {
        protected DatabaseConfigModel _config { get; set; }
        protected static bool IsEnsureCreated { get; set; } = false;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_config"></param>
        public LayerContext(IOptions<ServerConfigModel> set_config)
        {
            _config = set_config.Value.DatabaseConfig;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroupModelDB>().HasData(
                    new UserGroupModelDB("Test group 1", "Description group 1") { Id = 1, IsDeleted = true },
                    new UserGroupModelDB("Test group 2", "Description group 2") { Id = 2 },
                    new UserGroupModelDB("Test group 3", "Description group 3") { Id = 3 }
            );

            modelBuilder.Entity<ProjectModelDB>().HasData(
                    Enumerable.Range(1, 1024).Select(x => new ProjectModelDB($"Demo project {x}", $"Description project {x}") { Id = x })
            );

            modelBuilder.Entity<UserModelDB>().HasData(
                    new UserModelDB("Tom", AccessLevelsUsersEnum.ROOT) { Id = 1, Login = "222222222", PasswordHash = GlobalUtils.CalculateHashString("222222222"), Email = "tom@mail.ru" },
                    new UserModelDB("Bob", AccessLevelsUsersEnum.Confirmed) { Id = 2, Login = "bobb", PasswordHash = GlobalUtils.CalculateHashString("gsdfghjg"), Email = "bobb@mail.ru" },
                    new UserModelDB("Sam", AccessLevelsUsersEnum.Trusted) { Id = 3, Login = "samuel", PasswordHash = GlobalUtils.CalculateHashString("hdg6hw46s"), Email = "samuel@mail.ru" },
                    new UserModelDB("Kelly", AccessLevelsUsersEnum.Manager) { Id = 4, Login = "kiki", PasswordHash = GlobalUtils.CalculateHashString("dh6jwk45"), Email = "kiki@mail.ru" },
                    new UserModelDB("David", AccessLevelsUsersEnum.Admin) { Id = 5, Login = "diablo", PasswordHash = GlobalUtils.CalculateHashString("dfgh6qeh"), Email = "diablo@mail.ru" },
                    new UserModelDB("Rokki", AccessLevelsUsersEnum.Blocked) { Id = 6, Login = "tom", PasswordHash = GlobalUtils.CalculateHashString("ffsgfgggragf"), Email = "ronin@mail.ru" }
            );

            modelBuilder.Entity<UserToGroupLinkModelDb>().HasData(
                    new UserToGroupLinkModelDb() { Id = 1, GroupId = 1, UserId = 1 },
                    new UserToGroupLinkModelDb() { Id = 2, GroupId = 2, UserId = 1 },
                    new UserToGroupLinkModelDb() { Id = 3, GroupId = 3, UserId = 2 }
            );

            var rand = new Random();
            Array values = Enum.GetValues(typeof(AccessLevelsUsersToProjectsEnum));

            var rand_links = Enumerable.Range(1, 512).SelectMany(x => Enumerable.Range(1, 6).Select(y => new { project_id = x, user_id = y })).ToList();
            rand_links.Shuffle();
            rand_links = rand_links.Take(1024 * 3).ToList();
            int index_id = 0;
            modelBuilder.Entity<UserToProjectLinkModelDb>().HasData(
                    rand_links.Select(x => new UserToProjectLinkModelDb() { Id = ++index_id, ProjectId = x.project_id, UserId = x.user_id, IsDeleted = rand.Next(100) <= 20, AccessLevelUser = (AccessLevelsUsersToProjectsEnum)values.GetValue(rand.Next(values.Length)) })
            );
        }

        /// <summary>
        /// Подтверждение действия пользователя
        /// </summary>
        public DbSet<ConfirmationUserActionModelDb> ConfirmationsUsersActions { get; set; }

        /// <summary>
        /// Пользователи
        /// </summary>
        public DbSet<UserModelDB> Users { get; set; }

        /// <summary>
        /// Группа пользователей
        /// </summary>
        public DbSet<UserGroupModelDB> GroupsUsers { get; set; }

        /// <summary>
        /// Связи пользователей с группами
        /// </summary>
        public DbSet<UserToGroupLinkModelDb> UsersToGroupsLinks { get; set; }

        /// <summary>
        /// Пользовательские проекты
        /// </summary>
        public DbSet<ProjectModelDB> Projects { get; set; }

        /// <summary>
        /// Связи пользователей с пользовательскими проектами
        /// </summary>
        public DbSet<UserToProjectLinkModelDb> UsersToProjectsLinks { get; set; }


        /// <summary>
        /// Перечисления
        /// </summary>
        public DbSet<EnumModelDB> DesignEnums { get; set; }


        /// <summary>
        /// Справочники
        /// </summary>
        public DbSet<ReferenceBookModelDB> DesignReferenceBooks { get; set; }
        /// <summary>
        /// Поля справочников
        /// </summary>
        public DbSet<ReferenceBookPropertyModelDB> DesignReferenceBooksProperties { get; set; }
        /// <summary>
        /// Группы свойств полей справочников
        /// </summary>
        public DbSet<ReferenceBookPropertiesGroupModelDB> DesignReferenceBooksPropertiesGroups { get; set; }

        /// <summary>
        /// Документы
        /// </summary>
        public DbSet<DocumentModelDB> DesignDocuments { get; set; }
        /// <summary>
        /// Поля документов
        /// </summary>
        public DbSet<DocumentPropertyModelDB> DesignDocumentsProperties { get; set; }
        /// <summary>
        /// Группы свойств документов
        /// </summary>
        public DbSet<DocumentPropertiesGroupModelDB> DesignDocumentsPropertiesGroups { get; set; }
    }
}
