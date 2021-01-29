using System;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Common.Helper
{
    public class LocationChanger<TBaseDocument>
        where TBaseDocument : BaseDocument
    {

        public void ChangeOfficeName(string locationId, string newName)
        {
            
            var helper = new MongoRawQueryHelper<TBaseDocument>();
            var allRows = helper.GetAll();
            foreach (var row in allRows)
            {
                var objectToSupportNameChanges = (row as ISupportLocationNameChanges);
                if (objectToSupportNameChanges == null) throw new Exception($"{row.GetType()} does not support ISupportLocationNameChanges");

                objectToSupportNameChanges.ChangeOfficeName(locationId, newName);
            }
        }

    }
}