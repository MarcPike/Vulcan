using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;
using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;

namespace DAL.HRS.Mongo.Models
{
    public class PayGradeHistoryModel : BaseListModel, IHavePropertyValues
    {
        public string Id { get; set; }
        public PropertyValueRef PayGradeType { get; set; }
        public decimal Maximum { get; set; }
        public decimal Minimum { get; set; }
        public DateTime? CreateDateTime { get; set; }

        public override string ToString()
        {
            var nullString = "null";
            string created = nullString;
            if (CreateDateTime != null)
            {
                created = $"{CreateDateTime.Value.ToShortDateString()} {CreateDateTime.Value.ToShortTimeString()}";
            }

            return
                $"Id: {Id} PayGrade: {PayGradeType?.Code ?? nullString} Min: {Minimum} Max: {Maximum} Created: {created}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, PayGradeType);
        }


        public PayGradeHistoryModel()
        {
        }

        public PayGradeHistoryModel(PayGradeHistory h)
        {
            var enc = Encryption.NewEncryption;

            Id = h.Id.ToString();
            PayGradeType = h.PayGradeType;
            Minimum = enc.Decrypt<decimal>(h.Minimum);
            Maximum = enc.Decrypt<decimal>(h.Maximum);
            CreateDateTime = Encryption.NewEncryption.Decrypt<DateTime>(h.CreateDateTime);

            //UpdatePropertyReferences.Execute(this);

        }

    }
}