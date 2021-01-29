using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class DrugTestRef : ReferenceObject<DrugTest>, ISupportLocationNameChangesNested
    {
        public DrugTestRef() { }

        public DrugTestRef(DrugTest d) : base(d)
        {
            TestDate = d.TestDate;
            ResultDate = d.ResultDate;
            DrugTestType = d.DrugTestType;
            DrugTestResult = d.DrugTestResult;
        }

        public DateTime? TestDate { get; set; }
        public DateTime? ResultDate { get; set; }
        public PropertyValueRef DrugTestType { get; set; }

        public PropertyValueRef DrugTestResult;

        public override string ToString()
        {
            return
                $"TestDate: {TestDate} Result Date: {ResultDate} DrugTestType: {DrugTestType?.ToString() ?? ""} DrugTestResult: {DrugTestResult?.ToString() ?? ""}";
        }

        public bool ChangeOfficeName(string locationId, string newName, bool modified)
        {
            modified = DrugTestType.ChangeOfficeName(locationId, newName, modified);
            modified = DrugTestResult.ChangeOfficeName(locationId, newName, modified);

            return modified;
        }

        public DrugTest AsDrugTest()
        {
            return ToBaseDocument();
        }
    }
}