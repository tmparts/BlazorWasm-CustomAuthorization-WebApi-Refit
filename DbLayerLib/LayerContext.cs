﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MetaLib;
using System.Data.Entity.Infrastructure;
using DbLayerLib;

namespace SrvMetaApp
{
    public class LayerContext : DbContext
    {
        protected DatabaseConfigModel _config;
        protected static bool IsEnsureCreated = false;



        public static string DbFileName { get; set; } = string.Empty;

        //        protected override void OnConfiguring(DbContextOptionsBuilder options)
        //        {
        //            if (string.IsNullOrWhiteSpace(_config.Connect.ConnectionString))
        //            {
        //#if DEBUG
        //                if (!IsEnsureCreated)
        //                {
        //                    IsEnsureCreated = true;
        //                    File.Delete(DbPath);
        //                }
        //#endif
        //                options.UseSqlite($"Data Source={DbPath}");
        //            }
        //            else
        //            {
        //                options.UseSqlite(_config.Connect.ConnectionString);
        //            }
        //        }

        public LayerContext(IOptions<ServerConfigModel> set_config)
        {
            _config = set_config.Value.DatabaseConfig;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroupModelDB>().HasData(
                    new UserGroupModelDB("Test group 1", "Description group 1") { Id = 1 },
                    new UserGroupModelDB("Test group 2", "Description group 2") { Id = 2 },
                    new UserGroupModelDB("Test group 3", "Description group 3") { Id = 3 }
            );

            modelBuilder.Entity<ProjectModelDB>().HasData(
                    new ProjectModelDB("Demo project 1", "Description project 1") { Id = 1 },
                    new ProjectModelDB("Demo project 2", "Description project 2") { Id = 2 },
                    new ProjectModelDB("Demo project 3", "Description project 3") { Id = 3 }
            );

            modelBuilder.Entity<UserModelDB>().HasData(
                    new UserModelDB("Tom", AccessLevelsUsersEnum.Blocked) { Id = 1, Login = "tom", PasswordHash = GlobalUtils.CalculateHashString("ffsgfgggragf"), Email = "tom@mail.ru" },
                    new UserModelDB("Bob", AccessLevelsUsersEnum.Confirmed) { Id = 2, Login = "bobb", PasswordHash = GlobalUtils.CalculateHashString("gsdfghjg"), Email = "bobb@mail.ru" },
                    new UserModelDB("Sam", AccessLevelsUsersEnum.Trusted) { Id = 3, Login = "samuel", PasswordHash = GlobalUtils.CalculateHashString("hdg6hw46s"), Email = "samuel@mail.ru" },
                    new UserModelDB("Kelly", AccessLevelsUsersEnum.Manager) { Id = 4, Login = "kiki", PasswordHash = GlobalUtils.CalculateHashString("dh6jwk45"), Email = "kiki@mail.ru" },
                    new UserModelDB("David", AccessLevelsUsersEnum.Admin) { Id = 5, Login = "diablo", PasswordHash = GlobalUtils.CalculateHashString("dfgh6qeh"), Email = "diablo@mail.ru" },
                    new UserModelDB("Rokki", AccessLevelsUsersEnum.ROOT) { Id = 6, Login = "ronin", PasswordHash = GlobalUtils.CalculateHashString("ghyh356ust"), Email = "ronin@mail.ru" }
            );
        }

        public DbSet<ConfirmationModelDb> Confirmations { get; set; }

        public DbSet<UserModelDB> Users { get; set; }

        public DbSet<UserGroupModelDB> GroupsUsers { get; set; }
        public DbSet<UserToGroupLinkModelDb> UsersToGroupsLinks { get; set; }

        public DbSet<ProjectModelDB> Projects { get; set; }
        public DbSet<UserToProjectLinkModelDb> UsersToProjectsLinks { get; set; }
    }
}