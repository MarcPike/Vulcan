namespace Vulcan.SVC.WebApi.Core.Models
{
    public struct ImportLineItemResults
    {
        public int ItemNumber;
        public string ProductCode;
        public string ImportNotes;

        public ImportLineItemResults(int itemNumber, string productCode, string importNotes)
        {
            ItemNumber = itemNumber;
            ProductCode = productCode;
            ImportNotes = importNotes;
        }
    }
}