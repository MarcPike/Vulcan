using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.Vulcan.Mongo.Base.Encryption;
using Microsoft.SqlServer.Server;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.Models
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
        public bool OneDayNotificationSent{ get; set; }
        public bool TenDayNotificationSent { get; set; }
        public EmployeeRef ApprovedBy { get; set; }
        public DateTime? ApprovedByDate { get; set; }

        public int DaysAway { get; set; }
        public decimal HoursAway { get; set; }
        public int DaysRestrictedWork { get; set; }

        public int? IncidentDateYearOf => IncidentDate?.Year;

        public string MedicalNotes { get; set; }

        private static readonly Encryption Enc = Encryption.NewEncryption;

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


        public static List<QngEmployeeIncidentVarDataModel> GetValuesFor(string varDataField, DateTime minDate, DateTime maxDate)
        {
            var result = new List<QngEmployeeIncidentVarDataModel>();

            var incidents = EmployeeIncident.Helper.Find(x => x.IncidentDate >= minDate && x.IncidentDate <= maxDate)
                .ToList();

            result.AddRange(LoadForBodyPart(varDataField, incidents));
            result.AddRange(LoadForTypeOfContact(varDataField, incidents));
            result.AddRange(LoadForNatureOfInjury(varDataField, incidents));
            result.AddRange(LoadForRootCause(varDataField, incidents));
            result.AddRange(LoadForLackOfControl(varDataField, incidents));
            result.AddRange(LoadForPersonalFactors(varDataField, incidents));
            result.AddRange(LoadForJobFactors(varDataField, incidents));


            return result.OrderBy(x => x.VarDataFieldName).ThenByDescending(x => x.IncidentDate).ToList();
        }

        private static List<QngEmployeeIncidentVarDataModel> LoadForJobFactors(string varDataField, List<EmployeeIncident> incidents)
        {
            var result = new List<QngEmployeeIncidentVarDataModel>();
            if (varDataField != "Job Factors") return new List<QngEmployeeIncidentVarDataModel>();

            foreach (var i in incidents)
            {
                foreach (var iJobFactor in i.JobFactors)
                {
                    var v = new QngEmployeeIncidentVarDataModel();

                    v.VarDataFieldName = iJobFactor.Type.Code;
                    v.VarDataFieldComment = iJobFactor.Comments ?? string.Empty;

                    GetBaseFields(v, i);

                    result.Add(v);

                }
            }

            return result;

        }

        private static List<QngEmployeeIncidentVarDataModel> LoadForPersonalFactors(string varDataField, List<EmployeeIncident> incidents)
        {
            var result = new List<QngEmployeeIncidentVarDataModel>();
            if (varDataField != "Personal Factors") return new List<QngEmployeeIncidentVarDataModel>();

            foreach (var i in incidents)
            {
                foreach (var iPersonalFactor in i.PersonalFactors)
                {
                    var v = new QngEmployeeIncidentVarDataModel();

                    v.VarDataFieldName = iPersonalFactor.Type.Code;
                    v.VarDataFieldComment = iPersonalFactor.Comments ?? string.Empty;

                    GetBaseFields(v, i);

                    result.Add(v);

                }

            }

            return result;

        }

        private static List<QngEmployeeIncidentVarDataModel> LoadForLackOfControl(string varDataField, List<EmployeeIncident> incidents)
        {
            var result = new List<QngEmployeeIncidentVarDataModel>();
            if (varDataField != "Lack of Control") return new List<QngEmployeeIncidentVarDataModel>();

            foreach (var i in incidents)
            {
                foreach (var iLackOfControl in i.LackOfControls)
                {
                    var v = new QngEmployeeIncidentVarDataModel();

                    v.VarDataFieldName = iLackOfControl.Type.Code;
                    v.VarDataFieldComment = iLackOfControl.Comments ?? string.Empty;

                    GetBaseFields(v, i);

                    result.Add(v);

                }

            }

            return result;

        }


        private static List<QngEmployeeIncidentVarDataModel> LoadForRootCause(string varDataField, List<EmployeeIncident> incidents)
        {
            var result = new List<QngEmployeeIncidentVarDataModel>();
            if (varDataField != "Root Cause") return new List<QngEmployeeIncidentVarDataModel>();

            foreach (var i in incidents)
            {

                foreach (var iRootCause in i.RootCauses)
                {
                    var v = new QngEmployeeIncidentVarDataModel();

                    GetBaseFields(v, i);

                    v.VarDataFieldName = iRootCause.Type.Code;
                    v.VarDataFieldComment = iRootCause.Comments ?? string.Empty;
                    result.Add(v);

                }



            }

            return result;

        }


        private static List<QngEmployeeIncidentVarDataModel> LoadForNatureOfInjury(string varDataField, List<EmployeeIncident> incidents)
        {
            var result = new List<QngEmployeeIncidentVarDataModel>();
            if (varDataField != "Nature of Injury") return new List<QngEmployeeIncidentVarDataModel>();

            foreach (var i in incidents)
            {
                foreach (var iNatureOfInjury in i.NatureOfInjuries)
                {
                    var v = new QngEmployeeIncidentVarDataModel();

                    v.VarDataFieldName = iNatureOfInjury.Type.Code;
                    v.VarDataFieldComment = iNatureOfInjury.Comments ?? string.Empty;

                    GetBaseFields(v, i);

                    result.Add(v);

                }

            }

            return result;

        }



        private static List<QngEmployeeIncidentVarDataModel> LoadForTypeOfContact(string varDataField, List<EmployeeIncident> incidents)
        {
            var result = new List<QngEmployeeIncidentVarDataModel>();
            if (varDataField != "Type of Contact") return new List<QngEmployeeIncidentVarDataModel>();

            foreach (var i in incidents)
            {

                foreach (var contactWith in i.ContactWith)
                {
                    var v = new QngEmployeeIncidentVarDataModel();
                    v.VarDataFieldName = contactWith.Type.Code;
                    v.VarDataFieldComment = contactWith.Comments ?? string.Empty;

                    GetBaseFields(v, i);

                    result.Add(v);

                }

            }

            return result;

        }


        private static List<QngEmployeeIncidentVarDataModel> LoadForBodyPart(string varDataField, List<EmployeeIncident> incidents)
        {
            var result = new List<QngEmployeeIncidentVarDataModel>();
            if (varDataField != "Body Part") return new List<QngEmployeeIncidentVarDataModel>();

            foreach (var i in incidents)
            {

                foreach (var iAffectedBodyPart in i.AffectedBodyParts)
                {
                    var v = new QngEmployeeIncidentVarDataModel();

                    v.VarDataFieldName = iAffectedBodyPart.Type.Code;
                    v.VarDataFieldComment = iAffectedBodyPart.Comments ?? string.Empty;

                    GetBaseFields(v, i);
                    result.Add(v);

                }


            }

            return result;

        }

        private static void GetBaseFields(QngEmployeeIncidentVarDataModel v, EmployeeIncident i)
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
            v.RootCauses = GetStringFromList(i.RootCauses?.Select(x=>x.Type.Code).ToList());
            v.LacksOfControl = GetStringFromList(i.LackOfControls?.Select(x => x.Type.Code).ToList());
            v.PersonalFactors = GetStringFromList(i.PersonalFactors.Select(x=>x.Type.Code).ToList());
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



            var i = 0;
            foreach (var value in values)
            {
            
                if (i == 0)
                {
                    sb.Append(value);
                }
                else
                {
                    sb.Append(" | " + value);
                }

                i++;
            }


            return sb.ToString();
        }
    }
}
