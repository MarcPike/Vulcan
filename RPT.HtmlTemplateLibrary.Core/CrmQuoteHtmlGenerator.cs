//using SelectPdf;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using DAL.Vulcan.Mongo.Base.Core.DateTimeUtils;
using DAL.Vulcan.Mongo.Core.DocClass.Locations;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Extensions;
using DAL.Vulcan.Mongo.Core.Models;
using OpenHtmlToPdf;
using SelectPdf;
using Vulcan.IMetal.Helpers;

namespace RPT.HtmlTemplateLibrary.Core
{
    public class CrmQuoteHtmlGenerator : IDisposable
    {
        //private Object lockObject = new Object();

        public string QuoteId { get; set; }
        protected readonly CrmQuote _quote;
        protected readonly string _contactName;
        protected readonly string _contactEmail;
        protected readonly string _contactPhone;
        //private static readonly RepositoryBase<HtmlTemplate> Repository = new RepositoryBase<HtmlTemplate>();
        //private static readonly HelperPerson HelperPerson = new HelperPerson();
        //private static readonly HelperUser HelperUser = new HelperUser(HelperPerson);
        public string Html => _htmlBuilder.ToString();
        protected readonly int _pdfRowsPerPage = 6;

        private readonly StringBuilder _htmlBuilder = new StringBuilder();
        protected readonly CrmQuotesHtml _html = new CrmQuotesHtml();
        private readonly HelperCurrencyForIMetal _helperCurrency= new HelperCurrencyForIMetal();

        protected readonly string CurrencySymbol;

        protected virtual int GetPageCount() 
        {
            if (_quote == null) return 0;
            var pageCount = ((_quote.Items.Count) / _pdfRowsPerPage);
            var modPageCount = ((_quote.Items.Count) % _pdfRowsPerPage);
            if (modPageCount > 0)
            {
                pageCount++;
            }

            return pageCount;
        }
        protected int OnPage = 1;

        //public MemoryStream GetAsPdfDocument()
        //{

        //    //bool lockWasTaken = false;
        //    try
        //    {
        //        //System.Threading.Monitor.Enter(lockObject, ref lockWasTaken);

        //        var result = new MemoryStream();
        //        HtmlToPdf convertor = new HtmlToPdf();
        //        //convertor.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //        convertor.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
        //        convertor.Options.PdfPageSize = PdfPageSize.Letter;

        //        PdfDocument doc = convertor.ConvertHtmlString(Html);

        //        doc.Save(result);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //finally
        //    //{
        //    //    if (lockWasTaken) System.Threading.Monitor.Exit(lockObject);
        //    //}

        //    //throw new Exception("Uncaught error");
        //}

        public MemoryStream GetAsPdfDocument()
        {



            //bool lockWasTaken = false;
            try
            {
                //System.Threading.Monitor.Enter(lockObject, ref lockWasTaken);

                var pdf = Pdf.From(Html).Content();

                var result = new MemoryStream(pdf.ToArray());

                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{
            //    if (lockWasTaken) System.Threading.Monitor.Exit(lockObject);
            //}

            //throw new Exception("Uncaught error");
        }


        public static CrmQuoteHtmlGenerator GetQuoteHtmlGenerator(CrmQuote quote, string contactName, string contactEmail,
            string contactPhone)
        {

            //if (quote.QuoteId == 16400) throw new Exception("Out of Memory");

            if (quote.Team.Name == "Norway Sales Team")
            {
                return new CrmQuoteHtmlGeneratorNorwaySalesTeam(quote, contactName, contactEmail, contactPhone);
            }

            if (quote.Team.Name == "UK Sales Team")
            {
                return new CrmQuoteHtmlGeneratorUkSalesTeam(quote, contactName, contactEmail, contactPhone);
            }

            if (quote.Coid == "CAN") return new CrmQuoteHtmlGeneratorCan(quote, contactName, contactEmail, contactPhone);
            //if (quote.Coid == "EUR") return new CrmQuoteHtmlGeneratorEur(quote, contactName, contactEmail, contactPhone);
            if (quote.Coid == "SIN") return new CrmQuoteHtmlGeneratorSin(quote, contactName, contactEmail, contactPhone);
            if (quote.Coid == "MSA") return new CrmQuoteHtmlGeneratorMsa(quote, contactName, contactEmail, contactPhone);
            if (quote.Coid == "DUB") return new CrmQuoteHtmlGeneratorDub(quote, contactName, contactEmail, contactPhone);

            return new CrmQuoteHtmlGeneratorInc(quote, contactName, contactEmail, contactPhone);
        }

        protected CrmQuoteHtmlGenerator(CrmQuote quote, string contactName, string contactEmail, string contactPhone)
        {
            _quote = quote;
            _contactName = contactName;
            _contactEmail = contactEmail;
            _contactPhone = contactPhone;

            var displayCurrency = quote.DisplayCurrency;
            CurrencySymbol = _helperCurrency.GetSymbolForCurrency(displayCurrency).Symbol;

            _pdfRowsPerPage = quote.PdfRowsPerPage;

            //var exchangeRates = StockItemsAdvancedQuery.GetExchangeRatesFromCoid(quote.Coid);

            var css = _html.Css;
            var header = GetQuoteHeader();
            var lineItems = GetQuoteItems();
            var footer = GetFinalPageFooter();
            var closingTags = _html.ClosingTags;

            _htmlBuilder.Append(css);
            _htmlBuilder.Append(header);
            _htmlBuilder.Append(lineItems);
            _htmlBuilder.Append(footer);
            _htmlBuilder.Append(closingTags);


        }

        protected virtual StringBuilder GetPageFooter()
        {
            var pageFooter = new StringBuilder(_html.PageFooter);

            GetHtmlForValidityDate(pageFooter, _quote);

            return pageFooter;
        }

        protected virtual (string WeightSuffix, decimal Weight) GetWeightValues(List<CrmQuoteItem> quoteItems, List<CrmQuoteItem> quickQuoteItems, List<CrmQuoteItem> machinedParts, List<CrmQuoteItem> crozCalcs)
        {
            var weightSuffix = " lbs";
            var weight = quoteItems?.Sum(x => x.QuotePrice?.FinalPriceOverride?.FinalQuantity?.TotalPounds() ?? 0) ?? 0;
            weight += quickQuoteItems?.Sum(x => x.QuickQuoteData?.GetWeightInPounds() ?? 0) ?? 0;

            weight += machinedParts?.Sum(x => x.MachinedPartModel.PieceWeightLbs * x.MachinedPartModel.Pieces) ?? 0;

            weight += crozCalcs?.Sum(x => x.CrozCalcItem.TotalWeight) ?? 0;

            return (weightSuffix, weight);

        }

        protected virtual string GetFinalPageFooter()
        {
            var items = _quote.Items.Select(x => x.AsQuoteItem()).ToList();
            var quoteTotal = new QuoteTotal(items, false);

            var footer = GetPageFooter();

            footer.Replace("[OrderTotal]", $"{CurrencySymbol}{quoteTotal.TotalPrice:#,##0.00}");

            footer.Replace("[Validity]", _quote.ValidityDays + " Days");
            footer.Append($@"<div style='width:100%; margin-top:10px;'><center> PAGE <strong>{OnPage}</strong> of <strong>{GetPageCount()}</strong></center></div>");

            // normal
            var quoteItems = _quote.Items.Select(x => x.AsQuoteItem()).Where(x => !x.IsQuickQuoteItem && !x.IsMachinedPart).ToList();

            // quick
            var quickQuoteItems = _quote.Items.Select(x => x.AsQuoteItem())
                .Where(x => x.IsQuickQuoteItem).ToList();

            // machined
            var machinedParts = _quote.Items.Select(x => x.AsQuoteItem())
                .Where(x => x.IsMachinedPart).ToList();

            // croz calc items
            var crozCalcs = _quote.Items.Select(x => x.AsQuoteItem())
                .Where(x => x.IsCrozCalc).ToList();

            var weightValues = GetWeightValues(quoteItems, quickQuoteItems, machinedParts, crozCalcs);

            footer.Replace("[WeightTotal]", weightValues.Weight.ToString("F", CultureInfo.InvariantCulture) + weightValues.WeightSuffix);


            GetHtmlForValidityDate(footer, _quote);

            return footer.ToString();
        }

        protected virtual string GetQuoteHeader()
        {

            var salesPerson = _quote.SalesPerson.AsCrmUser();

            Location salesPersonLocation = salesPerson.User.AsUser().Location.AsLocation();
            if (!salesPerson.UseMyLocationForPdf)
            {
                var teamRef = _quote.Team ?? salesPerson.ViewConfig.Team;

                var team = teamRef.AsTeam();
                salesPersonLocation = team.Location.AsLocation();
            }

            var pageHeader = (OnPage <= 1) ? GetPageHeader() : GetNextPage();

            GetQuoteAddress(pageHeader, salesPersonLocation);

            pageHeader.Replace("[SalesPerson]", _quote.SalesPerson.FullName);

            var paymentTerm = _quote.PaymentTerm;
            var freightTerm = _quote.FreightTerm;
            if (((_quote.Coid == "MSA") || (_quote.Coid == "SIN") || (_quote.Coid == "DUB")) && _quote.IsProspect)
            {
                //paymentTerm = "100% TT in Advance";
                paymentTerm = "";
                freightTerm = "Ex Works";

            }

            pageHeader.Replace("[PaymentTerms]", paymentTerm);
            pageHeader.Replace("[FreightTerms]", freightTerm);


            pageHeader.Replace("[INQUIRY_OR_ORDER]", "Quote");
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

        protected virtual void GetHtmlForValidityDate(StringBuilder pageFooter, CrmQuote quote)
        {
            var token = "[ValidityDate]";
            var validityDate = quote.ValidityDate;
            if (validityDate == null)
            {
                pageFooter.Replace(token, " from date of quotation");  // should only appear on Drafts I think
                return;
            }


            validityDate = DateTimeUtilities.OffsetDateTimeForCoid(quote.Coid, validityDate.Value);
            pageFooter.Replace(token, validityDate.Value.ToString("dd MMM yyyy"));
        }

        protected virtual StringBuilder GetPageHeader()
        {
            return new StringBuilder(_html.PageHeader);
        }

        protected virtual StringBuilder GetNextPage()
        {
            return new StringBuilder(_html.NextPage);
        }

        protected virtual void GetQuoteAddress(StringBuilder pageHeader, Location salesPersonLocation)
        {
            pageHeader.Replace("[AddressLine1]", salesPersonLocation.Addresses.First().AddressLine1);
            pageHeader.Replace("[City]", salesPersonLocation.Addresses.First().City);
            pageHeader.Replace("[StateProvince]", salesPersonLocation.Addresses.First().StateProvince ?? "");
            pageHeader.Replace("[PostalCode]", salesPersonLocation.Addresses.First().PostalCode);
            pageHeader.Replace("[Phone]", salesPersonLocation.Phone);
            pageHeader.Replace("[PhoneTollFree]", salesPersonLocation.PhoneTollFree ?? "+1 800 392 7720");
            pageHeader.Replace("[Fax]", salesPersonLocation.Fax);
        }

        protected virtual string FormatDateForCoid(DateTime dateTime)
        {

            // Offset date for Coid
            dateTime = DateTimeUtilities.OffsetDateTimeForCoid(_quote.Coid, dateTime);

            // Get CultureInfo for Coid
            //var cultureInfo = DateTimeUtilities.GetCultureInfoForCoid(_quote.Coid);

            var result = dateTime.ToString("ddd, dd MMM yyyy h:mm tt");

            return result;
        }


        protected virtual string GetRevisionHtml(CrmQuote quote)
        {
            StringBuilder revision = new StringBuilder(_html.Revisions);

            revision.Replace("[RevisionNotes]", quote.CurrentRevision.RevisionNotesForPdf);

            return revision.ToString();
        }


        protected virtual string GetQuoteItems()
        {
            var helperCurrency = new HelperCurrencyForIMetal();
            var currencyValues = helperCurrency.GetSymbolForCurrency(_quote.DisplayCurrency);


            var baseHtmlForItem = _html.LineItems; //GetHtmlForTemplate(_quote.Coid, "CrmQuoteItem");
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

                GenerateMachinedPartsHtml(crmQuoteItem, helperCurrency, thisItemHtml);

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

        protected virtual void GenerateNormalQuoteItemHtml(CrmQuoteItem crmQuoteItem, HelperCurrencyForIMetal helperCurrency,
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

        protected virtual void GenerateHtmlForNoProductCode(CrmQuoteItem crmQuoteItem, StringBuilder thisItemHtml)
        {
            if (crmQuoteItem.IsQuickQuoteItem) return;

            if (!crmQuoteItem.ShowProductCodeOnQuote)
            {
                thisItemHtml.Replace("[ProductCode]", string.Empty);

                if (!String.IsNullOrEmpty(crmQuoteItem.PartSpecification))
                {
                    var separator = _html.PartSpecSeperatorNoProduct;
                    separator = separator.Replace("[PartSpec]", crmQuoteItem.PartSpecification).Trim();
                    thisItemHtml.Replace("[PartSpecSeparator]", separator.Trim());
                }
                else
                {
                    thisItemHtml.Replace("[PartSpec]", string.Empty);
                    thisItemHtml.Replace("[PartSpecSeparator]", string.Empty);
                }
            }
        }

        protected virtual string ConvertSpecialCharactersToHtml(string input)
        {
            if (input == null) input = string.Empty;
            byte[] bytes = Encoding.Default.GetBytes(input);
            input = Encoding.UTF8.GetString(bytes);

            var result = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                var thisChar = input.Substring(i, 1);
                if (thisChar == "\"") // Why is this not working
                {
                    result += "&quot;";
                }
                else if (thisChar == "\'")
                {
                    result += "&apos;";
                }
                else
                {
                    result += thisChar;
                }
            }

            return result;
        }

        protected virtual void GenerateHtmlForProductCode(CrmQuoteItem crmQuoteItem, StringBuilder thisItemHtml)
        {

            if (crmQuoteItem.ShowProductCodeOnQuote)
            {
                if (crmQuoteItem.IsQuickQuoteItem)
                {
                    if (crmQuoteItem.QuickQuoteData.Label == null)
                    {
                        thisItemHtml.Replace("[ProductCode]", string.Empty);
                    }
                    else
                    {
                        thisItemHtml.Replace("[ProductCode]", ConvertSpecialCharactersToHtml(crmQuoteItem.QuickQuoteData.Label));
                    }
                }
                else if (crmQuoteItem.IsCrozCalc)
                {
                    thisItemHtml.Replace("[ProductCode]", crmQuoteItem.CrozCalcItem.FinishedProductLabel);
                }
                else if (crmQuoteItem.IsMachinedPart)
                {
                    if (crmQuoteItem.MachinedPartModel.Label == null)
                    {
                        thisItemHtml.Replace("[ProductCode]", string.Empty);
                    }
                    else
                    {
                        thisItemHtml.Replace("[ProductCode]", ConvertSpecialCharactersToHtml(crmQuoteItem.MachinedPartModel.Label));
                    }
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

        protected virtual void GetHtmlForQuantityAndPrice(CrmQuoteItem crmQuoteItem, StringBuilder thisItemHtml)
        {
            if ((crmQuoteItem.IsQuickQuoteItem) || (crmQuoteItem.IsMachinedPart) || (crmQuoteItem.IsCrozCalc)) return;

            crmQuoteItem.QuotePrice.FinalPriceOverride.CustomerUom =
                crmQuoteItem.CalculateQuotePriceModel.CustomerUom;
            var unitPrice = crmQuoteItem.QuotePrice.FinalPriceOverride.FinalUnitCost;
            thisItemHtml.Replace("[#]", crmQuoteItem.Index.ToString());
            thisItemHtml.Replace("[Pieces]", crmQuoteItem.QuotePrice.RequiredQuantity.Pieces.ToString());
            thisItemHtml.Replace("[Quantity]",
                crmQuoteItem.QuotePrice.FinalPriceOverride.FinalQuantityValue.RoundAndNormalize(3).ToString("0.000"));

            thisItemHtml.Replace("[UOM]", crmQuoteItem.QuotePrice.FinalPriceOverride.FinalUom);
            thisItemHtml.Replace("[UnitPrice]",
                crmQuoteItem.QuotePrice.FinalPriceOverride.FinalUnitCost.RoundAndNormalize(2).ToString("0.00") +
                crmQuoteItem.QuotePrice.FinalPriceOverride.FinalUnitCostSuffix);

        }

        protected virtual void GenerateHtmlForNotes(CrmQuoteItem crmQuoteItem, StringBuilder thisItemHtml)
        {
            if (!String.IsNullOrEmpty(crmQuoteItem.CustomerNotes))
            {
                var lineItemNotes = _html.LineItemNotes;

                lineItemNotes = lineItemNotes.Replace("[CustomerItemNote]", crmQuoteItem.CustomerNotes.Replace("\n", "<br>"));
                thisItemHtml.Replace("[Notes]", lineItemNotes);
            }
            else
            {
                thisItemHtml.Replace("[Notes]", _html.LineItemNoNotes);
            }
        }

        protected virtual void ShowItemTotalForAllItems(CrmQuoteItem crmQuoteItem, HelperCurrencyForIMetal helperCurrency,
            StringBuilder thisItemHtml)
        {
            var itemCurrencyValues = helperCurrency.GetSymbolForCurrency(_quote.DisplayCurrency);
            thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);

            if (crmQuoteItem.IsQuickQuoteItem)
            {

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
                if (crmQuoteItem.CrozCalcItem.Regret)
                {
                    thisItemHtml.Replace("[CurrencySymbol]", string.Empty);
                    thisItemHtml.Replace("[ItemTotal]", "REGRET");
                }
                else
                {
                    thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                    thisItemHtml.Replace("[ItemTotal]", crmQuoteItem.CrozCalcItem.TotalPrice.ToString("#,##0.00"));
                }
            }
            else if (crmQuoteItem.IsMachinedPart)
            {
                thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                thisItemHtml.Replace("[ItemTotal]", crmQuoteItem.MachinedPartModel.TotalPrice.ToString("#,##0.00"));
            }
            else
            {
                thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                thisItemHtml.Replace("[ItemTotal]", crmQuoteItem.QuotePrice.FinalPrice.ToString("#,##0.00"));
            }

            thisItemHtml.Replace("[LeadTime]", crmQuoteItem.LeadTime ?? "TBD");

        }

        protected virtual void ShowOutsideAndInsideDiametersForAllQuoteItems(CrmQuoteItem crmQuoteItem, StringBuilder thisItemHtml)
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
                thisItemHtml.Replace("[OutsideDiameter]", crmQuoteItem.CrozCalcItem.StartingProductLabel);
                thisItemHtml.Replace("[InsideDiameter]", crmQuoteItem.CrozCalcItem.FinishedProductLabel);
            }
            else if (crmQuoteItem.IsMachinedPart)
            {
                thisItemHtml.Replace("[OutsideDiameter]", "n/a");
                thisItemHtml.Replace("[InsideDiameter]", "n/a");
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

        protected virtual void GenerateQuickQuoteItemHtml(CrmQuoteItem crmQuoteItem, HelperCurrencyForIMetal helperCurrency,
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

                thisItemHtml.Replace("[#]", crmQuoteItem.Index.ToString());
                thisItemHtml.Replace("[Pieces]", crmQuoteItem.QuickQuoteData.OrderQuantity.Pieces.ToString());
                if (crmQuoteItem.QuickQuoteData.OrderQuantity.EachBased)
                {
                    thisItemHtml.Replace("[Quantity]", "piece(s)");
                }
                else
                {
                    thisItemHtml.Replace("[Quantity]", quickQuoteItemQuantity.ToString("0.000"));
                }
                thisItemHtml.Replace("[UOM]", quickQuoteItemUom);


                var quickQuotePrice = crmQuoteItem.QuickQuoteData.Price;
                if (crmQuoteItem.QuickQuoteData.Regret)
                {
                    thisItemHtml.Replace("[CurrencySymbol]", string.Empty);
                }
                else
                {
                    thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                }
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

        protected virtual void GenerateCrozCalcItemHtml(CrmQuoteItem crmQuoteItem, HelperCurrencyForIMetal helperCurrency,
            StringBuilder thisItemHtml)
        {
            if (crmQuoteItem.IsCrozCalc)
            {
                var itemCurrencyValues = helperCurrency.GetSymbolForCurrency(_quote.DisplayCurrency);

                var itemQuantity =
                    crmQuoteItem.CrozCalcItem.OrderQuantity.Quantity;
                var unitPrice =
                    crmQuoteItem.CrozCalcItem.UnitPrice;

                thisItemHtml.Replace("[#]", crmQuoteItem.Index.ToString());
                thisItemHtml.Replace("[Pieces]", crmQuoteItem.CrozCalcItem.OrderQuantity.Pieces.ToString());
                var quantity = crmQuoteItem.CrozCalcItem.OrderQuantity.Quantity *
                               crmQuoteItem.CrozCalcItem.OrderQuantity.Pieces;
                if (crmQuoteItem.CrozCalcItem.OrderQuantity.QuantityType == "ea")
                {
                    thisItemHtml.Replace("[Quantity]", "piece(s)");
                }
                else
                {
                    thisItemHtml.Replace("[Quantity]", quantity.ToString());
                }
                thisItemHtml.Replace("[UOM]", crmQuoteItem.CrozCalcItem.OrderQuantity.QuantityType);

                if (crmQuoteItem.CrozCalcItem.Regret)
                {
                    thisItemHtml.Replace("[CurrencySymbol]", string.Empty);
                    thisItemHtml.Replace("[ItemTotal]", "REGRET");
                }

                thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                thisItemHtml.Replace("[UnitPrice]", unitPrice.ToString("0.00") + " / " + crmQuoteItem.CrozCalcItem.OrderQuantity.QuantityType);


                thisItemHtml.Replace("[ProductCode]", crmQuoteItem.CrozCalcItem.FinishedProductLabel);
            }
        }


        protected virtual void GenerateMachinedPartsHtml(CrmQuoteItem crmQuoteItem, HelperCurrencyForIMetal helperCurrency,
            StringBuilder thisItemHtml)
        {
            if (crmQuoteItem.IsMachinedPart)
            {
                var itemCurrencyValues = helperCurrency.GetSymbolForCurrency(_quote.DisplayCurrency);

                thisItemHtml.Replace("[#]", crmQuoteItem.Index.ToString());
                thisItemHtml.Replace("[Pieces]", crmQuoteItem.MachinedPartModel.Pieces.ToString());
                thisItemHtml.Replace("[Quantity]", "piece(s)");
                thisItemHtml.Replace("[UOM]", "each");


                thisItemHtml.Replace("[CurrencySymbol]", itemCurrencyValues.Symbol);
                thisItemHtml.Replace("[UnitPrice]", crmQuoteItem.MachinedPartModel.PiecePrice.ToString("0.00") + $" / each");

                thisItemHtml.Replace("[ProductCode]", crmQuoteItem.MachinedPartModel.Label);

            }
        }

        //private (string Currency, string Symbol, decimal ExchangeRate) GetCurrencyTypeSymbolAndExchangeRate(CrmQuoteItem item)
        //{
        //    var currencyValues = GetDefaultCurrencyForCoid(_quote.Coid);
        //    var currency = currencyValues.Currency;
        //    var symbol = currencyValues.Symbol;
        //    var exchangeRate = (decimal) 1;

        //    var displayCurrency = (_quote.DisplayCurrency == String.Empty) ? currency : _quote.DisplayCurrency;

        //    if (item.QuotePrice.StartingProduct.Coid == displayCurrency) 

        //}

        protected virtual StringBuilder HandleNewPage(StringBuilder finalHtmlForItems)
        {
            finalHtmlForItems = HandlePageCount(finalHtmlForItems);
            OnPage++;
            finalHtmlForItems = HandleNewPageHeader(finalHtmlForItems);

            return finalHtmlForItems;
        }

        protected virtual StringBuilder HandlePageCount(StringBuilder finalHtmlForItems)
        {

            var newPageHtml = _html.NewPageNumber;
            newPageHtml = newPageHtml.Replace("[OnPage]", OnPage.ToString());
            newPageHtml = newPageHtml.Replace("[PageCount]", GetPageCount().ToString());
            finalHtmlForItems.Append(newPageHtml);
            return finalHtmlForItems;
        }


        protected virtual StringBuilder HandleNewPageHeader(StringBuilder finalHtmlForItems)
        {
            StringBuilder pageHeader = new StringBuilder();
            pageHeader.Append(GetQuoteHeader());
            return finalHtmlForItems.Append(pageHeader);
        }


        public void Dispose()
        {
            _htmlBuilder.Clear();
            _html.Dispose();
        }
    }
}
