using System;
using System.Collections.Generic;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class DrugTestMedicalInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Comments { get; set; }
        public List<IncidentLeave> LeaveHistory { get; set; } = new List<IncidentLeave>();
    }

}
