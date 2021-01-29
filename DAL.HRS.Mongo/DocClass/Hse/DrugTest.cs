using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class DrugTest : BaseDocument, ISupportLocationNameChanges
    {
        public PropertyValueRef DrugTestType { get; set; }
        public DrugTestMedicalInfo MedicalInfo { get; set; }
        public PropertyValueRef DrugTestResult { get; set; }
        public DateTime? TestDate { get; set; }
        public DateTime? ResultDate { get; set; }
        public string Comments { get; set; }

        public static MongoRawQueryHelper<DrugTest> Helper = new MongoRawQueryHelper<DrugTest>();


        public DrugTestRef AsDrugTestRef()
        {
            return new DrugTestRef(this);
        }

        public void ChangeOfficeName(string locationId, string newName)
        {
            var modified = false;
            modified = DrugTestType.ChangeOfficeName(locationId, newName, modified);
            modified = DrugTestType.ChangeOfficeName(locationId, newName, modified);
            modified = DrugTestType.ChangeOfficeName(locationId, newName, modified);

            if (modified) Helper.Upsert(this);
        }
    }
}