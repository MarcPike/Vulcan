using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.Vulcan.Mongo.Base.Encryption;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.Models
{
    public class QngEmployeeIncidentModel
    {
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

        public int DaysAway { get; set; } = 0;
        public decimal HoursAway { get; set; } = 0;
        public int DaysRestrictedWork { get; set; } = 0;

        public string MedicalNotes { get; set; }

        private static readonly Encryption Enc = Encryption.NewEncryption;

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



        public static List<QngEmployeeIncidentModel> GetValuesFor(DateTime minDate, DateTime maxDate)
        {
            var result = new List<QngEmployeeIncidentModel>();

            var incidents = EmployeeIncident.Helper.Find(x => x.IncidentDate >= minDate && x.IncidentDate <= maxDate)
                .ToList();

            foreach (var i in incidents)
            {   
                var v = new QngEmployeeIncidentModel();
                GetBaseFields(v, i);
                result.Add(v);
            }

            return result.OrderByDescending(x => x.IncidentDate).ToList();
        }

        private static void GetBaseFields(QngEmployeeIncidentModel v, EmployeeIncident i)
        {
            v.IncidentId = i.IncidentId;
            v.IncidentDate = i.IncidentDate;
            v.StatusName = i.IncidentStatus?.Code ?? string.Empty;
            v.NearMissType = i.NearMissTypeCode?.Description ?? string.Empty;
            v.IncidentType = i.IncidentType?.Code ?? string.Empty;
            v.IncidentTypeNote = i.IncidentTypeNote;
            v.SeverityType = i.SeverityTypeCode?.Code ?? string.Empty;
            v.SeverityTypeNote = i.SeverityTypeNote;
            v.Employee = i.Employee;
            v.Manager = i.Manager;
            v.BodyParts = GetStringFromList(i.AffectedBodyParts?.Select(x => x.Type.Code).ToList());
            v.NatureOfInjury = GetStringFromList(i.NatureOfInjuries?.Select(x => x.Type.Code).ToList());
            v.TypeOfContact = GetStringFromList(i.ContactWith?.Select(x => x.Type.Code).ToList());
            v.Osha = i.Osha;
            v.Riddor = i.Riddor;
            v.Location = i.Location;
            v.PhysicalLocation = i.PhysicalLocation;
            v.PhysicalLocationType = i.PhysicalLocationType?.Code ?? string.Empty;
            v.WorkBeingPerformed = i.WorkBeingPerformed;
            v.WorkBeingPerformedType = i.WorkBeingPerformedType?.Code ?? string.Empty;
            v.AdditionalComments = i.AdditionalComments;
            v.CorrectiveAction = i.CorrectiveAction;
            v.PreventativeAction = i.PreventativeAction;
            v.Recommendation = i.Recommendation;
            v.MedicalTreatmentAdministered = i.MedicalTreatmentAdministeredType != null && i.MedicalTreatmentAdministeredType.Code != null ? i.MedicalTreatmentAdministeredType.Code : string.Empty; 
            v.RootCauses = GetStringFromList(i.RootCauses?.Select(x => x.Type.Code).ToList());
            v.LacksOfControl = GetStringFromList(i.LackOfControls?.Select(x => x.Type.Code).ToList());
            v.PersonalFactors = GetStringFromList(i.PersonalFactors.Select(x => x.Type.Code).ToList());
            v.JobFactors = GetStringFromList(i.JobFactors.Select(x => x.Type.Code).ToList());
            v.DrugTest = i.DrugTest?.DrugTestResult?.Code ?? string.Empty;
            v.OneDayNotificationSent = i.OneDayNotificationSent;
            v.TenDayNotificationSent = i.TenDayNotificationSent;
            v.ApprovedBy = i.ApprovedBy;
            v.ApprovedByDate = i.ApprovedDate;
            v.MedicalNotes = Enc.Decrypt<string>(i.MedicalNotes);
            if (i.MedicalLeaves.Any())
            {
                var enc = Encryption.NewEncryption;
                v.DaysAway = i.MedicalLeaves.Sum(m => enc.Decrypt<int>(m.DaysAway));
                v.HoursAway = i.MedicalLeaves.Sum(m => enc.Decrypt<decimal>(m.HoursAway));
                v.DaysRestrictedWork = i.MedicalLeaves.Sum(m => enc.Decrypt<int>(m.DaysRestrictedWork));
            }

        }
        private static string GetStringFromList(List<string> values)
        {
            if (values == null) return string.Empty;

            if (values.Count == 0) return string.Empty;

            StringBuilder sb = new StringBuilder();

            sb.Append(values[0]);

            if (values.Count > 1)
            {
                for (int i = 1; i < values.Count - 1; i++)
                {
                    sb.Append(" | " + values[i]);
                }

            }

            return sb.ToString();
        }

    }
}