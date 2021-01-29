using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.SqlServer.Model;
using DocumentFormat.OpenXml.ExtendedProperties;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class MedicalInfoGridModel
    {
        public string EmployeeId { get; set; }
        public string PayrollId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }

        public PropertyValueRef CostCenterCode { get; set; }
        public LocationRef Location { get; set; }
        public bool DrugTests { get; set; }
        public bool MedicalLeaves { get; set; }
        public bool MedicalExams { get; set; }
        public bool OtherMedicalInfo { get; set; }


        public MedicalInfoGridModel(
            ObjectId employeeId,
            string payrollId,
            string firstName,
            string lastName,
            PropertyValueRef costCenterCode,
            LocationRef location)
            //EmployeeMedicalInfo medicalInfo)


        {
            EmployeeId = employeeId.ToString();
            PayrollId = payrollId;
            FirstName = firstName;
            LastName = lastName;
            CostCenterCode = costCenterCode;
            Location = location;
            //if (medicalInfo == null)
            //{
            //    medicalInfo = new EmployeeMedicalInfo();
            //}

        }

        public MedicalInfoGridModel()
        {

        }

        public MedicalInfoGridModel(
            ObjectId employeeId,
            string payrollId,
            string firstName,
            string lastName,
            PropertyValueRef costCenterCode,
            LocationRef location,
            bool drugTest,
            bool medicalExams,
            bool otherMedical,
            bool leaveHistory)


        {
            EmployeeId = employeeId.ToString();
            PayrollId = payrollId;
            FirstName = firstName;
            LastName = lastName;
            CostCenterCode = costCenterCode;
            Location = location;

            DrugTests = drugTest;
            MedicalExams = medicalExams;
            OtherMedicalInfo = otherMedical;
            MedicalLeaves = leaveHistory;
        }

    }
}
