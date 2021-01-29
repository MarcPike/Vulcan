using System.Threading.Tasks;

namespace Vulcan.SVC.WebApi.Core.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}