# .NET6 BlazorWasm custom authorization + WebApi
Кастомная авторизация Blazor WebAssembly в связке с WebApi (через Refit) в т.ч. вертикальная иерархия прав (сквозная между UI и API) через стандартные политики .net. Сессии на базе Redis. База данных SQLite. Регистрация, вход, выход.

сессия от Blazor Wasm к WEB Api пробрасывется через HTTP Header - https://github.com/badhitman/BlazorWasm-CustomAuthorization-WebApi/blob/6a269f7435a0dac56c207d7b988d4ede851a89e9/WebMetaApp/Program.cs#L44

Планируется внедрение recaptcha
