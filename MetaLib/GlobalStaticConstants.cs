////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

public static class GlobalStaticConstants
{
    #region имена полей в storage браузера для хранения информации о сессии
    
    /// <summary>
    /// имя поля в storage браузера для хранения информации о 'логине' сессии
    /// </summary>
    public const string SESSION_STORAGE_KEY_LOGIN = "session_login";
    
    /// <summary>
    /// имя поля в storage браузера для хранения информации о 'уровне доступа' сессии
    /// </summary>
    public const string SESSION_STORAGE_KEY_LEVEL = "session_level";
    
    /// <summary>
    /// имя поля в storage браузера для хранения информации о 'токене' сессии
    /// </summary>
    public const string SESSION_STORAGE_KEY_TOKEN = "session_token";
    
    #endregion

    /// <summary>
    /// Имя заголовка для пердачи токена от клиента к серверу
    /// </summary>
    public const string SESSION_TOKEN_NAME = "token";
    
    #region имена контроллеров и действий

    public const string LOGOUT_ACTION_NAME = "logout";
    public const string LOGIN_ACTION_NAME = "login";
    public const string RESTORE_ACTION_NAME = "restore";
    public const string REGISTRATION_ACTION_NAME = "registration";

    public const string AUTHENTICATION_CONTROLLER_NAME = "authentication";

    #endregion
}