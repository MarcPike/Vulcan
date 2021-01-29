namespace Vulcan.WebApi2.Helpers
{
    public interface ILdapHelper
    {
        string Domain { get; }
        string Host { get; }
        int Port { get; }
    }
}