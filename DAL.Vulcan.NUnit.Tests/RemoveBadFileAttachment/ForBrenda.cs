using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.FileAttachment;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.RemoveBadFileAttachment
{
    [TestFixture]
    public class ForBrenda
    {

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void FindAndRemoveAttachments()
        {
            var quote = CrmQuote.Helper.Find(x => x.QuoteId == 132225).FirstOrDefault();

            foreach (var item in quote.Items)
            {
                var crmQuoteItem = CrmQuoteItem.Helper.FindById(item.Id);

                var fileAttachments = FileAttachmentsVulcan.GetAllAttachmentsForDocument(crmQuoteItem).ToList();
                foreach (var fileAttachment in fileAttachments)
                {
                    Console.WriteLine($"Removing file: {fileAttachment.Filename}");
                    FileAttachmentsVulcan.Remove(fileAttachment.Id);
                }

            }

        }
    }
}
