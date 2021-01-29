namespace DAL.Vulcan.Mongo.DocClass.Quotes
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