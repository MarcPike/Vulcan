using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.FileAttachment;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Helpers;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DAL.Vulcan.NUnit.Tests.Quote_Tests
{
    [TestFixture()]
    public class QuoteInspector
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void ExamineQuotePrice()
        {

            var quote = CrmQuote.Helper.Find(x => x.QuoteId == 140059).FirstOrDefault();
            var model = new QuoteModel("","",quote);


            foreach (var quoteItemModel in model.Items)
            {
                foreach (var fileAttachmentModel in quoteItemModel.FileAttachments)
                {
                    FileAttachmentsVulcan.Remove(ObjectId.Parse(fileAttachmentModel.Id));

                }
                

            }


        }

    }
}
