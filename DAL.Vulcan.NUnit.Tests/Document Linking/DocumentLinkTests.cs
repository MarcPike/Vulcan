using System.Diagnostics;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Currency;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Test;
using NUnit.Framework;

namespace DAL.Vulcan.NUnit.Tests.Document_Linking
{
    [TestFixture]
    public class DocumentLinkTests
    {
        /*
         * Every BaseDocument can have a List<Link> of any other BaseDocument(s)
         * 
         * =======================================================
         * Methods for Linking are included on every BaseDocument:
         * =======================================================
         * CreateLink(BaseDocument attachDoc)
         * RemoveLink(BaseDocument attachedDoc)
         * RemoveAllLinks()
         * RemoveAllLinksForType(PrimaryObjectType type)
         * List<Link> GetAllLinks()
         * List<Link> GetAllLinksForType(string typeFullName)
         * 
         * ===================================================================================
         * To turn the links into their original BaseDocument you have to use the LinkResolver
         * ===================================================================================
         * 
         * Example:
         * var myLinkedCurrencies = new LinkResolver<CurrencyType>(myLocation).GetAllLinkedDocuments();
         * 
         * The example above will give you any Linked CurrencyType documents associated with a location
         * 
         * 
         */


        [Test]
        public void CreateLinks()
        {
            var repLocations = new RepositoryBase<Location>();
            var repCurrencyTypes = new RepositoryBase<CurrencyType>();


            var myLocation = repLocations.AsQueryable().First();

            var allCurrencies = repCurrencyTypes.AsQueryable().ToList();
            foreach (var currencyType in allCurrencies)
            {
                myLocation.CreateLink(currencyType);
            }

            var linkedCurrencyTypes = new LinkResolver<CurrencyType>(myLocation).GetAllLinkedDocuments();

            foreach (var ct in linkedCurrencyTypes)
            {
                Trace.WriteLine(ObjectDumper.Dump(ct));
            }


            Assert.AreEqual(linkedCurrencyTypes.Count, allCurrencies.Count);

            repLocations.Upsert(myLocation);

        }

        [Test]
        public void RemoveLinks()
        {

            var repLocations = new RepositoryBase<Location>();
            var myLocation = repLocations.AsQueryable().First();

            myLocation.RemoveAllLinksForType(typeof(CurrencyType));
            repLocations.Upsert(myLocation);

        }

        [Test]
        public void TestRemoveAllLinks()
        {

            var repLocations = new RepositoryBase<Location>();
            var repCurrencyTypes = new RepositoryBase<CurrencyType>();


            var myLocation = repLocations.AsQueryable().First();

            var allCurrencies = repCurrencyTypes.AsQueryable().ToList();
            foreach (var currencyType in allCurrencies)
            {
                myLocation.CreateLink(currencyType);
            }

            myLocation.RemoveAllLinks();

        }
    }
}
