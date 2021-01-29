using System;
using System.Text;
using DAL.Vulcan.Mongo.Base.Core.DateTimeUtils;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace RPT.HtmlTemplateLibrary.Core
{
    public class CrmQuoteHtmlGeneratorDub : CrmQuoteHtmlGenerator
    {
        public CrmQuoteHtmlGeneratorDub(CrmQuote quote, string contactName, string contactEmail, string contactPhone) : base(quote, contactName, contactEmail, contactPhone)
        {
        }

        protected override string FormatDateForCoid(DateTime dateTime)
        {
            dateTime = DateTimeUtilities.OffsetDateTimeForCoid("DUB", dateTime);

            // Get CultureInfo for Coid
            //var cultureInfo = DateTimeUtilities.GetCultureInfoForCoid(_quote.Coid);

            var result = dateTime.ToString("dddd, dd MMM yyyy h:mm tt");

            return result;
        }

        protected override void GetQuoteAddress(StringBuilder pageHeader, Location salesPersonLocation)
        {
        //     pageHeader.Replace("[AddressLine1]", salesPersonLocation.Addresses.First().AddressLine1);
        //     pageHeader.Replace("[City]", salesPersonLocation.Addresses.First().City);
        //     pageHeader.Replace("[StateProvince]", salesPersonLocation.Addresses.First().StateProvince ?? "");
        //     pageHeader.Replace("[PostalCode]", salesPersonLocation.Addresses.First().PostalCode);
        //     pageHeader.Replace("[Phone]", salesPersonLocation.Phone);
        //     pageHeader.Replace("[PhoneTollFree]", salesPersonLocation.PhoneTollFree ?? "+1 800 392 7720");
        //     pageHeader.Replace("[Fax]", salesPersonLocation.Fax);
        }

        protected override StringBuilder GetPageHeader()
        {
            return new StringBuilder(PageHeaderForDubai);
        }

        protected override StringBuilder GetPageFooter()
        {
            return new StringBuilder(PageFooterForDubai);
        }

        protected override StringBuilder GetNextPage()
        {
            return new StringBuilder(NextPageForDubai);
        }

        private string NextPageForDubai
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
            <div id='divHeader' style='margin-top: 20px'>
                    <table border='0' cellpadding='0' cellspacing='0' style='width: 100%;'>
                        <tr>
                           <td style='width:150px; padding:10px;'>
                             <center> <img src='http://s-us-web02:82/images/howco-logo.png' width='120px' />  </center>
                           </td>
                            <td style='font-size: 13px; padding: 5px 0 5px 5px;'>
                                <strong>Howco Oilfield Services FZE</strong><br />
                                PO Box 20273 <br>
                                Hamriyah Free Zone <br>
                                Sharjah <br>
                                UAE <br>
                                <strong>Tel:</strong> +971 (0)6 5139 500<br />
                                <strong>Fax:</strong> +971 (0)6 5139 555
                           </td>
                    
                            <td style='text-align: right; '>
                            <span style='font-size: 25px;'>[INQUIRY_OR_ORDER]</span><br /> 
                            <span style='font-size: 18px;'># [ORDER_ID] REV. [RevisionId]</span><br>
                           [CUSTOMERRFQ]
                            </td>
                        </tr>
                    </table>
                </div>
           
        
                 <table border='0' cellspacing='0' cellpadding='0' class='gridtable' style='margin-top: 20px; margin-bottom: 0'>
                    <tr>
                        <th class='grid_top' style='width:24px;'>No.</th>
                        <th class='grid_top' >Product</th>
                        <th class='grid_top'>Pieces</th>
                        <th class='grid_top'>Qty</th>
                        <th class='grid_top'>UOM</th>
                        <th class='grid_top'>Unit Price</th>
                        <th class='grid_top'>Item Total</th>
                        <th class='grid_top'>Delivery</th>
                        <th class='grid_top'>PT#</th>
                    </tr>
                ");

                return result.ToString();
            }
        }





        public string PageHeaderForDubai
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
                        <div id='divHeader' style='margin-top: 20px'>
                    <table border='0' cellpadding='0' cellspacing='0' style='width: 100%;'>
                        <tr>
                           <td style='width:150px; padding:10px;'>
                             <center> <img src='http://s-us-web02:82/images/howco-logo.png' width='120px' />  </center>
                           </td>
                            <td style='font-size: 13px; padding: 5px 0 5px 5px;'>
                                <strong>Howco Oilfield Services FZE</strong><br />
                                PO Box 20273 <br>
                                Hamriyah Free Zone <br>
                                Sharjah <br>
                                UAE <br>
                                <strong>Tel:</strong> +971 (0)6 5139 500<br />
                                <strong>Fax:</strong> +971 (0)6 5139 555
                           </td>
                    
                            <td style='text-align: right; '>
                            <span style='font-size: 25px;'>[INQUIRY_OR_ORDER]</span><br /> 
                            <span style='font-size: 18px;'># [ORDER_ID] REV. [RevisionId]</span><br>
                           [CUSTOMERRFQ]
                            </td>
                        </tr>
                    </table>
                </div>
        <table id='tableContact' cellspacing='0' cellpadding='10' style='margin-top: 20px; width: 100%'>            
            <!--Header-->
            <tr >
                <th class='grid_top' >Account</th>
                <th class='grid_top'>Customer</th>
                <th class='grid_top'>Ship To</th>
                <th class='grid_top'>[DateLabel]</th>
            </tr>
            <!--CUSTOMERINFO-->
            <tr class='tableContactLines' style='margin: 20px 0 20px 0; text-align: left; font-size: 14px;' >
                <td class='odd' style='border: 1px solid #ABB4C9;'> [SoldToCustomerCode]</td>
                <td class='odd' style='border: 1px solid #ABB4C9;'> [SoldToCustomerName]<br /> [SoldToCustomerAddress]<br />[SoldToCityStatePostal]<br /> [SoldToCustomerPhoneNumber] </td>
                <td class='odd' style='border: 1px solid #ABB4C9;'> [ShipToCustomerName]<br /> [ShipToCustomerAddress]<br />[ShipToCityStatePostal]<br /> [ShipToCustomerPhoneNumber] </td>
                <td class='odd' style='border: 1px solid #ABB4C9;'> [QuoteDate] </td>
            </tr>
        </table>
        
        <table class='salesTable' border='0' cellspacing='0' cellpadding='0' >
            <tr>
                <td class='label' style='width:15%; background-color:#003362; color:#fff; border-bottom:1px solid #ccc; font-size: 12px;'>SalesPerson</td>
                <td  style=' width:35%; border-bottom:1px solid #ccc; font-size: 13px;'>[SalesPerson]</td>
                <td class='label' style='width:15%; background-color:#003362; color:#fff; border-bottom:1px solid #ccc; font-size: 12px;'>Freight Terms</td>
                 <td colspan='2' style=' width:35%; border-bottom:1px solid #ccc; font-size: 13px;'>[FreightTerms]</td>
          
            </tr>
            <tr>
               <td class='label' style='width:15%; background-color:#003362; color:#fff; border-bottom:1px solid #ccc; font-size: 12px;'>Customer Contact</td>
                 <td colspan='1' style=' width:35%; border-bottom:1px solid #ccc; font-size: 13px;'>[CustomerContact]</td>
                     <td class='label' style='width:15%; background-color:#003362; color:#fff; border-bottom:1px solid #ccc; font-size: 12px;'>Payment Terms</td>
                 <td style='width:35%; border-bottom:1px solid #ccc; font-size: 13px;'>[PaymentTerms]</td>
            </tr>
               <tr>
               <td class='label' style='width:15%; background-color:#003362; color:#fff; border-bottom:1px solid #ccc; font-size: 12px;'>Notes</td>
                 <td colspan='4' style='border-bottom:1px solid #ccc; font-size: 12px;' >[CustomerQuoteNote]</td>

            </tr>
            
                 [REVISION]
        </table>
        
                  <table border='0' cellspacing='0' cellpadding='0' class='gridtable' style='margin-top: 20px; margin-bottom: 0; '>
                    <tr>
                        <th class='grid_top' style='width:24px;'>No.</th>
                        <th class='grid_top' >Product</th>
                        <th class='grid_top'>Pieces</th>
                        <th class='grid_top'>Qty</th>
                        <th class='grid_top'>UOM</th>
                        <th class='grid_top'>Unit Price</th>
                        <th class='grid_top'>Item Total</th>
                        <th class='grid_top'>Delivery</th>
                        <th class='grid_top'>PT#</th>
                    </tr>
                ");

                return result.ToString();
            }
        }

        private string PageFooterForDubai
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
     <!-- Order Totals -->
        <table id='orderTable' border='0' cellspacing='0' cellpadding='0' class='totalsTable' style='margin-top: 20px;'>

             <!-- <tr style='border-bottom: none;'>
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
            <p><strong>CONDITIONS:</strong> Any offer either Verbal or Written, for goods or services is based on material and capacities remaining unsold prior to sale.  <span class='validity'>All offers are Valid for [Validity], [ValidityDate].</span> Howco standard Conditions of Sale are available here. Offered prices are exclusive of VAT. Please advise the end user destination country at the time of order. The above offer is valid on full quantity order, any changes may result in pricing adjustments. The above quotation has been reviewed by the sender. Our VAT number is TRN100393185200003. Please send all queries to <a href='mailto:sales.medubai@howcogroup.com'>sales.medubai@howcogroup.com</a> to ensure a prompt response.
                </p>
                          <p><b> TERMS OF PAYMENT:</b> Invoices are issued as of the date of delivery covering deliveries from our stocks, and as of the date of the shipment covering direct mill shipments.The acceptance of any order or specification and terms of payment on all sales and orders is subject to approval of the Seller's Credit Department, and Seller may at any time decline to make any shipment or delivery or perform any work except upon receipt of payment or security, or upon terms and conditions satisfactory to Seller's Credit Department.</p>
                </div>
                ");
                return result.ToString();
            }
        }

    }
}