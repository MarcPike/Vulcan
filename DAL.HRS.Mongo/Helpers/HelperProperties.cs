using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperProperties : IHelperProperties {
        public PropertyModel AddProperty(string propertyType, string description, string entityId)
        {
            var prop = GetProp(propertyType);
            if (prop != null) throw new Exception("Property does not exist");

            prop = new PropertyType()
            {
                Type = propertyType,
                Description = description,
            };

            prop = new RepositoryBase<PropertyType>().Upsert(prop);
            return new PropertyModel(prop, entityId);
        }

        public List<PropertyModel> GetProperties(string entityId)
        {
            return new RepositoryBase<PropertyType>().AsQueryable().ToList()
                .Select(x => new PropertyModel(x, entityId)).OrderBy(x=>x.Type).ToList();
        }

        public List<PropertyModel> GetAllProperties()
        {
            var entityIdHowco = Entity.GetRefByName("Howco");
            var entityIdEdgen = Entity.GetRefByName("Edgen Murray");

            var result = new List<PropertyModel>();
            foreach (var propertyType in PropertyType.Helper.GetAll())
            {
                var propValues = PropertyValue.Helper.Find(x => x.Type == propertyType.Type).ToList();

                var prop = new PropertyModel()
                {
                    Id = propertyType.Id.ToString(),
                    Type = propertyType.Type,
                    Description = propertyType.Description,
                    IsDirty = false,
                    Entity = entityIdHowco,
                };

                prop.PropertyValues.AddRange(propValues.Select(x=> new PropertyValueModel(x)).Where(x=>x.Active).OrderBy(x=>x.Code).ToList());
                prop.PropertyValues.AddRange(propValues.Select(x => new PropertyValueModel(x)).Where(x => x.Active == false).OrderBy(x => x.Code).ToList());

                //foreach (var propertyValue in propValues)
                //{

                //    prop.PropertyValues.Add(new PropertyValueModel(propertyValue));
                //}
                
                result.Add(prop);
            }

            return result.OrderBy(x => x.Type).ToList();
        }


        public PropertyModel GetProperty(string propertyType, string entityId)
        {
            var prop = GetProp(propertyType);
            ThrowExceptionIfNull(prop);
            return new PropertyModel(prop, entityId);
        }

        private static PropertyType GetProp(string propertyType)
        {
            var prop = new RepositoryBase<PropertyType>().AsQueryable().FirstOrDefault(x=>x.Type == propertyType);
            return prop;
        }

        public PropertyModel SaveProperty(PropertyModel model)
        {

            var property = PropertyType.Helper.FindById(model.Id) ?? new PropertyType()
            {
                Id = ObjectId.Parse(model.Id)
            };

            property.Description = model.Description;
            foreach (var propertyValue in model.PropertyValues)
            {
                // Add / Update existing
                var filter = PropertyValue.Helper.FilterBuilder.Where(x => x.Id == ObjectId.Parse(propertyValue.Id));

                var value = PropertyValue.Helper.Find(filter).FirstOrDefault() ?? 
                    new PropertyValue()
                    {
                        Id = ObjectId.Parse(propertyValue.Id)
                    };
                value.Code = propertyValue.Code;
                value.Entity = model.Entity;
                value.Description = propertyValue.Description;
                value.Locations = propertyValue.Locations;
                value.Type = propertyValue.Type;
                value.Active = propertyValue.Active;
                PropertyValue.Helper.Upsert(value);

                CreateCopyForOtherEntity(value);

            }

            var allValuesFilter = PropertyValue.Helper.FilterBuilder.Where(x => x.Type == model.Type && x.Entity.Id == model.Entity.Id);
            var allValues = PropertyValue.Helper.Find(allValuesFilter).ToList();
            // Remove any no longer in model
            foreach (var propertyValue in allValues)
            {
                if (model.PropertyValues.All(y => ObjectId.Parse(y.Id) != propertyValue.Id))
                {
                    RemoveCopyForOtherEntity(propertyValue.Type, propertyValue.Code, propertyValue.Entity);
                    PropertyValue.Helper.DeleteOne(propertyValue.Id);
                }
            }

            property = PropertyType.Helper.FindById(model.Id);
            return new PropertyModel(property, model.Entity.Id);
        }

        private void RemoveCopyForOtherEntity(string type, string code, EntityRef entity)
        {
            EntityRef otherEntity;
            if (entity.Name == "Howco")
            {
                otherEntity = Entity.GetRefByName("Edgen Murray");
            }
            else
            {
                otherEntity = Entity.GetRefByName("Howco");
            }

            var removeProp = PropertyValue.Helper
                .Find(x => x.Type == type && x.Code == code && x.Entity.Id == otherEntity.Id).FirstOrDefault();
            if (removeProp != null)
            {
                PropertyValue.Helper.DeleteOne(removeProp.Id);
            }
        }

        private void CreateCopyForOtherEntity(PropertyValue value)
        {
            EntityRef otherEntity;
            if (value.Entity.Name == "Howco")
            {
                otherEntity = Entity.GetRefByName("Edgen Murray");
            }
            else
            {
                otherEntity = Entity.GetRefByName("Howco");
            }

            var otherProp = PropertyValue.Helper
                .Find(x => x.Type == value.Type && x.Code == value.Code && x.Entity.Id == otherEntity.Id)
                .FirstOrDefault();
            if (otherProp == null)
            {
                otherProp = new PropertyValue()
                {
                    Type = value.Type,
                    Code = value.Code,
                };
            }

            otherProp.Description = value.Description;
            otherProp.Entity = otherEntity;
            PropertyValue.Helper.Upsert(otherProp);

        }


        private void ThrowExceptionIfNull(PropertyType type)
        {
            if (type == null) throw new Exception("Property Type does not exist");
        }

        public List<PropertyValueModel> GetPropertyValuesForProperty(string propertyType, string entityId)
        {
            var prop = GetProp(propertyType);
            ThrowExceptionIfNull(prop);

            var result = prop.PropertyValues.Where(x => x.Entity.Id == entityId && x.Active == true).Select(x => new PropertyValueModel(x)).OrderBy(x => x.Code).ToList();
            result.AddRange(prop.PropertyValues.Where(x => x.Entity.Id == entityId && x.Active == false).Select(x => new PropertyValueModel(x)).OrderBy(x => x.Code).ToList());

            return result;
        }

        public List<PropertyValueModel> GetOnlyActivePropertyValuesForProperty(string propertyType, string entityId)
        {
            var prop = GetProp(propertyType);
            ThrowExceptionIfNull(prop);

            return prop.PropertyValues.Where(x => x.Entity.Id == entityId && x.Active).Select(x => new PropertyValueModel(x)).OrderBy(x => x.Code).ToList();
        }


        public PropertyModel SavePropertyTypeInfoOnlyNoValues(PropertyModel model)
        {
            var prop = GetProp(model.Type);

            if (prop == null)
            {
                prop = new PropertyType()
                {
                    Type = model.Type,
                };
            }

            prop.Description = model.Description;
            prop = new RepositoryBase<PropertyType>().Upsert(prop);

            return new PropertyModel(prop, model.Entity.Id);
        }

        public void RemoveProperty(string propertyType)
        {
            var prop = GetProp(propertyType);
            ThrowExceptionIfNull(prop);
            var repPropertyValue = new RepositoryBase<PropertyValue>();
            foreach (var propPropertyValue in prop.PropertyValues)
            {
                var propertyValue = repPropertyValue.Find(propPropertyValue.Id);
                repPropertyValue.RemoveOne(propertyValue);
            }

            new RepositoryBase<PropertyType>().RemoveOne(prop);
        }

        public BaseHoursModel GetBaseHours()
        {
            var baseHours = new RepositoryBase<BaseHours>().AsQueryable().FirstOrDefault();
            if (baseHours != null) return new BaseHoursModel(baseHours);
            baseHours = new BaseHours();
            baseHours.SaveToDatabase();

            return new BaseHoursModel(baseHours);
        }


        public BaseHoursModel SaveBaseHours(BaseHoursModel model)
        {
            var baseHours = new RepositoryBase<BaseHours>().AsQueryable().FirstOrDefault() ?? new BaseHours();

            baseHours.Values = model.Values.OrderBy(x => x).ToList();
            baseHours.SaveToDatabase();
            return new BaseHoursModel(baseHours);
        }

        public TargetPercentagesModel GetTargetPercentages()
        {
            var targetPercentage = new RepositoryBase<TargetPercentage>().AsQueryable().FirstOrDefault();
            if (targetPercentage != null) return new TargetPercentagesModel(targetPercentage);
            targetPercentage = new TargetPercentage();
            targetPercentage.SaveToDatabase();

            return new TargetPercentagesModel(targetPercentage);
        }


        public TargetPercentagesModel SaveTargetPercentages(TargetPercentagesModel model)
        {
            var targetPercentage = new RepositoryBase<TargetPercentage>().AsQueryable().FirstOrDefault() ?? new TargetPercentage();

            targetPercentage.Values = model.Values.OrderBy(x => x).ToList();
            targetPercentage.SaveToDatabase();
            return new TargetPercentagesModel(targetPercentage);
        }


        public TrainingHoursModel GetTrainingHours()
        {
            var trainingHours = new RepositoryBase<TrainingHours>().AsQueryable().FirstOrDefault();
            if (trainingHours != null) return new TrainingHoursModel(trainingHours);
            trainingHours = new TrainingHours();
            trainingHours.SaveToDatabase();

            return new TrainingHoursModel(trainingHours);
        }


        public TrainingHoursModel SaveTrainingHours(TrainingHoursModel model)
        {
            var trainingHours = new RepositoryBase<TrainingHours>().AsQueryable().FirstOrDefault() ?? new TrainingHours();

            trainingHours.Values = model.Values.OrderBy(x => x).ToList();
            trainingHours.SaveToDatabase();
            return new TrainingHoursModel(trainingHours);
        }

    }
}