namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public enum ExportStatus
    {
        NotExported,
        Pending,
        Processing,
        Failed,
        Success,
        Retry
    }
}