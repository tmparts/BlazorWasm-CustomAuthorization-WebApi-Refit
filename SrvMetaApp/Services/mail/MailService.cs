////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace SrvMetaApp.Repositories.mail
{
    public class MailService : IMailServiceInterface
    {
        readonly IHttpContextAccessor _http_context;
        readonly ILogger<MailService> _logger;
        readonly IOptions<ServerConfigModel> _config;
        readonly RedisUtil _redis;
        readonly MetaAppContextDB _db_context;

        public MailService(IHttpContextAccessor set_http_context, ILogger<MailService> set_logger, IOptions<ServerConfigModel> set_config, RedisUtil set_redis, MetaAppContextDB set_db_context)
        {
            _http_context = set_http_context;
            _logger = set_logger;
            _config = set_config;
            _redis = set_redis;
            _db_context = set_db_context;
        }

        public async Task<bool> SendEmailRestoreUser(ConfirmationModelDb confirm_db)
        {
            if (_config.Value.SmtpConfig.IsEmptyConfig())
                return false;

            try
            {
                string subject = "Восстановление доступа к учётной записи. IQ-S.pro";
                string message = $"Доброго времени суток, {confirm_db.User.Name} {confirm_db.User.LastName}. Мы получили запрос на восстановление доступа к вашей учётной записи. Напоминаем вам, что ваш логин '{confirm_db.User.Login}'. Для сброса пароля перейдите по ссылке: <a href='{_config.Value.ApiConfig.GetFullUrl($"mvc/ConfirmView?confirm_id={confirm_db.Guid}")}'>создать новый пароль</a>.";
                await SendEmailAsync(confirm_db.User.Email, subject, message);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error - {nameof(SendEmailRestoreUser)}");
                return false;
            }
        }

        public async Task<bool> SendEmailRegistrationUser(ConfirmationModelDb confirm_db)
        {
            if (_config.Value.SmtpConfig.IsEmptyConfig())
                return false;

            try
            {
                string subject = "Подтверждение регистрации: iq-s.pro";
                string message = $"Доброго времени суток, {confirm_db.User.Name} {confirm_db.User.LastName}. Вы зарегистрировались в системе. Ваш логин '{confirm_db.User.Login}'. Для подтверждения перейдите по ссылке: <a href='{_config.Value.ApiConfig.GetFullUrl($"mvc/ConfirmView?confirm_id={confirm_db.Guid}")}'>подтвердить</a>.";
                await SendEmailAsync(confirm_db.User.Email, subject, message);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error - {nameof(SendEmailRegistrationUser)}");
                return false;
            }
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
            var res = await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}
