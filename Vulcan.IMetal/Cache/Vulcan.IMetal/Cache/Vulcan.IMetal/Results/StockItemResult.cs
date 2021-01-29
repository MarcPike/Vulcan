using Vulcan.IMetal.Context;
using Vulcan.IMetal.Context.StockItems;

namespace Vulcan.IMetal.Results
{
    public class StockItemResult
    {
        public string Coid { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSize { get; set; }
        public string ProductCondition { get; set; }
        public string TagNumber { get; set; }
        public string HeatNumber { get; set; }
        public string Mill { get; set; }
        public string MaterialTypeDescription { get; set; }
        public string MaterialType { get; set; }
        public string MetalType { get; set; }
        public string MetalType2 { get; set; }
        public string WarehouseName { get; set; }
        public string WarehouseCode { get; set; }




        public decimal InsideDiameter { get; set; }
        public decimal OutsideDiameter { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Thick { get; set; }
        public decimal Density { get; set; }
        public string DensityUnit { get; set; }
        public decimal TheoWeight { get; set; }


        public decimal PhysicalQuantity { get; set; }
        public decimal AllocatedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public string QuantityUnit { get; set; }


        public int PhysicalPieces { get; set; }
        public int AllocatedPieces { get; set; }
        public int AvailablePieces { get; set; }
        public string PiecesUnit { get; set; }


        public decimal PhysicalLengthBase { get; set; }
        public decimal AllocatedLengthBase { get; set; }
        public decimal AvailableLengthBase { get; set; }
        public string LengthUnit { get; set; }



        public decimal PhysicalWeightBase { get; set; }
        public decimal PhysicalWeightKgs { get; set; }
        public decimal PhysicalWeightLbs { get; set; }

        public decimal AllocatedWeightBase { get; set; }
        public decimal AllocatedWeightKgs { get; set; }
        public decimal AllocatedWeightLbs { get; set; }
     
        public decimal AvailableWeightBase { get; set; }
        public decimal AvailableWeightLbs { get; set; }
        public decimal AvailableWeightKgs { get; set; }

        public string WeightUnit { get; set; }



        public decimal ProductionValue { get; set; }
        public decimal MaterialValue { get; set; }
        public decimal TransportValue { get; set; }
        public decimal SurchargeValue { get; set; }
        public decimal MiscellaneousValue { get; set; }
        public decimal TotalValue => MaterialValue + ProductionValue + TransportValue + SurchargeValue + MiscellaneousValue;

        public decimal MaterialCost { get; set; }
        public decimal TransportCost { get; set; }
        public decimal ProductionCost { get; set; }
        public decimal SurchargeCost { get; set; }
        public decimal MiscellaneousCost { get; set; }
        public decimal TotalCost { get; set; }

        public decimal CostPerLb { get; set; }
        public decimal CostPerKg { get; set; }

        /// <summary>
        /// Not safe to use because PhysicalQuantity is not guaranteed to be inches.
        /// </summary>
        public decimal CostPerInch { get; set; }



        public StockItemResult(StockItem stockItem)
        {
            Coid = stockItem.Coid;
            Id = stockItem.Id;
            ProductId = stockItem.ProductId ?? 0;
            ProductCategory = stockItem.Product?.ProductCategory?.Category;
            ProductCondition = stockItem.SpecificationValue2;
            Description = stockItem.Product?.Description;
            ProductSize = stockItem.Product?.SizeDescription;
            ProductCode = stockItem.Product?.Code;
            TagNumber = stockItem.PC_TagNumber;
            HeatNumber = stockItem.PC_HeatNumber;
            Mill = stockItem.PC_Mill;
            MaterialTypeDescription = stockItem.PC_MaterialTypeDescription;
            MaterialType = stockItem.PC_MaterialType;
            MetalType = stockItem.PC_MetalType;
            MetalType2 = stockItem.Product?.ProductCategory?.StockAnalysisCode_Analysis2Id?.Description;
            WarehouseName = stockItem.Warehouse?.Name;
            WarehouseCode = stockItem.Warehouse?.Code;

            OutsideDiameter = stockItem.PC_OutsideDiameter;
            InsideDiameter = stockItem.PC_InsideDiameter;
            Length = stockItem.PC_Length;
            Density = stockItem.PC_Density;
            DensityUnit = stockItem.PC_DensityUnit;
            Width = stockItem.PC_Width;
            Thick = stockItem.PC_Thick;
            TheoWeight = stockItem.GetTheoWeight();

            PhysicalQuantity = stockItem.PC_PhysicalQuantity;
            AllocatedQuantity = stockItem.PC_AllocatedQuantity;
            AvailableQuantity = stockItem.PC_AvailableQuantity;
            QuantityUnit = stockItem.PC_QuantityUnit;

            PhysicalPieces = stockItem.PC_PhysicalPieces;
            AllocatedPieces = stockItem.PC_AllocatedPieces;
            AvailablePieces = stockItem.PC_AvailablePieces;
            PiecesUnit = stockItem.PC_PiecesUnit;

            PhysicalLengthBase = stockItem.PC_PhysicalLengthBase;
            AllocatedLengthBase = stockItem.PC_AllocatedLengthBase;
            AvailableLengthBase = stockItem.PC_AvailableLengthBase;
            LengthUnit = stockItem.PC_LengthUnit;

            PhysicalWeightBase = stockItem.PC_PhysicalWeightBase;
            PhysicalWeightLbs = stockItem.PC_PhysicalWeightLbs;
            PhysicalWeightKgs = stockItem.PC_PhysicalWeightKgs;

            AllocatedWeightBase = stockItem.PC_AllocatedWeightBase;
            AllocatedWeightLbs = stockItem.PC_AllocatedWeightLbs;
            AllocatedWeightKgs = stockItem.PC_AllocatedWeightKgs;

            AvailableWeightBase = stockItem.PC_AvailableWeightBase;
            AvailableWeightLbs = stockItem.PC_AvailableWeightLbs;
            AvailableWeightKgs = stockItem.PC_AvailableWeightKgs;

            WeightUnit = stockItem.PC_WeightUnit;

            ProductionValue = stockItem.ProductionValue ?? 0;
            MaterialValue = stockItem.MaterialValue ?? 0;
            MiscellaneousValue = stockItem.MiscellaneousValue ?? 0;
            SurchargeValue = stockItem.SurchargeValue ?? 0;
            TransportValue = stockItem.TransportValue ?? 0;

            MaterialCost = stockItem.PC_MaterialCostTotal;
            ProductionCost = stockItem.PC_ProductionCostTotal;
            TransportCost = stockItem.PC_TransportCostTotal;
            SurchargeCost = stockItem.PC_SurchargeCostTotal;
            MiscellaneousCost = stockItem.PC_MiscellaneousCostTotal;
            TotalCost = stockItem.PC_TotalCost;
            CostPerKg = stockItem.PC_CostPerKg;
            CostPerLb = stockItem.PC_CostPerLb;
            CostPerInch = stockItem.PC_CostPerInch;
        }
    }
}