using DAL.HRS.Mongo.DocClass.Employee;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.DocClass.Legal
{
   public class RegulatoryIncidentInvestigator
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public EmployeeRef Investigator { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? InvestigationDate { get; set; }
        

        public override int GetHashCode()
        {
            var hashValue = Investigator + InvestigationDate?.ToLongDateString();
            return hashValue.GetHashCode();
        }

        public override string ToString()
        {
            return $"Investigator: {Investigator} Date: {InvestigationDate}";
        }

        public RegulatoryIncidentInvestigator()
        {

        }

       

    }
}
