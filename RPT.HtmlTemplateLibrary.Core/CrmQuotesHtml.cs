using System;
using System.Text;

namespace RPT.HtmlTemplateLibrary.Core
{
    public class CrmQuotesHtml : IDisposable
    {
        public string PageHeader
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
                                <strong>Howco Metals Management</strong><br />
                                [AddressLine1]<br />[City], [StateProvince] [PostalCode]<br />
                                <strong>Tel:</strong>[Phone]<br />
                                <strong>Toll Free:</strong>[PhoneTollFree]<br>
                                <strong>Fax: [Fax]</strong>
                           </td>
                    
                            <td style='text-align: right; '>
                            <span style='font-size: 25px;'>[INQUIRY_OR_ORDER]</span><br /> 
                            <span style='font-size: 18px;'># [ORDER_ID] REV. [RevisionId]</span><br>
                           [CUSTOMERRFQ]
                            </td>
                        </tr>
                    </table>
                </div>
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
                <td class='label' style='width:15%; background-color:#003362; color:#fff; border-bottom:1px solid #ccc; font-size: 12px;'>Payment Terms</td>
                 <td style='width:35%; border-bottom:1px solid #ccc; font-size: 13px;'>[PaymentTerms]</td>
            </tr>
            <tr>
               <td class='label' style='width:15%; background-color:#003362; color:#fff; border-bottom:1px solid #ccc; font-size: 12px;'>Customer Contact</td>
                 <td colspan='1' style=' width:35%; border-bottom:1px solid #ccc; font-size: 13px;'>[CustomerContact]</td>
               <td class='label' style='width:15%; background-color:#003362; color:#fff; border-bottom:1px solid #ccc; font-size: 12px;'>Freight Terms</td>
                 <td colspan='2' style=' width:35%; border-bottom:1px solid #ccc; font-size: 13px;'>[FreightTerms]</td>
            </tr>
               <tr>
               <td class='label' style='width:15%; background-color:#003362; color:#fff; border-bottom:1px solid #ccc; font-size: 12px;'>Notes</td>
                 <td colspan='4' style='border-bottom:1px solid #ccc; font-size: 12px;' >[CustomerQuoteNote]</td>

            </tr>
            
                 [REVISION]
        </table>
        
                 <table border='0' cellspacing='0' cellpadding='0' class='gridtable' style='margin-top: 20px; margin-bottom: 0;'>
                    <tr>
                        <th class='grid_top' style='width:24px;'>#</th>
                        <th class='grid_top' >Product</th>
                        <th class='grid_top'>Pieces</th>
                        <th class='grid_top'>Quantity</th>
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




        public string NextPage
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
                                <strong>Howco Metals Management</strong><br />
                                [AddressLine1]<br />[City], [StateProvince] [PostalCode]<br />
                                <strong>Tel:</strong>[Phone]<br />
                                <strong>Toll Free:</strong>[PhoneTollFree]<br>
                                <strong>Fax: [Fax]</strong>
                           </td>
                    
                            <td style='text-align: right; '>
                            <span style='font-size: 25px;'>[INQUIRY_OR_ORDER]</span><br /> 
                            <span style='font-size: 18px;'># [ORDER_ID] REV. [RevisionId]</span><br>
                           [CUSTOMERRFQ]
                            </td>
                        </tr>
                    </table>
                </div>
           
        
                 <table border='0' cellspacing='0' cellpadding='0' class='gridtable' style='margin-top: 20px; margin-bottom: 0;'>
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





        public string PageFooter
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
                    <!-- Order Totals -->
                    <table id='orderTable' border='0' cellspacing='0' cellpadding='0' class='totalsTable' style='margin-top: 20px;'>

                            <tr style='border-bottom: none;'>
                            <td align='center' width='80%' colspan='12' style='text-align: right; border-right:none;'>
                                <span style='text-align: right; font-size: 14px; padding-top: 8px;'>WEIGHT TOTAL:</span>
                                </td>
                                <td style='text-align: right; font-size: 14px; font-weight: normal; padding-top: 8px;'>[WeightTotal]</td>
                        </tr>
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
                            <p><b>CONDITIONS:</b> All items are subject to prior sale. <span class='validity'>Validity of quotation is [Validity], [ValidityDate]. Delivery is based on capacity at time of quotation.</span> At this time, due to the potential Article 232 actions, Howco reserves the right to modify any part of this quotation. Thank you for the opportunity to offer our services in support of your business. All items are subject to price in effect at time of shipment unless we have specifically noted otherwise. Delivery date based upon lead time of quotation and is subject to change at time of order. All weights are theoretical and may be subject to adjustment. Any process not specifically quoted in the above price will be an additional charge. These commodities are controlled for export by the United States government under the Export Administration Regulations. Diversion contrary to U.S. law prohibited. Purchaser is responsible to comply with these regulations if the items are to be exported from the United States or re-exported from a foreign country. Please refer to our website at <a href='http://www.howcogroup.com' target='_blank'>www.howcogroup.com</a> for full terms and conditions.</p>
                        <p><b>TERMS OF PAYMENT:</b> Invoices are issued as of the date of delivery covering deliveries from our stocks, and as of the date of the shipment covering direct mill shipments. The acceptance of any order or specification and terms of payment on all sales and orders is subject to approval of the Seller's Credit Department, and Seller may at any time decline to make any shipment or delivery or perform any work except upon receipt of payment or security, or upon terms and conditions satisfactory to Seller's Credit Department.</p>
                    </div>                
                ");

                return result.ToString();
            }
        }

        public string Revisions
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
                    <tr>
                       <td class='label' style='width:15%; background-color:#003362; color:#fff; border-bottom:1px solid #ccc;'>Revision Notes</td>
                         <td colspan='4' style='border-bottom:1px solid #ccc;'>[RevisionNotes]</td>

                    </tr>            
                    ");
                return result.ToString();
            }
        }

        public string Css
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
   <!DOCTYPE html>
                <html>

                <head>
                    <title>[Title]</title>
                   <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
                   <style type='text/css'>
        

                        .logo {
                            margin: 20px auto;
                            display: block;

                        }

                        body {
                            font: normal 12px arial, verdana;
                            color: #003362;
                            padding: 10px
                               }

      
        
                        #divMain{
                           width:100%;
                           margin: 10px auto;
                        }
        
                        .totalsTable{
                            width:100%;
                            margin-bottom: 20px;
                        }
        
                        .totalsTable th{
            
                        }
                        .totalsTable td{
                                        color: #003362;
                            height: 20px;
                            padding: 4px;
                            font-size: 11px;
                        }
        

                        .gridtable {
                            width: 100%;
                            margin-bottom: 20px;
                            border-bottom:1px solid #ABB4C9;
                        }

                        .gridtable th {
                            color: #fff;
                            text-align: center;
                            background-color: #003362;
                            height: 20px;
                            padding: 4px;
                            vertical-align: middle;
                            border-bottom: 1px solid #ABB4C9;
                            border-right: 1px solid #ABB4C9;
                        }

                        .gridtable td {
                            color: #003362;
                            height: 20px;
                            padding: 4px;

                       
                            border-right: 1px solid #ABB4C9;
                            text-align: center;
                            font-size: 11px;
                        }
        
                      /*  .gridtable td:first-child{
                            border-left: 1px solid #ABB4C9;
                        }*/
        

                        .odd {
                            background-color: #FFF;

                        }

                        .even {
                            background-color: #DBE1E8;

                        }

                        .priceLine {
                            background-color: #FFF9EB;

                        }

                        .th_first {
                            color: #fff;
                            background-color: #C2CED8;
                            height: 24px;
                            vertical-align: middle;
                            border-bottom: 1px solid #ABB4C9;
                            border-right: 1px solid #ABB4C9;
                            border-left: 1px solid #ABB4C9;

                        }

                        .td_first {
                            color: #fff;
                            border-left: 1px solid #ABB4C9;
                            border-bottom: 1px solid #ABB4C9;
                            border-right: 1px solid #ABB4C9;
                            text-align: left;

                        }

                        .grid_top {
                           
                            padding: 10px;
                            
                        }

                        .lineitembox {
                            padding-top: 3px;
                            color: #fff;
                            font-weight: bold;
                            text-align: center;
                            vertical-align: middle;
                            border: 2px solid #333 !important;

                        }

                        #divLegal {
                            width: 100%;
                            display: block;
                            padding: 20px;
                            box-sizing: border-box;
                        }

                        #divLegal > p {
                            font-size: 14px!important;
                            color: #4f7293;
            
                        }

                          #divLegal > p span.validity {
                            color: red!important;
                              font-weight: bold;
                        }

                        .salesTable{
                            width:100%; 
                            border-top: 1px solid #ccc; 
                            margin-top:20px; margin-bottom: 20px;
                        }
        
                        .salesTable td {
                            padding:5px; 
                            border-right:1px solid #ccc;
                            border-bottom:1px solid #ccc;
                            font-size: 11px;
                            font-weight: bold;
                        }
        

                        @media print {
                            body {
                                color: #003362;
                            }

                            .validity {
                                color: #595B5E;

                            }
                            .th_first {
                                background-color: #C2CED8 !important;

                            }

                        }
                    </style>
                </head>

                <body>
                    <div id='divMain'> 
");

                return result.ToString();
            }
        }

        public string LineItems
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
				    <tr >
						 <td class='grid_top' style=' border-top:1px solid #ABB4C9; border-left:1px solid #ABB4C9;'>[#]</td>
							<td class='grid_top' style='font-size:13px; border-top:1px solid #ABB4C9;  border-left:0;  text-align: left; width:20%;'>
							<b>[ProductCode]</b>
							</td>
							<td class='grid_top' style='font-size:13px; border-left:0; border-top:1px solid #ABB4C9; width:50px;'>[Pieces]</td>
							<td class='grid_top' style='font-size:13px; border-left:0; border-top:1px solid #ABB4C9; width:70px;'>[Quantity]</td>
							<td class='grid_top' style='font-size:13px; border-left:0; border-top:1px solid #ABB4C9; width:40px;'>[UOM]</td>
							<td class='grid_top' style='font-size:13px; border-left:0; border-top:1px solid #ABB4C9' >[CurrencySymbol][UnitPrice]</td>
							<td class='grid_top' style='font-size:13px; border-left:0; border-top:1px solid #ABB4C9' ><strong>[CurrencySymbol][ItemTotal]</strong></td>
							<td class='grid_top' style='font-size:13px; border-left:0; border-top:1px solid #ABB4C9' >[LeadTime]</td>
							<td class='grid_top' style='font-size:13px; border-left:0; border-top:1px solid #ABB4C9' >[PartNumber]</td>
					</tr>
                    [PartSpecSeparator] 
                    [Notes]
                ");
                return result.ToString();
            }
        }


        public string PartSpecSeparator
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
				       <tr style='text-align: left'>
					   <td style='border-top:none; border-bottom:none; border-left:1px solid #ABB4C9'>&nbsp;</td>
					   <td colspan='1' class='grid_top' style='border-top: 1px solid #ABB4C9; width:40px;'><strong>PART SPECS</strong></td>
					   <td colspan='7' class='grid_top' style='border-top: 1px solid #ABB4C9; font-size: 12px; text-align: left!important; padding: 5px 10px'>[PartSpec]</td>
					</tr>
                ");
                return result.ToString();

            }
        }

        public string PartSpecSeperatorNoProduct
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
				  <tr style='text-align: left'>
					   <td style='border-top:none; border-bottom:none; border-left:1px solid #ABB4C9'>&nbsp;</td>
					   <td colspan='1' class='grid_top' style='border-top: 1px solid #ABB4C9; width:40px;'><strong>PART SPECS</strong></td>
					   <td colspan='7' class='grid_top' style='border-top: 1px solid #ABB4C9; font-size: 12px; text-align: left!important; padding: 5px 10px'>[PartSpec]</td>
					</tr>
				");
                return result.ToString();
            }
        }



        public string LineItemNotes
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
					 <tr>
					   <td style='border-top:none; border-bottom:none; border-left:1px solid #ABB4C9'>&nbsp;</td>
					   <td colspan='1' class='grid_top' style='border-top: 1px solid #ABB4C9; width:40px;'><strong>NOTES</strong></td>
					   <td colspan='7' class='grid_top' style='border-top: 1px solid #ABB4C9; font-size: 12px; text-align: left!important; padding: 5px 10px;'>[CustomerItemNote]</td>
					</tr>
                ");
                return result.ToString();
            }
        }

        public string LineItemNoNotes
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
                   <tr>
                       <td colspan='1' style='background-color:#ccc; height: 0px; border:none; padding: 0px;'></td>
                       <td colspan='7' style='background-color:#ccc; height: 0px; border:none; padding: 0px;'></td>
                    </tr>

                ");
                return result.ToString();
            }
        }

        public string NewPageNumber
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
                    <div style='width:100%; margin-top:10px;'><center> PAGE <strong>[OnPage]</strong> of <strong>[PageCount]</strong></center></div>
                    <div style = 'page-break-after: always;'></div>
                ");
                return result.ToString();
            }
        }

        public string ClosingTags
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
                </div>
                    </body>
                    </html>
                ");
                return result.ToString();
            }
        }

        public void Dispose()
        {

        }
    }
}