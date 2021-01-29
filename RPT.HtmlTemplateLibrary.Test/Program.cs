using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Driver;

namespace RPT.HtmlTemplateLibrary.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            EnvironmentSettings.CrmDevelopment();

            var quoteId = 16692;

            var quoteHelper = new MongoRawQueryHelper<CrmQuote>();

            var quote = quoteHelper.Find(x => x.QuoteId == quoteId).FirstOrDefault();

            if (quote == null)
            {
                Console.WriteLine("Quote not found");
                return;
            }


            if (quote.Contact == null) throw new Exception("This Quote is missing a Contact");

            string contactName;
            string contactEmail;
            string contactPhone;

            var contactHelper = new MongoRawQueryHelper<Contact>();

            var contact = contactHelper.FindById(quote.Contact.Id);
            if (contact != null)
            {
                contactName = contact.Person.FirstName + " " + contact.Person.LastName;
                contactEmail = contact.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business)?.Address ?? "";
                contactPhone = contact.Person.PhoneNumbers.FirstOrDefault(x => x.Type == PhoneType.Office)?.Number ?? "";
            }
            else
            {
                contactName = "<None>";
                contactEmail = "<None>";
                contactPhone = "<None>";
            }

            var htmlGenerator = CrmQuoteHtmlGenerator.GetQuoteHtmlGenerator(quote, contactName, contactEmail, contactPhone);
            Console.WriteLine(ObjectDumper.Dump(htmlGenerator));

            Console.WriteLine("Press any key");
            Console.ReadLine();

        }
    }
}
