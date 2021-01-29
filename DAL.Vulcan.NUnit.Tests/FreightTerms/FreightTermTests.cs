using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.FreightTerms
{
    [TestFixture]
    class FreightTermTests
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmDevelopment();
        }

        [Test]
        public void GetAllFreightTerms()
        {
            var coids = "DUB,EUR,CAN,SIN,MSA".Split(',').ToList();

            foreach (var coid in coids)
            {

                var terms = DAL.Vulcan.Mongo.DocClass.Quotes.FreightTerms.GetFreightTermsForCoid(coid);
                Console.WriteLine($"FreightTerms for {coid} Count: {terms.Count}");
                foreach (var term in terms)
                {
                    Console.WriteLine($"  {term.Coid} {term.Code} - {term.Terms}");
                }
            }

            Console.WriteLine("");
        }
    }
}
