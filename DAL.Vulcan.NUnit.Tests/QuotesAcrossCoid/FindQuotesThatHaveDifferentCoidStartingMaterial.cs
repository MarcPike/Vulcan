using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Test;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.QuotesAcrossCoid
{
    [TestFixture]
    public class FindQuotesThatHaveDifferentCoidStartingMaterial
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
        }

        [Test]
        public void Execute()
        {
            var quotes = new RepositoryBase<CrmQuote>().AsQueryable().Where(x =>
                x.Items.Any(i => i.IsQuickQuoteItem == false && i.AsQuoteItem().QuotePrice.StartingProduct.Coid != x.Coid)).ToList();
            foreach (var crmQuote in quotes)
            {
                Console.WriteLine(ObjectDumper.Dump(crmQuote));
            }
        }
    }
}
