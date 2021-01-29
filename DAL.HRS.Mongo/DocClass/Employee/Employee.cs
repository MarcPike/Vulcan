using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCore.Identity.MongoDB.Validators;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Encryption;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    [BsonIgnoreExtraElements]
    public class Employee : BaseDocument
    {
        private static readonly Encryption _encryption = Encryption.NewEncryption;

        public static MongoRawQueryHelper<Employee> Helper = new MongoRawQueryHelper<Employee>();


        //public List<MedicalExamRef> MedicalExams { get; set; } = new List<MedicalExamRef>();

        public int OldHrsId { get; set; }
        public string PayrollId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public byte[] GovernmentId { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? OriginalHireDate { get; set; }

        public byte[] Birthday { get; set; } = _encryption.Encrypt(DateTime.Now.Date);

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public PropertyValueRef Country { get; set; }
        public string ContactCountry { get; set; } = string.Empty;
        public string PostalCode { get; set; }

        public List<EmployeePhoneNumber> PhoneNumbers { get; set; } = new List<EmployeePhoneNumber>();
        public List<EmployeeEmailAddress> EmailAddresses { get; set; } = new List<EmployeeEmailAddress>();

        public string EmployeeImageFileName { get; set; } = string.Empty;

        //public string Mobile { get; set; }
        //public string Home { get; set; }
        public string OriginalManagerPayrollId { get; set; } = string.Empty;
        public string OriginalManagerName { get; set; }
        public string Login { get; set; } = string.Empty;

        public string ExternalLoginId { get; set; } = string.Empty;

        //public bool IsActive { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? LastRehireDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? PriorServiceDate { get; set; }


        public TimeSpan Seniority
        {
            get
            {
                var startDate = LastRehireDate ?? PriorServiceDate ?? OriginalHireDate ?? DateTime.Now;
                var endDate = TerminationDate ?? DateTime.Now;

                var result = endDate - startDate;

                return result;
            }
        }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? ConfirmationDate { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? TerminationDate { get; set; }

        public PropertyValueRef TerminationCode { get; set; }
        public string TerminationExplanation { get; set; } = string.Empty;
        public List<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();

        public List<EmployeeVerification> EmployeeVerifications { get; set; } = new List<EmployeeVerification>();
        public string TimeTrackingAccrualProfile { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Unspecified)]
        public DateTime? TimeTrackingAccrualProfileEffectiveDate { get; set; }

        public List<EmployeeRef> DirectReports { get; set; } = new List<EmployeeRef>();

        public EmployeeRef Manager { get; set; }
        public LocationRef Location { get; set; }
        public string ExternalLocationText { get; set; } = string.Empty;

        public PayrollRegionRef PayrollRegion { get; set; }

        public List<EducationCertification> EducationCertifications { get; set; } = new List<EducationCertification>();
        //public List<SupportingDocs> SupportingDocs { get; set; } = new List<SupportingDocs>();
        //public List<ReqActivities> ReqActivities { get; set; } = new List<ReqActivities>();

        // Property Values
        public PropertyValueRef EEOCode { get; set; } // JobClassification
        public PropertyValueRef RehireStatusCode { get; set; }
        public PropertyValueRef BusinessRegionCode { get; set; }

        public PropertyValueRef MaritalStatusCode { get; set; }

        public PropertyValueRef GenderCode { get; set; }
        public PropertyValueRef CostCenterCode { get; set; }
        public PropertyValueRef ShiftCode { get; set; }
        public PropertyValueRef KronosLaborLevelCode { get; set; }
        public EmployeeRef KronosManager { get; set; }
        public PropertyValueRef KronosDepartmentCode { get; set; }
        public PropertyValueRef EthnicityCode { get; set; }

        public PropertyValueRef NationalityCode { get; set; }

        //public PropertyValueRef JobTitleCode { get; set; }
        public JobTitleRef JobTitle { get; set; }

        //public List<PerformanceEvaluation> PerformanceEvaluations { get; set; } = new List<PerformanceEvaluation>();
        public PropertyValueRef CountryCode { get; set; }
        public PropertyValueRef CountryOfOriginCode { get; set; }
        public PropertyValueRef Status1Code { get; set; }
        public PropertyValueRef Status2Code { get; set; }
        public PropertyValueRef WorkAreaCode { get; set; }
        public List<Termination> Terminations { get; set; } = new List<Termination>();
        public List<IssuedEquipment> IssuedEquipment { get; set; } = new List<IssuedEquipment>();


        // PIKE CHANGES 8-17-2020
        public PropertyValueRef PerformanceEvaluationType { get; set; }
        public PropertyValueRef Lms { get; set; }

        public EmployeeRef HrRepresentative { get; set; }

        public List<Performance.Performance> Performance { get; set; } = new List<Performance.Performance>();

        public Benefits.Benefits Benefits { get; set; } = new Benefits.Benefits();

        public PropertyValueRef CompanyNumberCode { get; set; }
        public PropertyValueRef BusinessUnitCode { get; set; }

        public EntityRef Entity { get; set; }

        public LdapUserRef LdapUser { get; set; }

        public EmployeeMedicalInfo MedicalInfo { get; set; } = new EmployeeMedicalInfo();

        public PropertyValueRef DeviceGroup { get; set; }


        // Performance Grid Fields
        public DateTime? DateOfNextReview
        {
            get { return Performance?.FirstOrDefault(x => x.Locked == false)?.DateOfNextReview; }
        }

        public PropertyValueRef PerformanceReviewType
        {
            get { return Performance?.FirstOrDefault(x => x.Locked == false)?.PerformanceReviewType; }
        }


        public List<WorkStatusHistory> WorkStatusHistory { get; set; } = new List<WorkStatusHistory>();

        public List<Discipline.Discipline> Discipline { get; set; } = new List<Discipline.Discipline>();
        public Compensation.Compensation Compensation { get; set; } = new Compensation.Compensation();

        public List<TrainingEventRef> TrainingEvents { get; set; } = new List<TrainingEventRef>();

        public EmployeeRef AsEmployeeRef()
        {
            return new EmployeeRef(this);
        }

        public List<LdapUser> GetLdapUserData()
        {
            var result = new List<LdapUser>();
            var isActive = TerminationDate == null || TerminationDate > DateTime.Now;
            if (!isActive) return result;
            var queryHelper = new MongoRawQueryHelper<LdapUser>();
            var locationId = Location.AsLocation().Id.ToString();
            var filter = queryHelper.FilterBuilder.Where(x =>
                x.Location.Id == locationId && x.LastName == LastName && x.FirstName == FirstName);

            result.AddRange(queryHelper.Find(filter));

            var filterPrefer = queryHelper.FilterBuilder.Where(x =>
                x.Location.Id == locationId && x.LastName == LastName && x.FirstName == PreferredName);
            result.AddRange(queryHelper.Find(filterPrefer));
            return result;
        }

        public List<EmployeeRef> GetAllDirectReports()
        {
            var result = new List<EmployeeRef>();
            result.AddRange(DirectReports);
            foreach (var employeeRef in DirectReports)
            {
                var employee = Helper.FindById(employeeRef.Id);
                if (employee == null)
                {
                    var payrollIdFilter = Helper.FilterBuilder.Where(x => x.PayrollId == employeeRef.PayrollId);
                    employee = Helper.Find(payrollIdFilter).FirstOrDefault();
                }

                if (employee != null) result.AddRange(employee.GetAllDirectReports());
            }

            return result;
        }

        public int GetAge()
        {
            try
            {
                var encryption = Encryption.NewEncryption;
                var birthDay = encryption.Decrypt<DateTime>(Birthday);
                var now = DateTime.Today;
                var age = now.Year - birthDay.Year;
                if (birthDay > now.AddYears(-age)) age--;
                return age;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void UpdateRequiredActivityHasSupportingDocs()
        {
            var repRequiredActivity = new RepositoryBase<RequiredActivity>();
            var repSupportingDocs = new RepositoryBase<SupportingDocument>();
            var id = Id.ToString();
            foreach (var requiredActivity in repRequiredActivity.AsQueryable().Where(x => x.Employee.Id == id))
                requiredActivity.UpdateHasSupportingDocs(repRequiredActivity, repSupportingDocs);
        }

        public override List<ValidationError> Validate()
        {
            var result = new List<ValidationError>();
            if (Entity == null)
                result.Add(new ValidationError
                {
                    ErrorMessage = "Missing Entity value"
                });

            return result;
        }


        //public void GetDefaultContactCompany()
        //{

        //    if (ContactCountry == string.Empty)
        //    {
        //        string countryValue;
        //        if ((Country != null) && (Country.Code != string.Empty))
        //        {
        //            countryValue = Country.Code;
        //        } else if ((CountryCode != null) && (CountryCode.Code != string.Empty))
        //        {
        //            countryValue = CountryCode.Code;
        //        }
        //        else
        //        {
        //            return;
        //        }


        //        if (countryValue == "Western Australia") countryValue = "Australia";

        //        if (countryValue == "UAE") countryValue = "United Arab Emirates";

        //        if ((countryValue == "England") || (countryValue == "Scotland"))
        //        {
        //            countryValue = "United Kingdom";
        //        }

        //        var countryFound = CountryValue.Helper.Find(x => x.CountryName == countryValue).FirstOrDefault();
        //        if (countryFound != null)
        //        {
        //            ContactCountry = countryFound.CountryName;
        //            Employee.Helper.Upsert(this);
        //        }
        //        else
        //        {
        //            Console.WriteLine($"New country ignored: {Country.Code}");
        //        }
        //    }

        //}
    }
}