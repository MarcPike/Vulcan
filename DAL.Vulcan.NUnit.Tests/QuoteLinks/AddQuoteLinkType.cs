using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Bson;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.QuoteLinks
{
    [TestFixture]
    public class AddQuoteLinkType
    {

        [SetUp]
        public void SetUp()
        {
            //BsonDefaults.GuidRepresentation = 
            EnvironmentSettings.Database = MongoDatabase.VulcanCrm;
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            EnvironmentSettings.SecurityType = SecurityType.SalesPerson;
        }


        [Test]
        public void UpdateAllQuotes()
        {
            var rep = new RepositoryBase<CrmQuote>();
            var EmptyGuid = Guid.Empty;
            foreach (var crmQuote in rep.AsQueryable().ToList().Where(x=> !IsEmpty(x.QuoteLinkId)).ToList())
            {

                //if (crmQuote.QuoteLinkId != EmptyGuid)
                //if (!IsEmpty(crmQuote.QuoteLinkId))
                {
                    var originalQuoteId = rep.AsQueryable().Where(x => x.QuoteLinkId == crmQuote.QuoteLinkId)
                        .Min(x => x.QuoteId);
                    crmQuote.QuoteLinkType = crmQuote.QuoteId == originalQuoteId ? QuoteLinkType.Original : QuoteLinkType.Repeat;
                    rep.Upsert(crmQuote);
                }

            }

        }

        public bool IsEmpty(Guid g1)
        {
            var EmptyGuid = Guid.Empty;
            var result = g1.Equals(EmptyGuid);

            if (!result)
            {
                Console.WriteLine(result);
            }

            return result;
        }
    }
}
