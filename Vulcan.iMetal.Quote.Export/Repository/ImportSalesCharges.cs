namespace Vulcan.iMetal.Quote.Export.Repository
{
    public class ImportSalesCharges
    {
        public string ImportCompanyReference { get; set; }
        public int ImportBatchNumber { get; set; }
        public int ImportNumber => 1;
        public int ImportItem { get; set; }
        public int ItemNo { get; set; }
        public string CostGroupCode { get; set; }
        public decimal Charge { get; set; }
        //public string ChargeUnitCode { get; set; }
        //public decimal exchange_rate { get; set; }
        public string ChargeFixStatus => "V";
        public string ChargeVisibility { get; set; }
        public bool ConfirmAtInvoicing => false;
        public string Description { get; set; }
        public string ChargeUnitCode { get; set; }

        public ImportSalesCharges(string companyRef, int batchNumber, int importItem)
        {
            ImportCompanyReference = companyRef;
            ImportBatchNumber = batchNumber;
            ImportItem = importItem;
        }

    }

}
