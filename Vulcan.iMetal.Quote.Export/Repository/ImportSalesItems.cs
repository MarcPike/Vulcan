namespace Vulcan.iMetal.Quote.Export.Repository
{
    public class ImportSalesItems
    {
        public string ImportCompanyReference { get; set; }
        public int ImportBatchNumber { get; set; }
        public int ImportNumber => 1;
        public int ImportItem { get; set; }
        public string ProductCode { get; set; }
        public decimal Dim1 { get; set; }
        public decimal Dim2 { get; set; }
        public decimal Dim3 { get; set; }
        public int RequiredPieces { get; set; }
        public decimal RequiredQuantity { get; set; }
        public decimal RequiredWeight { get; set; }
        public string WeightUnitCode { get; set; }
        public string DeliveryBranchCode { get; set; }
        public bool UseMinimumGrade => true;
        public ImportSalesItems(string companyRef, int batchNumber, int importItem, string deliveryBranchCode)
        {
            ImportCompanyReference = companyRef;
            ImportBatchNumber = batchNumber;
            ImportItem = importItem;
            DeliveryBranchCode = deliveryBranchCode;
        }
        
    }
}
