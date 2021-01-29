using DAL.iMetal.Core.Queries;
using System;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class IncomingAnalysisDataModel
    {
        private decimal _costPerLb;
        private decimal _costPerKg;
        private decimal _costPerInch;
        private decimal _materialCostTotal;
        private decimal _productionCostTotal;
        private decimal _transportCostTotal;
        private decimal _surchargeCostTotal;
        private decimal _miscellaneousCostTotal;
        private decimal _totalCost;
        private decimal _exchangeRate { get; set; } = 1;
        public string Coid { get; set; }
        public string Status { get; set; }
        public int? PoNumber { get; set; }
        public int? ItemNumber { get; set; }
        public string StockType { get; set; }
        public string Location { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Thick { get; set; }
        public decimal InsideDiameter { get; set; }
        public decimal OuterDiameter { get; set; }
        public string OrderedPiecesUnit { get; set; } 
        public string OrderedLengthUnit { get; set; } 
        public string OrderedWeightUnit { get; set; } 
        public string OrderedQuantityUnit { get; set; }
        public int OrderedPieces { get; set; } 
        public decimal OrderedQuantity { get; set; }
        public decimal OrderedLength { get; set; } 
        public decimal OrderedWeight { get; set; }
        public decimal OrderedWeightLbs { get; set; }
    
        public decimal OrderedWeightKgs { get; set; }

        public string AllocatedPiecesUnit { get; set; } 
        public string AllocatedLengthUnit { get; set; } 
        public string AllocatedWeightUnit { get; set; } 
        public string AllocatedQuantityUnit { get; set; } 
        public int AllocatedPieces { get; set; } 
        public decimal AllocatedQuantity { get; set; } 
        public decimal AllocatedLength { get; set; } 
        public decimal AllocatedWeight { get; set; } 
        public decimal AllocatedWeightLbs { get; set; }
        public decimal AllocatedWeightKgs { get; set; }

        public string DeliveredPiecesUnit { get; set; }
        public string DeliveredLengthUnit { get; set; }
        public string DeliveredWeightUnit { get; set; }
        public string DeliveredQuantityUnit { get; set; }
        public int DeliveredPieces { get; set; }
        public decimal DeliveredQuantity { get; set; } 
        public decimal DeliveredLength { get; set; } 
        public decimal DeliveredWeight { get; set; } 
        public decimal DeliveredWeightLbs { get; set; }
        public decimal DeliveredWeightKgs { get; set; }

        public string BalancePiecesUnit { get; set; } 
        public string BalanceLengthUnit { get; set; } 
        public string BalanceWeightUnit { get; set; } 
        public string BalanceQuantityUnit { get; set; }
        public int BalancePieces { get; set; } 
        public decimal BalanceQuantity { get; set; }
        public decimal BalanceLength { get; set; }
        public decimal BalanceWeight { get; set; }
        public decimal BalanceWeightLbs { get; set; }
        public decimal BalanceWeightKgs { get; set; }
        public decimal FactorForKgs { get; set; }
        public decimal FactorForLbs { get; set; }


        public decimal MaterialCostTotal
        {
            get => _materialCostTotal * _exchangeRate;
            set => _materialCostTotal = value;
        }

        public decimal ProductionCostTotal
        {
            get => _productionCostTotal * _exchangeRate;
            set => _productionCostTotal = value;
        }

        public decimal TransportCostTotal
        {
            get => _transportCostTotal * _exchangeRate;
            set => _transportCostTotal = value;
        }

        public decimal SurchargeCostTotal
        {
            get => _surchargeCostTotal * _exchangeRate;
            set => _surchargeCostTotal = value;
        }

        public decimal MiscellaneousCostTotal
        {
            get => _miscellaneousCostTotal * _exchangeRate;
            set => _miscellaneousCostTotal = value;
        }

        public decimal TotalCost
        {
            get => _totalCost * _exchangeRate;
            set => _totalCost = value;
        }

        public decimal CostPerLb
        {
            get => _costPerLb * _exchangeRate;
            set => _costPerLb = value;
        }

        public decimal CostPerKg
        {
            get => _costPerKg * _exchangeRate;
            set => _costPerKg = value;
        }

        public decimal CostPerInch
        {
            get => _costPerInch * _exchangeRate;
            set => _costPerInch = value;
        }

        public string WarehouseCode { get; set; }
        public string WarehouseShortName { get; set; }
        public string WarehouseName { get; set; }
        public string StatusDescription { get; set; }
        public string StatusCode { get; set; }
        public string Supplier { get; set; }
        public string Buyer { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ItemDueDate { get; set; }


        public IncomingAnalysisDataModel()
        {
            
        }

        public IncomingAnalysisDataModel(PurchaseOrderItemsQuery queryItem, decimal exchangeRate)
        {
            _exchangeRate = exchangeRate;
            Coid = queryItem.Coid;
            Location = queryItem.Location;
            Status = queryItem.Status;
            PoNumber = queryItem.PoNumber;
            ItemNumber = queryItem.ItemNumber;
            StockType = queryItem.StockType;
            Location = queryItem.Location;
            Width = queryItem.Width;
            Length = queryItem.Length;
            Thick = queryItem.Thick;
            InsideDiameter = queryItem.InsideDiameter;
            OuterDiameter = queryItem.OuterDiameter;

            OrderedPiecesUnit = queryItem.OrderedPiecesUnit;
            OrderedLengthUnit = queryItem.OrderedLengthUnit;
            OrderedWeightUnit = queryItem.OrderedWeightUnit;
            OrderedQuantityUnit = queryItem.OrderedQuantityUnit;
            OrderedPieces = queryItem.OrderedPieces;
            OrderedQuantity = queryItem.OrderedQuantity;
            OrderedLength = queryItem.OrderedLength;
            OrderedWeight = queryItem.OrderedWeight;
            OrderedWeightLbs = queryItem.OrderedWeightLbs;
            OrderedWeightKgs = queryItem.OrderedWeightKgs;

            AllocatedPiecesUnit = queryItem.AllocatedPiecesUnit;
            AllocatedLengthUnit = queryItem.AllocatedLengthUnit;
            AllocatedWeightUnit = queryItem.AllocatedWeightUnit;
            AllocatedQuantityUnit = queryItem.AllocatedQuantityUnit;
            AllocatedPieces = queryItem.AllocatedPieces;
            AllocatedQuantity = queryItem.AllocatedQuantity;
            AllocatedLength = queryItem.AllocatedLength;
            AllocatedWeight = queryItem.AllocatedWeight;
            AllocatedWeightLbs = queryItem.AllocatedWeightLbs;
            AllocatedWeightKgs = queryItem.AllocatedWeightKgs;

            DeliveredPiecesUnit = queryItem.DeliveredPiecesUnit;
            DeliveredLengthUnit = queryItem.DeliveredLengthUnit;
            DeliveredWeightUnit = queryItem.DeliveredWeightUnit;
            DeliveredQuantityUnit = queryItem.DeliveredQuantityUnit;
            DeliveredPieces = queryItem.DeliveredPieces;
            DeliveredQuantity = queryItem.DeliveredQuantity;
            DeliveredLength = queryItem.DeliveredLength;
            DeliveredWeight = queryItem.DeliveredWeight;
            DeliveredWeightLbs = queryItem.DeliveredWeightLbs;
            DeliveredWeightKgs = queryItem.DeliveredWeightKgs;

            BalancePiecesUnit = queryItem.BalancePiecesUnit;
            BalanceLengthUnit = queryItem.BalanceLengthUnit;
            BalanceWeightUnit = queryItem.BalanceWeightUnit;
            BalanceQuantityUnit = queryItem.BalanceQuantityUnit;
            BalancePieces = queryItem.BalancePieces;
            BalanceQuantity = queryItem.BalanceQuantity;
            BalanceLength = queryItem.BalanceLength;
            BalanceWeight = queryItem.BalanceWeight;
            BalanceWeightLbs = queryItem.BalanceWeightLbs;
            BalanceWeightKgs = queryItem.BalanceWeightKgs;

            FactorForKgs = queryItem.FactorForKgs;
            FactorForLbs = queryItem.FactorForLbs;


            MaterialCostTotal = queryItem.MaterialCostTotal;
            ProductionCostTotal = queryItem.ProductionCostTotal;
            TransportCostTotal = queryItem.TransportCostTotal;
            SurchargeCostTotal = queryItem.SurchargeCostTotal;
            MiscellaneousCostTotal = queryItem.MiscellaneousCostTotal;
            TotalCost = queryItem.TotalCost;
            CostPerKg = queryItem.CostPerKg;
            CostPerLb = queryItem.CostPerLb;
            CostPerInch = queryItem.CostPerInch;
            WarehouseCode = queryItem.WarehouseCode;
            WarehouseShortName = queryItem.WarehouseShortName;
            WarehouseName = queryItem.WarehouseName;
            StatusCode = queryItem.StatusCode;
            StatusDescription = queryItem.StatusDescription;
            Supplier = queryItem.Supplier;
            Buyer = queryItem.Buyer;
            DueDate = queryItem.DueDate;
            ItemDueDate = queryItem.ItemDueDate;
        }
}
}
