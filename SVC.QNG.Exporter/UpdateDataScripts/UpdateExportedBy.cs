using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SVC.QNG.Exporter.Models;

namespace SVC.QNG.Exporter.UpdateDataScripts
{
    [TestFixture()]
    public class UpdateExportedBy
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            var quotesToUpdate = new RepositoryBase<CrmQuote>().AsQueryable().Where(x => x.ExportRequestedBy != null)
                .ToList();
            using (var context = new ODSContext())
            {
                foreach (var crmQuote in quotesToUpdate)
                {
                    var quoteFound = context.Vulcan_CrmQuote.FirstOrDefault(x => x.QuoteId == crmQuote.QuoteId);
                    if (quoteFound == null)
                    {
                        Console.WriteLine($"{crmQuote.QuoteId} was not found");
                        continue;
                    }

                    quoteFound.ExportRequestedBy = crmQuote.ExportRequestedBy.FullName;
                }

                context.SaveChanges();

            }

        }

        [Test]
        public void PeekAtData()
        {
            var quotesToView = new RepositoryBase<CrmQuote>().AsQueryable().
                    Where(x => x.ExportStatus == ExportStatus.Success && x.ExportRequestedBy != null).ToList()
                .ToList();
            foreach (var crmQuote in quotesToView)
            {
                if (crmQuote.ExportRequestedBy.Id != crmQuote.SalesPerson.Id)
                {
                    Console.WriteLine($"{crmQuote.QuoteId} - Exported By: {crmQuote.ExportRequestedBy.FullName} : SalesPerson: {crmQuote.SalesPerson.FullName}");
                }
            }

        }
    }
}
