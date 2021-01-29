using System;
using System.Linq;
using System.Text;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;

namespace RPT.HtmlTemplateLibrary.Core
{
    public class CrmQuoteHtmlGeneratorNorwaySalesTeam : CrmQuoteHtmlGeneratorUkSalesTeam
    {
        public CrmQuoteHtmlGeneratorNorwaySalesTeam(CrmQuote quote, string contactName, string contactEmail, string contactPhone) : base(quote, contactName, contactEmail, contactPhone)
        {
        }

        protected override string GetQuoteHeader()
        {

            var salesPerson = _quote.SalesPerson.AsCrmUser();

            var teamRef = _quote.Team;
            if (teamRef == null)
            {
                teamRef = salesPerson.ViewConfig.Team;
            }

            var team = teamRef.AsTeam();

            var salesPersonLocation = team.Location.AsLocation();

            var pageHeader = (OnPage <= 1) ? new StringBuilder(PageHeaderNorway) : new StringBuilder(NextPageNorway);

            pageHeader.Replace("[AddressLine1]", salesPersonLocation.Addresses.First().AddressLine1);
            pageHeader.Replace("[AddressLine2]", salesPersonLocation.Addresses.First().AddressLine2);
            //pageHeader.Replace("[City]", salesPersonLocation.Addresses.First().City);
            //pageHeader.Replace("[StateProvince]", salesPersonLocation.Addresses.First().StateProvince ?? "");
            //pageHeader.Replace("[PostalCode]", salesPersonLocation.Addresses.First().PostalCode);
            pageHeader.Replace("[Phone]", salesPersonLocation.Phone);
            pageHeader.Replace("[PhoneTollFree]", salesPersonLocation.PhoneTollFree ?? "+1 800 392 7720");
            pageHeader.Replace("[Fax]", salesPersonLocation.Fax);

            pageHeader.Replace("[SalesPerson]", _quote.SalesPerson.FullName);

            pageHeader.Replace("[PaymentTerms]", _quote.PaymentTerm);
            pageHeader.Replace("[FreightTerms]", _quote.FreightTerm);


            pageHeader.Replace("[INQUIRY_OR_ORDER]", "Howco Quotation Reference");
            pageHeader.Replace("[DateLabel]", "Quote Date");
            pageHeader.Replace("[TOTAL_LABEL]", "Quote");
            pageHeader.Replace("[ORDER_ID]", _quote.QuoteId.ToString());

            if ((_quote.RevisionNumber != 0) && (_quote.CurrentRevision != null) &&
                (_quote.CurrentRevision.RevisionNotesForPdf != string.Empty))
            {
                pageHeader.Replace("[REVISION]", GetRevisionHtml(_quote));
            }
            else
            {
                pageHeader.Replace("[REVISION]", string.Empty);
            }

            pageHeader.Replace("[RevisionId]", _quote.RevisionNumber.ToString());

            if (_quote.RfqNumber != String.Empty)
            {
                pageHeader.Replace("[CUSTOMERRFQ]",
                    $"<span style = 'font-size: 18px; padding: 5px;' > CUSTOMER RFQ#: {_quote.RfqNumber}</span>");
            }
            else
            {
                pageHeader.Replace("[CUSTOMERRFQ]", string.Empty);
            }

            pageHeader.Replace("[RfqNumber]", _quote.RfqNumber);

            pageHeader.Replace("[SoldToCustomerCode]", (_quote.IsProspect) ? _quote.Prospect.Code : _quote.Company.Code);
            pageHeader.Replace("[SoldToCustomerName]", (_quote.IsProspect) ? _quote.Prospect.Name : _quote.Company.Name);
            if (_quote.Company != null)
            {
                pageHeader.Replace("[SoldToCustomerAddress]", _quote.Company.AddressLine1);
                pageHeader.Replace("[SoldToCityStatePostal]",
                    $"{_quote.Company?.City}, {_quote.Company?.StateProvince} {_quote.Company?.PostalCode}");
            }
            else
            {
                pageHeader.Replace("[SoldToCustomerAddress]", "");
                pageHeader.Replace("[SoldToCityStatePostal]", "");
            }

            pageHeader.Replace("[SoldToCustomerPhoneNumber]", _contactPhone);

            pageHeader.Replace("[ShipToCustomerName]", (_quote.IsProspect) ? _quote.Prospect.Name : _quote.Company.Name);

            if (_quote.ShipToAddress != null)
            {
                pageHeader.Replace("[ShipToCustomerAddress]", _quote.ShipToAddress.AddressLine1);
                pageHeader.Replace("[ShipToCityStatePostal]",
                    $"{_quote.ShipToAddress?.City}, {_quote.ShipToAddress?.StateProvince} {_quote.ShipToAddress?.PostalCode}");
            }
            else
            {
                pageHeader.Replace("[ShipToCustomerAddress]", "");
                pageHeader.Replace("[ShipToCityStatePostal]", "");
            }

            pageHeader.Replace("[ShipToCustomerPhoneNumber]", _contactPhone);

            DateTime quoteDate;
            if ((_quote.Status == PipelineStatus.Draft))
            {
                quoteDate = _quote.ModifiedDateTime;
            }
            else
            {
                quoteDate = _quote.SubmitDate ?? _quote.ModifiedDateTime;
            }

            //var dateTime = _quote.WonDate ?? _quote.LostDate ?? _quote.SubmitDate ??
            //                ((_quote.ModifiedDateTime > _quote.CreateDateTime) ? _quote.ModifiedDateTime : _quote.CreateDateTime);
            if (_quote.CurrentRevision != null) quoteDate = _quote.CurrentRevision.RevisionDate;



            pageHeader.Replace("[QuoteDate]", FormatDateForCoid(quoteDate));


            pageHeader.Replace("[CustomerContact]", _contactName);
            pageHeader.Replace("[ContactEmail]", _contactEmail);


            if (_quote.CustomerNotes != null)
            {
                pageHeader.Replace("[CustomerQuoteNote]", _quote.CustomerNotes.Replace("\n", "<br>"));
            }
            else
            {
                pageHeader.Replace("[CustomerQuoteNote]", "");
            }

            return pageHeader.ToString();
        }

        public string PageHeaderNorway
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
                                <strong>Howco</strong><br />
                                [AddressLine1]<br />
                                [AddressLine2]<br />
                                <strong>Tel:</strong> [Phone]<br />
                                <strong>Fax:</strong>  [Fax]
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
                 <td style='width:35%; border-bottom:1px solid #ccc; font-size: 13px;'></td>
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
                        <th class='grid_top'>Part Number</th>
                        <th class='grid_top' >Sizes/ Drawings</th>
                        <th class='grid_top'>Qty</th>
                        <th class='grid_top'>UOM</th>
                        <th class='grid_top'>Unit Price</th>
                        <th class='grid_top'>Item Total</th>
                        <th class='grid_top'>Delivery</th>
                       
                    </tr>
                ");

                return result.ToString();
            }
        }


        public string NextPageNorway
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
                                    <strong>Howco</strong><br />
                                    [AddressLine1]<br />[AddressLine2]<br />
                                    <strong>Tel:</strong>[Phone]<br />
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
            <table id='tableContact' border='1' cellspacing='0' cellpadding='10' style='margin-top: 20px; width: 100%'>            
                <!--Header-->
                <tr>
                    <th class='grid_top'>Account</th>
                    <th class='grid_top'>Customer</th>
                    <th class='grid_top'>Ship To</th>
                    <th class='grid_top'>[DateLabel]</th>
                </tr>
                <!--CUSTOMERINFO-->
                <tr class='tableContactLines' style='margin: 20px 0 20px 0; text-align: left; font-size: 14px;' >
                    <td class='odd'> [SoldToCustomerCode]</td>
                    <td class='odd'> [SoldToCustomerName]<br /> [SoldToCustomerAddress]<br />[SoldToCityStatePostal]<br /> [SoldToCustomerPhoneNumber] </td>
                    <td class='odd'> [ShipToCustomerName]<br /> [ShipToCustomerAddress]<br />[ShipToCityStatePostal]<br /> [ShipToCustomerPhoneNumber] </td>
                    <td class='odd'> [QuoteDate] </td>
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
                     <td style='width:35%; border-bottom:1px solid #ccc; font-size: 13px;'></td>
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
                            <th class='grid_top'>Part Number</th>
                            <th class='grid_top' >Sizes/ Drawings</th>
                            <th class='grid_top'>Qty</th>
                            <th class='grid_top'>UOM</th>
                            <th class='grid_top'>Unit Price</th>
                            <th class='grid_top'>Item Total</th>
                            <th class='grid_top'>Delivery</th>
                           
                        </tr>
                    ");

                return result.ToString();
            }
        }

    }
}
