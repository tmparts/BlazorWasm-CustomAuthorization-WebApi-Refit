////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Models
{
    public class ConnectionStringModel
    {
        public string SqLiteFileName { get; set; } = "sqlite_database.db";
#if DEBUG
        public string ConnectionString { get; set; } = $"Data Source=.{Path.DirectorySeparatorChar}designer_meta_app.db";
#else
        public string ConnectionString { get; set; } = string.Empty;
#endif
    }
}
