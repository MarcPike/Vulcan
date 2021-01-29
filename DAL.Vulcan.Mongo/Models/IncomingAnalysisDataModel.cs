using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vulcan.IMetal.Helpers;
using Vulcan.IMetal.Queries.PurchaseOrderItems;

namespace DAL.Vulcan.Mongo.Models
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
        public PurchaseOrderItemUnitValues UnitValues { get; set; } = new PurchaseOrderItemUnitValues();

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

        public IncomingAnalysisDataModel(PurchaseOrderItemsAdvancedQuery queryItem, decimal exchangeRate)
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
            UnitValues = queryItem.UnitValues;
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
