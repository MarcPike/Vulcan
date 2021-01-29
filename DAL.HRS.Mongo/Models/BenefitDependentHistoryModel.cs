using DAL.HRS.Mongo.DocClass.Properties;
using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.DocClass.Benefits
{
    public class BenefitDependentHistoryModel: BaseListModel, IHavePropertyValues
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime? CreatedOn { get; set; }
        public string DependentName { get; set; }
        public PropertyValueRef RelationShip { get; set; }
        public string GovernmentId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }

        public BenefitDependentHistoryModel()
        {
        }

        public override string ToString()
        {
            return
                $"Id: {Id} Created: {CreatedOn?.ToLongDateString()} Dependent: {DependentName} Relation: {RelationShip?.Code} GovernmentId: {GovernmentId} DOB: {DateOfBirth?.ToShortDateString()} Begin: {BeginDate?.ToShortDateString()} End: {EndDate?.ToShortDateString()}";
        }

        public override int GetHashCode()
        {
            var hashValue = ToString();
            return hashValue.GetHashCode();
        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            LoadCorrectPropertyValueRef(entity, RelationShip);
        }

        public BenefitDependentHistoryModel(BenefitDependentHistory hist)
        {
            Id = hist.Id.ToString();
            CreatedOn = hist.CreatedOn;
            DependentName = hist.DependentName;
            RelationShip = hist.RelationShip;
            GovernmentId = hist.GovernmentId;
            DateOfBirth = hist.DateOfBirth?.Date;
            BeginDate = hist.BeginDate?.Date;
            EndDate = hist.EndDate?.Date;
        }
    }
}