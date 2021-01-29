using DAL.IntegrationDb;
using System;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class ExportAttempt
    {
        public string ImportCompanyReference { get; set; }
        public string ImportSource { get; set; }
        public int ImportBatchNumber { get; set; }
        public int ImportNumber { get; set; }
        public string ImportStatus { get; set; }
        public string SalesOrderId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ExecutionDate { get; set; }
        public string ImportNotes { get; set; } = string.Empty;
        public string Errors { get; set; } = string.Empty;

        public ExportAttempt()
        {

        }

        public ExportAttempt(import_sales_headers header)
        {
            ImportCompanyReference = header.import_company_reference;
            ImportSource = header.import_source;
            ImportBatchNumber = header.import_batch_number;
            ImportNumber = header.import_number;
            ImportStatus = header.import_status;
            ExecutionDate = DateTime.Now;
            ImportNotes = header.import_notes;
            SalesOrderId = ExtractSalesOrderIdFromImportNotes(header.import_notes);
        }

        private string ExtractSalesOrderIdFromImportNotes(string importNotes)
        {
            if (String.IsNullOrWhiteSpace(importNotes)) return "";

            return importNotes.Split('-').Last();
        }

    }
}