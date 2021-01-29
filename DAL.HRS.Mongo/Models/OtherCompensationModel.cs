using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;
using MongoDB.Bson.Serialization.Attributes;
using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;
using DAL.Vulcan.Mongo.Base.Static_Helper_Classes;

namespace DAL.HRS.Mongo.Models
{
    public class OtherCompensationModel : BaseListModel, IHavePropertyValues
    {
        public string Id { get; set; }
        public PropertyValueRef CompensationType { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EffectiveDate { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? EndDate { get; set; }

        public decimal Amount { get; set; }
        public string Comment { get; set; }

        public bool IsAllowance { get; set; }

        public decimal Annualized { get; set; }

        public override string ToString()
        {
            var nullString = "null";
            return
                $"Id: {Id} Comp Type: {CompensationType?.Code ?? nullString} Effective: {EffectiveDate.ToShortDateString()} EndDate: {EndDate?.ToShortDateString() ?? nullString} Amount: {Amount} Comment: {Comment} Annualized: {Annualized}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, CompensationType);
        }

        public OtherCompensationModel()
        {
        }

        public OtherCompensationModel(OtherCompensation o)
        {
            var enc = Encryption.NewEncryption;

            Amount = enc.Decrypt<decimal>(o.Amount);
            Comment = enc.Decrypt<string>(o.Comment);
            CompensationType = o.CompensationType;
            EffectiveDate = enc.Decrypt<DateTime>(o.EffectiveDate);
            Annualized = enc.Decrypt<decimal>(o.Annualized);
            IsAllowance = o.CompensationType?.Code?.Contains("Allowance") ?? false;
            try
            {
                if (o.EndDate != null)
                {
                    EndDate = enc.Decrypt<DateTime>(o.EndDate);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                EndDate = null;
            }
            Id = o.Id.ToString();

            //UpdatePropertyReferences.Execute(this);
        }

    }
}