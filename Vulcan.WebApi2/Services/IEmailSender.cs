using System.Threading.Tasks;

namespace Vulcan.WebApi.Controllers
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}