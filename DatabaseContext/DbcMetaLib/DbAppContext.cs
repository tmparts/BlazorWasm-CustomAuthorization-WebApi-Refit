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
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!IsEnsureCreated)
            {
                IsEnsureCreated = true;
                string prefix = "Data Source=";
                string DbPath = _config.Connect.ConnectionString;

                DbPath = DbPath.Substring(prefix.Length).Trim();
                if (DbPath.EndsWith(";"))
                {
                    DbPath = DbPath.Substring(0, DbPath.Length - 1);
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
            options.UseSqlite(_config.Connect.ConnectionString);
        }

        public DbAppContext(IOptions<ServerConfigModel> set_config) : base(set_config)
        {
            string? spec_path = AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
