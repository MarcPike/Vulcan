using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRS.Web.Client.CSharp.Referenced_Classes
{
    public class QngEmployeeIncidentVarDataModel
    {
        public string VarDataFieldName { get; set; }
        public string VarDataFieldComment { get; set; }
        public int IncidentId { get; set; }
        public DateTime? IncidentDate { get; set; }
        public string StatusName { get; set; }
        public string NearMissType { get; set; }
        public string IncidentType { get; set; }
        public string IncidentTypeNote { get; set; }
        public string SeverityType { get; set; }
        public string SeverityTypeNote { get; set; }
        public EmployeeRef Employee { get; set; }
        public EmployeeRef Manager { get; set; }
        public string BodyParts { get; set; }
        public string NatureOfInjury { get; set; }
        public string TypeOfContact { get; set; }
        public bool Osha { get; set; }
        public bool Riddor { get; set; }
        public LocationRef Location { get; set; }
        public string PhysicalLocation { get; set; }
        public string PhysicalLocationType { get; set; }
        public string WorkBeingPerformed { get; set; }
        public string WorkBeingPerformedType { get; set; }
        public string AdditionalComments { get; set; }
        public string CorrectiveAction { get; set; }
        public string PreventativeAction { get; set; }
        public string Recommendation { get; set; }
        public string MedicalTreatmentAdministered { get; set; }
        public string RootCauses { get; set; }
        public string LacksOfControl { get; set; }
        public string PersonalFactors { get; set; }
        public string JobFactors { get; set; }
        public string DrugTest { get; set; }
        public bool OneDayNotificationSent { get; set; }
        public bool TenDayNotificationSent { get; set; }
        public EmployeeRef ApprovedBy { get; set; }
        public DateTime? ApprovedByDate { get; set; }

        public int DaysAway { get; set; }
        public decimal HoursAway { get; set; }
        public int DaysRestrictedWork { get; set; }

        public int? IncidentDateYearOf => IncidentDate?.Year;

        public string MedicalNotes { get; set; }

        public string IncidentDateMonthOf
        {
            get
            {
                if (!IncidentDate.HasValue) return "";

                var month = IncidentDate.Value.Month;
                var monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                var monthPrefix = month.ToString().PadLeft(2, '0');
                return $"{monthPrefix} - {monthName}";
            }
        }

        public string IncidentDateTimeOf => IncidentDate?.ToString("HH:mm");

        public string IncidentDateDayOfWeek
        {
            get
            {
                if (!IncidentDate.HasValue) return "";

                var dowNumber = ((int)IncidentDate.Value.DayOfWeek) + 1;
                var dowName = IncidentDate.Value.ToString("dddd");
                return $"{dowNumber} - {dowName}";
            }
        }

    }
}
