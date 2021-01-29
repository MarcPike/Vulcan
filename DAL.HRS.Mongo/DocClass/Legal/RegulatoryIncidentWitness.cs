using DAL.HRS.Mongo.DocClass.Employee;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.DocClass.Legal
{
    public class RegulatoryIncidentWitness
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public EmployeeRef Employee { get; set; }
        public string ThirdParty { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? StatementDate { get; set; }

        public string Statement { get; set; }

        public override int GetHashCode()
        {
            var hashValue = Employee + ThirdParty + Statement + StatementDate?.ToLongDateString() ?? "";
            return hashValue.GetHashCode();
        }

        public override string ToString()
        {
            return $"Employee: {Employee} ThirdParty: {ThirdParty} Statement: {Statement} StatementDate: [{StatementDate?.ToLongDateString()}]";
        }
    }
}
