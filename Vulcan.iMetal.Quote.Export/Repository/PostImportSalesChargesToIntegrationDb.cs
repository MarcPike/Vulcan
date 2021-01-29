using System.Linq;
using DAL.IntegrationDb;

namespace Vulcan.iMetal.Quote.Export.Repository
{
    /*
    public static class PostImportSalesChargesToIntegrationDb
    {
        public static void Refresh(ImportSalesHeaders data)
        {
            using (var context = new IntegrationDb())
            {
                foreach (var importSalesItem in data.Items.OrderBy(x=>x.ImportItem))
                {
                    context.import_sales_items.Add(new import_sales_items()
                    {
                        import_company_reference = importSalesItem.ImportCompanyReference,
                        import_batch_number = importSalesItem.ImportBatchNumber,
                        import_number = importSalesItem.ImportNumber,
                        import_item = importSalesItem.ImportItem,
                        product_code = importSalesItem.ProductCode,
                        dim1 = importSalesItem.Dim1,
                        dim2 = importSalesItem.Dim2,
                        dim3 = importSalesItem.Dim3,
                        required_pieces = importSalesItem.RequiredPieces,
                        //required_quantity = importSalesItem.RequiredQuantity,
                        required_weight = importSalesItem.RequiredWeight,
                        weight_units_code = importSalesItem.WeightUnitCode,
                        delivery_branch_code = importSalesItem.DeliveryBranchCode,
                        use_minimum_grade = importSalesItem.UseMinimumGrade
                    });
                }

                foreach (var importSalesCharge in data.Charges.OrderBy(x=>x.ImportItem).ThenBy(x=>x.ItemNo))
                {
                    context.import_sales_charges.Add(new import_sales_charges()
                    {
                        import_company_reference = importSalesCharge.ImportCompanyReference,
                        import_batch_number = importSalesCharge.ImportBatchNumber,
                        import_number = importSalesCharge.ImportNumber,
                        import_item = importSalesCharge.ImportItem,
                        cost_group_code = importSalesCharge.CostGroupCode,
                        charge = importSalesCharge.Charge,
                        charge_fix_status = importSalesCharge.ChargeFixStatus,
                        charge_visibility = importSalesCharge.ChargeVisibility,
                        confirm_at_invoicing = importSalesCharge.ConfirmAtInvoicing,
                        description = importSalesCharge.Description,
                        item_no = importSalesCharge.ItemNo,
                        charge_unit_code = importSalesCharge.ChargeUnitCode
                    });
                }

                context.import_sales_headers.Add(new import_sales_headers()
                {
                    import_company_reference = data.ImportCompanyReference,
                    import_batch_number = data.ImportBatchNumber,
                    import_status = data.ImportStatus,
                    import_user_name = data.ImportUserName,
                    import_source = data.ImportSource,
                    import_date = data.ImportDate,
                    import_action = data.ImportAction,
                    branch_code = data.BranchCode,
                    type_code = data.TypeCode,
                    import_number = data.ImportNumber,
                    job_number = data.JobNumber,
                    customer_code = data.CustomerCode,
                    customer_transport_area_code = data.CustomerTransportAreaCode,
                    deliver_to_name_override = data.DeliverToNameOverride,
                    deliver_to_address_code = data.DeliverToAddressCode,
                    deliver_to_address = data.DeliverToAddress,
                    deliver_to_town = data.DeliverToTown,
                    deliver_to_county = data.DeliverToCounty,
                    deliver_to_postcode = data.DeliverToPostcode,
                    deliver_to_country_code = data.DeliverToCountryCode,
                    salesperson_name = data.SalespersonName,
                    sale_date = data.SaleDate,
                    transport_cost_rate = data.TransportCostRate,
                    transport_charge_rate_unit_code = data.TransportCostRateUnitCode,
                    transport_cost_amount = data.TransportCostAmount,
                    transport_type_code = data.TransportTypeCode

                });

                context.SaveChanges();
            }
        }
    }
    */
}
