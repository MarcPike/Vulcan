using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Compensation;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Encryption;

namespace DAL.HRS.Mongo.Models
{
    public class KronosExportModel
    {
        public string InternationalGovernmentId { get; set; } = string.Empty;
        public string USGovernmentId { get; set; }
        public DateTime BirthDay { get; set; }
        public string DefaultCompanyCode { get; set; } 
        public string BusinessUnit { get; set; }
        public string Gender { get; set; }
        public string MaritalStatus { get; set; }
        public string PayrollId { get; set; }
        public string CostCenterCode { get; set; }
        public string KronosDepartmentCode { get; set; }
        public string KronosLaborLevelCode { get; set; }
        public string Status1 { get; set; }
        public string Status2 { get; set; }
        public string WorkAreaCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Country { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime? OriginalHireDate { get; set; }
        public DateTime? LastHireDate { get; set; }
        public string ManagerId { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string KronosPayRule { get; set; }
        public decimal BaseHours { get; set; }
        public DateTime? PayRateEffectiveDate { get; set; }
        public decimal PayRate { get; set; }
        public string PayRateType { get; set; }
        public bool IsActive { get; set; }
        public string NetworkId { get; set; }
        public string KronosClockType { get; set; } = "<unknown>";
        public string TimeZone { get; set; } 
        public DateTime? TerminationDate { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? KronosPayRuleEffectiveDate { get; set; }
        public string PayFrequency { get; set; } 

        public KronosExportModel()
        {
            
        }

        public KronosExportModel(byte[] birthday, PropertyValueRef companyNumberCode,
            PropertyValueRef genderCode, PropertyValueRef maritalStatusCode, PropertyValueRef businessUnitCode, string payrollId,
            PropertyValueRef costCenterCode,
            PropertyValueRef kronosDepartmentCode, LocationRef locationRef, byte[] governmentId, PropertyValueRef status1,
            PropertyValueRef status2,
            PropertyValueRef workAreaCode, string firstName, string lastName, JobTitleRef jobTitle,
            string contactCountry,
            string address1, string address2, string address3, string city, string state, string postalCode,
            DateTime? originalHireDate, DateTime? lastRehireDate, EmployeeRef manager,
            List<EmployeePhoneNumber> phoneNumbers, List<EmployeeEmailAddress> emailAddresses, bool isActive,
            Compensation compensation,
            LdapUserRef ldapUser, string login, DateTime? terminationDate, PropertyValueRef deviceGroup)
        {
            var enc = Encryption.NewEncryption;

            BirthDay = enc.Decrypt<DateTime>(birthday);

            DefaultCompanyCode = companyNumberCode?.Code ?? "<unknown>";
            BusinessUnit = businessUnitCode?.Code ?? "<unknown>";

            Gender = genderCode?.Code ?? "";
            MaritalStatus = maritalStatusCode?.Code ?? "";
            PayrollId = payrollId ?? "";
            CostCenterCode = costCenterCode?.Code ?? "";
            KronosDepartmentCode = kronosDepartmentCode?.Code ?? "<unknown>";
            var location = locationRef?.AsLocation();
            KronosLaborLevelCode = (location?.KronosLaborLevel == string.Empty)
                ? "<missing in location>"
                : location?.KronosLaborLevel;

            if (deviceGroup != null)
            {
                KronosClockType = deviceGroup.Code;
            }
            else
            {
                KronosClockType = string.Empty;
            }

            var govId = enc.Decrypt<string>(governmentId);

            if ((location?.Country == "United States") || (payrollId.ToUpper().Contains("US"))) 
            {
                USGovernmentId = govId;
                InternationalGovernmentId = string.Empty;
            }
            else
            {
                USGovernmentId = string.Empty;
                InternationalGovernmentId = govId;
            }

            TimeZone = location?.TimeZone != null ? location?.TimeZone.Name : LocationTimeZone.Unspecified().Name;

            Status1 = status1?.Code ?? "null";
            Status2 = status2?.Code ?? "null";
            WorkAreaCode = workAreaCode?.Code ?? "";
            FirstName = firstName;
            LastName = lastName;
            JobTitle = jobTitle.Name;
            Country = contactCountry;
            Address1 = address1;
            Address2 = address2;
            Address3 = address3;
            PostalCode = postalCode;
            City = city;
            State = state;
            OriginalHireDate = originalHireDate;
            LastHireDate = lastRehireDate;
            manager.Refresh();
            ManagerId = manager?.PayrollId;
            if (phoneNumbers.Any())
            {
                HomePhone = phoneNumbers.FirstOrDefault(x => x.PhoneType != null && x.PhoneType.Code == "Home")?.PhoneNumber ?? string.Empty;
                MobilePhone = phoneNumbers.FirstOrDefault(x => x.PhoneType != null && x.PhoneType.Code == "Mobile")?.PhoneNumber ?? string.Empty;
            }
            if (compensation != null)
            {
                KronosPayRule = "<unknown>";
                if (compensation.KronosPayRuleType != null)
                {
                    var kronosPayRule = PropertyValue.Helper.FindById(compensation.KronosPayRuleType.Id);
                    if (kronosPayRule == null)
                    {
                        KronosPayRule = $"{compensation.KronosPayRuleType.Code} (Data Error)";
                    }
                    else
                    {
                        KronosPayRule = kronosPayRule.Code;
                    }

                }
                //KronosPayRule = compensation.KronosPayRuleType?.Code ?? "<unknown>";
                BaseHours = compensation?.BaseHours ?? 0;
                PayRateEffectiveDate = enc.Decrypt<DateTime>(compensation.EffectiveDate);
                PayRate = enc.Decrypt<decimal>(compensation.PayRateAmount);
                PayRateType = compensation.PayRateType?.Code ?? "<unknown>";
                KronosPayRuleEffectiveDate = compensation.KronosPayRuleEffectiveDate;

                //if (location.Country == "United States")
                if ((location?.Country == "United States") || (payrollId.ToUpper().Contains("US")))
                { 
                    PayFrequency = $"{compensation.PayFrequencyType?.Code ?? String.Empty}- US" ;
                    PayFrequency = PayFrequency.Replace("Bi-Weekly", "BiWeekly");
                }
                else
                {
                    PayFrequency = "International Employees";

                }

            }
            IsActive = isActive;

            
            if (ldapUser != null)
            {
                try
                {
                    var user = ldapUser.AsLdapUser();
                    NetworkId = user.NetworkId;
                }
                catch (Exception)
                {
                    NetworkId = login;
                }
            }
            else
            {
               
                NetworkId = login;
            }

            TerminationDate = terminationDate;

            if (emailAddresses.Any())
            {
                var workAddress = emailAddresses.FirstOrDefault(x => x.EmailType.Code == "Work");

                if (workAddress != null)
                {
                    EmailAddress = workAddress.EmailAddress;
                }
                else
                {
                    var personalAddress = emailAddresses.FirstOrDefault(x => x.EmailType.Code == "Personal");
                    if (personalAddress != null)
                    {
                        EmailAddress = personalAddress.EmailAddress;
                    }
                    else
                    {
                        EmailAddress = "<unknown>";
                    }
                }
            }


        }

    }
}
