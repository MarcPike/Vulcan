using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass;
using DAL.HRS.Mongo.DocClass.MedicalExams;
using MongoDB.Driver;
using EducationCertification = DAL.HRS.Mongo.DocClass.Employee.EducationCertification;
using EmergencyContact = DAL.HRS.Mongo.DocClass.Employee.EmergencyContact;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;
using PropertyValueRef = DAL.HRS.Mongo.DocClass.Properties.PropertyValueRef;
using WorkStatusHistory = DAL.HRS.Mongo.DocClass.Employee.WorkStatusHistory;
using HrRepresentative = DAL.HRS.Mongo.DocClass.Employee.HrRepresentative;


namespace DAL.HRS.Mongo.Models
{
    public class EmployeeModel : BaseModel, IHavePropertyValues
    {
        public string EmployeeId { get; set; }
        public int OldHrsId { get; set; }
        public string PayrollId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PreferredName { get; set; }
        public string GovernmentId { get; set; }
        public string AdditionalGovernmentId { get; set; }

        
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? OriginalHireDate { get; set; }

        ////[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Birthday { get; set; }


        public string BirthdayAsString
        {
            get
            {
                return Birthday.ToString("yyyy-MM-dd") + "T00:00:00";
            }
        }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        //public PropertyValueRef Country { get; set; }

        public string ContactCountry { get; set; }
        public string PostalCode { get; set; }

        public List<EmployeePhoneNumberModel> PhoneNumbers { get; set; } = new List<EmployeePhoneNumberModel>();
        private List<EmployeeEmailAddressModel> EmailAddresses { get; set; } = new List<EmployeeEmailAddressModel>();

        public string Login { get; set; }
        public string ExternalLoginId { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]

        public DateTime? LastRehireDate { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]

        public DateTime? PriorServiceDate { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? ConfirmationDate { get; set; }

        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? TerminationDate { get; set; }

        public TimeSpan Seniority { get; set; }

        public PropertyValueRef TerminationCode { get; set; }
        public string TerminationExplanation { get; set; } = string.Empty;
        public List<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();

        public List<EmployeeVerificationModel> EmployeeVerifications { get; set; } = new List<EmployeeVerificationModel>();
        public string TimeTrackingAccrualProfile { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? TimeTrackingAccrualProfileEffectiveDate { get; set; }
        public List<EmployeeRef> DirectReports { get; set; } = new List<EmployeeRef>();

        public EmployeeRef Manager { get; set; }
        public LocationRef Location { get; set; }

        public string ExternalLocationText { get; set; }


        public List<EducationCertification> EducationCertifications { get; set; } = new List<EducationCertification>();

        // Property Values

        public PropertyValueRef EEOCode { get; set; }
        public PropertyValueRef RehireStatusCode { get; set; }
        public PropertyValueRef BusinessRegionCode { get; set; }

        public PayrollRegionRef PayrollRegion { get; set; }
        public PropertyValueRef MaritalStatusCode { get; set; }

        public PropertyValueRef GenderCode { get; set; }
        public PropertyValueRef CostCenterCode { get; set; }

        public CompensationModel Compensation { get; set; }

        public EntityRef Entity { get; set; }
        public string CostCenterDescription
        {
            get
            {
                if (CostCenterCode == null) return string.Empty;
                try
                {
                    return CostCenterCode.AsPropertyValue()?.Description ?? String.Empty;
                }
                catch (Exception)
                {
                    return "Unknown CostCenterCode";
                }

            }
        }
        public PropertyValueRef ShiftCode { get; set; }
        public PropertyValueRef KronosLaborLevelCode { get; set; }
        //public PropertyValueRef JobClassificationCode { get; set; }
        public PropertyValueRef KronosDepartmentCode { get; set; }

        public EmployeeRef KronosManager { get; set; }
        public PropertyValueRef EthnicityCode { get; set; }
        public PropertyValueRef NationalityCode { get; set; }
        public JobTitleRef JobTitle { get; set; }
        //public List<PerformanceEvaluation> PerformanceEvaluations { get; set; } = new List<PerformanceEvaluation>();
        // public PropertyValueRef CountryCode { get; set; }
        public PropertyValueRef CountryOfOriginCode { get; set; }
        public PropertyValueRef Status1Code { get; set; }
        public PropertyValueRef Status2Code { get; set; }
        public PropertyValueRef WorkAreaCode { get; set; }

        public PropertyValueRef CompanyNumberCode { get; set; }
        public PropertyValueRef BusinessUnitCode { get; set; }
        public LdapUserRef LdapUser { get; set; }

        public PropertyValueRef DeviceGroup { get; set; }



        //public List<MedicalExamRef> MedicalExams { get; set; } = new List<MedicalExamRef>();

        public string WorkEmailAddress { get; set; } 
        public string PersonalEmailAddress { get; set; }
        public string EmployeeImageFileName { get; set; }

        //private string CurrentWorkEmailAddress
        //{
        //    get { return EmailAddresses.FirstOrDefault(x => x.EmailType.Code == "Work")?.EmailAddress; }
        //}

        //private string CurrentPersonalEmailAddress
        //{
        //    get { return EmailAddresses.FirstOrDefault(x => x.EmailType.Code == "Personal")?.EmailAddress; }
        //}

        public int Age => GetAge();

        public int GetAge()
        {
            try
            {
                var birthDay = Birthday;
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

        public List<WorkStatusHistory> WorkStatusHistory { get; set; }

        public HrsUserRef ModifiedByUser { get; set; }
        public PropertyValueRef Country { get; set; }

        // PIKE CHANGES 8-17-2020
        public PropertyValueRef Lms { get; set; }
        public PropertyValueRef PerformanceEvaluationType { get; set; }
        
        // END OF CHANGES


        public EmployeeModel()
        {
        }

        public EmployeeModel(Employee employee)
        {
            //UpdatePropertyReferences.Execute(employee);
            
            EmployeeId = employee.Id.ToString();
            OldHrsId = employee.OldHrsId;
            PayrollId = employee.PayrollId;
            FirstName = employee.FirstName;
            MiddleName = employee.MiddleName;
            LastName = employee.LastName;
            Birthday = _encryption.Decrypt<DateTime>(employee.Birthday);
            //if (Birthday != null)
            //{
            //    Birthday = Birthday.ToUniversalTime().Date;
            //}

            Address1 = employee.Address1;
            Address2 = employee.Address2;
            Address3 = employee.Address3;
            City = employee.City;
            //Country = employee.Country;
            ConfirmationDate = employee.ConfirmationDate?.Date;
            CostCenterCode = employee.CostCenterCode;
            JobTitle = employee.JobTitle;
            WorkAreaCode = employee.WorkAreaCode;
            IsActive = (employee.TerminationDate == null || employee.TerminationDate > DateTime.Now);
            CountryOfOriginCode = employee.CountryOfOriginCode;
            EmergencyContacts = employee.EmergencyContacts;
            EmployeeVerifications = EmployeeVerificationModel.ConvertBaseListToModelList(employee.EmployeeVerifications);
            EthnicityCode = employee.EthnicityCode;
            GenderCode = employee.GenderCode;
            GovernmentId = _encryption.Decrypt<string>(employee.GovernmentId);
            LdapUser = employee.LdapUser;
            //WorkEmailAddress = CurrentWorkEmailAddress;
            //PersonalEmailAddress = CurrentPersonalEmailAddress;
            ExternalLocationText = employee.ExternalLocationText;
            DeviceGroup = employee.DeviceGroup;
            KronosManager = employee.KronosManager?.Refresh();
            KronosLaborLevelCode = employee.KronosLaborLevelCode;

            ContactCountry = employee.ContactCountry;


            //if (employee.Location != null)
            //{
            //    var location = employee.Location.AsLocation();
            //    if (employee.Location.Id != location.Id.ToString())
            //    {
            //        employee.Location = location.AsLocationRef();
            //        employee.SaveToDatabase();
            //    }
            //}
            Location = employee.Location;

            // PIKE CHANGES 8-17-2020
            Lms = employee.Lms;
            PerformanceEvaluationType = employee.PerformanceEvaluationType;
            //HrRepresentative = DocClass.Employee.HrRepresentative.GetRepresentativeForLocation(Location);
            // END OF CHANGES

            LastRehireDate = employee.LastRehireDate?.Date;
            Login = employee.Login;
            ExternalLoginId = (employee.ExternalLoginId == String.Empty) ? employee.Login : employee.ExternalLoginId;
            //OriginalManagerPayrollId = employee.OriginalManagerPayrollId;
            //OriginalManagerName = employee.OriginalManagerName;
            Manager = employee.Manager?.Refresh();
            MaritalStatusCode = employee.MaritalStatusCode;
            NationalityCode = employee.NationalityCode;
            OriginalHireDate = employee.OriginalHireDate?.Date;

            PriorServiceDate = employee.PriorServiceDate?.Date;
            LastRehireDate = employee.LastRehireDate?.Date;

            Seniority = employee.Seniority;

            EmailAddresses = EmployeeEmailAddressModel.ConvertListForEmployee(employee);
            PhoneNumbers = EmployeePhoneNumberModel.ConvertListForEmployee(employee);

            WorkEmailAddress = EmailAddresses.SingleOrDefault(x => x.EmailType.Code == "Work")?.EmailAddress;
            PersonalEmailAddress = EmailAddresses.SingleOrDefault(x => x.EmailType.Code == "Personal")?.EmailAddress;

            PostalCode = employee.PostalCode;
            PreferredName = employee.PreferredName;
            State = employee.State;
            Status1Code = employee.Status1Code;
            Status2Code = employee.Status2Code;
            TerminationCode = employee.TerminationCode;
            TerminationDate = employee.TerminationDate?.Date;
            TerminationExplanation = employee.TerminationExplanation;
            TimeTrackingAccrualProfile = employee.TimeTrackingAccrualProfile;
            TimeTrackingAccrualProfileEffectiveDate = employee.TimeTrackingAccrualProfileEffectiveDate;
            EEOCode = employee.EEOCode;
            RehireStatusCode = employee.RehireStatusCode;
            KronosDepartmentCode = employee.KronosDepartmentCode?.Refresh();

            PayrollRegion = employee.PayrollRegion;
            BusinessRegionCode = employee.BusinessRegionCode;
            DirectReports = employee.DirectReports;
            foreach (var directReport in DirectReports)
            {
                directReport?.Refresh();
            }
            if (employee.WorkStatusHistory != null)
            {
                WorkStatusHistory = employee.WorkStatusHistory.OrderByDescending(x => x.EffectiveDate).ToList();
            }
            else
            {
                WorkStatusHistory = new List<WorkStatusHistory>();
            }
            EducationCertifications = employee.EducationCertifications;

            CompanyNumberCode = employee.CompanyNumberCode;
            BusinessUnitCode = employee.BusinessUnitCode;

            Compensation = new CompensationModel(employee);
            Entity = employee.Entity;
            EmployeeImageFileName = employee.EmployeeImageFileName;

            LoadPropertyValuesWithThisEntity(Entity);

        }

        public void LoadPropertyValuesWithThisEntity(EntityRef entity)
        {
            if (entity == null) return;

            LoadCorrectPropertyValueRefs(Entity, new List<PropertyValueRef>()
            {
                TerminationCode,
                EEOCode,
                RehireStatusCode,
                BusinessRegionCode,
                MaritalStatusCode,
                GenderCode,
                CostCenterCode,
                ShiftCode,
                KronosDepartmentCode,
                EthnicityCode,
                NationalityCode,
                CountryOfOriginCode,
                Status1Code,
                Status2Code,
                WorkAreaCode,
                CompanyNumberCode,
                BusinessUnitCode,
                DeviceGroup,
                Lms, 
                PerformanceEvaluationType

            });

            foreach (var employeePhoneNumberModel in PhoneNumbers)
            {
                employeePhoneNumberModel.LoadPropertyValuesWithThisEntity(entity);
            }
            foreach (var emailAddressModel in EmailAddresses)
            {
                emailAddressModel.LoadPropertyValuesWithThisEntity(entity);
            }

            foreach (var employeeVerificationModel in EmployeeVerifications)
            {
                employeeVerificationModel.LoadPropertyValuesWithThisEntity(entity);
            }
            Compensation?.LoadPropertyValuesWithThisEntity(entity);

        }
    }
}
