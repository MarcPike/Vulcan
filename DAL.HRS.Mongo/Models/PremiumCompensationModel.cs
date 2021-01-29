using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;
using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;

namespace DAL.HRS.Mongo.Models
{
    public class PremiumCompensationModel : BaseListModel, IHavePropertyValues
    {
        public string Id { get; set; }
        public string Branch { get; set; }
        public decimal? Value { get; set; }
        public PropertyValueRef PremiumCompensationType { get; set; }
        public PropertyValueRef ValueType { get; set; }
        public decimal OvertimeRateFactor { get; set; }
        public decimal DoubleOvertimeRateFactor { get; set; }
        public string Comment { get; set; }

        public override string ToString()
        {
            var nullString = "null";
            return $"Id: {Id} Branch: {Branch} Value: {Value?.ToString() ?? nullString} CompType: {PremiumCompensationType.Code} ValueType: {ValueType.Code} Overtime Rate Factor: {OvertimeRateFactor} Double Factor: {DoubleOvertimeRateFactor} Comments: {Comment}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, PremiumCompensationType);
            LoadCorrectPropertyValueRef(entity, ValueType);
        }

        public PremiumCompensationModel()
        {
        }

        public PremiumCompensationModel(PremiumComp p)
        {
            var enc = Encryption.NewEncryption;

            Branch = p.Branch;
            Comment = enc.Decrypt<string>(p.Comment);
            OvertimeRateFactor = p.OvertimeRateFactor;
            DoubleOvertimeRateFactor = p.DoubleOvertimeRateFactor;
            PremiumCompensationType = p.PremiumCompensationType;
            Id = p.Id.ToString();
            ValueType = p.ValueType;
            Value = p.Value;

            //UpdatePropertyReferences.Execute(this);

        }

    }
}