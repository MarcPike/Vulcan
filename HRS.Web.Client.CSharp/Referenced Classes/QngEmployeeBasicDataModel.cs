using System;
using System.Collections.Generic;

namespace HRS.Web.Client.CSharp.Referenced_Classes
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

        public DateTime? PriorServiceDate { get; set; }
        public DateTime? OriginalHireDate { get; set; }
        public DateTime? LastRehireDate { get; set; }

        public PropertyValueRef WorkAreaCode { get; set; }

        public List<Termination> Terminations { get; set; }
        public DateTime? TerminationDate { get; set; }
        public PropertyValueRef TerminationCode { get; set; }
        public string TerminationExplanation { get; set; } = string.Empty;

        public PropertyValueRef RehireStatusCode { get; set; }

        //public PropertyValueRef KronosLaborLevelCode { get; set; }
        public string KronosLaborLevelForLocation { get; set; }
        public PropertyValueRef KronosDepartmentCode { get; set; }

        public PropertyValueRef NationalityCode { get; set; }

        public Discipline LastDiscipline { get; set; }


        public string FullName { get; set; }

        public bool IsActive { get; set; }

        public TimeSpan Seniority { get; set; }

        public int Age { get; set; }

        public PropertyValueRef CostCenterCode { get; set; }
        public PropertyValueRef ShiftCode { get; set; }

        public PropertyValueRef BusinessUnitCode { get; set; }

        public PropertyValueRef CompanyNumberCode { get; set; }

        public DateTime? ConfirmationDate { get; set; }



    }
}
