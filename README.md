# .NET6 BlazorWasm custom authorization + WebApi
Кастомная авторизация Blazor WebAssembly в связке с WebApi (через Refit) в т.ч. вертикальная иерархия прав (сквозная между UI и API) через стандартные политики .net [Authorize(Policy)]. 
Сессии на базе Redis. 
База данных SQLite. 
Регистрация, вход, выход. 
Подтверждение регистрации (Email пользователя). 
Сброс/изменение пароля (восстановление доступа по Email).

сессия от Blazor Wasm к WEB Api пробрасывется через Refit/HTTP Header

Планируется внедрение recaptcha
