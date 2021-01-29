using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.EMail;
using DAL.Vulcan.Mongo.Base.Context;
using SVC.QuoteExpireNotification.Models;
using SVC.QuoteExpireNotification.Repository;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace SVC.QuoteExpireNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            EnvironmentSettings.CrmProduction();
            var quoteFinder = new FindQuotesExpiring();
            quoteFinder.Execute();
            var expiringQuotes = quoteFinder.ExpiringQuotes;

            var expiringQuoteModels = expiringQuotes.Select(x => new ExpiringQuoteEmailModel(x)).ToList();

            var expiringEmailBuilder = new ExpiringEmailBuilder(expiringQuoteModels);

            var emails = expiringEmailBuilder.GetEmails();
            foreach (var expiringEmail in emails)
            {
                SendEMail.Execute(expiringEmail.Subject, expiringEmail.Body, expiringEmail.To, expiringEmail.From, true);
                //SendEMail.Execute(expiringEmail.Subject, expiringEmail.Body, new List<string>() { "marc.pike@howcogroup.com" }, expiringEmail.From, true);
            }
        }
    }
}
