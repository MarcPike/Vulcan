using System;
using System.Text;
using DAL.Vulcan.Mongo.Base.Core.DateTimeUtils;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace RPT.HtmlTemplateLibrary.Core
{
    public class CrmQuoteHtmlGeneratorSin : CrmQuoteHtmlGenerator
    {
        public CrmQuoteHtmlGeneratorSin(CrmQuote quote, string contactName, string contactEmail, string contactPhone) : base(quote, contactName, contactEmail, contactPhone)
        {
        }

        protected override string FormatDateForCoid(DateTime dateTime)
        {
            dateTime = DateTimeUtilities.OffsetDateTimeForCoid("SIN", dateTime);

            // Get CultureInfo for Coid
            //var cultureInfo = DateTimeUtilities.GetCultureInfoForCoid(_quote.Coid);

            var result = dateTime.ToString("dddd, dd MMM yyyy h:mm tt");

            return result;
        }

        protected override void GetQuoteAddress(StringBuilder pageHeader, Location salesPersonLocation)
        {
			// pageHeader.Replace("[AddressLine1]", salesPersonLocation.Addresses.First().AddressLine1);
            // pageHeader.Replace("[City]", salesPersonLocation.Addresses.First().City);
            // pageHeader.Replace("[StateProvince]", salesPersonLocation.Addresses.First().StateProvince ?? "");
            // pageHeader.Replace("[PostalCode]", salesPersonLocation.Addresses.First().PostalCode);
            // pageHeader.Replace("[Phone]", salesPersonLocation.Phone);
            // pageHeader.Replace("[PhoneTollFree]", salesPersonLocation.PhoneTollFree ?? "+1 800 392 7720");
            // pageHeader.Replace("[Fax]", salesPersonLocation.Fax);
        }

        protected override StringBuilder GetPageFooter()
        {
            return new StringBuilder(PageFooterForSingapore);
        }

        protected override StringBuilder GetPageHeader()
        {
            return new StringBuilder(PageHeaderSingapore);
        }


        protected override StringBuilder GetNextPage()
        {
            return new StringBuilder(NextPageForSingapore);
        }

        private string NextPageForSingapore
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
                                <strong>Howco Metals Management Pte Ltd</strong><br />
                                No. 21 Tuas South Street 5<br />Singapore, 637384<br />
                                <strong>Tel:</strong> +65 6861 3885<br />
                                <strong>Fax:</strong> +65 6861 4827
                           </td>
                    
                            <td style='text-align: right; '>
                            <span style='font-size: 25px;'>[INQUIRY_OR_ORDER]</span><br /> 
                            <span style='font-size: 18px;'># [ORDER_ID] REV. [RevisionId]</span><br>
                           [CUSTOMERRFQ]
                            </td>
                        </tr>
                    </table>
                </div>
           
        
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

        private string PageFooterForSingapore
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
            </tr>-->
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
            <p><strong>CONDITIONS:</strong> All items are subject to prior sale.  <span class='validity'>Validity of quotation is [Validity], [ValidityDate]. Delivery is based on capacity at time of quotation.</span> Thank you for the opportunity to offer our services in support of your business. All items are subject to price in effect at time of shipment unless we have specifically noted otherwise. Delivery date based upon lead time of quotation and is subject to change at time of order. All weights are theoretical and may be subject to adjustment. Any process not specifically quoted in the above price will be an additional charge. These commodities are controlled for export by the Singapore government under the Export Administration Regulations. Diversion contrary to Singapore law prohibited. Purchaser is responsible to comply with these regulations if the items are to be exported from Singapore or reexported from a foreign country. Please refer to our website at <a href='http://www.howcogroup.com'>www.howcogroup.com</a> for full terms and conditions. </p>
						  <p><b> TERMS OF PAYMENT:</b> Invoices are issued as of the date of delivery covering deliveries from our stocks, and as of the date of the shipment covering direct mill shipments. The acceptance of any order or specification and terms of payment on all sales and orders is subject to approval of the Seller's Credit Department, and Seller may at any time decline to make any shipment or delivery or perform any work except upon receipt of payment or security, or upon terms and conditions satisfactory to Seller's Credit Department.</p>
                </div>
                ");
                return result.ToString();
            }
        }


        private string PageHeaderSingapore
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
                                <strong>Howco Metals Management Pte Ltd</strong><br />
                                No. 21 Tuas South Street 5<br />Singapore, 637384<br />
                                <strong>Tel:</strong> +65 6861 3885<br />
                                <strong>Fax:</strong> +65 6861 4827
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


    }
}