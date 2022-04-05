﻿# .NET6 BlazorWasm custom authorization + WebApi
Кастомная авторизация Blazor WebAssembly в связке с WebApi (через Refit) в т.ч. вертикальная иерархия прав (сквозная между UI и API).

Иерархия/уровни прав формируется перечислением:

```c#
public enum AccessLevelsUsersEnum
{
    Anonim = -20,

    /// <summary>
    /// Заблокирован
    /// </summary>
    [Display(Name = "Заблокирован", Description = "Заблокирован")]
    Blocked = -10,

    /// <summary>
    /// Зарегистрированный (но НЕ подтверждённый)
    /// </summary>
    [Display(Name = "Зарегистрированый", Description = "Рядовой зарегистрированный пользователь (не подтверждённый)")]
    Auth = 10,

    /// <summary>
    /// Подтверждён (подтвердил по Email и/или Telegram)
    /// </summary>
    [Display(Name = "Проверенный", Description = "Подтверждённый пользователь (подтвердил по Email и/или Telegram)")]
    Confirmed = 20,

    /// <summary>
    /// Привилегированный
    /// </summary>
    [Display(Name = "Привилегированный", Description = "Особые разрешения, но не администрация")]
    Trusted = 30,

    /// <summary>
    /// 4.Менеджер (управляющий/модератор)
    /// </summary>
    [Display(Name = "Менеджер/Модератор", Description = "Младший администратор")]
    Manager = 40,

    /// <summary>
    /// Администратор
    /// </summary>
    Admin = 50,

    /// <summary>
    /// Владелец (суперпользователь)
    /// </summary>
    [Display(Name = "ROOT/Суперпользователь")]
    ROOT = 60
}
```
> в готовой иерархии индексы специально раряженые, что бы в случае необходимости было возможно добавить дополнительные/промежуточные уровни без нарушения ссылочной/логической целостности данных пользователей в БД.

Смысл данной иерархии такой, что на контроллер либо его метод (а в Razor политика) устанавливается минимальный требуемый уровень доступа при помощи специального атрибута фильтра авторизации,

```c#
/// <summary>
/// Получить профиль пользователя
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
[HttpGet("{id}")]
[TypeFilter(typeof(AuthAsyncFilterAttribute), Arguments = new object[] { AccessLevelsUsersEnum.Confirmed })]
public async Task<GetUserProfileResponseModel> Get([FromRoute] int id)
{
```

пользователь в свою очередь имеет того же типа поле. Вот по нему и проходит проверка уровня доступа.

```c#
public AccessLevelsUsersEnum AccessLevelUser { get; set; } = AccessLevelsUsersEnum.Anonim;
```

а на стороне Blazor клиента реализован схожий механизм, но силами политик
```razor
<AuthorizeView Policy="MinimumLevelConfirmed">
            <Authorized>
```

Имя политики формируется из ```MinimumLevel``` и имени уровня доступа. Например: ```MinimumLevelConfirmed``` или ```MinimumLevelManager```

для поддержки данного хозяйства на клиенте Blazor (wasm) регистрируется требуемая оснаска:

```c#
builder.Services.InitAccessMinLevelHandler();
```

```c#
public static void InitAccessMinLevelHandler(this IServiceCollection services)
{
    services.AddSingleton<IAuthorizationPolicyProvider, MinimumLevelPolicyProvider>();
    services.AddSingleton<IAuthorizationHandler, MinimumLevelAuthorizationHandler>();

    services.AddAuthorizationCore(opts =>
    {
        opts.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });
}
```

а так же

```c#
builder.Services.AddScoped<CustomAuthStateProvider>();
```

на серверной стороне регистрировать ни чего не требуется. Фильтр выполняет всё что требуется

```c#
public class AuthAsyncFilterAttribute : Attribute, IAuthorizationFilter
{
    ISessionService _session_service;
    AccessLevelsUsersEnum _minimum_access_level;
    public AuthAsyncFilterAttribute(ISessionService set_session_service, AccessLevelsUsersEnum set_minimum_access_level)//
    {
        _session_service = set_session_service;
        _minimum_access_level = set_minimum_access_level;
    }

    void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
    {
        if (_minimum_access_level > _session_service.SessionMarker.AccessLevelUser)
        {
            context.Result = new ObjectResult(new ResponseBaseModel() { IsSuccess = false, Message = "Не достаточно прав для доступа к ресурсу" });
        }
    }
}
```

В случае неудачи прохождения контроля доступа - возвращается базовый класс от которого, кстати говоря, наследуются все ответы всех контроллеров (по крайней мере те, которые подразумевают ответ).
```c#
ResponseBaseModel()
{
    IsSuccess = false,
    Message = "Не достаточно прав для доступа к ресурсу" 
}
``` 

Сессии сейчас хранятся в Redis. Это значит, что на сервере требуется данный демон. В конфигах по умолчанию дефлоные настройки, но можно прописать и другие.

```json
"RedisConfig": {
    "ResolveDns": false,
    "Password": "",
    "User": "",
    "KeepAlive": 5,
    "HighPrioritySocketThreads": false,
    "EndPoint": "localhost:6379",
    "AbortOnConnectFail": false,
    "ConnectTimeout": 10000,
    "ConfigurationChannel": "",
    "ConnectRetry": 5,
    "ClientName": "",
    "AsyncTimeout": 10000,
    "AllowAdmin": true,
    "Ssl": false,
    "SslHost": "",
    "SyncTimeout": 10000
  }
```

База данных в данный момент используется SQLite, но я стараюсь разделять сервисы таким образом, что бы перевод на другую бд был простым и безболезненным.

###### Базовый функционал системы авторизации/регистрации
- Регистрация, вход, выход.
- Подтверждение регистрации (по Email пользователя). 
- Сброс/изменение пароля (восстановление доступа по Email).
- reCaptcha v2 (в т.ч. Invisible)
- сессия от Blazor Wasm к WEB Api пробрасывется через Refit/HTTP Header. На стороне браузера сессия хранится в Storage.

На старте работает с Redis, но легко переводится на другие рельсы через реализацию интерфейса
```c#
builder.Services.AddScoped<IMemoryCashe, RedisMemoryCasheService>();
```

сессии целиком обрабатываются в
```c#
builder.Services.AddScoped<ISessionService, SessionService>();
```
Отныне сессии я буду хранить где угодно, хоть в бд, хоть Mongo хоть в записной книжке.

P.S.
Да простит меня великий и могучий IT за этот велосипед, но инструмент годный.