using System;
using System.Linq;
using Vulcan.IMetal.Context.PurchaseOrders;
using Vulcan.IMetal.Models;
using Vulcan.IMetal.Queries.Companies;

namespace DAL.iMetal.SearchResults
{
    public class PurchaseOrderItemResult
    {

        public string Coid { get; set; }
        public int Id { get; set; }
        public int PoNumber { get; set; }
        public int PoHeaderId { get; set; }
        public int PoItemNumber { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSize { get; set; }
        public string ProductCondition { get; set; }
        public DateTime DueDate { get; set; }
        public string HeatNumber { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string VendorCode { get; set; }
        public string VendorName { get; set; }
        public CompanyAddressModel DeliverToAddress { get; set; }



        public decimal InsideDiameter { get; set; }
        public decimal OutsideDiameter { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Thick { get; set; }
        public decimal Density { get; set; }
        public string DensityUnit { get; set; }
        public decimal TheoWeight { get; set; }


        public decimal OrderedQuantity { get; set; }
        public decimal BalanceQuantity { get; set; }
        public decimal AllocatedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public string QuantityUnit { get; set; }


        public int OrderedPieces { get; set; }
        public int BalancePieces { get; set; }
        public int AllocatedPieces { get; set; }
        public int AvailablePieces { get; set; }
        public string PiecesUnit { get; set; }


        public decimal OrderedLengthBase { get; set; }
        public decimal BalanceLengthBase { get; set; }
        public decimal AllocatedLengthBase { get; set; }
        public decimal AvailableLengthBase { get; set; }
        public string LengthUnit { get; set; }


        public decimal OrderedWeightBase { get; set; }
        public decimal OrderedWeightKgs { get; set; }
        public decimal OrderedWeightLbs { get; set; }

        public decimal BalanceWeightBase { get; set; }
        public decimal BalanceWeightKgs { get; set; }
        public decimal BalanceWeightLbs { get; set; }

        public decimal AllocatedWeightBase { get; set; }
        public decimal AllocatedWeightKgs { get; set; }
        public decimal AllocatedWeightLbs { get; set; }

        public decimal AvailableWeightBase { get; set; }
        public decimal AvailableWeightLbs { get; set; }
        public decimal AvailableWeightKgs { get; set; }

        public string WeightUnit { get; set; }



        public decimal MaterialCost { get; set; }
        public decimal TransportCost { get; set; }
        public decimal ProductionCost { get; set; }
        public decimal SurchargeCost { get; set; }
        public decimal MiscellaneousCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal CostPerLb { get; set; }
        public decimal CostPerKg { get; set; }
        public decimal CostPerInch { get; set; }



        public PurchaseOrderItemResult(PurchaseOrdersContext context, string coid, PurchaseOrderItem item)
        {
            Coid = coid;
            Id = item.Id;
            ProductCategory = item.Product?.ProductCategory?.Category;
            ProductCondition = item.SpecificationValue2;
            Description = item.Product?.Description;
            ProductSize = item.Product?.SizeDescription;
            ProductCode = item.Product?.Code;
            PoNumber = item.PurchaseOrderHeader_BtbPurchaseHeaderId?.Number ?? 0;
            PoHeaderId = item.PurchaseOrderHeader_BtbPurchaseHeaderId?.Id ?? 0;
            PoItemNumber = item.ItemNumber ?? 0;
            DueDate = item.DueDate ?? DateTime.Parse("1/1/1980");
            HeatNumber = item.PC_HeatNumber;

            var header = item.PurchaseOrderHeader_BtbPurchaseHeaderId;
            VendorName = header?.Company_SupplierId?.Name;
            VendorCode = header?.Company_SupplierId?.Code;
            var deliverTo = header?.Address_DeliverFromAddressId;
            DeliverToAddress = (deliverTo != null) ? new CompanyAddressModel(deliverTo, Coid, VendorName) : null;

            var warehouseId = item.PurchaseOrderHeader_BtbPurchaseHeaderId?.DeliveryWarehouseId ?? 0;
            var warehouse = context.Warehouse.FirstOrDefault(x => x.Id == warehouseId);
            WarehouseCode = warehouse?.Code;
            WarehouseName = warehouse?.Name;

            OutsideDiameter = item.PC_OutsideDiameter;
            InsideDiameter = item.PC_InsideDiameter;
            Length = item.PC_Length;
            Density = item.PC_Density;
            DensityUnit = item.PC_DensityUnit;
            Width = item.PC_Width;
            Thick = item.PC_Thick;
            TheoWeight = item.GetTheoWeight();


            OrderedQuantity = item.PC_OrderedQuantity;
            BalanceQuantity = item.PC_BalanceQuantity;
            AllocatedQuantity = item.PC_AllocatedQuantity;
            AvailableQuantity = item.PC_AvailableQuantity;
            QuantityUnit = item.PC_QuantityUnit;

            OrderedPieces = item.PC_OrderedPieces;
            BalancePieces = item.PC_BalancePieces;
            AllocatedPieces = item.PC_AllocatedPieces;
            AvailablePieces = item.PC_AvailablePieces;
            PiecesUnit = item.PC_PiecesUnit;

            OrderedLengthBase = item.PC_OrderedLengthBase;
            BalanceLengthBase = item.PC_BalanceLengthBase;
            AllocatedLengthBase = item.PC_AllocatedLengthBase;
            AvailableLengthBase = item.PC_AvailableLengthBase;
            LengthUnit = item.PC_LengthUnit;

            OrderedWeightBase = item.PC_OrderedWeightBase;
            OrderedWeightLbs = item.PC_OrderedWeightLbs;
            OrderedWeightKgs = item.PC_OrderedWeightKgs;

            BalanceWeightBase = item.PC_BalanceWeightBase;
            BalanceWeightLbs = item.PC_BalanceWeightLbs;
            BalanceWeightKgs = item.PC_BalanceWeightKgs;

            AllocatedWeightBase = item.PC_AllocatedWeightBase;
            AllocatedWeightLbs = item.PC_AllocatedWeightLbs;
            AllocatedWeightKgs = item.PC_AllocatedWeightKgs;

            AvailableWeightBase = item.PC_AvailableWeightBase;
            AvailableWeightLbs = item.PC_AvailableWeightLbs;
            AvailableWeightKgs = item.PC_AvailableWeightKgs;

            WeightUnit = item.PC_WeightUnit;

            MaterialCost = item.PC_MaterialCostTotal;
            ProductionCost = item.PC_ProductionCostTotal;
            TransportCost = item.PC_TransportCostTotal;
            SurchargeCost = item.PC_TransportCostTotal;
            MiscellaneousCost = item.PC_MiscellaneousCostTotal;
            TotalCost = item.PC_TotalCost;

            //CostPerInch = item.PC_CostPerInch;
            //CostPerLb = item.PC_CostPerLb;
            //CostPerKg = item.PC_CostPerKg;
        }
    }
}
