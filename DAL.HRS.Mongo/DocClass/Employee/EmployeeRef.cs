using System;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson;
using MongoDB.Driver;
using PayrollRegionRef = DAL.Common.DocClass.PayrollRegionRef;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class EmployeeRef : ReferenceObject<Employee>
    {
        public string PayrollId { get; set; }
        //public string FullName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        public PayrollRegionRef PayrollRegion { get; set; }

        public string Login { get; set; } = string.Empty;
        
        public string FullName
        {
            get { return GetFullName(); }
        }

        public LocationRef Location { get; set;}

        public EmployeeRef()
        {
        }

        public EmployeeRef Refresh()
        {
            var projection = Employee.Helper.ProjectionBuilder.Expression(x =>
            new 
            {
                Id = x.Id.ToString(),
                LastName = x.LastName,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                PreferredName = x.PreferredName,
                Location = x.Location,
                PayrollRegion = x.PayrollRegion,
                PayrollId = x.PayrollId,
                Login = x.Login,
            });

            var id = ObjectId.Parse(Id);
            var newValues = Employee.Helper.Find(x => x.Id == id).Project(projection).FirstOrDefault();

            if (newValues != null)
            {
                LastName = newValues.LastName;
                FirstName = newValues.FirstName;
                MiddleName = newValues.MiddleName;
                PreferredName = newValues.PreferredName;
                Location = newValues.Location;
                PayrollRegion = newValues.PayrollRegion;
                PayrollId = newValues.PayrollId;
                Login = newValues.Login;
            }
            return this;
        }

        public EmployeeRef(Employee emp) : base(emp)
        {
            if (emp == null) return;

            Login = emp.Login;
            PayrollId = emp.PayrollId;
            PayrollRegion = emp.PayrollRegion;
            Location = emp.Location;
            FirstName = emp.FirstName;
            LastName = emp.LastName;
            MiddleName = emp.MiddleName;
            PreferredName = emp.PreferredName;
        }

        public string GetFullName()
        {
            if (!String.IsNullOrWhiteSpace(PreferredName))
            {
                return $"{LastName}, {PreferredName} {MiddleName}".TrimEnd();
            }
            return $"{LastName}, {FirstName} {MiddleName}".TrimEnd();
        }

        public string GetFormalName()
        {
            if (PreferredName != string.Empty)
            {
                return $"{PreferredName} {LastName}".TrimEnd();
            }
            return $"{FirstName} {LastName}".TrimEnd();
        }

        public Employee AsEmployee()
        {
            return ToBaseDocument();
        }

        public override string ToString()
        {
            return $"{PayrollId}:{FullName}";
        }

    }
}