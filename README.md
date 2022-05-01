# .NET6 BlazorWasm custom authorization + WebApi
��������� ����������� Blazor WebAssembly � ������ � WebApi (����� Refit) � �.�. ������������ �������� ���� (�������� ����� UI � API).

��������/������ ���� ����������� �������������:

```c#
public enum AccessLevelsUsersEnum
{
    Anonim = -20,

    /// <summary>
    /// ������������
    /// </summary>
    [Display(Name = "������������", Description = "������������")]
    Blocked = -10,

    /// <summary>
    /// ������������������ (�� �� �������������)
    /// </summary>
    [Display(Name = "�����������������", Description = "������� ������������������ ������������ (�� �������������)")]
    Auth = 10,

    /// <summary>
    /// ���������� (���������� �� Email �/��� Telegram)
    /// </summary>
    [Display(Name = "�����������", Description = "������������� ������������ (���������� �� Email �/��� Telegram)")]
    Confirmed = 20,

    /// <summary>
    /// �����������������
    /// </summary>
    [Display(Name = "�����������������", Description = "������ ����������, �� �� �������������")]
    Trusted = 30,

    /// <summary>
    /// 4.�������� (�����������/���������)
    /// </summary>
    [Display(Name = "��������/���������", Description = "������� �������������")]
    Manager = 40,

    /// <summary>
    /// �������������
    /// </summary>
    Admin = 50,

    /// <summary>
    /// �������� (�����������������)
    /// </summary>
    [Display(Name = "ROOT/�����������������")]
    ROOT = 60
}
```
> � ������� �������� ������� ���������� ���������, ��� �� � ������ ������������� ���� �������� �������� ��������������/������������� ������ ��� ��������� ���������/���������� ����������� ������ ������������� � ��.

����� ������ �������� �����, ��� �� ���������� ���� ��� ����� (� � Razor ��������) ��������������� ����������� ��������� ������� ������� ��� ������ ������������ �������� ������� �����������,

```c#
/// <summary>
/// �������� ������� ������������
/// </summary>
/// <param name="id"></param>
/// <returns></returns>
[HttpGet("{id}")]
[TypeFilter(typeof(AuthAsyncFilterAttribute), Arguments = new object[] { AccessLevelsUsersEnum.Confirmed })]
public async Task<GetUserProfileResponseModel> Get([FromRoute] int id)
{
```

������������ � ���� ������� ����� ���� �� ���� ����. ��� �� ���� � �������� �������� ������ �������.

```c#
public AccessLevelsUsersEnum AccessLevelUser { get; set; } = AccessLevelsUsersEnum.Anonim;
```

� �� ������� Blazor ������� ���������� ������ ��������, �� ������ �������
```razor
<AuthorizeView Policy="MinimumLevelConfirmed">
            <Authorized>
```

��� �������� ����������� �� ```MinimumLevel``` � ����� ������ �������. ��������: ```MinimumLevelConfirmed``` ��� ```MinimumLevelManager```

��� ��������� ������� ��������� �� ������� Blazor (wasm) �������������� ��������� �������:

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

� ��� ��

```c#
builder.Services.AddScoped<CustomAuthStateProvider>();
```

�� ��������� ������� �������������� �� ���� �� ���������. ������ ��������� �� ��� ���������

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
            context.Result = new ObjectResult(new ResponseBaseModel() { IsSuccess = false, Message = "�� ���������� ���� ��� ������� � �������" });
        }
    }
}
```

� ������ ������� ����������� �������� ������� - ������������ ������� ����� �� ��������, ������ ������, ����������� ��� ������ ���� ������������ (�� ������� ���� ��, ������� ������������� �����).
```c#
ResponseBaseModel()
{
    IsSuccess = false,
    Message = "�� ���������� ���� ��� ������� � �������" 
}
``` 

������ �� ������� ������� ������ �������� � Redis. ��� ������, ��� �� ������� ��������� ������ �����. � �������� �� ��������� �������� ���������, �� ����� ��������� � ������.

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

> ���� ���� ������, �� ������ ������������� ��������, ������� ������ ���������� �� ������ Redis.
```c#
builder.Services.AddScoped<IManualMemoryCashe, RedisMemoryCasheService>();
```
���� ������ ����������� �� ������ �������, �� ������ ����� ���� � ���� ����� �����.

���� ������ � ������ ������ ������������ SQLite, �� � �������� ��������� ������� ����� �������, ��� �� ������� �� ������ �� ��� ������� � ��������������.
> ��������, ��� ������������ �� Postgree - ���������� ������� ������ ����������� � �������� **ApiMetaApp** � �������� ����������� � ���� ��������.

### ��� ������� ���� ��� ������������ �� ������ ����!
�� ����� ������ ����������� ��� ����: SQLite � Postgre. ��� ��������� ��� ��� ���� ���� ��������� ��� ��������:
 - ���������� ������ ����������� � �� � �������� ApiMetaApp. � ����������� �� �������� ���� �� ������ ��������� ����������� �������.
 ```json
 "DatabaseConfig": {
    "Connect": {
      "ConnectionString": "Host=localhost;Port=5432;Database=edb;Username=postgres;Password=555555555"
      //"ConnectionString": "Data Source=./bin/mydb.db;"
    },
    "SqlLogDebug": true,
    "EnableSensitiveDataLoggingDebug": true
  }
 ```
 - ��� ������� **DbTablesLib** ���������� ���� �� ������������ �� �������� �������. ���� ������� *SQLite*, �� ���������� ����������� �� ������� **DbSqliteLib**. � �� �� �����, ��������� ��� �� ������ ����������� �� ������ **DbPostgreLib**. ���� �� ������� *Postgre*, �� ������� (DbPostgreLib) ����������� ����������� �� **DbPostgreLib**, �� �� **DbSqliteLib** (����������� �� ���� ����� �� �����������)
 - ��� ������� **ServerLib** ���������� ����������� ��� �� � ��� ������� **DbTablesLib**

###### ������� ���������� ������� �����������/�����������
- �����������, ����, �����.
- ������������� ����������� (�� Email ������������). 
- �����/��������� ������ (�������������� ������� �� Email).
- reCaptcha v2 (� �.�. Invisible)
- ������ �� Blazor Wasm � WEB Api ������������� ����� Refit/HTTP Header. �� ������� �������� ������ �������� � Microsoft.Extensions.Caching.Memory.IMemoryCache.

�� ������ �������� � Redis, �� ����� ����������� �� ������ ������ ����� ���������� ����������
```c#
builder.Services.AddScoped<IManualMemoryCashe, RedisMemoryCasheService>();
```

������ ������� �������������� �
```c#
builder.Services.AddScoped<ISessionService, SessionService>();
```
������ ������ � ���� ������� ��� ������, ���� � ��, ���� Mongo ���� � �������� ������.

P.S.
�� ������� ���� ������� � ������� IT �� ���� ���������, �� ���������� ������.