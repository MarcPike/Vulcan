namespace Vulcan.SVC.WebApi.Core.Helpers
{
    public interface ILdapHelper
    {
        string Domain { get; }
        string Host { get; }
        int Port { get; }
    }
}