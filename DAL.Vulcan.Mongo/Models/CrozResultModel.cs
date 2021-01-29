using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Vulcan.Mongo.Models
{
    public class CrozCalculationDataModel
    {
        public CrozUnitValue WeightEach { get; set; }
        public CrozUnitValue SellingProductWeightEach { get; set; }
        public CrozUnitValue TestingPieceWeightEach { get; set; }
        public CrozUnitValue WeightTotal { get; set; }
        public CrozMaterialCost MaterialCost { get; set; }
        public CrozCostPriceValue MaterialCostEach { get; set; }
        public CrozCostPriceValue SawingCostEach { get; set; }
        public CrozCostPriceValue MachiningCostEach { get; set; }
        public CrozCostPriceValue HeatTreatmentCostEach { get; set; }
        public CrozCostPriceValue HardeningCostEach { get; set; }
        public CrozCostPriceValue ForgingCostEach { get; set; }
        public CrozCostPriceValue MechanicalTestingCostEach { get; set; }
        public CrozCostPriceValue UltraSonicProbeTestingCostEach { get; set; }
        public MachiningCostDetailEach MachiningCostDetailEach { get; set; }
        public CrozCostPriceValue CostEach { get; set; }
        public CrozCostPriceValue CostTotal { get; set; }
        public CrozCostPriceValue SellingPriceEach { get; set; }
        public CrozUnitValue SellingPricePerUnit { get; set; }
        public CrozCostPriceValue SellingPriceTotal { get; set; }
        public CrozCostPriceValue TotalItemCostPriceValue { get; set; }
        public CrozUnitValue CostPerUnit { get; set; }

    }

    public class MachiningCostDetailEach
    {
        public CrozCostPriceValue TotalCost { get; set; }
        public CrozCostPriceValue TurningCost { get; set; }
        public CrozCostPriceValue TrepanningCost { get; set; }
        public CrozCostPriceValue CbsCost { get; set; }
        public CrozCostPriceValue DhsCost { get; set; }
        public CrozCostPriceValue TurretsSmallBoresCost { get; set; }
        public CrozCostPriceValue TurretsTusCost { get; set; }
        public CrozCostPriceValue SteppedBoresCost { get; set; }
        public CrozCostPriceValue BlindBoresCost { get; set; }
        public CrozCostPriceValue LiftingHolesCost { get; set; }

    }

    public class CrozUnitValue
    {
        public decimal Value { get; set; } = 0;
        public string Unit { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
    }

    public class CrozCostPriceValue
    {
        public decimal Value { get; set; } = 0;
        public string Currency { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
    }

    public class CrozMaterialCost
    {
        public string Grade { get; set; } = string.Empty;
        public decimal Cost { get; set; } = 0;
        public string Unit { get; set; } = string.Empty;
    }


}
