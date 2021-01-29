using DAL.Vulcan.Mongo.Base.Repository;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DAL.Vulcan.Mongo.DocClass.Email
{
    public class EmailScanner
    {
        private static readonly string UserName = "vulcancrm@howcogroup.com";
        private static readonly string Password = "xbFaC9Q2CO3u7erLN1ps";

        public static ExchangeService Service => new ExchangeService(ExchangeVersion.Exchange2013_SP1)
        {
            Credentials = new WebCredentials(UserName, Password),
            Url = new Uri("https://outlook.office365.com/EWS/Exchange.asmx")
        };

        private readonly RepositoryBase<EmailConfig> _configRepository = new RepositoryBase<EmailConfig>();

        public EmailLog Execute()
        {
            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                var emailRep = new RepositoryBase<Email>();
                var config = _configRepository.AsQueryable().FirstOrDefault();
                if (config == null)
                {
                    config = new EmailConfig(); // take default values
                    config.SaveToDatabase();
                }

                var emailLog = new EmailLog()
                {
                    ExecutedOn = DateTime.Now
                };

                var emailMessages = GetEmailsSince(config.LastEmailScan);
                foreach (var emailMessage in emailMessages)
                {
                    if (!emailRep.AsQueryable().Any(x => x.EmailId.Equals(emailMessage.Id)))
                    {
                        var email = new Email(
                            emailMessage.Id,
                            emailMessage.From,
                            emailMessage.ToRecipients,
                            emailMessage.Subject,
                            emailMessage.Body.Text,
                            emailMessage.Attachments,
                            emailMessage.DateTimeCreated,
                            emailMessage.DateTimeSent,
                            emailMessage.DateTimeReceived,
                            emailMessage.CcRecipients,
                            emailMessage.BccRecipients);
                        emailLog.EmailsAdded.Add(email.AsEmailRef());
                    }
                }

                timer.Stop();
                emailLog.ProcessTime = timer.Elapsed;
                emailLog.SaveToDatabase();
                var rep = new RepositoryBase<EmailLog>();
                var removeOldLogs = rep.AsQueryable().Where(x=>x.EmailsAdded.Count == 0 && x.Id != emailLog.Id).ToList();
                foreach (var log in removeOldLogs)
                {
                    rep.RemoveOne(log);                    
                }

                config.LastEmailScan = emailLog.ExecutedOn;
                config.SaveToDatabase();
                return emailLog;
            }
            catch (Exception e)
            {
                EmailExceptionLog.CreateEmailExceptionLog(e);
                Console.WriteLine(e);
                throw;
            }
        }

        private static List<EmailMessage> GetEmailsSince(DateTime fromDate)
        {

            SearchFilter.IsGreaterThanOrEqualTo filter = new SearchFilter.IsGreaterThanOrEqualTo(ItemSchema.DateTimeReceived, fromDate);
            ItemView itemView = new ItemView(100)
            {
                PropertySet = new PropertySet(BasePropertySet.IdOnly)
            };

            itemView.OrderBy.Add(ItemSchema.DateTimeReceived, SortDirection.Descending);

            var findResults = Service.FindItems(WellKnownFolderName.Inbox, filter, itemView);
            var result = new List<EmailMessage>();
            var propertySet = new PropertySet(
                BasePropertySet.FirstClassProperties, ItemSchema.Subject, ItemSchema.Body, ItemSchema.Attachments)
            {
                RequestedBodyType = BodyType.Text
            };

            foreach (var findItemsResult in findResults)
            {
                result.Add(EmailMessage.Bind(Service, findItemsResult.Id, propertySet));
            }
            return result;
        }


    }
}