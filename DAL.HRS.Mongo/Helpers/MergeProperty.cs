using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.Helpers
{
    public class MergeProperty
    {
        public void Execute(string sourceType, string destinationType, string entityName, bool removeSource = false)
        {
            if (!VerifyType(sourceType)) throw new Exception("Source not found");
            if (!VerifyType(destinationType)) throw new Exception("Destination not found");

            var sourceFilter = PropertyValue.Helper.FilterBuilder.Where(x => x.Type == sourceType && x.Entity.Name == entityName);
            var sourceValues = PropertyValue.Helper.Find(sourceFilter).ToList();

            var destinationFilter = PropertyValue.Helper.FilterBuilder.Where(x => x.Type == destinationType && x.Entity.Name == entityName);
            var destinationValues = PropertyValue.Helper.Find(destinationFilter).ToList();

            foreach (var sourceValue in sourceValues)
            {
                if (destinationValues.All(x => x.Code != sourceValue.Code))
                {
                    var newPropertyValue = new PropertyValue()
                    {
                        Active = sourceValue.Active,
                        Code = sourceValue.Code,
                        Type = destinationType,
                        Description = sourceValue.Description,
                        Entity = sourceValue.Entity
                    };
                    PropertyValue.Helper.Upsert(newPropertyValue);
                }
            }

            RemoveSource(sourceType, removeSource, sourceValues);


        }

        private static void RemoveSource(string sourceType, bool removeSource, List<PropertyValue> sourceValues)
        {
            if (removeSource)
            {
                foreach (var sourceValue in sourceValues)
                {
                    PropertyValue.Helper.DeleteOne(sourceValue.Id);
                }

                var propertyValuesRemainingFilter = PropertyValue.Helper.FilterBuilder.Where(x => x.Type == sourceType);
                var remaining = PropertyValue.Helper.Find(propertyValuesRemainingFilter).ToList();

                if (remaining.Any()) return;

                var sourceTypeFilter = PropertyType.Helper.FilterBuilder.Where(x => x.Type == sourceType);
                var sourceTypeDoc = PropertyType.Helper.Find(sourceTypeFilter).Single();
                PropertyType.Helper.DeleteOne(sourceTypeDoc.Id);
            }
        }

        private bool VerifyType(string propertyType)
        {
            var filter = PropertyType.Helper.FilterBuilder.Where(x => x.Type == propertyType);
            var type = PropertyType.Helper.Find(filter).SingleOrDefault();
            return (type != null);
        }
    }
}
