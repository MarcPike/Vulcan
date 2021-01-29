using System.Threading.Tasks;

namespace HRS.WebApi2.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}