using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Test;
using NUnit.Framework;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.PartNumberQueries
{
    [TestFixture]
    public class PartNumberSearch
    {
        [Test]
        public void GetPartNumbers()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<CrmQuoteItem>();
            var quoteItems = rep.AsQueryable().Where(x => x.PartNumber != null && x.PartNumber != string.Empty && x.QuotePrice != null).ToList();

            var partNumbers = quoteItems.GroupBy(info =>
                new {
                    info.PartNumber, info.QuotePrice.StartingProduct.ProductCode, info.QuotePrice.StartingProduct.ProductId
                })
                .Select(group => new {group.Key.PartNumber, group.Key.ProductCode, group.Key.ProductId, Count = group.Count()}).OrderBy(x => x.PartNumber)
                .ToList();
            foreach (var partNumber in partNumbers)
            {
                Console.WriteLine($"PartNumber: {partNumber.PartNumber} Starting Product: {partNumber.ProductCode} => Counter: {partNumber.Count}");
            }
        }

        [Test]
        public void GetItemsForPartNumber()
        {
            var partNumber = "P150543";
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var quoteItems = new RepositoryBase<CrmQuoteItem>().AsQueryable().Where(x=>x.PartNumber == partNumber);
            foreach (var crmQuoteItem in quoteItems)
            {
                Console.WriteLine(ObjectDumper.Dump(crmQuoteItem));
            }

        }

        [Test]
        public void GetAllPartSpecs()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.QualityControl;
            var rep = new RepositoryBase<CrmQuoteItem>();
            var partSpecs = rep.AsQueryable().Select(x => new {x.PartNumber, x.PartSpecification}).Where(x=>x.PartNumber != String.Empty && x.PartSpecification != String.Empty).ToList();
            foreach (var partSpec in partSpecs)
            {
                Console.WriteLine(ObjectDumper.Dump(partSpec));
            }
        }
    }
}
