using System;
using System.Collections.Generic;
//using Vulcan.iMetal.Export.Repository;
using DAL.Vulcan.Mongo.DocClass.Quotes;

namespace Vulcan.iMetal.Quote.Export.Repository
{
    public class ImportSalesHeaders
    {
        public string ImportCompanyReference { get; set; }
        public int ImportBatchNumber { get; set; }
        public string ImportStatus => "E";
        public string ImportUserName => "metalogic";
        public string ImportSource => "Portal";
        public DateTime ImportDate { get; set; }
        public string ImportAction { get; set; }
        public string BranchCode { get; set; }
        public string TypeCode => "ENQ";
        public int ImportNumber => 1;
        public string JobNumber { get; set; } // GUID
        public string CustomerCode { get; set; }
        public string CustomerTransportAreaCode { get; set; }
        public string DeliverToNameOverride { get; set; }
        public string DeliverToAddressCode { get; set; }
        public string DeliverToAddress { get; set; }
        public string DeliverToTown { get; set; }
        public string DeliverToCounty { get; set; }
        public string DeliverToPostcode { get; set; }
        public string DeliverToCountryCode { get; set; }
        public string SalespersonName { get; set; }
        public DateTime SaleDate { get; set; }
        public string TransportTypeCode => "SFC";
        //public string CarrierCode { get; set; }
        public decimal TransportCostRate { get; set; }
        public string TransportCostRateUnitCode { get; set; }
        public decimal TransportCostAmount { get; set; }

        public List<ImportSalesItems> Items;
        public List<ImportSalesCharges> Charges;

        public static ImportSalesHeaders LoadFromQuote(CrmQuote quote, bool postToIntegrationDb=false)
        {
            return null;

            //try
            //{

            //    var result = new ImportSalesHeaders();
            //    var customer = quote.SoldToCompanyRef;
            //    var deliverTo = quote.ShipToAddress;


            //    //var salesTransportType = uowPortal.SalesTransportTypes.SingleOrDefault(x => x.Coid == location.Coid);
            //    //if (salesTransportType != null)
            //    //{
            //    //    result.CarrierCode =
            //    //        salesTransportType.SalesTransportTypeId;
            //    //}

            //    result.CompanyReference = quote.Ssid;
            //    result.BranchCode = quote.BranchCode;
            //    result.CustomerCode = customer.Code;

            //    result.DeliverToNameOverride = deliverTo.Name;
            //    result.DeliverToAddress = deliverTo.Address;
            //    result.DeliverToTown = deliverTo.Town;
            //    result.DeliverToCounty = deliverTo.County;
            //    result.DeliverToPostcode = deliverTo.PostCode;
            //    result.DeliverToCountryCode = deliverTo.CountryName;

            //    result.SaleDate = quote.CreatedOn;

            //    //result.TransportCostRate = extraCharges.FreightChargePerLb;
            //    //result.TransportCostAmount = quoteModel.TotalFreightCost;
            //    result.TransportCostRateUnitCode = "LBS";

            //    var itemNumber = 1;
            //    result.Items = new List<ImportSalesItems>();
            //    result.Charges = new List<ImportSalesCharges>();

            //    foreach (var quoteItem in quote.QuoteItems)
            //    {
            //        var dim2 = (decimal)0;
            //        var dim3 = (decimal)0;
            //        if (quoteItem.StartingMatlSpec.MatchedProductType.MetalType == "FLAT BAR")
            //        {
            //            dim2 = quoteItem.Width ?? (decimal)0;
            //            dim3 = quoteItem.Height ?? (decimal)0;
            //        }
            //        else if (quoteItem.ProductType == "ROUND BAR")
            //        {
            //            dim2 = quoteItem.OutsideDiameter ?? (decimal)0;
            //        }
            //        else if (quoteItem.ProductType == "TUBE")
            //        {
            //            dim2 = quoteItem.OutsideDiameter ?? (decimal)0;
            //            dim3 = quoteItem.InsideDiameter ?? (decimal)0;
            //        }
            //        var newItem = new
            //            ImportSalesItems(result.CompanyReference, result.ImportBatchNumber, itemNumber, result.BranchCode)
            //        {
            //            ProductCode = quoteItem.ProductCode,
            //            Dim1 = quoteItem.LengthInches,
            //            Dim2 = dim2,
            //            Dim3 = dim3,
            //            RequiredPieces = quoteItem.Pieces,
            //            //RequiredQuantity = quoteItem.LengthInches,
            //            RequiredWeight = quoteItem.TotalWeight,
            //            WeightUnitCode = "LBS"
            //        };
            //        result.Items.Add(newItem);


            //        // EstPrice
            //        var newCharge = new
            //            ImportSalesCharges(result.CompanyReference, result.ImportBatchNumber, itemNumber)
            //        {
            //            Description = quoteItem.ProductDescription,
            //            Charge = quoteItem.PricePerInch,
            //            ChargeVisibility = "S",
            //            CostGroupCode = "MAT",
            //            ItemNo = 1,
            //            ChargeUnitCode = "IN"
            //        };
            //        result.Charges.Add(newCharge);

            //        newCharge = new
            //            ImportSalesCharges(result.CompanyReference, result.ImportBatchNumber, itemNumber)
            //        {
            //            Description = "Sawing $" + quoteItem.SawCostPerCut + "/cut",
            //            Charge = quoteItem.SawCostPerCut,
            //            ChargeVisibility = "A",
            //            CostGroupCode = "MSC",
            //            ItemNo = 2,
            //            ChargeUnitCode = "PCS"
            //        };
            //        result.Charges.Add(newCharge);

            //        newCharge = new
            //            ImportSalesCharges(result.CompanyReference, result.ImportBatchNumber, itemNumber)
            //        {
            //            Description = "Freight $" + quoteItem.FreightCostLb + "/lb",
            //            Charge = quoteItem.FreightCostLb,
            //            ChargeVisibility = "A",
            //            CostGroupCode = "TRN",
            //            ItemNo = 3,
            //            ChargeUnitCode = "LBS"
            //        };
            //        result.Charges.Add(newCharge);

            //        itemNumber++;
            //    }

            //    if (postToIntegrationDb)
            //    {
            //        PostImportSalesChargesToIntegrationDb.Refresh(result);
            //        quote.ImportBatchNumber = result.ImportBatchNumber;
            //        quote.QuoteState = uowPortal.QuoteStates.Single(x => x.State == "OrderPlaced");
            //        quote.SentDate = DateTime.Now;
            //        quote.SentToiMetalDate = DateTime.Now;
            //        uowPortal.SaveChanges();
            //    }

            //    return result;

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        }

        public ImportSalesHeaders()
        {
            JobNumber = GuidGenerator.AsString();
            //ImportBatchNumber = GetNextBatchNumber.Refresh();
            ImportDate = DateTime.Now;
            ImportAction = "A";
        }
    }
}
