namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperLdap
    {
        string Domain { get; }
        string Host { get; }
        int Port { get; }
    }
}