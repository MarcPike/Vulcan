using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Properties;
using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;

namespace DAL.HRS.Mongo.Models
{
    public class BonusModel : BaseListModel, IHavePropertyValues
    {
        public string Id { get; set; }
        public PropertyValueRef BonusType { get; set; }
        public decimal PercentPaid { get; set; }
        public decimal Amount { get; set; }
        public DateTime DatePaid { get; set; }

        public int FiscalYear { get; set; }
        public string Comment { get; set; }

        public BonusModel()
        {
        }

        public override string ToString()
        {
            var nullString = "null";

            return
                $"Id: {Id} Type: {BonusType?.Code ?? nullString} Percent: {PercentPaid} Amount: {Amount} Date: {DatePaid} FiscalYr: {FiscalYear} Comment: {Comment}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, BonusType);
        }

        public BonusModel(Bonus b)
        {
            Id = b.Id.ToString();
            Amount = b.Amount;
            BonusType = b.BonusType;
            Comment = b.Comment;
            DatePaid = b.DatePaid.Date;
            FiscalYear = b.FiscalYear;
            PercentPaid = b.PercentPaid;

            //UpdatePropertyReferences.Execute(this);

        }

    }
}