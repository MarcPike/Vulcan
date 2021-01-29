namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class CrozCalcData
    {
        public decimal BaseCost { get; set; } = 0;
        public decimal BasePrice { get; set; } = 0;
        public int Pieces { get; set; } = 0;
        public decimal Quantity { get; set; } = 0;
        public string Uom { get; set; } = string.Empty;
        public string JsonData { get; set; } = string.Empty;


    }
}