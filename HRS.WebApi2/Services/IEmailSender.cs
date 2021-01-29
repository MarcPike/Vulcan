using System.Threading.Tasks;

namespace HRS.WebApi2.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}