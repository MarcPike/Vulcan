using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;

namespace DAL.HRS.Mongo.Models
{
    public class QngEmployeeIncidentModelOld
    {
        public string VarDataFieldName { get; set; }
        public string VarDataFieldComment { get; set; }
        public string IncidentId { get; set; }
        public EmployeeRef Employee { get; set; }

        public EmployeeRef Manager { get; set; }

        public EmployeeRef ReportedBy { get; set; }
        public DateTime? IncidentDate { get; set; }
        public LocationRef Location { get; set; }

        public PropertyValueRef IncidentStatus { get; set; }
        public PropertyValueRef NearMissTypeCode { get; set; }
        public PropertyValueRef IncidentType { get; set; }
        public string AdditionalComments { get; set; } = string.Empty;
        public string ImmediateActionTakenNote { get; set; } = string.Empty;
        public string PhysicalLocation { get; set; } = string.Empty;
        public string IncidentTypeNote { get; set; } = string.Empty;
        public PropertyValueRef SeverityTypeCode { get; set; }
        public string SeverityTypeNote { get; set; } = string.Empty;

        public string BodyParts { get; set; }
        public string NatureOfInjury { get; set; }
        public string TypeOfContact { get; set; }
        public bool? OSHA { get; set; }
        public bool? RIDDOR { get; set; }
        public string WorkBeingPerformed { get; set; }
        public string CorrectiveAction { get; set; }
        public string PreventativeAction { get; set; }
        public string Recommendation { get; set; }
        public string MedicalNotes { get; set; }
        public string MedicalTreatmentAdministered { get; set; }
        public string RootCauses { get; set; }
        public string LacksOfControl { get; set; }
        public string PersonalFactors { get; set; }
        public string JobFactors { get; set; }
        public string DrugTest { get; set; }
        public bool? OneDayNotificationSent { get; set; }
        public bool? TenDayNotificationSent { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedByDate { get; set; }
        public int DaysAway { get; set; }
        public decimal HoursAway { get; set; }
        public int DaysRestrictedWork { get; set; }

        public int? IncidentDateYearOf => IncidentDate?.Year;

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
