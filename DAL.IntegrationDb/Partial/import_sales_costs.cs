using DAL.IntegrationDb.Partial;

// ReSharper disable once CheckNamespace
namespace DAL.IntegrationDb
{
    public partial class import_sales_costs : IImportStatusItem
    {



        public import_sales_costs()
        {
        }

        public import_sales_costs(import_sales_headers importHeader, int importItem)
        {
            import_company_reference = importHeader.import_company_reference;
            import_batch_number = importHeader.import_batch_number;
            import_number = importHeader.import_number;
            import_item = importItem;
        }

        public string GetStatusSummary()
        {
            return $"Cost Item # {item_no}, Charge: {cost}/{cost_unit_code}, Notes: {import_notes}";
        }


        public string EntityDescription => $"Cost {item_no}: {description} | {cost} / {cost_unit_code}";

        public string StatusText => import_notes;

    }
}
