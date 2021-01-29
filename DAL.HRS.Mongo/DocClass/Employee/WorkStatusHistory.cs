using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class WorkStatusHistory: IHavePropertyValues
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? EffectiveDate { get; set; }

        public PropertyValueRef ReasonForChange { get; set; }
        public string Comments { get; set; }
        public string FieldChanged { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? TerminationDate { get; set; }

        public override string ToString()
        {
            return
                $"Id: {Id} Effective: {EffectiveDate} Reason: {ReasonForChange?.Code} Termination: {TerminationDate} Fields: {FieldChanged} Old: {OldValue} New: {NewValue}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            ReasonForChange?.Load(entity);
        }
    }
}
