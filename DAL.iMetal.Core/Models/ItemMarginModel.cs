namespace DAL.iMetal.Core.Models
{
    public class ItemMarginModel
    {
        public ChargeCostModel Material { get; set; }

        public ChargeCostModel Production { get; set; }
        public ChargeCostModel Transport { get; set; }
        public ChargeCostModel Miscellaneous { get; set; }
        public ChargeCostModel Surcharge { get; set; }

        public ChargeCostModel Total { get; set; }

        public ItemMarginModel AddTo(ItemMarginModel result)
        {
            result.Material.Cost += Material.Cost;
            result.Material.Charge += Material.Charge;

            result.Production.Cost += Production.Cost;
            result.Production.Charge += Production.Charge;
            result.Transport.Cost += Transport.Cost;
            result.Transport.Charge += Transport.Charge;
            result.Surcharge.Cost += Surcharge.Cost;
            result.Surcharge.Charge += Surcharge.Charge;
            result.Miscellaneous.Cost += Miscellaneous.Cost;
            result.Miscellaneous.Charge += Miscellaneous.Charge;

            result.Total.Cost += Material.Cost + Production.Cost + Transport.Cost + Miscellaneous.Cost + Surcharge.Cost;
            result.Total.Charge += Material.Charge + Production.Charge + Transport.Charge + Miscellaneous.Charge + Surcharge.Charge;
            return result;
        }

        public ItemMarginModel(
            decimal totalCost, decimal totalCharge,
            decimal materialCost, decimal materialCharge,
            decimal productionCost, decimal productionCharge,
            decimal transportCost, decimal transportCharge,
            decimal miscCost, decimal miscCharge,
            decimal surchargeCost, decimal surchargeCharge)
        {
            Total = new ChargeCostModel(totalCharge, totalCost);
            Material = new ChargeCostModel(materialCharge, materialCost);
            Production = new ChargeCostModel(productionCharge, productionCost);
            Transport = new ChargeCostModel(transportCharge, transportCost);
            Miscellaneous = new ChargeCostModel(miscCharge, miscCost);
            Surcharge = new ChargeCostModel(surchargeCharge, surchargeCost);
        }
    }
}