using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Vulcan.Mongo.Base.Core.DateTimeUtils;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Extensions;
using DAL.Vulcan.Mongo.Core.Models;
using Vulcan.IMetal.Helpers;

namespace RPT.HtmlTemplateLibrary.Core
{
    public class CrmQuoteHtmlGeneratorUkSalesTeam : CrmQuoteHtmlGenerator
    {
        public CrmQuoteHtmlGeneratorUkSalesTeam(CrmQuote quote, string contactName, string contactEmail, string contactPhone) : base(quote, contactName, contactEmail, contactPhone)
        {
        }

        protected override string FormatDateForCoid(DateTime dateTime)
        {
            dateTime = DateTimeUtilities.OffsetDateTimeForCoid("EUR", dateTime);

            // Get CultureInfo for Coid
            //var cultureInfo = DateTimeUtilities.GetCultureInfoForCoid(_quote.Coid);

            var result = dateTime.ToString("dddd, dd MMM yyyy h:mm tt");

            return result;
        }

        protected override StringBuilder GetPageFooter()
        {
            var pageFooter = new StringBuilder(PageFooterForEurope);

            GetHtmlForValidityDate(pageFooter, _quote);

            return pageFooter;

        }

        protected override (string WeightSuffix, decimal Weight) GetWeightValues(List<CrmQuoteItem> quoteItems, List<CrmQuoteItem> quickQuoteItems, List<CrmQuoteItem> machinedParts, List<CrmQuoteItem> crozCalcs)
        {
            var weightSuffix = " kg";
            var weight = quoteItems.Sum(x => x.QuotePrice.FinalPriceOverride.FinalQuantity.TotalKilograms());
            weight += quickQuoteItems.Sum(x => x.QuickQuoteData?.GetWeightInKilos() ?? 0);
            weight += machinedParts.Sum(x => x.MachinedPartModel.PieceWeightKilos * x.MachinedPartModel.Pieces);
            weight += crozCalcs.Sum(x => x.CrozCalcItem.TotalWeight);
            return (weightSuffix, weight);
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

            var pageHeader = (OnPage <= 1) ? new StringBuilder(PageHeaderEurope) : new StringBuilder(NextPageEurope);

            pageHeader.Replace("[AddressLine1]", salesPersonLocation.Addresses.First().AddressLine1);
            pageHeader.Replace("[City]", salesPersonLocation.Addresses.First().City);
            pageHeader.Replace("[StateProvince]", salesPersonLocation.Addresses.First().StateProvince ?? "");
            pageHeader.Replace("[PostalCode]", salesPersonLocation.Addresses.First().PostalCode);
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

            //pageHeader.Replace("[RfqNumber]", _quote.RfqNumber);

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

        protected override string GetQuoteItems()
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            var currencyValues = helperCurrency.GetSymbolForCurrency(_quote.DisplayCurrency);


            var baseHtmlForItem = LineItemsEurope; //GetHtmlForTemplate(_quote.Coid, "CrmQuoteItem");
            // var baseHtmlForHiddenItem = _html.LineItemsNoProductCode;


            var lineCount = 0;
            var additionalPages = new List<string>();
            StringBuilder finalHtmlForItems = new StringBuilder();
            StringBuilder thisItemHtml;
            //var index = 0;
            foreach (var crmQuoteItem in _quote.Items.Select(x => x.AsQuoteItem()).OrderBy(x => x.Index).ToList())
            {
                if (crmQuoteItem.IsLost) continue;

                lineCount = CheckForNewPage();
                thisItemHtml = new StringBuilder(baseHtmlForItem);

                //var currencyTypeSymbolAndExchangeRate = GetCurrencyTypeSymbolAndExchangeRate(crmQuoteItem);

                GenerateQuickQuoteItemHtml(crmQuoteItem, helperCurrency, thisItemHtml);

                GenerateCrozCalcItemHtml(crmQuoteItem, helperCurrency, thisItemHtml);

                GenerateNormalQuoteItemHtml(crmQuoteItem, helperCurrency, thisItemHtml, finalHtmlForItems);
            }

            finalHtmlForItems.Append("</table>");
            return (finalHtmlForItems.ToString());

            int CheckForNewPage()
            {
                lineCount++;
                if (lineCount > _pdfRowsPerPage)
                {
                    finalHtmlForItems.Append("</table>");
                    finalHtmlForItems = HandleNewPage(finalHtmlForItems);
                    lineCount = 1;
                }

                return lineCount;
            }
        }

        protected override void GenerateNormalQuoteItemHtml(CrmQuoteItem crmQuoteItem, HelperCurrencyForIMetal helperCurrency,
    StringBuilder thisItemHtml, StringBuilder finalHtmlForItems)
        {
            GetHtmlForQuantityAndPrice(crmQuoteItem, thisItemHtml);

            GenerateHtmlForProductCode(crmQuoteItem, thisItemHtml);

            GenerateHtmlForNoProductCode(crmQuoteItem, thisItemHtml);

            ShowOutsideAndInsideDiametersForAllQuoteItems(crmQuoteItem, thisItemHtml);

            ShowItemTotalForAllItems(crmQuoteItem, helperCurrency, thisItemHtml);

            GenerateHtmlForNotes(crmQuoteItem, thisItemHtml);

            finalHtmlForItems.AppendLine(thisItemHtml.ToString());
        }

        protected override void GenerateHtmlForNoProductCode(CrmQuoteItem crmQuoteItem, StringBuilder thisItemHtml)
        {
            if (crmQuoteItem.IsQuickQuoteItem) return;

            if (crmQuoteItem.IsCrozCalc) return;

            if (!crmQuoteItem.ShowProductCodeOnQuote)
            {
                thisItemHtml.Replace("[ProductCode]", string.Empty);

                //if (!String.IsNullOrEmpty(crmQuoteItem.PartSpecification))
                //{
                //    var separator = _html.PartSpecSeperatorNoProduct;
                //    separator = separator.Replace("[PartSpec]", crmQuoteItem.PartSpecification).Trim();
                //    thisItemHtml.Replace("[PartSpecSeparator]", separator.Trim());
                //}
                //else
                //{
                //    thisItemHtml.Replace("[PartSpec]", string.Empty);
                //    thisItemHtml.Replace("[PartSpecSeparator]", string.Empty);
                //}
                if (!String.IsNullOrEmpty(crmQuoteItem.PartSpecification))
                {
                    var separator = _html.PartSpecSeparator;
                    separator = separator.Replace("[PartSpec]", crmQuoteItem.PartSpecification.Replace("\n", "<br>")).Trim();
                    thisItemHtml.Replace("[PartSpecSeparator]", separator);
                }
                else
                {
                    thisItemHtml.Replace("[PartSpec]", string.Empty);
                    thisItemHtml.Replace("[PartSpecSeparator]", string.Empty);
                }

            }
        }

        protected override void GenerateHtmlForProductCode(CrmQuoteItem crmQuoteItem, StringBuilder thisItemHtml)
        {

            if (crmQuoteItem.ShowProductCodeOnQuote)
            {
                if (crmQuoteItem.IsQuickQuoteItem)
                {
                    thisItemHtml.Replace("[ProductCode]", ConvertSpecialCharactersToHtml(crmQuoteItem.QuickQuoteData.Label));
                }
                else if (crmQuoteItem.IsCrozCalc)
                {
                    thisItemHtml.Replace("[ProductCode]", ConvertSpecialCharactersToHtml(crmQuoteItem.CrozCalcItem.FinishedProductLabel));
                }
                else
                {
                    thisItemHtml.Replace("[ProductCode]", ConvertSpecialCharactersToHtml(crmQuoteItem.QuotePrice.FinishedProduct.ProductCode));
                }

                if (!String.IsNullOrEmpty(crmQuoteItem.PartSpecification))
                {
                    var separator = _html.PartSpecSeparator;
                    separator = separator.Replace("[PartSpec]", crmQuoteItem.PartSpecification.Replace("\n", "<br>")).Trim();
                    thisItemHtml.Replace("[PartSpecSeparator]", separator);
                }
                else
                {
                    thisItemHtml.Replace("[PartSpec]", string.Empty);
                    thisItemHtml.Replace("[PartSpecSeparator]", string.Empty);
                }
            }
        }

        protected override void GetHtmlForQuantityAndPrice(CrmQuoteItem crmQuoteItem, StringBuilder thisItemHtml)
        {
            if (crmQuoteItem.IsQuickQuoteItem) return;
            if (crmQuoteItem.IsCrozCalc) return;

            crmQuoteItem.QuotePrice.FinalPriceOverride.CustomerUom =
                crmQuoteItem.CalculateQuotePriceModel.CustomerUom;
            var unitPrice = crmQuoteItem.QuotePrice.FinalPriceOverride.FinalUnitCost;
            thisItemHtml.Replace("[#]", crmQuoteItem.Index.ToString());
            thisItemHtml.Replace("[Pieces]", crmQuoteItem.QuotePrice.RequiredQuantity.Pieces.ToString());
            thisItemHtml.Replace("[Quantity]",
                crmQuoteItem.QuotePrice.FinalPriceOverride.FinalQuantityValue.ToString("0.000"));

            thisItemHtml.Replace("[UOM]", crmQuoteItem.QuotePrice.FinalPriceOverride.FinalUom);
            thisItemHtml.Replace("[UnitPrice]",
                crmQuoteItem.QuotePrice.FinalPriceOverride.FinalUnitCost.RoundAndNormalize(2).ToString("0.00") +
                crmQuoteItem.QuotePrice.FinalPriceOverride.FinalUnitCostSuffix);
        }

        protected override void GenerateHtmlForNotes(CrmQuoteItem crmQuoteItem, StringBuilder thisItemHtml)
        {
            if (!String.IsNullOrEmpty(crmQuoteItem.CustomerNotes))
            {
                var lineItemNotes = LineItemNotesEurope;

                lineItemNotes = lineItemNotes.Replace("[CustomerItemNote]", crmQuoteItem.CustomerNotes.Replace("\n", "<br>"));
                thisItemHtml.Replace("[Notes]", lineItemNotes);
            }
            else
            {
                thisItemHtml.Replace("[Notes]", _html.LineItemNoNotes);
            }
        }

        protected override void ShowItemTotalForAllItems(CrmQuoteItem crmQuoteItem, HelperCurrencyForIMetal helperCurrency,
            StringBuilder thisItemHtml)
        {
            if (crmQuoteItem.IsQuickQuoteItem)
            {
                var itemCurrencyValues = helperCurrency.GetSymbolForCurrency(_quote.DisplayCurrency);

                thisItemHtml.Replace("[Currency]", itemCurrencyValues.Currency);
                if (crmQuoteItem.QuickQuoteData.Regret)
                {
                    thisItemHtml.Replace("[CurrencySymbol]", string.Empty);
                    thisItemHtml.Replace("[ItemTotal]", "REGRET");
                }
                else
                {
                    thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                    thisItemHtml.Replace("[ItemTotal]", crmQuoteItem.QuickQuoteData.Price.ToString("#,##0.00"));
                }
            }
            else if (crmQuoteItem.IsCrozCalc)
            {
                var itemCurrencyValues = helperCurrency.GetSymbolForCurrency(_quote.DisplayCurrency);

                if (crmQuoteItem.CrozCalcItem.Regret)
                {
                    thisItemHtml.Replace("[Currency]", string.Empty);
                    thisItemHtml.Replace("[CurrencySymbol]", string.Empty);
                    thisItemHtml.Replace("[ItemTotal]", "REGRET");
                }
                else
                {
                    thisItemHtml.Replace("[Currency]", itemCurrencyValues.Currency);
                    thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                    thisItemHtml.Replace("[ItemTotal]", crmQuoteItem.CrozCalcItem.TotalPrice.ToString("#,##0.00"));
                }
            }
            else
            {
                var itemCurrencyValues = helperCurrency.GetSymbolForCurrency(_quote.DisplayCurrency);
                thisItemHtml.Replace("[Currency]", itemCurrencyValues.Currency);
                thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                thisItemHtml.Replace("[ItemTotal]", crmQuoteItem.QuotePrice.FinalPrice.ToString("#,##0.00"));
            }

            thisItemHtml.Replace("[LeadTime]", crmQuoteItem.LeadTime);

        }

        protected override void ShowOutsideAndInsideDiametersForAllQuoteItems(CrmQuoteItem crmQuoteItem, StringBuilder thisItemHtml)
        {
            if (crmQuoteItem.IsQuickQuoteItem)
            {
                thisItemHtml.Replace("[OutsideDiameter]",
                    crmQuoteItem.QuickQuoteData.FinishedProduct?.OuterDiameter.ToString("0.00") ?? "0.00");
                thisItemHtml.Replace("[InsideDiameter]",
                    crmQuoteItem.QuickQuoteData.FinishedProduct?.InsideDiameter.ToString("0.00") ?? "0.00");
            }
            else if (crmQuoteItem.IsCrozCalc)
            {
                thisItemHtml.Replace("[OutsideDiameter]",
                    crmQuoteItem.CrozCalcItem.StartingProductLabel);
                thisItemHtml.Replace("[InsideDiameter]",
                    crmQuoteItem.CrozCalcItem.FinishedProductLabel);
            }
            else
            {
                thisItemHtml.Replace("[OutsideDiameter]",
                    crmQuoteItem.QuotePrice.FinishedProduct?.OuterDiameter.ToString("0.00") ?? "0.00");
                thisItemHtml.Replace("[InsideDiameter]",
                    crmQuoteItem.QuotePrice.FinishedProduct?.InsideDiameter.ToString("0.00") ?? "0.00");
            }
            thisItemHtml.Replace("[PartNumber]", crmQuoteItem.PartNumber);

        }

        protected override void GenerateQuickQuoteItemHtml(CrmQuoteItem crmQuoteItem, HelperCurrencyForIMetal helperCurrency,
            StringBuilder thisItemHtml)
        {
            if (crmQuoteItem.IsQuickQuoteItem)
            {
                var itemCurrencyValues = helperCurrency.GetSymbolForCurrency(_quote.DisplayCurrency);

                var quickQuoteItemQuantity =
                    crmQuoteItem.QuickQuoteData.QuantityValueForPdf();
                var quickQuoteItemUom = crmQuoteItem.QuickQuoteData.UomValueForPdf();
                var quickQuoteItemUnitPrice =
                    crmQuoteItem.QuickQuoteData.UnitPriceValueForPdf();
                var quickQuoteItemPieces = crmQuoteItem.QuickQuoteData.OrderQuantity.Pieces;

                thisItemHtml.Replace("[#]", crmQuoteItem.Index.ToString());
                thisItemHtml.Replace("[Pieces]", crmQuoteItem.QuickQuoteData.OrderQuantity.Pieces.ToString());
                if (crmQuoteItem.QuickQuoteData.OrderQuantity.EachBased)
                {
                    thisItemHtml.Replace("[Quantity]", "piece(s)");
                }
                else
                {
                    thisItemHtml.Replace("[Quantity]", quickQuoteItemQuantity.ToString("0.00"));
                }
                thisItemHtml.Replace("[UOM]", quickQuoteItemUom);

                if (quickQuoteItemUom == "ea")
                {
                    thisItemHtml.Replace("[Quantity]", quickQuoteItemPieces.ToString("0.000"));

                }
                else
                {
                    thisItemHtml.Replace("[Quantity]", quickQuoteItemQuantity.ToString("0.00"));
                }


                var quickQuotePrice = crmQuoteItem.QuickQuoteData.Price;
                if (crmQuoteItem.QuickQuoteData.Regret)
                {
                    thisItemHtml.Replace("[CurrencySymbol]", string.Empty);
                }
                else
                {
                    thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                }
                thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                thisItemHtml.Replace("[UnitPrice]", quickQuoteItemUnitPrice.ToString("0.00") + $" / {quickQuoteItemUom}");

                if (crmQuoteItem.QuickQuoteData.FinishedProduct != null)
                {
                    thisItemHtml.Replace("[ProductCode]", crmQuoteItem.QuickQuoteData.FinishedProduct.ProductCode);
                }
                else
                {
                    thisItemHtml.Replace("[ProductCode]", crmQuoteItem.QuickQuoteData.Label);
                }
            }
        }

        protected override string GetFinalPageFooter()
        {
            var items = _quote.Items.Select(x => x.AsQuoteItem()).ToList();
            var quoteTotal = new QuoteTotal(items, false);

            var footer = GetPageFooter();

            footer.Replace("[OrderTotal]", $"{CurrencySymbol}{quoteTotal.TotalPrice:#,##0.00}");

            footer.Replace("[Validity]", _quote.ValidityDays + " Days");
            footer.Append($@"<div style='width:100%; margin-top:10px;'><center> PAGE <strong>{OnPage}</strong> of <strong>{GetPageCount()}</strong></center></div>");

            //var quoteItems = _quote.Items.Select(x => x.AsQuoteItem()).Where(x => !x.IsQuickQuoteItem).ToList();
            //var quickQuoteItems = _quote.Items.Select(x => x.AsQuoteItem())
            //    .Where(x => x.IsQuickQuoteItem).ToList();

            footer.Replace("[DisplayCurrency]", _quote.DisplayCurrency);

            return footer.ToString();
        }

        public string PageHeaderEurope
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
                                211 Carbrook Street, Sheffield S9 2JN<br />
                                <strong>Tel:</strong> +44(0)114 244 6711<br />
                                3 Blairlinn Road, Cumbernauld G67 2TF<br />
                                <strong>Tel:</strong> +44 (0)123 645 4111
                           </td>
                    
                            <td style='text-align: right; '>
                            <span style='font-size: 25px;'>[INQUIRY_OR_ORDER]</span><br /> 
                            <span style='font-size: 18px;'># [ORDER_ID] REV. [RevisionId]</span><br />
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
                 <td colspan='2' style=' width:35%; border-bottom:1px solid #ccc; font-size: 13px;'>[PaymentTerms]</td>
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
                        <th class='grid_top'>Sizes/ Drawings</th>
                        <th class='grid_top'>Pieces</th>
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

        public string LineItemsEurope
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
                       <tr >
                        <td class='grid_top' style=' border-top:1px solid #ABB4C9;  border-left:1px solid #ABB4C9;'>[#]</td>
                            <td class='grid_top' style='font-size:13px; border-top:1px solid #ABB4C9; border-left:none;'>[PartNumber]</td>
                            <td class='grid_top' style='font-size:13px; border-top:1px solid #ABB4C9; border-left:none; text-align: left; width:30%;'><b>[ProductCode]</b></td>
                            <td class='grid_top' style='font-size:13px;  border-top:1px solid #ABB4C9; border-left:none;'>[Pieces]</td>
                            <td class='grid_top' style='font-size:13px;  border-top:1px solid #ABB4C9; border-left:none;'>[Quantity]</td>
                            <td class='grid_top' style='font-size:13px;  border-top:1px solid #ABB4C9; border-left:none;'>[UOM]</td>
                            <td class='grid_top' style='font-size:13px;  border-top:1px solid #ABB4C9; border-left:none;'><strong>[CurrencySymbol][UnitPrice]</strong></td>
                            <td class='grid_top' style='font-size:13px;  border-top:1px solid #ABB4C9; border-left:none;'>[CurrencySymbol][ItemTotal]</td>
                            <td class='grid_top' style='font-size:13px;  border-top:1px solid #ABB4C9; border-left:none; width:70px'>[LeadTime]</td>
                    </tr>
                    [PartSpecSeparator]
                    [Notes]
                ");
                return result.ToString();
            }
        }

        public string LineItemNotesEurope
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
					<tr>
					    <td style='border-top:none; border-bottom:none; border-left:1px solid #ABB4C9'>&nbsp;</td>
					   <td colspan='1' class='grid_top'  style=' border-top:1px solid #ABB4C9; text-align:left; width:40px;'><strong>ADDITIONAL INFORMATION</strong></td>
					   <td colspan='7' class='grid_top'  style=' border-top:1px solid #ABB4C9; font-size: 12px; text-align: left!important; padding: 5px 10px;'>[CustomerItemNote]</td>
					</tr>
                ");
                return result.ToString();
            }
        }

        public string NextPageEurope
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
                                211 Carbrook Street, Sheffield S9 2JN<br />
                                <strong>Tel:</strong> +44(0)114 244 6711<br />
                                3 Blairlinn Road, Cumbernauld G67 2TF<br />
                                <strong>Tel:</strong> +44 (0)123 645 4111
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
                 <td colspan='2' style=' width:35%; border-bottom:1px solid #ccc; font-size: 13px;'>[PaymentTerms]</td>
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
                        <th class='grid_top'>Sizes/ Drawings</th>
                        <th class='grid_top'>Pieces</th>
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

        public string PageFooterForEurope
        {
            get
            {
                var result = new StringBuilder();
                result.Append(@"
     <!-- Order Totals -->
        <table id='orderTable' border='0' cellspacing='0' cellpadding='0' class='totalsTable' style='margin-top: 20px;'>

           <!--  <tr style='border-bottom: none;'>
                <td align='center' width='80%' colspan='12' style='text-align: right; border-right:none;'>
                 <span style='text-align: right; font-size: 14px; padding-top: 8px;'>Currency:</span>
                 </td>
                  <td style='text-align: right; font-size: 14px; font-weight: normal; padding-top: 8px;'>[DisplayCurrency]</td>
            </tr> -->
                          <tr>
                <td align='center' width='80%' colspan='12' style='text-align: right; border-right:none;'>
                 <span style='text-align: right; font-size: 18px; font-weight: 700; padding-top: 8px;'>TOTAL:</span>
                 </td>
                  <td style='text-align: right; font-size: 18px; font-weight: 700; padding-top: 8px;'>[OrderTotal]</td>
            </tr>
        </table>
        <!--[ORDERCOMMENTS]-->
           
        
<!--Legal-->
 <div id='divLegal'>
    <p><b>CONDITIONS:</b><br>
    <span class='validity'>Quote valid for [Validity], [ValidityDate] and subject to raw material remaining unsold and subject to Incoterms&reg;</span> At this time, due to the potential Article 232 actions, Howco reserves the right to modify any part of this quotation. Thank you for the opportunity to offer our services in support of your business. All items are subject to price in effect at time of shipment unless we have specifically noted otherwise. Lead time offered is based as at time of this quotation. Any process not specifically quoted in the above price may be subject to additional charges. Please refer to our website at <a href='http://www.howcogroup.com' target='_blank'>www.howcogroup.com</a> for full terms and conditions.</p>
    <p><b>TERMS OF PAYMENT:</b> Invoices are issued as of the date of delivery covering deliveries from our stocks, and as of the date of the shipment covering direct mill shipments. The acceptance of any order or specification and terms of payment on all sales and orders is subject to approval of the Seller's Credit Department, and Seller may at any time decline to make any shipment or delivery or perform any work except upon receipt of payment or security, or upon terms and conditions satisfactory to Seller's Credit Department.</p>
    <p>Where applicable orders placed may be subject to increases in base prices and surcharge (including scrap, alloy, graphite electrodes, refractory materials and energy) applicable at the month of despatch.
Please note prices quoted may be subject to additional charges resulting from changes in duty, tariffs, currency fluctuations, documentation requirements and any handling costs incurred as a direct result of United Kingdom exiting the EU.</p>
</div>  
                ");
                return result.ToString();
            }
        }


    }
}