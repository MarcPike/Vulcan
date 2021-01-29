using System;
using DAL.Vulcan.Mongo.Base.Core.DateTimeUtils;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace RPT.HtmlTemplateLibrary.Core
{
    public class CrmQuoteHtmlGeneratorInc : CrmQuoteHtmlGenerator
    {
        public CrmQuoteHtmlGeneratorInc(CrmQuote quote, string contactName, string contactEmail, string contactPhone) : base(quote, contactName, contactEmail, contactPhone)
        {
        }

        protected override string FormatDateForCoid(DateTime dateTime)
        {
            dateTime = DateTimeUtilities.OffsetDateTimeForCoid("INC", dateTime);

            // Get CultureInfo for Coid
            //var cultureInfo = DateTimeUtilities.GetCultureInfoForCoid(_quote.Coid);

            var result = dateTime.ToString("dddd, dd MMM yyyy h:mm tt");

            return result;
        }
    }
}