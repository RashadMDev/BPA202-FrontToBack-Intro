
using ProniaFrontToBack.Utilities.Email;

namespace ProniaFrontToBack.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}