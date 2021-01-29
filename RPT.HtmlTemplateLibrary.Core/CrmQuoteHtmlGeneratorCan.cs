using System;
using System.Text;
using DAL.Vulcan.Mongo.Base.Core.DateTimeUtils;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace RPT.HtmlTemplateLibrary.Core
{
    public class CrmQuoteHtmlGeneratorCan : CrmQuoteHtmlGenerator
    {
        public CrmQuoteHtmlGeneratorCan(CrmQuote quote, string contactName, string contactEmail, string contactPhone) : base(quote, contactName, contactEmail, contactPhone) 
        {
            
        }

        protected override string FormatDateForCoid(DateTime dateTime)
        {
            dateTime = DateTimeUtilities.OffsetDateTimeForCoid("CAN", dateTime);

            // Get CultureInfo for Coid
            //var cultureInfo = DateTimeUtilities.GetCultureInfoForCoid(_quote.Coid);

            var result = dateTime.ToString("dddd, dd MMM yyyy h:mm tt");

            return result;
        }

        protected override StringBuilder GetPageFooter()
        {
            return new StringBuilder(PageFooterForCanada);
        }

        public string PageFooterForCanada
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
     <!-- Order Totals -->
        <table id='orderTable' border='0' cellspacing='0' cellpadding='0' class='totalsTable' style='margin-top: 20px;'>

              <!--<tr style='border-bottom: none;'>
                <td align='center' width='80%' colspan='12' style='text-align: right; border-right:none;'>
                 <span style='text-align: right; font-size: 14px; padding-top: 8px;'>WEIGHT TOTAL:</span>
                 </td>
                  <td style='text-align: right; font-size: 14px; font-weight: normal; padding-top: 8px;'>[WeightTotal]</td>
            </tr> -->
                          <tr>
                <td align='center' width='80%' colspan='12' style='text-align: right; border-right:none;'>
                 <span style='text-align: right; font-size: 18px; font-weight: 700; padding-top: 8px;'>ORDER TOTAL:</span>
                 </td>
                  <td style='text-align: right; font-size: 18px; font-weight: 700; padding-top: 8px;'>[OrderTotal]</td>
            </tr>
        </table>
        <!--[ORDERCOMMENTS]-->
           
        
        <!--Legal-->
        <div id='divLegal'>
            <p><strong>CONDITIONS:</strong> Any offer either Verbal or Written, for goods or services is based on material and capacities remaining unsold prior to sale. All offers are  <span class='validity'>Valid for 30 Days</span> from date of quotation. 
Howco standard Conditions of Sale are available <a href='http://www.howcogroup.com/assets/download_files/terms-conditions/CanadaTerms_Address.pdf' target='_blank'>here.</a>
                </p>
                          <p><b> TERMS OF PAYMENT:</b> Invoices are issued as of the date of delivery covering deliveries from our stocks, and as of the date of the shipment covering direct mill shipments.The acceptance of any order or specification and terms of payment on all sales and orders is subject to approval of the Seller's Credit Department, and Seller may at any time decline to make any shipment or delivery or perform any work except upon receipt of payment or security, or upon terms and conditions satisfactory to Seller's Credit Department.</p>
                </div>
                ");
                return result.ToString();
            }
        }

    }
}