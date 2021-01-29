using System;
using System.Linq;
using Vulcan.IMetal.Context.Orders;
using Vulcan.IMetal.Queries.PurchaseOrderItems;

namespace Vulcan.IMetal.Results
{
    public class OrderResult
    {
        public int Number;
        public Company Company { get; set; }

        public decimal ActualMaterialCost;
        public decimal ActualTransportCost { get; set; }

        public decimal ActualSurchargeCost { get; set; }

        public decimal ActualProductionCost { get; set; }

        public decimal ActualMiscellaneousCost { get; set; }

        public decimal TotalPnl => (CustomerMaterialValue + CustomerMiscellaneousValue + CustomerProductionValue +
                                    CustomerSurchargeValue + CustomerTransportValue) -
                                   (ActualMaterialCost + ActualMiscellaneousCost + ActualProductionCost +
                                    ActualSurchargeCost + ActualTransportCost);

        public string SalesTypeCode { get; set; }
        public string SalesTypeDescription { get; set; }
        public Branch OrderBranch { get; set; }
        public Branch DeliverToBranch { get; set; }
        public Warehouse DeliverToWarehouse { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? SaleDate { get; set; }
        public decimal CustomerTransportValue { get; set; }

        public decimal CustomerSurchargeValue { get; set; }

        public decimal CustomerMiscellaneousValue { get; set; }

        public decimal CustomerProductionValue { get; set; }

        public decimal CustomerMaterialValue { get; set; }
        public string DeliverToPostCode { get; set; }

        public string DeliverToTown { get; set; }

        public string DeliverToCounty { get; set; }

        public string DeliverToCompanyName { get; set; }

        public string DeliverToAddress { get; set; }

        public string OrderType { get; set; }

        public OrderResult()
        {
        }

        public OrderResult(SalesHeader header)
        {
            //Company = header.Company_CustomerId;
            ActualMaterialCost = header.SalesTotal_BalanceTotalId?.ActualMaterialCost ?? 0;
            ActualMiscellaneousCost = header.SalesTotal_BalanceTotalId?.ActualMiscellaneousCost ?? 0;
            ActualProductionCost = header.SalesTotal_BalanceTotalId?.ActualProductionCost ?? 0;
            ActualSurchargeCost = header.SalesTotal_BalanceTotalId?.ActualSurchargeCost ?? 0;
            ActualTransportCost = header.SalesTotal_BalanceTotalId?.ActualTransportCost ?? 0;
            CustomerMaterialValue = header.SalesTotal_BalanceTotalId?.CustomerMaterialValue ?? 0;
            CustomerProductionValue = header.SalesTotal_BalanceTotalId?.CustomerProductionValue ?? 0;
            CustomerMiscellaneousValue = header.SalesTotal_BalanceTotalId?.CustomerMiscellaneousValue ?? 0;
            CustomerSurchargeValue = header.SalesTotal_BalanceTotalId?.CustomerSurchargeValue ?? 0;
            CustomerTransportValue = header.SalesTotal_BalanceTotalId?.CustomerTransportValue ?? 0;
            SalesTypeCode = header.SalesType.Code;
            SalesTypeDescription = header.SalesType.Description;
            DeliverToCompanyName = header.Address_DeliverToAddressId.Company.FirstOrDefault()?.Name;
            DeliverToAddress = header.Address_DeliverToAddressId.Address1;
            DeliverToCounty = header.Address_DeliverToAddressId.County;
            DeliverToTown = header.Address_DeliverToAddressId.Town;
            DeliverToPostCode = header.Address_DeliverToAddressId.Postcode;
            OrderBranch = header.Branch_BranchId;
            DeliverToBranch = header.Branch_DeliveryBranchId;
            DeliverToWarehouse = header.Warehouse_DeliveryWarehouseId;
            DueDate = header.DueDate;
            Number = header.Number ?? 0;
            OrderType = header.SalesType.Code;
            
            SaleDate = header.SaleDate;
        }

    }
}
