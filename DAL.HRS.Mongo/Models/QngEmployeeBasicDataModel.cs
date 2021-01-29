using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Discipline;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.Models
{
    public class QngEmployeeBasicDataModel
    {
        public DateTime DateOf { get; set; }
        public string EmployeeId { get; set; }
        public string PayrollId { get; set; }
        public string GovernmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName{ get; set; }
        public string LdapNetworkId { get; set; }
        public string OldLoginId { get; set; }
        public DateTime BirthDay { get; set; }
        public JobTitleRef JobTitle { get; set; }
        public PropertyValueRef EEOCode { get; set; }
        public PropertyValueRef EthnicityCode { get; set; }
        public PropertyValueRef GenderCode { get; set; }
        public LocationRef Location { get; set; }
        public EmployeeRef Manager { get; set; }
        public PropertyValueRef CountryCode { get; set; }
        public PropertyValueRef CountryOfOriginCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public PropertyValueRef Country { get; set; }
        public string PostalCode { get; set; }
        public List<EmployeePhoneNumber> PhoneNumbers { get; set; } 
        public List<EmployeeEmailAddress> EmailAddresses { get; set; } 
        public List<EmergencyContact> EmergencyContacts { get; set; } 
        public PropertyValueRef Status1Code { get; set; }
        public PropertyValueRef Status2Code { get; set; }
        public PayrollRegionRef PayrollRegion { get; set; }

        public PropertyValueRef BusinessRegionCode { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? PriorServiceDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? OriginalHireDate { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? LastRehireDate { get; set; }

        public PropertyValueRef WorkAreaCode { get; set; }

        public List<Termination> Terminations { get; set; }
        //[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? TerminationDate { get; set; }
        public PropertyValueRef TerminationCode { get; set; }
        public string TerminationExplanation { get; set; } = string.Empty;

        public PropertyValueRef RehireStatusCode { get; set; }

        //public PropertyValueRef KronosLaborLevelCode { get; set; }
        public string KronosLaborLevelForLocation { get; set; }
        public PropertyValueRef KronosDepartmentCode { get; set; }

        public PropertyValueRef NationalityCode { get; set; }

        public Discipline LastDiscipline { get; set; }

        public PropertyValueRef CostCenterCode { get; set; }

        public PropertyValueRef ShiftCode { get; set; }

        public PropertyValueRef BusinessUnitCode { get; set; }

        public PropertyValueRef CompanyNumberCode { get; set; }

        public string ExternalLoginId { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        public string FullName {
            get {
                if (PreferredName != string.Empty)
                {
                    if (MiddleName != string.Empty)
                    {
                        return $"{PreferredName} {MiddleName} {LastName}";
                    }
                    else
                    {
                        return $"{PreferredName} {LastName}";
                    }
                }
                else
                {
                    if (MiddleName != string.Empty)
                    {
                        return $"{FirstName} {MiddleName} {LastName}";
                    }
                    else
                    {
                        return $"{FirstName} {LastName}";
                    }
                }
            }
        }

        public bool IsActive
        {
            get
            {
                var maxDate = DateTime.MaxValue;
                var minDate = DateTime.MinValue;
                
                maxDate = TerminationDate ?? maxDate;

                minDate = LastRehireDate ?? OriginalHireDate ?? minDate;

                return DateOf >= minDate && DateOf <= maxDate;

            }
        }

        public TimeSpan Seniority
        {
            get
            {
                var startDate = (LastRehireDate ?? PriorServiceDate ?? OriginalHireDate ?? DateOf);
                var endDate = (TerminationDate ?? DateOf);

                var result = endDate - startDate;
                return result;

            }
        }



        public int Age
        {
            get
            {
                try
                {
                    DateTime now = DateOf.Date;
                    int age = now.Year - BirthDay.Year;
                    if (BirthDay > now.AddYears(-age)) age--;
                    return age;
                }
                catch (Exception)
                {
                    return 0;
                }

            }
        }
        public QngEmployeeBasicDataModel() { }

        public QngEmployeeBasicDataModel(
            DateTime dateOf, ObjectId id, string payrollId, byte[] governmentId, 
            string firstName, string lastName, string middleName, string preferredName,
            LdapUserRef ldapUser, byte[] birthday, JobTitleRef jobTitle, PropertyValueRef eeoCode,
            PropertyValueRef ethnicityCode, PropertyValueRef genderCode, LocationRef location,
            EmployeeRef manager, PropertyValueRef countryCode, PropertyValueRef countryOfOriginCode,
            string address1, string address2, string address3, string city, string state, PropertyValueRef country,
            string postalCode, List<EmployeePhoneNumber> phoneNumbers, List<EmployeeEmailAddress> emailAddresses,
            List<EmergencyContact> emergencyContacts, PropertyValueRef status1Code, PropertyValueRef status2Code,
            PayrollRegionRef payrollRegion, PropertyValueRef businessRegionCode, 
            DateTime? priorServiceDate, DateTime? originalHireDate, DateTime? lastRehireDate,
            PropertyValueRef workAreaCode, List<Termination> terminations, 
            DateTime? terminationDate, PropertyValueRef terminationCode, string terminationExplanation,
            PropertyValueRef rehireStatusCode, PropertyValueRef kronosLaborLevelCode, PropertyValueRef kronosDepartmentCode,
            PropertyValueRef nationalityCode, Discipline lastDiscipline, PropertyValueRef costCenterCode, 
            PropertyValueRef shiftCode, string oldLoginId, string externalLoginId,
            PropertyValueRef companyNumberCode, PropertyValueRef businessUnitCode, DateTime? confirmationDate)
        {
            var enc = Encryption.NewEncryption;

            DateOf = dateOf;
            EmployeeId = id.ToString();
            PayrollId = payrollId;

          
            GovernmentId = enc.Decrypt<string>(governmentId) ?? string.Empty;
            BirthDay = enc.Decrypt<DateTime>(birthday);
            if (BirthDay != null)
            {
                BirthDay = BirthDay.ToUniversalTime().Date;
            }

            if (location != null)
            {
                KronosLaborLevelForLocation = location.AsLocation().KronosLaborLevel;
            }

            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PreferredName = preferredName;
            LdapNetworkId = ldapUser?.NetworkId ?? Guid.Empty.ToString();
            OldLoginId = oldLoginId;

            CompanyNumberCode = companyNumberCode;
            BusinessUnitCode = businessUnitCode;

            JobTitle = jobTitle;
            EEOCode = eeoCode;
            EthnicityCode = ethnicityCode;
            GenderCode = genderCode;
            Location = location;
            Manager = manager;
            CountryCode = countryCode;
            CountryOfOriginCode = countryOfOriginCode;
            Address1 = address1;
            Address2 = address2;
            Address3 = address3;
            City = city;
            State = state;
            Country = country;
            PostalCode = postalCode;
            PhoneNumbers = phoneNumbers;
            EmailAddresses = emailAddresses;
            EmergencyContacts = emergencyContacts;
            Status1Code = status1Code;
            Status2Code = status2Code;
            PayrollRegion = payrollRegion;
            BusinessRegionCode = businessRegionCode;
            PriorServiceDate = priorServiceDate?.ToUniversalTime();
            OriginalHireDate = originalHireDate?.ToUniversalTime();
            LastRehireDate = lastRehireDate?.ToUniversalTime();
            WorkAreaCode = workAreaCode;
            Terminations = terminations;
            TerminationDate = terminationDate?.ToUniversalTime();
            TerminationCode = terminationCode;
            TerminationExplanation = terminationExplanation;
            RehireStatusCode = rehireStatusCode;
            //KronosLaborLevelCode = kronosLaborLevelCode;
            KronosDepartmentCode = kronosDepartmentCode;
            NationalityCode = nationalityCode;
            LastDiscipline = lastDiscipline;
            CostCenterCode = costCenterCode;
            ExternalLoginId = externalLoginId;
            ConfirmationDate = confirmationDate?.ToUniversalTime();

        }

        public string SeniorityAsString()
        {
            DateTime age = DateTime.MinValue + Seniority;

            int ageInYears = age.Year - 1;
            int ageInMonths = age.Month - 1;
            int ageInDays = age.Day - 1;

            if (ageInYears > 0)
            {
                return $"{ageInYears} years, {ageInMonths} months, {ageInDays} days";
            }

            if (ageInMonths > 0)
            {
                return $"{ageInMonths} months, {ageInDays} days";
            }
            else
            {
                return $"{ageInDays} days";
            }
        }
    }
}
