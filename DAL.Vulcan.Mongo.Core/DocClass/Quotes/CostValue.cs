using System;
using System.Linq;
using DAL.Vulcan.Mongo.Core.Quotes;

namespace DAL.Vulcan.Mongo.Core.DocClass.Quotes
{
    public class CostValue
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public PerType PerType { get; set; } = PerType.PerPound;
        public string PerTypeName { get; set; } = "PerPound";
        public string TypeName { get; set; }
        public decimal InternalCost { get; set; }
        public decimal ProductionCost { get; set; }
        public decimal GrossProfitPercent => (InternalCost > 0) ? (ProductionCost / InternalCost) * 100 : 0;
        public decimal MinimumCost { get; set; } = 0;
        public bool IsActive { get; set; } = false;

        public decimal TotalPounds { get; set; }
        public decimal TotalInches { get; set; }
        public decimal TotalPieces { get; set; }

        public string Currency { get; set; } = string.Empty;

        public decimal TotalInternalCost
        {
            get
            {
                var result = (decimal)0;
                if (!IsActive) return result;
                switch (PerType)
                {
                    case PerType.PerPiece:
                        result = InternalCost * TotalPieces;
                        break;
                    case PerType.PerInch:
                        result = InternalCost * TotalInches;
                        break;
                    case PerType.PerFoot:
                        result = InternalCost * TotalInches / 12;
                        break;
                    case PerType.PerPound:
                        result = InternalCost * TotalPounds;
                        break;
                    case PerType.PerKg:
                        result = InternalCost * (TotalPounds * (decimal)0.453592);
                        break;
                    case PerType.PerLot:
                        result = InternalCost;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if ((result > 0) && (result < MinimumCost))
                    result = MinimumCost;

                return result;

            }

        }

        public decimal TotalProductionCost
        {
            get
            {
                var result = (decimal)0;
                if (!IsActive) return result;
                switch (PerType)
                {
                    case PerType.PerPiece:
                        result = ProductionCost * TotalPieces;
                        break;
                    case PerType.PerInch:
                        result = ProductionCost * TotalInches;
                        break;
                    case PerType.PerFoot:
                        result = ProductionCost * TotalInches / 12;
                        break;
                    case PerType.PerPound:
                        result = ProductionCost * TotalPounds;
                        break;
                    case PerType.PerKg:
                        result = ProductionCost * (TotalPounds * (decimal)0.453592);
                        break;
                    case PerType.PerLot:
                        result = ProductionCost;
                        break;
                    default:
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }

                if ((result == 0) && (MinimumCost > 0))
                {
                    result = MinimumCost;
                } else if ((result > 0) && (result < MinimumCost))
                    result = MinimumCost;

                return result;

            }

        }

    }
}