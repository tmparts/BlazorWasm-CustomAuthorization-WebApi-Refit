////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using DbLayerLib;

namespace DbcLib
{
    /// <summary>
    /// Контекст доступа к SQLite БД
    /// </summary>
    public class DbAppContext : LayerContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
#if DEBUG
            options.LogTo(Console.WriteLine);
#endif

            if (!IsEnsureCreated)
            {
                IsEnsureCreated = true;
                string prefix = "Data Source=";
                string DbPath = _config.Connect.ConnectionString;

                DbPath = DbPath[prefix.Length..].Trim();
                if (DbPath.EndsWith(";"))
                {
                    DbPath = DbPath[0..^1];
                }
                FileInfo? fi = new FileInfo(DbPath);
#if DEBUG
                if (fi.Exists)
                {
                    File.Delete(DbPath);
                }
#endif   
                _config.Connect.ConnectionString = $"{prefix}{fi.FullName}";
            }
            options
#if DEBUG
                .EnableSensitiveDataLogging()
#endif
                .UseSqlite(_config.Connect.ConnectionString);
        }

        public DbAppContext(IOptions<ServerConfigModel> set_config) : base(set_config)
        {
            string? spec_path = AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
