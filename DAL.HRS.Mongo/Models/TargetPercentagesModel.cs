using System.Collections.Generic;
using System.Linq;
using DAL.HRS.Mongo.DocClass.Compensation;

namespace DAL.HRS.Mongo.Models
{
    public class TargetPercentagesModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<decimal> Values { get; set; }
        public TargetPercentagesModel()
        {
        }

        public TargetPercentagesModel(TargetPercentage targetPercentage)
        {
            Id = targetPercentage.Id.ToString();
            Name = targetPercentage.Name;
            Values = targetPercentage.Values;
        }
    }
}