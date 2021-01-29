using DAL.Vulcan.Mongo.Core.Quotes;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using DAL.iMetal.Core.Queries;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class ProductMaster
    {
        public string Coid { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal TheoWeight { get; set; }
        public decimal OuterDiameter { get; set; }
        public decimal InsideDiameter { get; set; }
        public decimal Wall => (InsideDiameter > 0 ) ? ((OuterDiameter - InsideDiameter) / 2) : 0;
        public string ProductType { get; set; }
        public string StockGrade { get; set; } = string.Empty;
        public string MetalCategory { get; set; }
        public string ProductCondition { get; set; }
        public string ProductCategory { get; set; } = string.Empty;
        public bool IsNewProduct { get; set; } = false;
        public decimal FactorForKilograms { get; set; }
        public decimal FactorForLbs { get; set; }
        public string TagNumber { get; set; } = string.Empty;
        public bool IsMachineComponent { get; set; } = false;
        public string HeatNumber { get; set; } = string.Empty;
        public string StratificationRank { get; set; } = "unknown";
        public decimal Density { get; set; } = 0;
        public void SetStockGrade()
        {
            if (!IsNewProduct)
            {
                var fullProductMaster = GetProductMasterFull();
                if (fullProductMaster != null)
                {
                    StockGrade = fullProductMaster.StockGrade;
                }
                else
                {
                    StockGrade = "";
                }
            }
        }
        private string Normalize(decimal value)
        {
            return (value / 1.000000000000000000000000000000000m).ToString(CultureInfo.InvariantCulture);
        }

        public string ProductSize
        {
            get
            {
                if (InsideDiameter > 0)
                    return Normalize(OuterDiameter) + "-" + Normalize(InsideDiameter);
                else if (OuterDiameter > 0)
                {
                    return Normalize(OuterDiameter);
                }
                else
                {
                    if (ProductCode == null) return "<Unknown>";
                    var productCodeTokens = ProductCode.Split(' ');
                    if (productCodeTokens.Count() > 2)
                    {
                        return productCodeTokens[1];
                    }
                    else
                    {
                        return "<Unknown>";
                    }
                }

            }
        }


        public ProductMaster()
        {

        }

        public static (ProductMaster ProductMaster, StockItemsQuery StockItem) FromStockId(string coid, int stockItemId)
        {
            var stockItem = StockItemsQuery.FromStockId(coid, stockItemId).FirstOrDefault();
            
            if (stockItem == null) throw new Exception("StockItem not found");

            var productMaster = new ProductMaster()
            {
                Coid = coid,
                ProductCode = stockItem.ProductCode,
                ProductId = stockItem.ProductId,
                TheoWeight = stockItem.TheoWeight,
                OuterDiameter = stockItem.OuterDiameter,
                InsideDiameter = stockItem.InsideDiameter,
                MetalCategory = stockItem.MetalCategory,
                ProductType = stockItem.ProductType,
                ProductCondition = stockItem.ProductCondition,
                FactorForLbs = stockItem.FactorForLbs,
                FactorForKilograms = stockItem.FactorForKgs,
                TagNumber = stockItem.TagNumber,
                StockGrade = stockItem.StockGrade,
                IsMachineComponent = stockItem.IsMachinedPart,
                HeatNumber = stockItem.HeatNumber,
                ProductCategory = stockItem.ProductCategory,
                StratificationRank = stockItem.StratificationRank,
                Density = stockItem.Density
            };
            return (productMaster,stockItem);
        }


        public ProductMaster(string coid, string productCode)
        {
            var result = StockItemsQuery.GetForCode(coid, productCode).FirstOrDefault();
            if (result == null)
            {
                throw new Exception($"ProductCode: {productCode} not found in {coid}");
            }

            Coid = coid;
            ProductCode = result.ProductCode;
            ProductId = result.ProductId;
            TheoWeight = result.TheoWeight;
            OuterDiameter = result.OuterDiameter;
            InsideDiameter = result.InsideDiameter;
            MetalCategory = result.MetalCategory;
            ProductType = result.ProductType;
            ProductCondition = result.ProductCondition;
            FactorForLbs = result.FactorForLbs;
            FactorForKilograms = result.FactorForKgs;
            TagNumber = result.TagNumber;
            StockGrade = result.StockGrade;
            HeatNumber = result.HeatNumber;
            ProductCategory = result.ProductCategory;
            IsMachineComponent = (result.MetalType == "MACHINE COMPONENT");
            StratificationRank = result.StratificationRank;
            Density = result.Density;
        }

        public ProductMaster(string coid, int productId)
        {
            Coid = coid;
            ProductId = productId;
            var result = ProductMastersQuery.GetForId(coid, productId);
            if (result == null) return;
            ProductCode = result.ProductCode;
            ProductId = result.ProductId;
            TheoWeight = result.TheoWeight;
            OuterDiameter = result.OuterDiameter;
            InsideDiameter = result.InsideDiameter;
            MetalCategory = result.MetalCategory;
            ProductType = result.ProductType;
            ProductCondition = result.ProductCondition;
            FactorForLbs = result.FactorForLbs;
            FactorForKilograms = result.FactorForKgs;
            StockGrade = result.StockGrade;
            ProductCategory = result.ProductCategory;
            IsMachineComponent = (result.MetalType == "MACHINE COMPONENT");
            StratificationRank = result.StratificationRank;
            Density = result.Density;
        }

        public ProductMastersQuery GetProductMasterFull()
        {
            return ProductMastersQuery.GetForId(Coid, ProductId);
        }

        public StockItemCalculatedCost GetStockItemCalculatedCost()
        {
            return new StockItemCalculatedCost(Coid,ProductId);
        }

        public PurchaseOrderItemCalculatedCost GetPurchaseOrderItemCalculatedCost()
        {
            return new PurchaseOrderItemCalculatedCost(Coid, ProductId);
        }

        /*
         ProductId: 8995
          Coid: "INC"
          ProductCode: "718 8 SAAH"
          ProductCondition: "SAAH"
          ProductCategory: "BR718"
          MetalCategory: "NICKEL"
          MetalType: "NICKEL ALLOY ROUND BAR"
          StockGrade: "718"
          InsideDiameter: 0
          OuterDiameter: 8.0000
          StockType: "Round Bar (Western)"
          Density: 0
          ProductControlCode: "DW"
          ControlPieces: 1
          VolumeDensity: 0.296000
          Dim1StaticDimension: 1
          Dim2StaticDimension: 8.0000
          Dim3StaticDimension: 1
          ProductDensity: 1.000000
          Width: 0
          Length: 0
          Thick: 0
          FactorForKilograms: 0.45359
          FactorForLbs: 1
          ProductType: "Bar"
          ProductSize: "8"
          TheoWeight: 14.87857971200000000000000000

         */
        public string GetLongDescription(RequiredQuantity requiredQuantity, string partNumber, string partSpecification)
        {

            var result = new StringBuilder();
            var productMasterFull = ProductMastersQuery.GetForId(Coid, ProductId);

            if (productMasterFull == null) return result.ToString();

            result.Append(
                $"{productMasterFull.Description}");

            //if (productMasterFull.InsideDiameter == 0)
            //{
                result.Append($"\n{productMasterFull.SizeDescription} X {requiredQuantity.PieceLength.Inches:0.000}\"");
            //}
            //else
            //{
            //    result.Append($" {productMasterFull.ProductSize}\" DIA X {requiredQuantity.PieceLength.Inches}\"");
            //}

            if (partSpecification != string.Empty)
            {
                result.Append($"\nSPEC: {partSpecification}");
            }

            if (partNumber != string.Empty)
            {
                result.Append($"\nPART NUMBER: {partNumber}");
            }

            return result.ToString();
        }
    }
}