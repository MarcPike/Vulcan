using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using Vulcan.iMetal.Quote.Export.Ado;

namespace Vulcan.SVC.WebApi.Core.Models
{
    public class QuoteExportAttemptDetails
    {
        public DateTime ExecutionDate;
        public string Error = string.Empty;
        public string Result = string.Empty;

        public string ImportStatus;

        public string StatusDefinition
        {
            get
            {
                if (ImportStatus == "E") return "(E)ntered - Waiting to be Imported";
                if (ImportStatus == "I") return "(I)mported - Quote was successfully imported";
                if (ImportStatus == "F") return "(F)ailed - Quote import was unsuccessful";
                return string.Empty;
            }
        }

        public List<ImportLineItemResults> LineItemResults = new List<ImportLineItemResults>();

        public QuoteExportAttemptDetails() { }
        public QuoteExportAttemptDetails(ExportAttempt attempt)
        {
            ExecutionDate = attempt.ExecutionDate;
            var connectionFactory = new ConnectionFactory();
            var connection = connectionFactory.Connection;

            var sqlForSalesHeader =
                $"SELECT import_status, import_notes FROM {connectionFactory.DatabaseNamePrefix}import_sales_headers WHERE import_batch_number = {attempt.ImportBatchNumber}";


            var sqlForSalesItems =
                $"SELECT import_item, product_code, import_notes FROM {connectionFactory.DatabaseNamePrefix}import_sales_items WHERE import_batch_number = {attempt.ImportBatchNumber} ORDER BY import_item";

            try
            {
                connection.Open();
                try
                {

                    SqlCommand cmdForHeader = new SqlCommand(sqlForSalesHeader, connection);
                    SqlDataReader rdrForHeader = cmdForHeader.ExecuteReader();
                    while (rdrForHeader.Read())
                    {
                        ImportStatus = rdrForHeader.GetString(0);
                        if (ImportStatus == "F")
                        {
                            Error = rdrForHeader.GetString(1);
                        }
                        else
                        {
                            Result = rdrForHeader.GetString(1);
                        }

                    }


                    SqlCommand cmdForLineItems = new SqlCommand(sqlForSalesItems, connection);
                    SqlDataReader rdrForLineItems = cmdForLineItems.ExecuteReader();
                    while (rdrForLineItems.Read())
                    {
                        var newLine = new ImportLineItemResults(rdrForLineItems.GetInt32(0),
                            rdrForLineItems.GetString(1), rdrForLineItems.GetString(2));
                        if (!string.IsNullOrEmpty(newLine.ImportNotes))
                        {
                            LineItemResults.Add(newLine);
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            finally
            {
                connection.Close();
            }
        }
    }
}