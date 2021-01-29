namespace Vulcan.IMetal.Models
{
    public class ItemCostModel
    {
        public decimal Material { get; set; } 
        public decimal Transport { get; set; }
        public decimal Production { get; set; }
        public decimal Miscellaneous { get; set; }
        public decimal Surcharge { get; set; }

        public decimal Total => Material + Transport + Production + Miscellaneous + Surcharge;

        public void AddTo(ItemCostModel model)
        {
            model.Material += Material;
            model.Transport += Transport;
            model.Production += Production;
            model.Miscellaneous += Miscellaneous;
            model.Surcharge += Surcharge;
        }
    }


}