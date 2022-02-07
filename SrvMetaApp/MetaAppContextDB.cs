////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using LibMetaApp;

namespace SrvMetaApp
{
    public class MetaAppContextDB : DbContext
    {
        private DatabaseConfigModel _config;
        private static bool IsEnsureCreated = false;

        public string DbPath { get; } = string.Empty;

        public static string DbFileName { get; set; } = string.Empty;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (string.IsNullOrWhiteSpace(_config.Connect.ConnectionString))
            {
#if DEBUG
                if (!IsEnsureCreated)
                {
                    IsEnsureCreated = true;
                    File.Delete(DbPath);
                }
#endif
                options.UseSqlite($"Data Source={DbPath}");
            }
            else
            {
                options.UseSqlite(_config.Connect.ConnectionString);
            }
        }

        public MetaAppContextDB(IOptions<ServerConfigModel> set_config)
        {
            //Environment.SpecialFolder spec_folder = Environment.SpecialFolder.LocalApplicationData;
            string? spec_path = AppDomain.CurrentDomain.BaseDirectory;

            _config = set_config.Value.DatabaseConfig;
            if (string.IsNullOrWhiteSpace(_config.Connect.SqLiteFileName))
            {
                DbPath = Path.Join(spec_path, string.IsNullOrWhiteSpace(DbFileName) ? "designer_meta_app.db" : DbFileName);
            }
            else
            {
                DbPath = Path.Combine(spec_path, _config.Connect.SqLiteFileName);
            }

            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupUserModelDB>().HasData(
                    new GroupUserModelDB(1, "Test group 1", "Description group 1"),
                    new GroupUserModelDB(2, "Test group 2", "Description group 2"),
                    new GroupUserModelDB(3, "Test group 3", "Description group 3")
            );

            modelBuilder.Entity<ProjectModelDB>().HasData(
                    new ProjectModelDB(1, "Demo project 1", "Description project 1"),
                    new ProjectModelDB(2, "Demo project 2", "Description project 2"),
                    new ProjectModelDB(3, "Demo project 3", "Description project 3")
            );

            modelBuilder.Entity<UserModelDB>().HasData(
                    new UserModelDB(1, "Tom", AccessLevelsUsersEnum.Blocked) { Login = "tom", PasswordHash = GlobalUtils.CalculateHashString("ffsgfgggragf"), Email = "tom@mail.ru" },
                    new UserModelDB(2, "Bob", AccessLevelsUsersEnum.Verified) { Login = "bobb", PasswordHash = GlobalUtils.CalculateHashString("gsdfghjg"), Email = "bobb@mail.ru" },
                    new UserModelDB(3, "Sam", AccessLevelsUsersEnum.Trusted) { Login = "samuel", PasswordHash = GlobalUtils.CalculateHashString("hdg6hw46s"), Email = "samuel@mail.ru" },
                    new UserModelDB(4, "Kelly", AccessLevelsUsersEnum.Manager) { Login = "kiki", PasswordHash = GlobalUtils.CalculateHashString("dh6jwk45"), Email = "kiki@mail.ru" },
                    new UserModelDB(5, "David", AccessLevelsUsersEnum.Admin) { Login = "diablo", PasswordHash = GlobalUtils.CalculateHashString("dfgh6qeh"), Email = "diablo@mail.ru" },
                    new UserModelDB(6, "Rokki", AccessLevelsUsersEnum.ROOT) { Login = "ronin", PasswordHash = GlobalUtils.CalculateHashString("ghyh356ust"), Email = "ronin@mail.ru" }
            );
        }

        public DbSet<UserModelDB> Users { get; set; }

        public DbSet<GroupUserModelDB> GroupsUsers { get; set; }

        public DbSet<ProjectModelDB> Projects { get; set; }
    }
}
