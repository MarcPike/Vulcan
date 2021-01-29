using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.Models
{
    public class EmployeeIncidentModel : BaseModel
    {
        public string Id { get; set; }
        public EmployeeRef Employee { get; set; }

        public EmployeeRef Manager { get; set; }

        public EmployeeRef ReportedBy { get; set; }

        public LocationRef Location { get; set; }
        public bool Osha { get; set; }
        public bool Riddor { get; set; }

        public PropertyValueRef NearMissTypeCode { get; set; }
        public PropertyValueRef IncidentType { get; set; }
        public string AdditionalComments { get; set; } = string.Empty;
        public string ImmediateActionTakenNote { get; set; } = string.Empty;
        public string PhysicalLocation { get; set; } = string.Empty;
        public PropertyValueRef PhysicalLocationType { get; set; }
        public string IncidentTypeNote { get; set; } = string.Empty;
        public PropertyValueRef SeverityTypeCode { get; set; }
        public string SeverityTypeNote { get; set; } = string.Empty;
        public string WorkBeingPerformed { get; set; } = string.Empty;
        public PropertyValueRef WorkBeingPerformedType { get; set; }
        public string CorrectiveAction { get; set; } = string.Empty;
        public string PreventativeAction { get; set; } = string.Empty;
        public string Recommendation { get; set; } = string.Empty;
        public EmployeeRef ApprovedBy { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? ApprovedDate { get; set; }
        public DrugTestRef DrugTest { get; set; }
        public List<AffectedBodyPart> AffectedBodyParts { get; set; } = new List<AffectedBodyPart>();
        public List<BodyContact> BodyContacts { get; set; } = new List<BodyContact>();
        public List<ContactWith> ContactWith { get; set; } = new List<ContactWith>();
        public List<EquipmentInvolved> EquipmentInvolved { get; set; } = new List<EquipmentInvolved>();
        public PropertyValueRef FirstAidTreatment { get; set; }
        public string FirstAidTreatmentAdministered { get; set; } = string.Empty;
        public string ImmediateCauses { get; set; } = string.Empty;

        public IIQ IIQ { get; set; } = new IIQ();
        public List<JobFactor> JobFactors { get; set; } = new List<JobFactor>();
        public List<LackOfControl> LackOfControls { get; set; } = new List<LackOfControl>();
        public List<MedicalLeaveModel> MedicalLeaves { get; set; } = new List<MedicalLeaveModel>();
        public List<MedicalTreatmentModel> MedicalTreatments { get; set; } = new List<MedicalTreatmentModel>();
        public List<MedicalWorkStatusModel> MedicalWorkStatus { get; set; } = new List<MedicalWorkStatusModel>();
        public List<RootCause> RootCauses { get; set; } = new List<RootCause>();
        public List<PersonalFactor> PersonalFactors { get; set; } = new List<PersonalFactor>();
        public List<NatureOfInjury> NatureOfInjuries { get; set; } = new List<NatureOfInjury>();
        public List<BbsPrecaution> BbsPrecautions { get; set; } = new List<BbsPrecaution>();
        public List<Witness> Witnesses { get; set; } = new List<Witness>();


        public List<Investigation> Investigations { get; set; } = new List<Investigation>();
        public string MedicalNotes { get; set; }
        public PropertyValueRef MedicalTreatmentAdministeredType { get; set; }

        public List<SupportingDocument> SupportingDocuments { get; set; } = new List<SupportingDocument>();

        public HrsUserRef HrsUser { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? IncidentDate { get; set; }
        public string IncidentId { get; set; }
        public PropertyValueRef IncidentStatus { get; set; }
        public bool OneDayNotificationSent { get; set; }
        public bool TenDayNotificationSent { get; set; }
        public bool SubmittedFromWeb { get; set; }

        public string ThirdParty { get; set; }

        public PropertyValueRef GenderCode { get; set; }
        public int Age { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? OriginalHireDate { get; set; }
        public JobTitleRef JobTitle { get; private set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        private DateTime Birthday { get; set; }

        public List<ChangeHistory> ChangeHistory { get; set; } = new List<ChangeHistory>();

        public string IncidentUrl = string.Empty;
        public bool SendEmail { get; set; } = true;


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

        private void GetEmployeeValues()
        {
            if (Employee == null) return;
            var queryHelper = new MongoRawQueryHelper<Employee>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Id == ObjectId.Parse(Employee.Id));
            var project = queryHelper.ProjectionBuilder.Expression(
                x => new { x.Birthday, x.GenderCode, x.OriginalHireDate, x.JobTitle, x.CostCenterCode});

            var employeeValues = queryHelper.FindWithProjection(filter, project).FirstOrDefault();

            if (employeeValues == null) return;


            Birthday = _encryption.Decrypt<DateTime>(employeeValues.Birthday);
            GenderCode = employeeValues.GenderCode;
            Age = GetAge();
            OriginalHireDate = employeeValues.OriginalHireDate;
            JobTitle = employeeValues.JobTitle;
            CostCenterCode = employeeValues.CostCenterCode;
        }

        public PropertyValueRef CostCenterCode { get; set; }

        public EmployeeIncidentModel()
        {
        }

        public EmployeeIncidentModel(HrsUserRef user)
        {
            HrsUser = user;
        }

        public EmployeeIncidentModel(EmployeeIncident e)
        {
            Id = e.Id.ToString();
            var incidentId = e.NearMissTypeCode?.Code ?? "?";
            incidentId += e.IncidentId.ToString();

            IncidentId = incidentId;
            Employee = e.Employee?.Refresh();
            ReportedBy = e.ReportedBy?.Refresh();

            Manager = e.Manager?.Refresh();
            Location = e.Location;
            Osha = e.Osha;
            Riddor = e.Riddor;
            IncidentType = e.IncidentType;
            NatureOfInjuries = e.NatureOfInjuries;
            BbsPrecautions = e.BbsPrecautions;
            Witnesses = e.Witnesses;
            ThirdParty = e.ThirdParty;

            ImmediateCauses = e.ImmediateCauses;

            NearMissTypeCode = e.NearMissTypeCode;
            AdditionalComments = e.AdditionalComments ?? string.Empty;
            ImmediateActionTakenNote = e.ImmediateActionTakenNote ?? string.Empty;
            PhysicalLocation = e.PhysicalLocation ?? string.Empty;
            PhysicalLocationType = e.PhysicalLocationType;
            IncidentTypeNote = e.IncidentTypeNote ?? string.Empty;
            SeverityTypeCode = e.SeverityTypeCode;
            SeverityTypeNote = e.SeverityTypeNote ?? string.Empty;
            WorkBeingPerformed = e.WorkBeingPerformed ?? string.Empty;
            WorkBeingPerformedType = e.WorkBeingPerformedType;
            CorrectiveAction = e.CorrectiveAction ?? string.Empty;
            PreventativeAction = e.PreventativeAction ?? string.Empty;
            Recommendation = e.Recommendation ?? string.Empty;
            ApprovedBy = e.ApprovedBy?.Refresh();
            ApprovedDate = e.ApprovedDate;
            DrugTest = e.DrugTest;
            AffectedBodyParts = e.AffectedBodyParts;
            BodyContacts = e.BodyContacts;
            ContactWith = e.ContactWith;
            EquipmentInvolved = e.EquipmentInvolved;
            FirstAidTreatment = e.FirstAidTreatment;
            FirstAidTreatmentAdministered = e.FirstAidTreatmentAdministered;
            IIQ = e.IIQ;
            Investigations = e.Investigations;
            JobFactors = e.JobFactors;
            LackOfControls = e.LackOfControls;
            MedicalLeaves = MedicalLeaveModel.ConvertList(e.MedicalLeaves);
            MedicalTreatments = MedicalTreatmentModel.ConvertList(e.MedicalTreatments);
            MedicalWorkStatus = MedicalWorkStatusModel.ConvertList(e.MedicalWorkStatus);
            MedicalNotes = _encryption.Decrypt<String>(e.MedicalNotes);
            MedicalTreatmentAdministeredType = e.MedicalTreatmentAdministeredType;
            IncidentDate = e.IncidentDate;
            IncidentStatus = e.IncidentStatus;
            OneDayNotificationSent = e.OneDayNotificationSent;
            TenDayNotificationSent = e.TenDayNotificationSent;
            SubmittedFromWeb = e.SubmittedFromWeb;
            RootCauses = e.RootCauses;
            PersonalFactors = e.PersonalFactors;

            ChangeHistory = e.ChangeHistory;

            GetEmployeeValues();

            var queryHelper = new MongoRawQueryHelper<SupportingDocument>();
            var filter = queryHelper.FilterBuilder.Where(x => x.BaseDocumentId == e.Id);
            var project =
                queryHelper.ProjectionBuilder.Expression(doc => 
                    new SupportingDocumentModel(
                        doc.Id.ToString(),
                        doc.BaseDocumentId.ToString(),
                        doc.Module,
                        doc.DocumentType,
                        doc.DocumentDate,
                        doc.FileName,
                        doc.FileSize,
                        doc.FileCreateDate,
                        doc.UploadedByUser,
                        doc.Comments,
                        doc.FileInfo.Id.ToString(),
                        doc.MimeType
                    )
                );
            SupportingDocuments = queryHelper.Find(filter).ToList();
        }

    }
}
