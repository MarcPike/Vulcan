using System;
using System.Globalization;
using System.Linq;
using DAL.iMetal.Core.Queries;

namespace DAL.iMetal.Core.Models
{
    
    public class StockItemsCondensed
    {
        public string Coid;
        public string ProductCategory;
        public string ProductCondition;
        public int AvailablePieces;
        public decimal AvailableLength;
        public string TagNumber;
        public string HeatNumber;
        public string MillCode;
        public string MillName;
        public string WarehouseCode;
        public string WarehouseName;
        public string Location;
        public DateTime ReceivedDate;
        public string StockHoldUser;
        public string StockHoldReason;
        public int ProductId;
        public decimal AvailableQuantity;
        public decimal InsideDiameter;
        public decimal OuterDiameter;
        public string ProductCode;
        public int StockItemId;
        public decimal Length;
        public decimal AllocatedLength;
        public string Currency;
        public string ProductControlCode;
        public decimal ExchangeFactor { get; set; }

        public decimal CostPerWeight { get; set; }
        public decimal CostPerLb
        {
            get
            {
                if (new[] { "SIN", "MSA", "DUB", "EUR" }.Contains(Coid))
                {
                    return CostPerWeight * (decimal)0.453592;
                }
                else
                {
                    return CostPerWeight;
                }
            }
        } //CostPerWeight * FactorForLbs;

        public decimal CostPerKg => CostPerLb * (decimal)2.20462;


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

        public bool IsMachinedPart { get; set; }
        public bool IsZeroWeightService { get; set; }
        public decimal PieceCost { get; set; }
        public decimal TotalCost { get; set; }
        public string StratificationRank { get; set; }

        public StockItemsCondensed()
        {
            
        }

        public StockItemsCondensed(StockItemsQuery stockItem, string currency)
        {
            Coid = stockItem.Coid;
            ProductCategory = stockItem.ProductCategory;
            ProductCondition = stockItem.ProductCondition;
            AvailablePieces = stockItem.AvailablePieces;
            AvailableLength = stockItem.AvailableLength;
            TagNumber = stockItem.TagNumber;
            HeatNumber = stockItem.HeatNumber;
            MillCode = stockItem.MillCode;
            WarehouseCode = stockItem.WarehouseCode;
            Location = stockItem.Location;
            ReceivedDate = stockItem.ReceivedDate;
            StockHoldUser = stockItem.StockHoldUser;
            StockHoldReason = stockItem.StockHoldReason;
            ProductId = stockItem.ProductId;
            AvailableQuantity = stockItem.AvailableQuantity;
            InsideDiameter = stockItem.InsideDiameter;
            OuterDiameter = stockItem.OuterDiameter;
            ProductCode = stockItem.ProductCode;
            CostPerWeight = stockItem.CostPerWeight;
            StockItemId = stockItem.StockItemId;
            Length = stockItem.Length;
            AllocatedLength = stockItem.AllocatedLength;
            ProductControlCode = stockItem.ProductControlCode;
            IsMachinedPart = stockItem.IsMachinedPart;
            IsZeroWeightService = stockItem.IsZeroWeightService;
            TotalCost = stockItem.TotalCost;
            PieceCost = stockItem.PieceCost;
            MillName = stockItem.MillName;
            WarehouseName = stockItem.WarehouseName;
            StratificationRank = stockItem.StratificationRank;
        }

    }
}