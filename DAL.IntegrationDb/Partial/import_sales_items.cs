using DAL.IntegrationDb.Partial;

// ReSharper disable once CheckNamespace
namespace DAL.IntegrationDb
{
    public partial class import_sales_items : IImportStatusItem
    {


        public import_sales_items()
        {
        }

        public import_sales_items(import_sales_headers importHeader, int itemNumber)
        {
            import_company_reference = importHeader.import_company_reference;
            import_batch_number = importHeader.import_batch_number;
            import_number = importHeader.import_number;
            delivery_branch_code = importHeader.delivery_branch_code;
            import_item = itemNumber;
        }

        public string GetStatusSummary()
        {
            return $"Item # {import_item}, Notes: {import_notes}";
        }


        public string EntityDescription
        {
            get
            {
                var desc = !string.IsNullOrWhiteSpace(description) ? $"  ({description})" : "";
                return $"SALES ITEM {import_item}: {desc}";
            }
        }

        public string StatusText => import_notes;
    }
}
