using System.Collections.Generic;
using DAL.IntegrationDb.Partial;

// ReSharper disable once CheckNamespace
namespace DAL.IntegrationDb
{
    public partial class import_sales_headers : IImportStatusItem
    {


        public List<import_sales_items> Items;
        public List<import_sales_charges> Charges;
        public List<import_sales_costs> Costs;
        public List<import_stock_allocations> StockAllocations;

        
        public string GetStatusSummary()
        {
            var quoteNumber = GetCompoundQuoteNumberFromIMetalImportNumber(import_number);

            var status = "?";
            if (import_status == "E") status = "Entered";
            if (import_status == "I") status = "Imported";
            if (import_status == "F") status = "Failed";

            return $"Quote #: {quoteNumber}, Ref: {import_company_reference}, Batch #: {import_batch_number}, Status: {status}, Notes: {import_notes}";
        }


        public static string GetCompoundQuoteNumberFromIMetalImportNumber(int importNumber)
        {
            var quoteNumber = GetQuoteNumberFromIMetalImportNumber(importNumber);
            var versionNumber = GetVersionNumberFromIMetalImportNumber(importNumber);
            return GetCompoundQuoteNumber(quoteNumber, versionNumber);
        }

        public static int GetQuoteNumberFromIMetalImportNumber(int importNumber)
        {
            var rawQuoteNumber = importNumber / 1000M;
            return (int)decimal.Floor(rawQuoteNumber);
        }

        public static int GetVersionNumberFromIMetalImportNumber(int importNumber)
        {
            return importNumber % 1000;
        }

        public static string GetCompoundQuoteNumber(int quoteNumber, int versionNumber)
        {
            return $"{quoteNumber}-{versionNumber.ToString().PadLeft(3, '0')}";
        }


        public string EntityDescription
        {
            get
            {
                var quoteNumber = GetCompoundQuoteNumberFromIMetalImportNumber(import_number);
                var entityType = $"SALES HEADER for Quote {quoteNumber}";

                var status = "?";
                if (import_status == "E") status = "Entered";
                if (import_status == "I") status = "Imported";
                if (import_status == "F") status = "Failed";

                return $"{entityType} ({status})";
            }
        }

        public string StatusText => import_notes;


    }
}
