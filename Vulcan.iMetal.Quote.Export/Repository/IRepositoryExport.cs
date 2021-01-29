using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace Vulcan.iMetal.Quote.Export.Repository
{
    public interface IRepositoryExport
    {
        string CheckStatusOfQuote(CrmQuote quote);
        bool ExportCrmQuote(CrmQuote quote);
    }
}