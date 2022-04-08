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
            options.UseNpgsql(_config.Connect.ConnectionString);
        }

        public DbAppContext(IOptions<ServerConfigModel> set_config) : base(set_config)
        {

        }
    }
}
