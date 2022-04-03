////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MetaLib.MemCash;

namespace SrvMetaApp.Repositories.mail
{
    public class MailService : IMailServiceInterface
    {
        readonly IHttpContextAccessor _http_context;
        readonly ILogger<MailService> _logger;
        readonly IOptions<ServerConfigModel> _config;
        readonly IMemoryCashe _mem_cashe;

        public MailService(IHttpContextAccessor set_http_context, ILogger<MailService> set_logger, IOptions<ServerConfigModel> set_config, IMemoryCashe set_mem_cashe)
        {
            _http_context = set_http_context;
            _logger = set_logger;
            _config = set_config;
            _mem_cashe = set_mem_cashe;
        }

        public async Task<bool> SendUserConfirmationEmail(ConfirmationModelDb confirm_db)
        {
            string subject, message;

            switch (confirm_db.ConfirmationType)
            {
                case MetaLib.Models.enums.ConfirmationsTypesEnum.RegistrationUser:
                    subject = $"Подтверждение регистрации: {_config.Value.ClientConfig.Host}";
                    message = $"Доброго времени суток, {confirm_db.User.Name}. Вы зарегистрировались в системе. Ваш логин '{confirm_db.User.Login}'. Для подтверждения этого действия и активации акаунта, перейдите по ссылке: <a href='{_config.Value.ApiConfig.GetFullUrl($"mvc/ConfirmView?confirm_id={confirm_db.GuidConfirmation}")}'>подтвердить</a>.";

                    break;
                case MetaLib.Models.enums.ConfirmationsTypesEnum.RestoreUser:
                    subject = $"Восстановление доступа к учётной записи. {_config.Value.ClientConfig.Host}";
                    message = $"Доброго времени суток, {confirm_db.User.Name}. Мы получили запрос на восстановление доступа к вашей учётной записи. Напоминаем вам, что ваш логин '{confirm_db.User.Login}'. Для сброса пароля перейдите по ссылке: <a href='{_config.Value.ApiConfig.GetFullUrl($"mvc/ConfirmView?confirm_id={confirm_db.GuidConfirmation}")}'>создать новый пароль</a>.";

                    break;
                default:
                    _logger.LogError($"Ошибка отправки Email подтверждения '{confirm_db.GuidConfirmation}'. Тип подвтерждения '{confirm_db.ConfirmationType}' не определён", nameof(confirm_db.ConfirmationType));
                    return false;
            }
            try
            {
                await SendEmailAsync(confirm_db.User.Email, subject, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка отправки Email подтверждения '{confirm_db.GuidConfirmation}'.");
                return false;
            }

            return true;
        }

        public async Task SendEmailAsync(string email, string subject, string message, MimeKit.Text.TextFormat format = MimeKit.Text.TextFormat.Html)
        {
            MimeMessage? emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_config.Value.SmtpConfig.PublicName, _config.Value.SmtpConfig.Email));
            emailMessage.To.Add(new MailboxAddress(string.Empty, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(format)
            {
                Text = message
            };

            using SmtpClient? client = new SmtpClient();
            await client.ConnectAsync(_config.Value.SmtpConfig.Host, _config.Value.SmtpConfig.Port, _config.Value.SmtpConfig.UseSsl);
            await client.AuthenticateAsync(_config.Value.SmtpConfig.Login, _config.Value.SmtpConfig.Password);
            string? res = await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
