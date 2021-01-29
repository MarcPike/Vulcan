using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.FileOperations;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Encryption;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    [BsonIgnoreExtraElements]
    public class EmployeeIncident : BaseDocument, ICloneable
    {
        public int OldHrsId { get; set; } = 0;
        public EmployeeRef Employee { get; set; }
        public EmployeeRef Manager { get; set; }
        public EmployeeRef ReportedBy { get; set; }

        public LocationRef Location { get; set; }
        public bool Osha { get; set; }
        public bool Riddor { get; set; }

        public PropertyValueRef NearMissTypeCode { get; set; }

        public string AdditionalComments { get; set; } = string.Empty;
        public string ImmediateActionTakenNote { get; set; } = string.Empty;
        public string PhysicalLocation { get; set; } = string.Empty;
        public PropertyValueRef PhysicalLocationType { get; set; }
        public string IncidentTypeNote { get; set; } = string.Empty;
        public PropertyValueRef SeverityTypeCode { get; set; }
        public string SeverityTypeNote { get; set; } = string.Empty;
        public string WorkBeingPerformed { get; set; } = string.Empty;
        public PropertyValueRef WorkBeingPerformedType { get; set; }
        public List<SupportingDocument> IncidentImages { get; set; } = new List<SupportingDocument>();
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
        public string FirstAidTreatmentAdministered { get; set; }
        public string ImmediateCauses { get; set; } = string.Empty;

        public IIQ IIQ { get; set; } = new IIQ();
        public List<Investigation> Investigations { get; set; } = new List<Investigation>();
        public List<JobFactor> JobFactors { get; set; } = new List<JobFactor>();
        public List<LackOfControl> LackOfControls { get; set; } = new List<LackOfControl>();
        public List<MedicalLeave> MedicalLeaves { get; set; } = new List<MedicalLeave>();
        public List<MedicalTreatment> MedicalTreatments { get; set; } = new List<MedicalTreatment>();
        public List<MedicalWorkStatus> MedicalWorkStatus { get; set; } = new List<MedicalWorkStatus>();
        public List<RootCause> RootCauses { get; set; } = new List<RootCause>();
        public List<PersonalFactor> PersonalFactors { get; set; } = new List<PersonalFactor>();
        public List<NatureOfInjury> NatureOfInjuries { get; set; } = new List<NatureOfInjury>();
        public List<Witness> Witnesses { get; set; } = new List<Witness>();
        public List<BbsPrecaution> BbsPrecautions { get; set; } = new List<BbsPrecaution>();
        public byte[] MedicalNotes { get; set; }
        public PropertyValueRef MedicalTreatmentAdministeredType { get; set; }
        public PropertyValueRef IncidentType { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? IncidentDate { get; set; }

        public int IncidentId { get; set; }
        public PropertyValueRef IncidentStatus { get; set; }
        public bool OneDayNotificationSent { get; set; }
        public bool TenDayNotificationSent { get; set; }
        public bool SubmittedFromWeb { get; set; }

        public string ThirdParty { get; set; } = string.Empty;



        public List<ChangeHistory> ChangeHistory { get; set; } = new List<ChangeHistory>();

        public EmployeeIncident()
        {

            IncidentId = GetNextIncidentId();

        }

        public static int GetNextIncidentId()
        {

            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            var filter = queryHelper.FilterBuilder.Empty;
            var project = queryHelper.ProjectionBuilder.Expression(x => x.IncidentId);
            var incidents = queryHelper.FindWithProjection(filter, project).ToList();
            if (!incidents.Any()) return 1;

            return incidents.Max() + 1;
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public ChangeHistory SaveChangeHistory(EmployeeIncident original, HrsUserRef hrsUser)
        {
            if (original == null) return null;

            var changeHistory = new ChangeHistory();
            changeHistory.HrsUser = hrsUser;
            var enc = Encryption.NewEncryption;

            ProcessEmployeeRefFields();

            ProcessLocationFields();

            ProcessPropertyFields();

            ProcessIIQ();

            ProcessBooleans();

            ProcessStringFields();

            ProcessIntFields();

            ProcessEncryptedFields();

            ProcessDateFields();

            ProcessAffectedBodyParts();

            ProcessBodyContacts();

            ProcessContactWith();

            ProcessEquipmentInvolved();

            ProcessIncidentImages();

            ProcessInvestigations();

            ProcessJobFactors();

            ProcessLackOfControls();

            ProcessMedicalLeaves();

            ProcessMedicalTreatments();

            ProcessMedicalWorkStatus();

            ProcessRootCauses();

            ProcessPersonalFactors();

            ProcessNatureOfInjuries();

            ProcessWitnesses();

            ProcessBbsPrecautions();

            SaveChangeHistory();

            return changeHistory;

            void ProcessIIQ()
            {
                if (IIQ.EmployeeAuthorized != original.IIQ.EmployeeAuthorized)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.EmployeeAuthorized",
                            original.IIQ.EmployeeAuthorized.ToString(),
                            IIQ.EmployeeAuthorized.ToString()));
                if (IIQ.EmployeeAuthorizedComment != original.IIQ.EmployeeAuthorizedComment)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.EmployeeAuthorizedComment",
                            original.IIQ.EmployeeAuthorizedComment,
                            IIQ.EmployeeAuthorizedComment));

                if (IIQ.EquipmentInDangerousPosition != original.IIQ.EquipmentInDangerousPosition)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.EquipmentInDangerousPosition",
                            original.IIQ.EquipmentInDangerousPosition.ToString(),
                            IIQ.EquipmentInDangerousPosition.ToString()));
                if (IIQ.EquipmentInDangerousPositionComments != original.IIQ.EquipmentInDangerousPositionComments)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.EquipmentInDangerousPositionComments",
                            original.IIQ.EquipmentInDangerousPositionComments,
                            IIQ.EquipmentInDangerousPositionComments));

                if (IIQ.EquipmentPreChecksCompleted != original.IIQ.EquipmentPreChecksCompleted)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.EquipmentPreChecksCompleted",
                            original.IIQ.EquipmentPreChecksCompleted.ToString(),
                            IIQ.EquipmentPreChecksCompleted.ToString()));
                if (IIQ.EquipmentPreChecksCompletedComments != original.IIQ.EquipmentPreChecksCompletedComments)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.EquipmentPreChecksCompletedComments",
                            original.IIQ.EquipmentPreChecksCompletedComments, IIQ.EquipmentPreChecksCompletedComments));

                if (IIQ.OthersInAreaWarned != original.IIQ.OthersInAreaWarned)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.OthersInAreaWarned",
                            original.IIQ.OthersInAreaWarned.ToString(),
                            IIQ.OthersInAreaWarned.ToString()));
                if (IIQ.OthersInAreaWarnedComments != original.IIQ.OthersInAreaWarnedComments)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.OthersInAreaWarnedComments",
                            original.IIQ.OthersInAreaWarnedComments,
                            IIQ.OthersInAreaWarnedComments));


                if (IIQ.RiskAssessmentAvailable != original.IIQ.RiskAssessmentAvailable)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.RiskAssessmentAvailable",
                            original.IIQ.RiskAssessmentAvailable.ToString(), IIQ.RiskAssessmentAvailable.ToString()));
                if (IIQ.RiskAssessmentAvailableComments != original.IIQ.RiskAssessmentAvailableComments)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.RiskAssessmentAvailableComments",
                            original.IIQ.RiskAssessmentAvailableComments, IIQ.RiskAssessmentAvailableComments));

                if (IIQ.SafeWorkingProceduresFollowed != original.IIQ.SafeWorkingProceduresFollowed)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.SafeWorkingProceduresFollowed",
                            original.IIQ.SafeWorkingProceduresFollowed.ToString(),
                            IIQ.SafeWorkingProceduresFollowed.ToString()));
                if (IIQ.SafeWorkingProceduresFollowedComments != original.IIQ.SafeWorkingProceduresFollowedComments)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.SafeWorkingProceduresFollowedComments",
                            original.IIQ.SafeWorkingProceduresFollowedComments,
                            IIQ.SafeWorkingProceduresFollowedComments));

                if (IIQ.UsingEquipmentCorrectly != original.IIQ.UsingEquipmentCorrectly)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.UsingEquipmentCorrectly",
                            original.IIQ.UsingEquipmentCorrectly.ToString(), IIQ.UsingEquipmentCorrectly.ToString()));
                if (IIQ.UsingEquipmentCorrectlyComments != original.IIQ.UsingEquipmentCorrectlyComments)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.UsingEquipmentCorrectlyComments",
                            original.IIQ.UsingEquipmentCorrectlyComments, IIQ.UsingEquipmentCorrectlyComments));

                if (IIQ.UsingRequiredPPE != original.IIQ.UsingRequiredPPE)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.UsingRequiredPPE", original.IIQ.UsingRequiredPPE.ToString(),
                            IIQ.UsingRequiredPPE.ToString()));
                if (IIQ.UsingRequiredPPEComments != original.IIQ.UsingRequiredPPEComments)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IIQ.UsingRequiredPPEComments",
                            original.IIQ.UsingRequiredPPEComments,
                            IIQ.UsingRequiredPPEComments));
            }

            void ProcessEmployeeRefFields()
            {
                var nullString = "null";
                var currentEmployeeName = Employee?.FullName ?? nullString;
                var originalEmployeeName = original.Employee?.FullName ?? nullString;
                if (currentEmployeeName != originalEmployeeName)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Employee", originalEmployeeName, currentEmployeeName));

                var currentManagerName = Manager?.FullName ?? nullString;
                var originalManagerName = original.Manager?.FullName ?? nullString;
                if (currentManagerName != originalManagerName)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Manager", originalManagerName, currentManagerName));

                var currentApprovedByName = ApprovedBy?.FullName ?? nullString;
                var originalApprovedByName = original.ApprovedBy?.FullName ?? nullString;
                if (currentApprovedByName != originalApprovedByName)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("ApprovedBy", originalApprovedByName, currentApprovedByName));

                var currentReportedByName = ReportedBy?.FullName ?? nullString;
                var originalReportedByName = original.ReportedBy?.FullName ?? nullString;
                if (currentReportedByName != originalReportedByName)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("ReportedBy", originalReportedByName, currentReportedByName));

            }

            void ProcessLocationFields()
            {
                if (Location?.Office != original.Location?.Office)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Location", original.Location?.Office ?? "null",
                            Location?.Office ?? "null"));
            }

            void ProcessPropertyFields()
            {
                if (DrugTest?.DrugTestType?.Code != original.DrugTest?.DrugTestType?.Code)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("DrugTestType", original.DrugTest?.DrugTestType?.Code ?? "null",
                            DrugTest?.DrugTestType?.Code ?? "null"));
                if (DrugTest?.DrugTestResult?.Code != original.DrugTest?.DrugTestResult?.Code)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("DrugTestResult",
                            original.DrugTest?.DrugTestResult?.Code ?? "null",
                            DrugTest?.DrugTestResult?.Code ?? "null"));

                if (SeverityTypeCode?.Code != original.SeverityTypeCode?.Code)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("SeverityTypeCode", original.SeverityTypeCode?.Code ?? "null",
                            SeverityTypeCode?.Code ?? "null"));

                if (NearMissTypeCode?.Code != original.NearMissTypeCode?.Code)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("NearMissTypeCode", original.NearMissTypeCode?.Code ?? "null",
                            NearMissTypeCode?.Code ?? "null"));

                if (FirstAidTreatment?.Code != original.FirstAidTreatment?.Code)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("FirstAidTreatment", original.FirstAidTreatment?.Code ?? "null",
                            FirstAidTreatment?.Code ?? "null"));

                if (MedicalTreatmentAdministeredType?.Code != original.MedicalTreatmentAdministeredType?.Code)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("MedicalTreatmentAdministeredType",
                            original.MedicalTreatmentAdministeredType?.Code ?? "null",
                            MedicalTreatmentAdministeredType?.Code ?? "null"));

                if (IncidentType?.Code != original.IncidentType?.Code)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IncidentType", original.IncidentType?.Code ?? "null",
                            IncidentType?.Code ?? "null"));

                if (IncidentStatus?.Code != original.IncidentStatus?.Code)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IncidentStatus", original.IncidentStatus?.Code ?? "null",
                            IncidentStatus?.Code ?? "null"));

                if (PhysicalLocationType?.Code != original.PhysicalLocationType?.Code)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("PhysicalLocationType", original.PhysicalLocationType?.Code ?? "null",
                            PhysicalLocationType?.Code ?? "null"));

                if (WorkBeingPerformedType?.Code != original.WorkBeingPerformedType?.Code)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("WorkBeingPerformedType", original.WorkBeingPerformedType?.Code ?? "null",
                            WorkBeingPerformedType?.Code ?? "null"));

            }

            void ProcessIntFields()
            {
                if (IncidentId != original.IncidentId)
                {
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IncidentId", original.IncidentId.ToString(),
                            IncidentId.ToString()));
                }
            }

            void ProcessBooleans()
            {
                if (Osha != original.Osha)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Osha", original.Osha.ToString(), Osha.ToString()));

                if (Riddor != original.Riddor)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Riddor", original.Riddor.ToString(), Riddor.ToString()));

                if (OneDayNotificationSent != original.OneDayNotificationSent)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("OneDayNotificationSent",
                            original.OneDayNotificationSent.ToString(), OneDayNotificationSent.ToString()));

                if (TenDayNotificationSent != original.TenDayNotificationSent)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("TenDayNotificationSent",
                            original.TenDayNotificationSent.ToString(), TenDayNotificationSent.ToString()));

                if (SubmittedFromWeb != original.SubmittedFromWeb)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("SubmittedFromWeb", original.SubmittedFromWeb.ToString(),
                            SubmittedFromWeb.ToString()));
            }

            void ProcessStringFields()
            {
                if (ImmediateCauses != original.ImmediateCauses)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("ImmediateCauses", original.ImmediateCauses, ImmediateCauses));

                if (SeverityTypeNote != original.SeverityTypeNote)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("SeverityTypeNote", original.SeverityTypeNote, SeverityTypeNote));

                if (WorkBeingPerformed != original.WorkBeingPerformed)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("WorkBeingPerformed", original.WorkBeingPerformed,
                            WorkBeingPerformed));

                if (CorrectiveAction != original.CorrectiveAction)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("CorrectiveAction", original.CorrectiveAction, CorrectiveAction));

                if (PreventativeAction != original.PreventativeAction)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("PreventativeAction", original.PreventativeAction,
                            PreventativeAction));

                if (Recommendation != original.Recommendation)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("Recommendation", original.Recommendation, Recommendation));

                if (ThirdParty != original.ThirdParty)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("ThirdParty", original.ThirdParty, ThirdParty));

                if (AdditionalComments != original.AdditionalComments)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("AdditionalComments", original.AdditionalComments,
                            AdditionalComments));

                if (ImmediateActionTakenNote != original.ImmediateActionTakenNote)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("ImmediateActionTakenNote", original.ImmediateActionTakenNote,
                            ImmediateActionTakenNote));

                if (PhysicalLocation != original.PhysicalLocation)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("PhysicalLocation", original.PhysicalLocation, PhysicalLocation));

                if (IncidentTypeNote != original.IncidentTypeNote)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IncidentTypeNote", original.IncidentTypeNote, IncidentTypeNote));

                if (FirstAidTreatmentAdministered != original.FirstAidTreatmentAdministered)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("FirstAidTreatmentAdministered",
                            original.FirstAidTreatmentAdministered,
                            FirstAidTreatmentAdministered));

            }

            void ProcessDateFields()
            {
                if ((ApprovedDate ?? DateTime.Parse("1/1/1900")) !=
                    (original.ApprovedDate ?? DateTime.Parse("1/1/1900")))
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("ApprovedDate",
                            original.ApprovedDate?.ToLongDateString() ?? "null",
                            ApprovedDate?.ToLongDateString() ?? "null"));

                if (DrugTest?.TestDate != original.DrugTest?.TestDate)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("EmployeeDrugTest.TestDate",
                            original.DrugTest?.TestDate?.ToLongDateString() ?? "null",
                            DrugTest?.TestDate?.ToLongDateString() ?? "null"));

                if (DrugTest?.ResultDate != original.DrugTest?.ResultDate)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("EmployeeDrugTest.ResultDate",
                            original.DrugTest?.ResultDate?.ToLongDateString() ?? "null",
                            DrugTest?.ResultDate?.ToLongDateString() ?? "null"));

                if (IncidentDate != original.IncidentDate)
                    changeHistory.ValueChanges.Add(
                        new ChangeHistory.ValueChange("IncidentDate",
                            original.IncidentDate?.ToLongDateString() ?? "null",
                            IncidentDate?.ToLongDateString() ?? "null"));
            }

            void ProcessEncryptedFields()
            {
                var currentMedicalNotes = enc.Decrypt<string>(MedicalNotes);
                var originalMedicalNotes = enc.Decrypt<string>(original.MedicalNotes);
                if (currentMedicalNotes != originalMedicalNotes)
                {
                    changeHistory.ValueChanges.Add(new ChangeHistory.ValueChange("MedicalNotes",
                        originalMedicalNotes,
                        currentMedicalNotes));
                }
            }

            void ProcessAffectedBodyParts()
            {
                foreach (var affectedBodyPart in AffectedBodyParts)
                {
                    if (original.AffectedBodyParts.All(x => x.Id != affectedBodyPart.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("AffectedBodyParts", "Added",
                            affectedBodyPart.Type.Code));
                    }

                    if (original.AffectedBodyParts.Any(x =>
                        x.Id == affectedBodyPart.Id && x.GetHashCode() != affectedBodyPart.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("AffectedBodyParts", "Modified",
                            affectedBodyPart.ToString()));
                    }

                }


                foreach (var affectedBodyPart in original.AffectedBodyParts)
                {
                    if (AffectedBodyParts.All(x => x.Type.Code != affectedBodyPart.Type.Code))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("AffectedBodyParts", "Removed",
                            affectedBodyPart.Type.Code));
                    }
                }
            }

            void ProcessBodyContacts()
            {
                foreach (var bodyContact in BodyContacts)
                {
                    if (original.BodyContacts.All(x => x.Id != bodyContact.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BodyContacts", "Added",
                            bodyContact.Type.Code));
                    }

                    if (original.BodyContacts.Any(x =>
                        x.Id == bodyContact.Id && x.GetHashCode() != bodyContact.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BodyContacts", "Modified",
                            bodyContact.ToString()));
                    }

                }

                foreach (var bodyContact in original.BodyContacts)
                {
                    if (BodyContacts.All(x => x.Id != bodyContact.Id))
                    {
                        changeHistory.ListChanges.Add(
                            new ChangeHistory.ListChange("BodyContacts", "Removed", bodyContact.Type.Code));
                    }
                }
            }

            void ProcessContactWith()
            {
                foreach (var contactWith in ContactWith)
                {
                    if (original.ContactWith.All(x => x.Type.Code != contactWith.Type.Code))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("ContactWith", "Added",
                            contactWith.Type.Code));
                    }

                    if (original.ContactWith.Any(x =>
                        x.Id == contactWith.Id && x.GetHashCode() != contactWith.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("ContactWith", "Modified",
                            contactWith.ToString()));
                    }

                }

                foreach (var contactWith in original.ContactWith)
                {
                    if (ContactWith.All(x => x.Id != contactWith.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("ContactWith", "Removed",
                            contactWith.Type.Code));
                    }
                }
            }

            void ProcessEquipmentInvolved()
            {
                foreach (var equipmentInvolved in EquipmentInvolved)
                {
                    if (original.EquipmentInvolved.All(x => x.Id != equipmentInvolved.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EquipmentInvolved", "Added",
                            equipmentInvolved.EquipmentCode.Code));
                    }

                    if (original.EquipmentInvolved.Any(x =>
                        x.Id == equipmentInvolved.Id && x.GetHashCode() != equipmentInvolved.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EquipmentInvolved", "Modified",
                            equipmentInvolved.ToString()));
                    }

                }

                foreach (var equipmentInvolved in original.EquipmentInvolved)
                {
                    if (EquipmentInvolved.All(x => x.Id != equipmentInvolved.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("EquipmentInvolved", "Removed",
                            equipmentInvolved.EquipmentCode.Code));
                    }
                }
            }

            void ProcessIncidentImages()
            {
                foreach (var incidentImage in IncidentImages)
                {
                    if (original.IncidentImages.All(x => x.Id != incidentImage.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("IncidentImages", "Added",
                            incidentImage.FileName));
                    }
                }

                foreach (var incidentImage in original.IncidentImages)
                {
                    if (IncidentImages.All(x => x.Id != incidentImage.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("IncidentImages", "Removed",
                            incidentImage.FileName));
                    }
                }
            }

            void ProcessInvestigations()
            {
                foreach (var investigation in Investigations)
                {
                    if (original.Investigations.All(x =>
                        x.Id != investigation.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Investigations", "Added",
                            investigation.InvestigatorName + ":" + investigation.InvestigatedDate?.ToLongDateString() ??
                            "null"));
                    }

                    if (original.Investigations.Any(x =>
                        x.Id == investigation.Id && x.GetHashCode() != investigation.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Investigations", "Modified",
                            investigation.ToString()));
                    }


                }

                foreach (var investigation in original.Investigations)
                {
                    if (Investigations.All(x =>
                        x.Id != investigation.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Investigations", "Removed",
                            investigation.InvestigatorName + ":" + investigation.InvestigatedDate?.ToLongDateString() ??
                            "null"));
                    }
                }
            }

            void ProcessJobFactors()
            {
                foreach (var jobFactor in JobFactors)
                {
                    if (original.JobFactors.All(x => x.Id != jobFactor.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("JobFactors", "Added",
                            jobFactor.Type.Code));
                    }

                    if (original.JobFactors.Any(x =>
                        x.Id == jobFactor.Id && x.GetHashCode() != jobFactor.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("JobFactors", "Modified",
                            jobFactor.ToString()));
                    }

                }

                foreach (var jobFactor in original.JobFactors)
                {
                    if (JobFactors.All(x => x.Type.Code != jobFactor.Type.Code))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("JobFactors", "Removed",
                            jobFactor.Type.Code));
                    }
                }
            }

            void ProcessLackOfControls()
            {
                foreach (var lackOfControl in LackOfControls)
                {
                    if (original.LackOfControls.All(x => x.Id != lackOfControl.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("LackOfControls", "Added",
                            lackOfControl.Type.Code));
                    }

                    if (original.LackOfControls.Any(x =>
                        x.Id == lackOfControl.Id && x.GetHashCode() != lackOfControl.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("LackOfControls", "Modified",
                            lackOfControl.ToString()));
                    }

                }

                foreach (var lackOfControl in original.LackOfControls)
                {
                    if (LackOfControls.All(x => x.Type.Code != lackOfControl.Type.Code))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("LackOfControls", "Removed",
                            lackOfControl.Type.Code));
                    }
                }
            }

            void ProcessMedicalLeaves()
            {
                foreach (var medicalLeave in MedicalLeaves)
                {
                    if (original.MedicalLeaves.All(x => x.Id != medicalLeave.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("MedicalLeaves", "Added",
                            medicalLeave.ToString()));
                    }

                    if (original.MedicalLeaves.Any(x =>
                        x.Id == medicalLeave.Id && x.GetHashCode() != medicalLeave.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("MedicalLeaves", "Modified",
                            medicalLeave.ToString()));
                    }
                }

                foreach (var medicalLeave in original.MedicalLeaves)
                {
                    if (MedicalLeaves.All(x => x.Id != medicalLeave.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("MedicalLeaves", "Removed",
                            medicalLeave.ToString()));
                    }
                }
            }

            void ProcessMedicalTreatments()
            {
                foreach (var medicalTreatment in MedicalTreatments)
                {
                    if (original.MedicalTreatments.All(x => x.Id != medicalTreatment.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("MedicalTreatments", "Added",
                            medicalTreatment.ToString()));
                    }

                    if (original.MedicalTreatments.Any(x =>
                        x.Id == medicalTreatment.Id && x.GetHashCode() != medicalTreatment.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("MedicalTreatments", "Modified",
                            medicalTreatment.ToString()));
                    }
                }

                foreach (var medicalTreatment in original.MedicalTreatments)
                {
                    if (MedicalTreatments.All(x => x.Id != medicalTreatment.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("MedicalTreatments", "Removed",
                            medicalTreatment.ToString()));
                    }
                }

            }

            void ProcessMedicalWorkStatus()
            {
                foreach (var workStatus in MedicalWorkStatus)
                {
                    if (original.MedicalWorkStatus.All(x => x.Id != workStatus.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("MedicalWorkStatus", "Added",
                            workStatus.ToString()));
                    }

                    if (original.MedicalWorkStatus.Any(x =>
                        x.Id == workStatus.Id && x.GetHashCode() != workStatus.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("MedicalWorkStatus", "Modified",
                            workStatus.ToString()));
                    }
                }

                foreach (var workStatus in original.MedicalWorkStatus)
                {
                    if (MedicalWorkStatus.All(x => x.Id != workStatus.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("MedicalWorkStatus", "Removed",
                            workStatus.ToString()));
                    }
                }


            }

            void ProcessRootCauses()
            {
                foreach (var rootCause in RootCauses)
                {
                    if (original.RootCauses.All(x => x.Id != rootCause.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("RootCauses", "Added",
                            rootCause.ToString()));
                    }

                    if (original.RootCauses.Any(x =>
                        x.Id == rootCause.Id && x.GetHashCode() != rootCause.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("RootCauses", "Modified",
                            rootCause.ToString()));
                    }
                }

                foreach (var rootCause in original.RootCauses)
                {
                    if (RootCauses.All(x => x.Id != rootCause.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("RootCauses", "Removed",
                            rootCause.ToString()));
                    }
                }


            }

            void ProcessPersonalFactors()
            {
                foreach (var personalFactor in PersonalFactors)
                {
                    if (original.PersonalFactors.All(x => x.Id != personalFactor.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PersonalFactors", "Added",
                            personalFactor.ToString()));
                    }

                    if (original.PersonalFactors.Any(x =>
                        x.Id == personalFactor.Id && x.GetHashCode() != personalFactor.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PersonalFactors", "Modified",
                            personalFactor.ToString()));
                    }
                }

                foreach (var personalFactor in original.PersonalFactors)
                {
                    if (PersonalFactors.All(x => x.Id != personalFactor.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("PersonalFactors", "Removed",
                            personalFactor.ToString()));
                    }
                }


            }

            void ProcessNatureOfInjuries()
            {
                foreach (var natureOfInjury in NatureOfInjuries)
                {
                    if (original.NatureOfInjuries.All(x => x.Id != natureOfInjury.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("NatureOfInjuries", "Added",
                            natureOfInjury.ToString()));
                    }

                    if (original.NatureOfInjuries.Any(x =>
                        x.Id == natureOfInjury.Id && x.GetHashCode() != natureOfInjury.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("NatureOfInjuries", "Modified",
                            natureOfInjury.ToString()));
                    }
                }

                foreach (var natureOfInjury in original.NatureOfInjuries)
                {
                    if (NatureOfInjuries.All(x => x.Id != natureOfInjury.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("NatureOfInjuries", "Removed",
                            natureOfInjury.ToString()));
                    }
                }


            }

            void ProcessWitnesses()
            {
                foreach (var witness in Witnesses)
                {
                    if (original.Witnesses.All(x => x.Id != witness.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Witnesses", "Added",
                            witness.ToString()));
                    }

                    if (original.Witnesses.Any(x =>
                        x.Id == witness.Id && x.GetHashCode() != witness.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Witnesses", "Modified",
                            witness.ToString()));
                    }
                }

                foreach (var witness in original.Witnesses)
                {
                    if (Witnesses.All(x => x.Id != witness.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("Witnesses", "Removed",
                            witness.ToString()));
                    }
                }


            }

            void ProcessBbsPrecautions()
            {
                foreach (var bbsPrecaution in BbsPrecautions)
                {
                    if (original.BbsPrecautions.All(x => x.Id != bbsPrecaution.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BbsPrecautions", "Added",
                            bbsPrecaution.ToString()));
                    }

                    if (original.BbsPrecautions.Any(x =>
                        x.Id == bbsPrecaution.Id && x.GetHashCode() != bbsPrecaution.GetHashCode()))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BbsPrecautions", "Modified",
                            bbsPrecaution.ToString()));
                    }
                }

                foreach (var bbsPrecaution in original.BbsPrecautions)
                {
                    if (BbsPrecautions.All(x => x.Id != bbsPrecaution.Id))
                    {
                        changeHistory.ListChanges.Add(new ChangeHistory.ListChange("BbsPrecautions", "Removed",
                            bbsPrecaution.ToString()));
                    }
                }


            }

            void SaveChangeHistory()
            {
                if ((changeHistory.ValueChanges.Count == 0) && (changeHistory.ListChanges.Count == 0))
                {
                    changeHistory = null;
                    return;

                }

                ChangeHistory.Add(changeHistory);
            }

        }

        public static MongoRawQueryHelper<EmployeeIncident> Helper = new MongoRawQueryHelper<EmployeeIncident>();



    }
}
