////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

/// <summary>
/// Константы
/// </summary>
public static class GlobalStaticConstants
{
    #region имена полей для хранения информации о сессии

    /// <summary>
    /// имя поля в storage браузера для хранения информации о 'id пользователя' сессии
    /// </summary>
    public const string SESSION_STORAGE_KEY_USER_ID = "session_user_id";

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

    public const string PROFILE_ACTION_NAME = "profile";
    public const string LOGOUT_ACTION_NAME = "logout";
    public const string LOGIN_ACTION_NAME = "login";
    public const string RESTORE_ACTION_NAME = "restore";
    public const string REGISTRATION_ACTION_NAME = "registration";

    public const string AUTHENTICATION_CONTROLLER_NAME = "authentication";
    public const string USERS_CONTROLLER_NAME = "users";

    #endregion
}