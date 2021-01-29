using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Vulcan.Mongo.DocClass.CRM;

namespace SVC.QuoteExpireNotification.Models
{
    public class ExpiringEmailBuilder
    {
        private readonly StringBuilder _quotesAboutToExpire = new StringBuilder();
        private readonly List<ExpiringQuoteEmailModel> _workList;

        private List<string> _uniqueSalesPersonIds
        {
            get
            {
                return _workList.Select(x => x.SalesPerson.Id).Distinct().ToList();
            }
        }

        public ExpiringEmailBuilder(List<ExpiringQuoteEmailModel> emailList)
        {
            _workList = emailList;
        }

        public List<ExpiringEmail> GetEmails()
        {
            var result = new List<ExpiringEmail>();

            foreach (var salesPersonId in _uniqueSalesPersonIds)
            {
                var salesPersonQuotes = _workList.Where(x => x.SalesPerson.Id == salesPersonId).ToList();

                var newEmail = GetNewEmail(salesPersonQuotes);

                var quoteLineForBody = new StringBuilder();
                foreach (var expiringQuote in salesPersonQuotes)
                {

                    quoteLineForBody.AppendLine("<hr>");

                    if (expiringQuote.Company != null)
                    {
                        quoteLineForBody.AppendLine(
                            $"{expiringQuote.QuoteId} - ({expiringQuote.Company.Code}) {expiringQuote.Company.Name} Quote Total:{expiringQuote.QuoteTotal:0.00} {expiringQuote.Currency} {expiringQuote.Link}<br>");
                    } else if (expiringQuote.Prospect != null)
                    {
                        quoteLineForBody.AppendLine(
                            $"{expiringQuote.QuoteId} - Prospect: {expiringQuote.Prospect.Name} Quote Total:{expiringQuote.QuoteTotal:0.00} {expiringQuote.Currency} {expiringQuote.Link}<br>");
                    }
                }

                newEmail.Body += quoteLineForBody.ToString();
                result.Add(newEmail);

            }

            return result;
        }

        private static ExpiringEmail GetNewEmail(List<ExpiringQuoteEmailModel> salesPersonQuotes)
        {
            var salesPerson = salesPersonQuotes.First().SalesPerson.AsCrmUser();
            var crmUser = salesPerson.User.AsUser();
            var salesPersonEmail = crmUser.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business);

            var newEmail = new ExpiringEmail()
            {
                //To = "marc.pike@howcogroup.com",
                Body = $"{crmUser.FirstName} has the following Quotes that are about to Expire"
            };

            newEmail.To.Add(salesPersonEmail?.Address);
            //newEmail.To.Add("marc.pike@howcogroup.com");
            //newEmail.To.Add("isidro.gallego@howcogroup.com ");

            return newEmail;
        }
    }

}
