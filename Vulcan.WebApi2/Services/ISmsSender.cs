using System.Threading.Tasks;

namespace Vulcan.WebApi.Controllers
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}