using DAL.HRS.Mongo.Models;
using System.Collections.Generic;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperProperties
    {
        PropertyModel AddProperty(string propertyType, string description, string entityId);
        List<PropertyModel> GetProperties(string entityId);
        List<PropertyValueModel> GetPropertyValuesForProperty(string propertyType, string companyId);
        List<PropertyValueModel> GetOnlyActivePropertyValuesForProperty(string propertyType, string entityId);
        PropertyModel GetProperty(string propertyType, string entityId);

        PropertyModel SavePropertyTypeInfoOnlyNoValues(PropertyModel model);
        void RemoveProperty(string propertyType);

        BaseHoursModel GetBaseHours();
        BaseHoursModel SaveBaseHours(BaseHoursModel model);

        TrainingHoursModel GetTrainingHours();
        TrainingHoursModel SaveTrainingHours(TrainingHoursModel model);

        PropertyModel SaveProperty(PropertyModel model);

        TargetPercentagesModel GetTargetPercentages();
        TargetPercentagesModel SaveTargetPercentages(TargetPercentagesModel model);
        List<PropertyModel> GetAllProperties();
    }
}
