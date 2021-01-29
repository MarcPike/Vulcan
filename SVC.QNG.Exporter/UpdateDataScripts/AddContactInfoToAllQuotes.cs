using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using MongoDB.Driver;
using NUnit.Framework;
using SVC.QNG.Exporter.Models;

namespace SVC.QNG.Exporter.UpdateDataScripts
{
    [TestFixture]
    public class AddContactInfoToAllQuotes
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.CrmProduction();
        }

        [Test]
        public void Execute()
        {
            using (var context = new ODSContext())
            {


                var allQuotesWithContacts = CrmQuote.Helper.Find(x => x.Contact != null).ToList();
                var onRow = 0;
                foreach (var crmQuote in allQuotesWithContacts)
                {

                    var odsQuote = context.Vulcan_CrmQuote.FirstOrDefault(x => x.QuoteId == crmQuote.QuoteId);
                    if ((odsQuote != null) && (odsQuote.ContactFirstName != crmQuote.Contact.FirstName))
                    {
                        var contact = crmQuote.Contact.AsContact();
                        odsQuote.ContactFirstName = LeftString(contact.Person.FirstName,30);
                        odsQuote.ContactLastName = LeftString(contact.Person.LastName,30);
                        odsQuote.ContactMiddleName = LeftString(contact.Person.MiddleName,30);
                        odsQuote.ContactEmailAddress = LeftString(contact.Person.EmailAddresses.FirstOrDefault()?.Address,50);
                        odsQuote.ContactPhoneNumber = LeftString(contact.Person.PhoneNumbers.FirstOrDefault()?.Number,30);
                        context.SaveChanges();
                    }

                    onRow++;
                }
            }
        }

        public string LeftString(string value, int length)
        {
            if (value == null) return String.Empty;

            if (value.Length > length) return value.Substring(0, length);

            return value;

        }

    }
}
