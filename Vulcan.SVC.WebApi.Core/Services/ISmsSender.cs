using System.Threading.Tasks;

namespace Vulcan.SVC.WebApi.Core.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}