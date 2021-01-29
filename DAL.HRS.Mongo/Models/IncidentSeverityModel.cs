using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.Models
{
    public class IncidentSeverityModel
    {
        public string Name { get; set; }  //Employee Location

        public IncidentSeverityTypeModel[] Series { get; set; }


        public IncidentSeverityModel()
        {

        }

        public class IncidentSeverityTypeModel
        {
            public string Name { get; set; }
            public int Value { get; set; }


        }
    }
}
