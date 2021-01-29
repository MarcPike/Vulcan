using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vulcan.IMetal.Queries.ProductBalances;

namespace Vulcan.WebApi2.Models
{
    public class ProductBalancesModel
    {
        public int ProductId { get; set; }
        public int StockItemId { get; set; }
        public string Coid { get; set; }
        public DateTime CreateDate { get; set; }
        public string MetalCategory { get; set; }
        public string MetalType { get; set; }
        public string StockType { get; set; }
        public string ProductCode { get; set; }
        public string StockGrade { get; set; }
        public string ProductCondition { get; set; }
        public string ProductCategory { get; set; }
        public decimal Width { get; set; }
        public decimal Length { get; set; }
        public decimal Thick { get; set; }
        public decimal InsideDiameter { get; set; }
        public decimal OuterDiameter { get; set; }
        public decimal Density { get; set; }
        public string ProductControlCode { get; set; }
        public decimal TheoWeight { get; set; }

        public BalanceMeasurement Available { get; set; }
        public BalanceMeasurement Physical { get; set; } 
        public BalanceMeasurement Averaging { get; set; }
        // Quarantine
        public BalanceMeasurement Quarantine { get; set; }
        // SalesAllocated
        public BalanceMeasurement SalesAllocated { get; set; } 
        // SalesOrder
        public BalanceMeasurement SalesOrder { get; set; } 
        // Production Allocated
        public BalanceMeasurement ProductionAllocated { get; set; } 
        // Transient
        public BalanceMeasurement Transient { get; set; } 
        // TransientAllocated
        public BalanceMeasurement TransientAllocated { get; set; } 
        // Incoming
        public BalanceMeasurement Incoming { get; set; } 
        // Reserved
        public BalanceMeasurement Reserved { get; set; } 
        // SalesReserved
        public BalanceMeasurement SalesReserved { get; set; } 
        // Production Due
        public BalanceMeasurement ProductionDue { get; set; } 
        // Stock Unavailable
        public BalanceMeasurement StockUnavailable { get; set; } 
        // Production Due Allocated
        public BalanceMeasurement ProductionDueAllocated { get; set; } 

        public ProductBalancesModel(ProductBalancesAdvancedQuery row)
        {
            try
            {
                ProductId = row.ProductId;
                StockItemId = row.StockItemId;
                Coid = row.Coid;
                CreateDate = row.CreateDate;
                MetalCategory = row.MetalCategory;
                MetalType = row.MetalType;
                StockType = row.StockType;
                ProductCode = row.ProductCode;
                StockGrade = row.StockGrade;
                ProductCondition = row.ProductCondition;
                ProductCategory = row.ProductCategory;
                Width = row.Width;
                Length = row.Length;
                Thick = row.Thick;
                InsideDiameter = row.InsideDiameter;
                OuterDiameter = row.OuterDiameter;
                Density = row.Density;
                TheoWeight = row.TheoWeight;
                ProductControlCode = row.ProductControlCode;
                Available = row.Available;
                Physical = row.Physical;
                Averaging = row.Averaging;
                Quarantine = row.Quarantine;
                SalesAllocated = row.SalesAllocated;
                SalesOrder = row.SalesOrder;
                ProductionAllocated = row.ProductionAllocated;
                Transient = row.Transient;
                TransientAllocated = row.TransientAllocated;
                Incoming = row.Incoming;
                Reserved = row.Reserved;
                SalesReserved = row.SalesReserved;
                ProductionDue = row.ProductionDue;
                StockUnavailable = row.StockUnavailable;
                ProductionDueAllocated = row.ProductionDueAllocated;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
