using DAL.IntegrationDb;
using DAL.IntegrationDb.Partial;

// ReSharper disable once CheckNamespace
namespace DAL.IntegrationDb
{
    public partial class import_stock_allocations : IImportStatusItem
    {


        /// <summary>
        /// Not a field in the import_stock_allocations table, but used to create a unique composite_key in cases where there are 
        /// multiple stock allocations to the same sales line item.
        /// </summary>
        public int AllocationIndex { get; set; } = 1;

        public import_stock_allocations()
        {
        }

        public import_stock_allocations(import_sales_headers importHeader, import_sales_items importItem)
        {
            import_company_reference = importHeader.import_company_reference;
            import_batch_number = importHeader.import_batch_number;
            import_status = "E";

            allocation_type = "S";
            allocation_branch_code = importHeader.branch_code;
            allocation_header_number = importHeader.order_number;
            allocation_import_number = importHeader.import_number;
            allocation_item_number = importItem.import_item;

            import_user_name = importHeader.import_user_name;
            import_source = importHeader.import_source;
            import_action = "A";

            stock_branch_code = importHeader.branch_code;

            composite_key = GetCompositeKey();
        }

        public string GetCompositeKey()
        {
            return $"{import_company_reference}-{import_source}-{import_batch_number}-{allocation_branch_code}-{allocation_import_number}-{allocation_item_number}-{AllocationIndex}";
        }

        /// <summary>
        /// Regenerates the composite_key property based on other property values.
        /// </summary>
        public void SetCompositeKey()
        {
            composite_key = GetCompositeKey();
        }

        public string GetStatusSummary()
        {
            return "TBD..."; // $"Cost Item # {item_no}, Charge: {cost}/{cost_unit_code}, Notes: {import_notes}";
        }


        public string EntityDescription => "TBD..."; //$"Cost {item_no}: {description} | {cost} / {cost_unit_code}";

        public string StatusText => import_notes;


    }
}
