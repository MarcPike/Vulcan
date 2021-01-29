using DAL.HRS.Mongo.DocClass.Properties;
using MongoDB.Bson.Serialization.Attributes;
using System;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Compensation
{
    public class Bonus 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime DatePaid { get; set; }
        public decimal Amount { get; set; }
        public PropertyValueRef BonusType { get; set; }
        public decimal PercentPaid { get; set; }
        public int FiscalYear { get; set; }

        public int CalendarYear => DatePaid.Year;

        public string Comment { get; set; }
    }


}