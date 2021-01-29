using DAL.HRS.Mongo.DocClass.Properties;
using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.DocClass.Benefits
{
    public class BenefitDependentModel: BaseListModel, IHavePropertyValues
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DependentName { get; set; }
        public PropertyValueRef RelationShip { get; set; }
        public PropertyValueRef Gender { get; set; }
        public string GovernmentId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PrimaryCarePhysicianId { get; set; }
        public bool? Surcharge { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public PropertyValueRef DesignationType { get; set; }
        public PropertyValueRef BeneficiaryPercentageType { get; set; }
        public bool AddedOrModified { get; set; } = false;

        public override string ToString()
        {
            return $"Id: {Id} Dependent: {DependentName} Relationship: {RelationShip?.Code} Gender: {Gender?.Code} Begin: {BeginDate?.ToLongDateString()} End: {EndDate?.ToLongDateString()}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, RelationShip);
            LoadCorrectPropertyValueRef(entity, Gender);
            LoadCorrectPropertyValueRef(entity, DesignationType);
            LoadCorrectPropertyValueRef(entity, BeneficiaryPercentageType);
        }

        public BenefitDependentModel()
        {
        }

        public BenefitDependentModel(BenefitDependent dep)
        {
            Id = dep.Id.ToString();
            DependentName = dep.DependentName;
            RelationShip = dep.RelationShip;
            GovernmentId = dep.GovernmentId;
            DateOfBirth = dep.DateOfBirth?.Date;
            PrimaryCarePhysicianId = dep.PrimaryCarePhysicianId;
            BeginDate = dep.BeginDate?.Date;
            EndDate = dep.EndDate?.Date;
            DesignationType = dep.DesignationType;
            BeneficiaryPercentageType = dep.BeneficiaryPercentageType;
            Gender = dep.Gender;
            Surcharge = dep.Surcharge ?? false;
            AddedOrModified = false;
        }

    }
}