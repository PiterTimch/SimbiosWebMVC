using SimbiosWebMVC.SMTP;

namespace SimbiosWebMVC.Interfaces
{
    public interface ISMTPService
    {
        Task<bool> SendEmailAsync(Message message);
    }
}
