using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class CostValueModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PerType PerType { get; set; }
        public string PerTypeName { get; set; }
        public string TypeName { get; set; }
        public decimal InternalCost { get; set; }
        public decimal ProductionCost { get; set; }
        public decimal GrossProfitPercent => (InternalCost > 0) ? (ProductionCost / InternalCost) * 100 : 0;
        public decimal MinimumCost { get; set; } = 0;
        public bool IsActive { get; set; }
        public decimal TotalPounds { get; set; } = 0;
        public decimal TotalInches { get; set; } = 0;
        public decimal TotalPieces { get; set; } = 0;
        public decimal TotalInternalCost { get; set; } = 0;
        public decimal TotalProductionCost { get; set; } = 0;

        public CostValueModel()
        {
        }
        
        public CostValueModel(CostValue value)
        {
            Id = value.Id;
            PerType = value.PerType;
            PerTypeName = value.PerType.ToString();
            TypeName = value.TypeName;
            InternalCost = value.InternalCost;
            ProductionCost = value.ProductionCost;
            MinimumCost = value.MinimumCost;
            TotalPounds = value.TotalPounds;
            TotalInches = value.TotalInches;
            TotalPieces = value.TotalPieces;
            TotalInternalCost = value.TotalInternalCost;
            TotalProductionCost = value.TotalProductionCost;
            IsActive = value.IsActive;
        }

    public CostValue AsCostValue()
        {
            return new CostValue()
            {
                Id = Id,
                MinimumCost = MinimumCost,
                InternalCost = InternalCost,
                ProductionCost = ProductionCost,
                PerType = PerType,
                PerTypeName = PerType.ToString(),
                TypeName = TypeName,
                IsActive = IsActive,
                TotalPounds = TotalPounds,
                TotalInches = TotalInches,
                TotalPieces = TotalPieces,
            };
        }

    }
}
