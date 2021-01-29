using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Vulcan.Mongo.HtmlTemplates;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.Vulcan.NUnit.Tests.HtmlTemplateLoader
{
    [TestFixture]
    public class LoadHtmlTemplates
    {
        private readonly RepositoryBase<HtmlTemplate> _repository = new RepositoryBase<HtmlTemplate>();

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CurrentEnvironment = Environment.Development;
        }

        [Test]
        public void SaveTEmplate(string coid, string tEmplateName, string fileName)
        {
            var tEmplate = _repository.AsQueryable().SingleOrDefault(x => x.Name == tEmplateName && x.Coid == coid);
            if (tEmplate == null)
            {
                tEmplate = new HtmlTemplate(coid, tEmplateName,@fileName);
            }
            else
            {
                tEmplate.UpdateFromFile(fileName);
            }
            tEmplate.SaveToDatabase();

        }

        [Test]
        public void LoadAllTEmplates()
        {
            /*
            SaveTEmplate("CAN","CrmQuote", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteTEmplateForCAN.html");
            SaveTEmplate("CAN","CrmQuoteItem", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteItemTEmplateForCAN.html");
            SaveTEmplate("CAN","CrmQuoteCommentsHeader", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsHeaderTEmplateForCAN.html");
            SaveTEmplate("CAN","CrmQuoteComments", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsTEmplateForCAN.html");

            SaveTEmplate("DUB", "CrmQuote", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteTEmplateForDUB.html");
            SaveTEmplate("DUB", "CrmQuoteItem", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteItemTEmplateForDUB.html");
            SaveTEmplate("DUB", "CrmQuoteCommentsHeader", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsHeaderTEmplateForDUB.html");
            SaveTEmplate("DUB", "CrmQuoteComments", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsTEmplateForDUB.html");
            */
            SaveTEmplate("INC", "CrmQuote", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteTEmplateForInc.html");
            SaveTEmplate("INC", "CrmQuoteItem", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteItemTEmplateForInc.html");
            SaveTEmplate("INC", "CrmQuoteLineItem", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteLineItemTEmplateForInc.html");
            SaveTEmplate("INC", "CrmQuoteCommentsHeader", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsHeaderTEmplateForInc.html");
            SaveTEmplate("INC", "CrmQuoteComments", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsTEmplateForInc.html");
            SaveTEmplate("INC", "CrmQuoteRevisions", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteRevisionsTEmplateForInc.html");
            /*
            SaveTEmplate("EUR", "CrmQuote", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteTEmplateForEUR.html");
            SaveTEmplate("EUR", "CrmQuoteItem", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteItemTEmplateForEUR.html");
            SaveTEmplate("EUR", "CrmQuoteCommentsHeader", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsHeaderTEmplateForEUR.html");
            SaveTEmplate("EUR", "CrmQuoteComments", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsTEmplateForEUR.html");

            SaveTEmplate("MSA", "CrmQuote", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteTEmplateForMSA.html");
            SaveTEmplate("MSA", "CrmQuoteItem", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteItemTEmplateForMSA.html");
            SaveTEmplate("MSA", "CrmQuoteCommentsHeader", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsHeaderTEmplateForMSA.html");
            SaveTEmplate("MSA", "CrmQuoteComments", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsTEmplateForMSA.html");

            SaveTEmplate("SIN", "CrmQuote", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteTEmplateForSIN.html");
            SaveTEmplate("SIN", "CrmQuoteItem", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteItemTEmplateForSIN.html");
            SaveTEmplate("SIN", "CrmQuoteCommentsHeader", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsHeaderTEmplateForSIN.html");
            SaveTEmplate("SIN", "CrmQuoteComments", @"E:\Code\Vulcan\DAL.Vulcan.Mongo\HtmlTEmplates\CrmQuoteCommentsTEmplateForSIN.html");
            */
        }

        [Test]
        public void CheckAllTEmplates()
        {
            var tEmplate = _repository.AsQueryable().Single(x => x.Name == "CrmQuote");
            Console.Write(tEmplate.Html);
            tEmplate = _repository.AsQueryable().Single(x => x.Name == "CrmQuoteItem");
            Console.Write(tEmplate.Html);
            tEmplate = _repository.AsQueryable().Single(x => x.Name == "CrmQuoteCommentsHeader");
            Console.Write(tEmplate.Html);
            tEmplate = _repository.AsQueryable().Single(x => x.Name == "CrmQuoteComments");
            Console.Write(tEmplate.Html);
        }

    }
}
