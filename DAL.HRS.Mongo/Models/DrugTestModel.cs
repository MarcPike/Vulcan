using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.Models
{
    public class DrugTestModel
    {
        public string Id { get; set; }
        public PropertyValueRef DrugTestType { get; set; }
        public DrugTestMedicalInfo MedicalInfo { get; set; }
        public PropertyValueRef DrugTestResult { get; set; }
        public DateTime? TestDate { get; set; }
        public DateTime? ResultDate { get; set; }
        public string Comments { get; set; }

        public DrugTestModel() { }

        public DrugTestModel(DrugTest d)
        {
            Id = d.Id.ToString();
            DrugTestType = d.DrugTestType;
            MedicalInfo = d.MedicalInfo;
            DrugTestResult = d.DrugTestResult;
            TestDate = d.TestDate;
            ResultDate = d.ResultDate;
            Comments = d.Comments;
        }

    }
}
