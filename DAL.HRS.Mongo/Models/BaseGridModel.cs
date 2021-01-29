using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.SqlServer;
using MongoDB.Bson;
using System;
using PayrollRegionRef = DAL.Common.DocClass.PayrollRegionRef;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;

namespace DAL.HRS.Mongo.Models
{
    public struct BaseGridModel //: BaseModel
    {
        public ObjectId Id { get; set; }

        public string EmployeeId => Id.ToString();

        public PropertyValueRef GenderCode { get; set; }
        public byte[] Birthday { get; set; }
        public int Age => GetAge();

        public DateTime? HireDate { get; set; }
        public JobTitleRef JobTitle { get; set; }
        public EmployeeRef Manager { get; set; }

        public string PayrollId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        public PropertyValueRef CostCenterCode { get; set; }
        public string CostCenterDescription => CostCenterCode?.AsPropertyValue()?.Description ?? "Unknown";
        public LocationRef Location { get; set; }
        public PropertyValueRef KronosDepartmentCode { get; set; }
        public bool IsActive { get; set; }
    

        public byte[] GovernmentIdEncrypted { get; set; }

        public string GovernmentId
        {
            get
            {
                var enc = Encryption.NewEncryption;

                return enc.Decrypt<string>(GovernmentIdEncrypted);
            }
        }

        public PropertyValueRef PayGradeType { get; set; }
        public PayrollRegionRef PayrollRegion { get; set; }
        public decimal BaseHours { get; set; }
        public PropertyValueRef BusinessRegionCode { get; set; }
        public PropertyValueRef Status1Code { get; set; }

        public DateTime? TerminationDate { get; set; }

        public int OldHrsId { get; set; }
        public LdapUserRef LdapUser { get; set; }
        public PropertyValueRef CompanyNumber { get; set; }

        public PropertyValueRef BusinessUnit { get; set; }


        public int GetAge()
        {
            try
            {
                var encryption = Encryption.NewEncryption;
                var birthDay = encryption.Decrypt<DateTime>(Birthday);
                DateTime now = DateTime.Today;
                int age = now.Year - birthDay.Year;
                if (birthDay > now.AddYears(-age)) age--;
                return age;

            }
            catch (Exception)
            {
                return 0;
            }
        }

        public BaseGridModel(
            bool isActive,
            byte[] birthday,
            decimal baseHours,
            PropertyValueRef businessRegionCode,
            PropertyValueRef costCenterCode,
            ObjectId id,
            string firstName,
            PropertyValueRef genderCode,
            byte[] governmentIdEncrypted,
            DateTime? hireDate,
            DateTime? terminationDate,
            JobTitleRef jobTitle,
            PropertyValueRef kronosDepartmentCode,
            string lastName,
            LocationRef location,
            EmployeeRef manager,
            string middleName,
            PropertyValueRef payGradeType,
            string payrollId,
            PayrollRegionRef payrollRegion,
            string preferredName,
            PropertyValueRef status1Code,
            int oldHrsId,
            LdapUserRef ldapUser,
            PropertyValueRef businessUnit,
            PropertyValueRef companyNumber
            )
        {
            IsActive = isActive;
            Birthday = birthday;
            BaseHours = baseHours;
            BusinessRegionCode = businessRegionCode;
            CostCenterCode = costCenterCode;
            Id = id;
            FirstName = firstName;
            GenderCode = genderCode;
            GovernmentIdEncrypted = governmentIdEncrypted;
            HireDate = hireDate;
            TerminationDate = terminationDate;
            JobTitle = jobTitle;
            KronosDepartmentCode = kronosDepartmentCode;
            LastName = lastName;
            Location = location;
            Manager = manager;
            MiddleName = middleName;
            PayGradeType = payGradeType;
            PayrollId = payrollId;
            PayrollRegion = payrollRegion;
            PreferredName = preferredName;
            Status1Code = status1Code;
            OldHrsId = oldHrsId;
            LdapUser = ldapUser;
            BusinessUnit = businessUnit;
            CompanyNumber = companyNumber;

        }

    }
}