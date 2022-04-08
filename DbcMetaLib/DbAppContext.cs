////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MetaLib;

namespace SrvMetaApp
{
    public class DbAppContext : LayerContext
    {
        public string DbPath { get; protected set; } = string.Empty;
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

        public DbAppContext(IOptions<ServerConfigModel> set_config) : base(set_config)
        {
            string? spec_path = AppDomain.CurrentDomain.BaseDirectory;
            DbPath = Path.Combine(spec_path, _config.Connect.DatabaseFileName);
        }
    }
}
