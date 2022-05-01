////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib
{
    /// <summary>
    /// Сессия (лёгкая модель)
    /// </summary>
    public class SessionServiceLiteModel
    {
        /// <summary>
        /// Токен сессии
        /// </summary>
        public string GuidToken { get; set; } = string.Empty;
    }
}
