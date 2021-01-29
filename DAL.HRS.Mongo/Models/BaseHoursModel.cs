using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.Models
{
    public class BaseHoursModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<decimal> Values { get; set; }
        public BaseHoursModel()
        {
        }

        public BaseHoursModel(BaseHours baseHours)
        {
            Id = baseHours.Id.ToString();
            Name = baseHours.Name;
            Values = baseHours.Values;
        }
    }
}
