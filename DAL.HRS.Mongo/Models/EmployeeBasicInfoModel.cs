using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeBasicInfoModel
    {
        public string Id { get; set; } 
        public string PayrollId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        public string FullName { get; set; } 
        public JobTitleRef JobTitle { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? OriginalHireDate { get; set; }
        public PropertyValueRef Status1Code { get; set; }
        public PropertyValueRef WorkAreaCode { get; set; }
        public LocationRef Location { get; set; }
        public PropertyValueRef CostCenterCode { get; set; }
        public EmployeeRef Manager { get; set; }
        public bool IsActive { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? TerminationDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? LastRehireDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? PriorServiceDate { get; set; }
        public string EmployeeImageFileName { get; set; }

        public EmployeeBasicInfoModel() { }

        public EmployeeBasicInfoModel(Employee emp)
        {
            Id = emp.Id.ToString();
            LastName = emp.LastName;
            FirstName = emp.FirstName;
            MiddleName = emp.MiddleName;
            PreferredName = emp.PreferredName;
            JobTitle = emp.JobTitle;
            OriginalHireDate = emp.OriginalHireDate;
            Status1Code = emp.Status1Code;
            WorkAreaCode = emp.WorkAreaCode;
            CostCenterCode = emp.CostCenterCode;
            Manager = emp.Manager;
            IsActive = (emp.TerminationDate == null || emp.TerminationDate > DateTime.Now);
            TerminationDate = emp.TerminationDate;
            PayrollId = emp.PayrollId;
            Location = emp.Location;
            EmployeeImageFileName = emp.EmployeeImageFileName;
            LastRehireDate = emp.LastRehireDate;
            PriorServiceDate = emp.PriorServiceDate;

            FullName = PreferredName != string.Empty ? 
                $"{LastName}, {PreferredName} {MiddleName}".TrimEnd() : 
                $"{LastName}, {FirstName} {MiddleName}".TrimEnd();

        }


        public static EmployeeBasicInfoModel FindById(string employeeId)
        {
            return EmployeeBasicInfoModel.FindById(ObjectId.Parse(employeeId));
        }

        public static EmployeeBasicInfoModel FindById(ObjectId employeeId)
        {
            var employee = new MongoRawQueryHelper<Employee>().FindById(employeeId);
            if (employee != null)
            {
                return new EmployeeBasicInfoModel(employee);
            }
            return new EmployeeBasicInfoModel();
        }

    }
}
