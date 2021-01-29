using DAL.IntegrationDb;
using DAL.IntegrationDb.Partial;

// ReSharper disable once CheckNamespace
namespace DAL.IntegrationDb
{
    public partial class import_sales_charges : IImportStatusItem
    {

        public import_sales_charges()
        {
        }

        public import_sales_charges(import_sales_headers importHeader, int importItem)
        {
            import_company_reference = importHeader.import_company_reference;
            import_batch_number = importHeader.import_batch_number;
            import_number = importHeader.import_number;
            import_item = importItem;
        }

        public string GetStatusSummary()
        {
            return $"Charge Item # {item_no}, Charge: {charge}/{charge_unit_code}, Notes: {import_notes}";
        }

        public string EntityDescription
        {
            get
            {
                var visibility = "";
                if (charge_visibility == "S") visibility = " | Stand Alone";
                if (charge_visibility == "I") visibility = " | Included";
                if (charge_visibility == "A") visibility = " | Added";

                return $"Charge {item_no}: {description} | {charge} / {charge_unit_code}{visibility}";
            }
        }

        public string StatusText => import_notes;

    }
}
