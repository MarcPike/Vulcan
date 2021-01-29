using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.EMail;
using DAL.IntegrationDb;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.DocClass.Locations;
using DAL.Vulcan.Mongo.DocClass.Quotes;
using DAL.Vulcan.Mongo.Extensions;
using DAL.Vulcan.Mongo.Models;
using DAL.Vulcan.Mongo.PublishSignalR;
using Vulcan.IMetal.Queries.Companies;
using Vulcan.IMetal.Queries.GeneralInfo;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace Vulcan.iMetal.Quote.Export.Repository
{
    /// <summary>
    /// Exports quotes from the Howco Quote system into iMetal.
    /// </summary>
    public class QuoteExporter
    {


        private const bool DisableCostExport = true;
        private static readonly string IMPORT_SOURCE = "QuoteTool";



        /// <summary>
        /// Generates the required import records for the specified Quote, and then writes them to the iMetal import database.
        /// </summary>
        /// <param name="quote">The Quote to send to iMetal.</param>
        /// <param name="writeToImportDatabase">Set to false for testing only. If false, then it will only generate the import_sales_headers, 
        /// but it won't write to the import database table.</param>
        /// <returns></returns>
        public bool Export(CrmQuote quote, bool writeToImportDatabase)
        {
            var qcMode = EnvironmentSettings.CurrentEnvironment == Environment.Development;

            var quoteItems = new List<CrmQuoteItem>();

            //Add Items and Charges
            foreach (var quoteItem in quote.Items.Where(x=>!x.IsQuickQuoteItem).Select(x=>x.AsQuoteItem()))
            {
                if (quoteItem.IsLost) continue;

                if (quoteItem.IsMachinedPart) continue;
                if (quoteItem.IsCrozCalc) continue;

                quoteItems.Add(quoteItem);
            }

            if (quoteItems.Count == 0)
            {
                quote.ExportStatus = ExportStatus.Failed;
                var thisExportAttempt = new ExportAttempt
                {
                    Errors = "No items found that could be exported"
                };
                quote.ExportAttempts.Add(thisExportAttempt);
                CrmQuote.Helper.Upsert(quote);
                var exception = new Exception("No items found that could be exported to iMetal");
                SendExceptionEMailToSalesPerson(quote, exception, thisExportAttempt);
                throw exception;
            }

            import_sales_headers importHeader;
            try
            {
                importHeader = GetImportSalesHeader(quote, qcMode);
                foreach (var quoteItem in quoteItems)
                {
                    AddImportSalesItem(importHeader, quoteItem);
                    AddImportSalesChargesAndCosts(importHeader, quoteItem);
                }

            }
            catch (Exception e)
            {
                quote.ExportStatus = ExportStatus.Failed;
                var thisExportAttempt = new ExportAttempt();
                thisExportAttempt.Errors =
                    "No data was sent to iMetal due to the following validation errors:\r\n" + e.Message;
                quote.ExportAttempts.Add(thisExportAttempt);
                SendExceptionEMailToSalesPerson(quote, e, thisExportAttempt);
                quote.SaveToDatabase();
                throw;
            }

            //Write import records to Database.
            if (!writeToImportDatabase)
            {
                //Diagnostic Dump
                Console.WriteLine("ERROR: Unable to write to Import Database");
                Console.WriteLine(ObjectDumper.Dump(importHeader));
                return true;
            }

            var exportAttempt = new ExportAttempt(importHeader);
            try
            {
                using (var context = new IntegrationDb())
                {
                    context.import_sales_headers.Add(importHeader);
                    importHeader.Items.ForEach(x => context.import_sales_items.Add(x));
                    importHeader.Charges.ForEach(x => context.import_sales_charges.Add(x));
                    if (!DisableCostExport)
                    {
                        //importHeader.Costs.ForEach(x => context.import_sales_costs.Add(x));
                    }
                    context.SaveChanges();

                    quote.ExportStatus = ExportStatus.Processing;
                    quote.ExportAttempts.Add(exportAttempt);
                    quote.SaveToDatabase();
                    try
                    {
                        var task = PublishSignalREvents.QuoteExportStatusChanged(quote.Id.ToString());
                        task.Wait();
                    }
                    catch (Exception e)
                    {
                        SendExceptionEMailToSalesPerson(quote, e, exportAttempt);
                        Console.WriteLine(e);
                    }
                }
            }
            catch (DbEntityValidationException dbve)
            {
                var errors = GetEntityValidationErrors(dbve);
                quote.ExportStatus = ExportStatus.Failed;
                exportAttempt.Errors =
                    "No data was sent to iMetal due to the following validation errors:\r\n" + errors;
                quote.ExportAttempts.Add(exportAttempt);
                quote.SaveToDatabase();
                SendExceptionEMailToSalesPerson(quote, dbve, exportAttempt);
                var task = PublishSignalREvents.QuoteExportStatusChanged(quote.Id.ToString());
                task.Wait();

                throw;
            }
            catch (Exception ex)
            {
                quote.ExportStatus = ExportStatus.Failed;
                exportAttempt.Errors =
                    "No data was sent to iMetal due to the following errors:\r\n" + ex.Message;
                quote.ExportAttempts.Add(exportAttempt);
                quote.SaveToDatabase();
                SendExceptionEMailToSalesPerson(quote, ex, exportAttempt);
                var task = PublishSignalREvents.QuoteExportStatusChanged(quote.Id.ToString());
                task.Wait();

                throw;
            }

            return true;
        }

        private void SendExceptionEMailToSalesPerson(CrmQuote crmQuote, Exception exception, ExportAttempt exportAttempt)
        {
            var subject = $"IMetal Export Failed for QuoteId {crmQuote.QuoteId}";
            var body = exportAttempt.Errors;
            var exportUser = crmQuote.ExportRequestedBy.AsCrmUser();
            var emailAddresses = new List<string>();
            var exportUserEmail = exportUser.User.AsUser().Person?.EmailAddresses?
                .FirstOrDefault(x => x.Type == EmailType.Business).Address;
            if (exportUserEmail != null)
            {
                emailAddresses.Add(exportUserEmail);
            }
            emailAddresses.Add("marc.pike@howcogroup.com");
            SendEMail.Execute(subject, emailAddresses, body);
        }


        //public static string GetImportStatusNotes(string importCompanyReference, int importBatchNumber)
        //{
        //    using (var context = new IntegrationDb())
        //    {
        //        var importHeader = context.import_sales_headers.FirstOrDefault(x => x.import_company_reference == importCompanyReference && x.import_batch_number == importBatchNumber);
        //        var importItems = context.import_sales_items.Where(x => x.import_company_reference == importCompanyReference && x.import_batch_number == importBatchNumber).ToList();
        //        var importCharges = context.import_sales_charges.Where(x => x.import_company_reference == importCompanyReference && x.import_batch_number == importBatchNumber).ToList();

        //        var tabs = new List<string> { "", "\t", "\t\t" };

        //        if (importHeader == null)
        //        {
        //            return $"No import header with Import Company Reference '{importCompanyReference}' and Import Batch Number '{importBatchNumber}' was found.";
        //        }

        //        var sb = new StringBuilder();

        //        //Header
        //        sb.AppendLine(importHeader.GetStatusSummary());

        //        //Items
        //        foreach (var item in importItems)
        //        {
        //            sb.AppendLine(tabs[1] + item.GetStatusSummary());

        //            //Charges
        //            var charges = importCharges.Where(x => x.import_item == item.import_item).ToList();
        //            charges.ForEach(x => sb.AppendLine(tabs[2] + x.GetStatusSummary()));
        //        }

        //        return sb.ToString();
        //    }
        //}

        //public static List<ImportStatusTreeNode> GetImportStatusTreeNodes(string importCompanyReference, int importBatchNumber)
        //{
        //    var nodes = new List<ImportStatusTreeNode>();
        //    var nodeID = 1;

        //    using (var context = EntityFrameworkDbContextProvider.GetIntegrationDbContext())
        //    {
        //        var importHeader = context.import_sales_headers.FirstOrDefault(x => x.import_company_reference == importCompanyReference && x.import_batch_number == importBatchNumber);
        //        var importItems = context.import_sales_items.Where(x => x.import_company_reference == importCompanyReference && x.import_batch_number == importBatchNumber).ToList();
        //        var importCharges = context.import_sales_charges.Where(x => x.import_company_reference == importCompanyReference && x.import_batch_number == importBatchNumber).ToList();
        //        var importCosts = context.import_sales_costs.Where(x => x.import_company_reference == importCompanyReference && x.import_batch_number == importBatchNumber).ToList();

        //        if (importHeader == null) return nodes;

        //        //Header
        //        var headerNode = new ImportStatusTreeNode(nodeID++, 0, importHeader);
        //        nodes.Add(headerNode);

        //        //Items
        //        foreach (var item in importItems)
        //        {
        //            var itemNode = new ImportStatusTreeNode(nodeID++, headerNode.ID, item);
        //            nodes.Add(itemNode);

        //            //Charges
        //            var chargeGroupNode = new ImportStatusTreeNode(nodeID++, itemNode.ID, "Charges");
        //            nodes.Add(chargeGroupNode);
        //            var charges = importCharges.Where(x => x.import_item == item.import_item).ToList();
        //            foreach (var charge in charges)
        //            {
        //                var chargeNode = new ImportStatusTreeNode(nodeID++, chargeGroupNode.ID, charge);
        //                nodes.Add(chargeNode);
        //            }

        //            //Costs
        //            var costGroupNode = new ImportStatusTreeNode(nodeID++, itemNode.ID, "Costs");
        //            nodes.Add(costGroupNode);
        //            var costs = importCosts.Where(x => x.import_item == item.import_item).ToList();
        //            foreach (var cost in costs)
        //            {
        //                var costNode = new ImportStatusTreeNode(nodeID++, costGroupNode.ID, cost);
        //                nodes.Add(costNode);
        //            }
        //        }

        //        return nodes;
        //    }
        //}


        private import_sales_headers GetImportSalesHeader(CrmQuote quote, bool qcMode)
        {
            //Validate prerequisites
            var customer = quote.Company ?? throw new ArgumentNullException(nameof(quote.Company));
            //var deliverTo = quote.ShipToAddress ?? throw new ArgumentNullException(nameof(quote.ShipToAddress));
            var importBatchNumber = GetNextBatchNumber.Execute(); //Exceptions to be caught in calling method.

            var team = quote.Team.AsTeam();
            var location = team.Location.AsLocation();
            var branch = location.Branch;

            var importReference = (qcMode) ? "QC" : "";
            var branchCode = branch;
            if (branch == "USA")
            {
                importReference = "HOU" + importReference;
                branchCode = "HOU";
            }
            else 
            {
                importReference = branch + importReference;
            }

            var contactRef = quote.Contact;

            if (contactRef == null) throw new Exception($"No Contact defined for this QuoteId: {quote.QuoteId}");

            var contact = contactRef.AsContact();
            var contactOfficePhone = contact.Person.PhoneNumbers.FirstOrDefault(x => x.Type == PhoneType.Office) ??
                                     contact.Person.PhoneNumbers.FirstOrDefault();

            var contactMobilePhone = contact.Person.PhoneNumbers.FirstOrDefault(x => x.Type == PhoneType.Mobile) ??
                                     contact.Person.PhoneNumbers.FirstOrDefault(x => x.Type == PhoneType.Office) ??
                                     contact.Person.PhoneNumbers.FirstOrDefault();

            var contactFaxPhone = contact.Person.PhoneNumbers.FirstOrDefault(x => x.Type == PhoneType.Fax);

            var contactEmailAddress = contact.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Business) ??
                                      contact.Person.EmailAddresses.FirstOrDefault(x => x.Type == EmailType.Personal);

            var today = DateTime.Now.Date;

            var freightTerms = FreightTerms.GetFreightTermsForCoid(quote.Coid);
            var transportTypeCode = freightTerms.FirstOrDefault(x => x.Terms == quote.FreightTerm);
            var usingDeliveryCode = (!String.IsNullOrEmpty(quote.ShipToAddress.ExternalCode));

            var importHeader = new import_sales_headers
            {
                job_number = string.Empty,
                import_batch_number = importBatchNumber,
                //import_date = DateTime.Now, -- documentation says to leave blank
                import_action = "A",
                import_status = "E",
                import_source = IMPORT_SOURCE,
                import_user_name = quote.ExportRequestedBy.FullName,
                //import_user_name = quote.SalesPerson.FullName,
                salesperson_name = quote.SalesPerson.FullName,
                inside_salesperson_name = quote.SalesPerson.FullName,
                import_number = quote.QuoteId, //This is QuoteNumber * 1000 + Version Number
                import_company_reference = importReference, // iMetal appears to be using Branch Code rather than SSID (or COID) here.
                type_code = "ORD",
                branch_code = branchCode,
                //sale_date = today, 
                
                
                //due_date = today.AddDays(3),

                customer_code = customer.Code,
                customer_order_number = quote.PoNumber,

                sales_group_code = quote.SalesGroupCode,



                //deliver_to_address_code = deliverTo.Code,
                //All other deliver_to fields default from deliver_to_address_code,
                /*
                deliver_to_address_code = (quote.ShipToAddress.Type != AddressType.ShippingNew && !String.IsNullOrEmpty(quote.ShipToAddress.ExternalCode)) ? quote.ShipToAddress.ExternalCode : null,
                deliver_to_name_override = (quote.ShipToAddress.Type == AddressType.ShippingNew) ? quote.Company.Name : null,
                deliver_to_address = (quote.ShipToAddress.Type == AddressType.ShippingNew) ? quote.ShipToAddress.AddressLine1 : null,
                deliver_to_town = (quote.ShipToAddress.Type == AddressType.ShippingNew) ? quote.ShipToAddress.City : null,
                deliver_to_county = (quote.ShipToAddress.Type == AddressType.ShippingNew) ? quote.ShipToAddress.StateProvince : null,
                deliver_to_postcode = (quote.ShipToAddress.Type == AddressType.ShippingNew) ? quote.ShipToAddress.PostalCode : null,
                deliver_to_country_code = (quote.ShipToAddress.Type == AddressType.ShippingNew) ? GetCountryCodeForCountry(quote.ShipToAddress.Country) : null,
                */

                

                deliver_to_address_code =  (usingDeliveryCode) ? quote.ShipToAddress.ExternalCode : null,
                deliver_to_name_override = (!usingDeliveryCode) ? quote.Company.Name : null,
                deliver_to_address =  (!usingDeliveryCode) ? quote.ShipToAddress.AddressLine1 : null,
                deliver_to_town =  (!usingDeliveryCode) ? quote.ShipToAddress.City : null,
                deliver_to_county =  (!usingDeliveryCode) ? quote.ShipToAddress.StateProvince : null,
                deliver_to_postcode =  (!usingDeliveryCode) ? quote.ShipToAddress.PostalCode : null,
                deliver_to_country_code = (!usingDeliveryCode) ? GetCountryCodeForCountry(quote.ShipToAddress.Country) : null,
               
                contact_forename = contactRef.FirstName,
                contact_surname = contactRef.LastName,

                contact_telephone_override = contactOfficePhone?.Number ?? string.Empty,
                contact_mobile_override = contactMobilePhone?.Number ?? string.Empty,
                contact_fax_override = contactFaxPhone?.Number ?? string.Empty,

                contact_email_override = contactEmailAddress?.Address ?? string.Empty,
                contact_creation_allowed = true,

                transport_type_code = transportTypeCode?.Code ?? null,
                //transport_cost_rate = extraCharges.FreightChargePerLb,
                //transport_cost_amount = quoteModel.TotalFreightCost,
                transport_cost_rate_unit_code = "LBS",

                //Notes
                despatch_text = quote.ExternalHeaderCrmData.DespatchNotes,
                header_text = quote.ExternalHeaderCrmData.HeaderNotes,
                internal_text = quote.ExternalHeaderCrmData.InternalNotes,
                no_fixed_date = false
            };

            if (quote.ExternalHeaderCrmData.TransportCharge != 0M)
            {
                importHeader.transport_charge_rate = quote.ExternalHeaderCrmData.TransportCharge;
                importHeader.transport_charge_rate_unit_code = "LOT";
            }


            importHeader.Items = new List<import_sales_items>();
            importHeader.Charges = new List<import_sales_charges>();
            importHeader.Costs = new List<import_sales_costs>();

            return importHeader;
        }

        private string GetCountryCodeForCountry(string country)
        {
            var query = new QueryCompany("INC");

            var countryCodes = CountryCodeModel.GetForCoid("INC");
            var countryCodeForCountry = countryCodes.SingleOrDefault(x => x.Country == country);
            return countryCodeForCountry?.Code ?? "";
        }

        private void AddImportSalesItem(import_sales_headers importHeader, CrmQuoteItem quoteItem)
        {
            var finishedProduct = quoteItem.QuotePrice?.FinishedProduct;
            if (finishedProduct == null) return;

            var dim2 = 0M;
            var dim3 = 0M;

            //TODO: Write a dimension converter to encapsulate this logic into one class.
            if (finishedProduct.ProductType.ToUpper().Contains("FLAT BAR"))
            {
                var fullProductMaster = finishedProduct.GetProductMasterFull();
                dim2 = fullProductMaster.Width;
                dim3 = fullProductMaster.Thick;
            }
            else if (finishedProduct.ProductType.ToUpper().Contains("ROUND BAR"))
            {
                dim2 = finishedProduct.OuterDiameter;
            }
            else if (finishedProduct.ProductType.ToUpper().Contains("TUBE"))
            {
                dim2 = finishedProduct.OuterDiameter;
                dim3 = finishedProduct.InsideDiameter;
            }

            var deliveryBranchCode = quoteItem.GetQuote().Company.AsCompany().Location.Branch;
            if (deliveryBranchCode == "USA") deliveryBranchCode = "HOU";

            //var kerfPounds = quoteItem.QuotePrice.MaterialCostValue.KerfTotalPounds;
            //var finishPounds = quoteItem.QuotePrice.FinishQuantity.TotalPounds();

            var testPiecePounds =
                quoteItem.CalculateQuotePriceModel.TestPieces.Sum(x => x.RequiredQuantity.TotalPounds());

            var requiredWeight = quoteItem.QuotePrice.FinishQuantity.TotalPounds();
            if ((quoteItem.Coid == "SIN") || (quoteItem.Coid == "MSA") || (quoteItem.Coid == "DUB"))
            {
                requiredWeight = quoteItem.QuotePrice.FinishQuantity.TotalKilograms();
            }



            //Create the new item.
            var newItem = new import_sales_items()
            {
                import_batch_number = importHeader.import_batch_number,
                import_company_reference = importHeader.import_company_reference,
                import_number = importHeader.import_number,
                import_item = quoteItem.Index,
                import_source = importHeader.import_source,
                sales_group_code = importHeader.sales_group_code,

                product_code = finishedProduct.ProductCode,
                dim1 = quoteItem.QuotePrice.FinishQuantity.PieceLength.Inches,
                dim2 = dim2,
                dim3 = dim3,
                required_pieces = quoteItem.QuotePrice.FinishQuantity.Pieces,
                //required_quantity = quoteItem.LengthInches,
                


                required_weight = requiredWeight,
                //weight_units_code = IMetalUomHelper.GetIMetalAbbreviationFromUom(UOM.LB), //Documentation says to ignore this field - not used in iMetal.
                delivery_branch_code = deliveryBranchCode,

                due_date = DateTime.Today.AddDays(3),
                
                //Notes
                despatch_notes = quoteItem.ExternalItemCrmData.DespatchNotes,
                production_notes = quoteItem.ExternalItemCrmData.ProductionNotes,
                works_notes = quoteItem.ExternalItemCrmData.WorkNotes,
                acknowledgement_notes = quoteItem.ExternalItemCrmData.AcknowledgementNotes,
                use_minimum_grade = false,
                part_number = quoteItem.PartNumber.Left(35),
                specification_value6 = quoteItem.OemType.Left(30),
                specification_value5 = quoteItem.PartSpecification.Left(30),
                description = quoteItem.QuotePrice.FinishedProduct.GetLongDescription(quoteItem.QuotePrice.FinishQuantity, quoteItem.PartNumber, quoteItem.PartSpecification)

            };

            //newItem.required_weight = newItem.required_weight - kerfPounds;

            importHeader.Items.Add(newItem);
        }

        private void AddImportSalesChargesAndCosts(import_sales_headers importHeader, CrmQuoteItem quoteItem)
        {
            /* From page 26 of the iMetalSalesDocumentImport.pdf:
             
               The cost group code must be one of the following:
               • MAT = Material
               • TRN = Transport
               • PRD = Production
               • MSC = Miscellaneous
               • SUR = Surcharge
             
               Charge Visibility can be (S)tand-alone, (I)ncluded, or (A)dded.
               Cost Visibility can only be (S)tand-alone.

             */

            int chargeItemNumber = 1;

            //Material Charge includes base material charge plus any other VAS charges that are rolled up (i.e. not broken out) into the material charge.
            if (quoteItem.IsCrozCalc) return;
            var customerUom = quoteItem.CalculateQuotePriceModel.CustomerUom;
            var currency = GetDefaultCurrencyForCoid(quoteItem.Coid);
            var customerUomCode = string.Empty;

            decimal basePrice;
            switch (customerUom)
            {
                case CustomerUom.Inches:
                    basePrice = (quoteItem.QuotePrice.FinalPrice / quoteItem.QuotePrice.FinishQuantity.TotalInches()).RoundAndNormalize(2);
                    customerUomCode = "IN";
                    break;
                case CustomerUom.Feet:
                    basePrice = (quoteItem.QuotePrice.FinalPrice / quoteItem.QuotePrice.FinishQuantity.TotalFeet()).RoundAndNormalize(2);
                    customerUomCode = "FT";
                    break;
                case CustomerUom.Pounds:
                    basePrice = (quoteItem.QuotePrice.FinalPrice / quoteItem.QuotePrice.FinishQuantity.TotalPounds()).RoundAndNormalize(2);
                    customerUomCode = "LBS";
                    break;
                case CustomerUom.Kilograms:
                    basePrice = (quoteItem.QuotePrice.FinalPrice / quoteItem.QuotePrice.FinishQuantity.TotalKilograms()).RoundAndNormalize(2);
                    customerUomCode = "KGS";
                    break;
                case CustomerUom.PerPiece:
                    basePrice = (quoteItem.QuotePrice.FinalPrice / quoteItem.QuotePrice.FinishQuantity.Pieces).RoundAndNormalize(2);
                    customerUomCode = "PCS";
                    break;
                default:
                    basePrice = 0;
                    break;
            }

            decimal finalPriceConvertedToCorrectCurrency = basePrice;

            //var itemRolledUpMaterialChargePerPricingUnit = 
            var newCharge = new import_sales_charges()
            {
                import_batch_number = importHeader.import_batch_number,
                import_company_reference = importHeader.import_company_reference,
                import_number = importHeader.import_number,
                import_item = quoteItem.Index,
                import_source = importHeader.import_source,

                description = "Material Charge",
                charge = basePrice,
                charge_visibility = ChargeVisibility.StandAlone,
                cost_group_code = CostGroupCode.Material,
                item_no = chargeItemNumber++,
                charge_unit_code = customerUomCode,
                
            };

            //Material cost is determined from pre-reserved stock.
            //Material Charge includes base material charge plus any other VAS charges that are rolled up (i.e. not broken out) into the material charge.
            //var itemRolledUpMaterialCostPerPricingUnit = quoteItem.GetItemTotalRolledUpMaterialCostPerPricingUnit();
            //var newCost = new import_sales_costs(importHeader, quoteItem.LineNumber)
            //{
            //    description = "Material Cost",
            //    cost = itemRolledUpMaterialCostPerPricingUnit.Value,
            //    visibility = ChargeVisibility.StandAlone,
            //    cost_group_code = CostGroupCode.Material,
            //    item_no = costItemNumber++,
            //    internal_cost = true, //TODO: Add internal/external cost functionality.
            //    cost_unit_code = IMetalUomHelper.GetIMetalAbbreviationFromUom(itemRolledUpMaterialCostPerPricingUnit.Unit).ToUpper(),
            //};

            //Add material cost and charge to header. 
            importHeader.Charges.Add(newCharge);
            //importHeader.Costs.Add(newCost);

            //Now add cost and charge for each VAS.
            //foreach (var productionCost in quoteItem.QuotePrice.ProductionCosts)
            //{
                //var productionCharge = new import_sales_charges()
                //{
                //    import_batch_number = importHeader.import_batch_number,
                //    import_company_reference = importHeader.import_company_reference,
                //    import_number = importHeader.import_number,
                //    import_item = quoteItem.Index,
                //    import_source = importHeader.import_source,

                //    description = productionCost.ResourceTypeName,
                //    charge = productionCost.ProductionCost,
                //    //charge_visibility = productionCost.IsPriceBlended ? ChargeVisibility.Included : ChargeVisibility.StandAlone,
                //    charge_visibility = ChargeVisibility.StandAlone,
                //    cost_group_code = (productionCost.ResourceType == ResourceType.Ship) ? "TRN" : "PRD",
                //    item_no = chargeItemNumber++,
                //    charge_unit_code = "LOT",
                //};



                //var addtionalCost = new import_sales_costs()
                //{
                //    import_batch_number = importHeader.import_batch_number,
                //    import_company_reference = importHeader.import_company_reference,
                //    import_number = importHeader.import_number,
                //    import_item = quoteItem.Index,
                //    import_source = importHeader.import_source,

                //    description = productionCost.ResourceTypeName,
                //    cost = productionCost.ProductionCost,
                //    visibility = ChargeVisibility.StandAlone, //vas.BreakOut ? ChargeVisibility.StandAlone : ChargeVisibility.Included,
                //    cost_group_code = "PRD",
                //    item_no = costItemNumber++,
                //    cost_unit_code = "LOT",
                //};

                //var miscCharge = new import_sales_costs()
                //{
                //    import_batch_number = importHeader.import_batch_number,
                //    import_company_reference = importHeader.import_company_reference,
                //    import_number = importHeader.import_number,
                //    import_item = quoteItem.Index,
                //    import_source = importHeader.import_source,

                //    description = "Miscellaneous",
                //    cost = 10,
                //    visibility = ChargeVisibility.StandAlone, //vas.BreakOut ? ChargeVisibility.StandAlone : ChargeVisibility.Included,
                //    cost_group_code = "MSC",
                //    item_no = costItemNumber++,
                //    cost_unit_code = "LOT",
                //};


                // importHeader.Charges.Add(productionCharge);

                //importHeader.Charges.Add(TestMiscellaneousCharge());
                //importHeader.Charges.Add(TestSurchargeCharge());

                //Header charges are added in the GetImportSalesHeader() method.
                //importHeader.Costs.Add(addtionalCost);
            //}

            //import_sales_charges TestMiscellaneousCharge()
            //{
            //    return new import_sales_charges()
            //    {
            //        import_batch_number = importHeader.import_batch_number,
            //        import_company_reference = importHeader.import_company_reference,
            //        import_number = importHeader.import_number,
            //        import_item = quoteItem.Index,
            //        import_source = importHeader.import_source,

            //        description = "MISCELLANEOUS",
            //        charge = 100,
            //        charge_visibility = ChargeVisibility.Added,
            //        cost_group_code = "MSC",
            //        item_no = chargeItemNumber++,
            //        charge_unit_code = "LOT",
            //    };
            //}

            //import_sales_charges TestSurchargeCharge()
            //{
            //    return new import_sales_charges()
            //    {
            //        import_batch_number = importHeader.import_batch_number,
            //        import_company_reference = importHeader.import_company_reference,
            //        import_number = importHeader.import_number,
            //        import_item = quoteItem.Index,
            //        import_source = importHeader.import_source,

            //        description = "SURCHARGE",
            //        charge = 200,
            //        charge_visibility = ChargeVisibility.Added,
            //        cost_group_code = "SUR",
            //        item_no = chargeItemNumber++,
            //        charge_unit_code = "LOT",
            //    };
            //}

        }

        private string GetDefaultCurrencyForCoid(string coid)
        {
        //public static CurrencyType USD => GetCurrencyByCode("USD");
        //public static CurrencyType GBP => GetCurrencyByCode("GBP");
        //public static CurrencyType CNY => GetCurrencyByCode("CNY");
        //public static CurrencyType CAD => GetCurrencyByCode("CAD");

            if (coid == "CAN") return "CAD";
            if (coid == "EUR") return "GBP";
            if (coid == "CHI") return "CYN";
            return "USD";
        }



        public string GetSystemAbbreviationFromUomAbbreviation(string uomAbbreviation)
        {
            if (string.IsNullOrWhiteSpace(uomAbbreviation)) return "";

            uomAbbreviation = uomAbbreviation.ToLower();

            switch (uomAbbreviation)
            {
                case "ft": return "FT";
                case "in": return "IN";
                case "cm": return "CM";
                case "m": return "M";
                case "yd": return "";
                case "mm": return "MM";

                case "lb": return "LBS";
                case "kg": return "KGS";
                case "mt": return "TNE";
                case "t": return "TON";
                case "cwt": return "CWT";

                case "pc": return "PCS";
                case "ea": return "PCS";
                case "lot": return "LOT";
                case "rol": return "PCS";

                case "in2": return "";
                case "ft2": return "SFT";
                case "cm2": return "";
                case "m2": return "SQM";
                case "yd2": return "";

                case "yr": return "";
                case "mon": return "";
                case "wk": return "";
                case "day": return "DYS";
                case "hr": return "HRS";
                case "min": return "MIN";
                case "sec": return "";
            }

            return "";
        }

        private string GetEntityValidationErrors(DbEntityValidationException dbve)
        {
            var msg = "";

            foreach (var eve in dbve.EntityValidationErrors)
            {
                msg += $"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation errors:\r\n\r\n";
                msg = eve.ValidationErrors.Aggregate(msg, (current, ve) => current + $"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"\r\n");
            }

            return msg;
        }
    }

    public static class StringExtensions
    {
        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                ? value
                : value.Substring(0, maxLength)
            );
        }
    }
}
