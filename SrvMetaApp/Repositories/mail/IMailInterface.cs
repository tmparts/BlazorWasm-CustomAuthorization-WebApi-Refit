////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;

namespace SrvMetaApp.Repositories
{
    public interface IMailInterface
    {
        public Task<bool> SendEmailRestoreUser(UserModelDB user);

        public Task SendEmailAsync(string email, string subject, string message, MimeKit.Text.TextFormat format);
    }    
}
