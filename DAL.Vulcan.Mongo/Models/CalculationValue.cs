namespace DAL.Vulcan.Mongo.Models
{
    public class CalculationValue
    {
        public string Name { get; set; }
        public decimal Value { get; set; }

        public CalculationValue()
        {

        }

        public CalculationValue(string name, decimal value)
        {
            Name = name;
            Value = value;
        }
    }
}