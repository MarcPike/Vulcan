using System;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class EmergencyContact
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Relationship { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} Name: {Name} Relationship {Relationship} Phone: {PhoneNumber}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }
    }
}