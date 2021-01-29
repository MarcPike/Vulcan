namespace DAL.Vulcan.Mongo.Core.DocClass.Croz
{
    public class CrozGrade
    {
        public string Grade { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int SawingIndex { get; set; } = 0;
        public CrozDensity Density { get; set; } = new CrozDensity();
        public CrozMaterialCost MaterialCost { get; set; } = new CrozMaterialCost();
        public CrozHeatTreatmentMinimumCost HeatTreatmentMinimumCost { get; set; } = new CrozHeatTreatmentMinimumCost();
        public CrozTestingMinimumCost TestingMinimumCost { get; set; } = new CrozTestingMinimumCost();

    }
}