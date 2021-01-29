using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace Vulcan.iMetal.Quote.Export.Repository
{
    public class RepositoryExport : IRepositoryExport
    {
        public bool ExportCrmQuote(CrmQuote quote)
        {
            return false;
        }

        public string CheckStatusOfQuote(CrmQuote quote)
        {
            return "Not yet implemented";
        }
    }
}
