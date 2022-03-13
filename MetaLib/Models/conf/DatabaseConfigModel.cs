﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class DatabaseConfigModel
    {
        public ConnectionStringModel Connect { get; set; } = new ConnectionStringModel();

        /// <summary>
        /// #if DEBUG protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(message => Debug.WriteLine(message));
        /// </summary>
        public bool SqlLogDebug { get; set; } = true;

        /// <summary>
        /// #if DEBUG protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.EnableSensitiveDataLogging(true);
        /// </summary>
        public bool EnableSensitiveDataLoggingDebug { get; set; } = true;
    }
}