using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.EMail;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using Environment = DAL.Vulcan.Mongo.Base.Context.Environment;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperEmployeeIncidents : HelperBase, IHelperEmployeeIncidents
    {

        public List<EmployeeIncidentModel> GetAllEmployeeIncidents()
        {
            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            var filter = queryHelper.FilterBuilder.Empty;
            var project = queryHelper.ProjectionBuilder.Expression(x => new EmployeeIncidentModel(x));
            return queryHelper.FindWithProjection(filter, project).ToList();
        }

        public List<EmployeeIncidentGridModel> GetEmployeeIncidentGridRows()
        {
            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            var filter = queryHelper.FilterBuilder.Empty;
            var project = queryHelper.ProjectionBuilder.Expression(x => 
                new EmployeeIncidentGridModel(
                    x.Id, x.IncidentId, x.NearMissTypeCode, x.IncidentType, x.Employee, x.IncidentDate, x.Location, x.IncidentStatus, 
                    x.SubmittedFromWeb, x.Osha, x.Riddor, x.OneDayNotificationSent, x.TenDayNotificationSent));
            return queryHelper.FindWithProjection(filter, project).ToList();
        }


        public EmployeeIncidentModel GetNewEmployeeIncidentModel()
        {
            return new EmployeeIncidentModel(new EmployeeIncident());
        }

        public EmployeeIncidentModel GetEmployeeIncidentModel(string id)
        {
            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            var incident = queryHelper.FindById(id);
            if (incident == null) throw new Exception("Incident not found");
            return new EmployeeIncidentModel(incident);
        }


        //public DrugTestModel GetDrugTestModel(string employeeIncidentId, string drugTestId)
        //{

        //    return new DrugTestModel(new EmployeeDrugTest());
        //    //var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();

        //    //var row = queryHelper.FindById(employeeIncidentId);
        //    //if (row == null) return new DrugTestModel();

        //    //var row.EmployeeDrugTest

        //    //return new DrugTestModel(row);
        //}

        //public DrugTestModel GetNewDrugTestModel()
        //{
        //    return new DrugTestModel(new EmployeeDrugTest());
        //}



        public void RemoveEmployeeIncident(string id)
        {
            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            queryHelper.DeleteOne(id);
        }


        public EmployeeIncidentModel SaveEmployeeIncident(EmployeeIncidentModel model)
        {
            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            EmployeeIncident original = null;
            var i = queryHelper.FindById(model.Id);
            if (i != null)
            {
                original = (EmployeeIncident)i.Clone();
            }
            else
            {
                i = new EmployeeIncident() { Id = ObjectId.Parse(model.Id)};
                Console.WriteLine(i.IncidentId);
            }

            //Created filter for VALID entries(to not allow nulls)
            var validRootCauses = model.RootCauses.Where(r => r.Type != null).ToList();
            var validPersonalFactors = model.PersonalFactors.Where(p => p.Type != null).ToList();
            var validNatureOfInjuries = model.NatureOfInjuries.Where(n => n.Type != null).ToList();
            var validBodyContacts = model.BodyContacts.Where(b => b.Type != null).ToList();
            var validContactWiths = model.ContactWith.Where(c => c.Type != null).ToList();
            var validEquipmentInvolved = model.EquipmentInvolved.Where(e => e.EquipmentType != null).ToList();
            var validBodyParts = model.AffectedBodyParts.Where(b => b.Type != null).ToList();


            i.SubmittedFromWeb = model.SubmittedFromWeb;
            i.OneDayNotificationSent = model.OneDayNotificationSent;
            i.TenDayNotificationSent = model.TenDayNotificationSent;

            i.IncidentStatus = model.IncidentStatus;
            i.ThirdParty = model.ThirdParty;
            i.IncidentDate = model.IncidentDate;
            i.IncidentType = model.IncidentType;

            i.Employee = model.Employee;
            i.Manager = model.Manager;
            i.ReportedBy = model.ReportedBy;
            i.Location = model.Location;
            i.Osha = model.Osha;
            i.Riddor = model.Riddor;
            i.NearMissTypeCode = model.NearMissTypeCode;
            i.AdditionalComments = model.AdditionalComments;
            i.ImmediateActionTakenNote = model.ImmediateActionTakenNote;
            i.ImmediateCauses = model.ImmediateCauses;
            i.PhysicalLocation = model.PhysicalLocation;
            i.PhysicalLocationType = model.PhysicalLocationType;
            i.IncidentTypeNote = model.IncidentTypeNote;
            i.SeverityTypeCode = model.SeverityTypeCode;
            i.SeverityTypeNote = model.SeverityTypeNote;
            i.WorkBeingPerformed = model.WorkBeingPerformed;
            i.WorkBeingPerformedType = model.WorkBeingPerformedType;
            i.CorrectiveAction = model.CorrectiveAction;
            i.PreventativeAction = model.PreventativeAction;
            i.Recommendation = model.Recommendation;
            i.ApprovedBy = model.ApprovedBy;
            i.ApprovedDate = model.ApprovedDate;
            i.DrugTest = model.DrugTest;
            i.AffectedBodyParts = validBodyParts;
            i.BodyContacts = validBodyContacts;
            i.ContactWith = validContactWiths;
            i.EquipmentInvolved = validEquipmentInvolved;
            i.FirstAidTreatment = model.FirstAidTreatment;
            i.FirstAidTreatmentAdministered = model.FirstAidTreatmentAdministered;
            i.IIQ = model.IIQ;
            i.Investigations = model.Investigations;
            i.JobFactors = model.JobFactors;
            i.LackOfControls = model.LackOfControls;
            i.MedicalLeaves = MedicalLeaveModel.ConvertBackToBase(model.MedicalLeaves);
            i.MedicalTreatments = MedicalTreatmentModel.ConvertBackToBase(model.MedicalTreatments);
            i.MedicalWorkStatus = MedicalWorkStatusModel.ConvertBackToBase(model.MedicalWorkStatus); 
            i.MedicalNotes = _encryption.Encrypt(model.MedicalNotes);
            i.MedicalTreatmentAdministeredType = model.MedicalTreatmentAdministeredType;

            i.NatureOfInjuries = validNatureOfInjuries;
            i.BbsPrecautions = model.BbsPrecautions;
            i.Witnesses = model.Witnesses;
            i.RootCauses = validRootCauses;
            i.PersonalFactors = validPersonalFactors;

            if (model.HrsUser != null)
            {
                i.SaveChangeHistory(original, model.HrsUser);
            }


            queryHelper.Upsert(i);

            var returnModel = new EmployeeIncidentModel(i);

            if (model.SendEmail)
            {
                    var emailBuilder = new EMailBuilder();
                    emailBuilder.Subject =
                        $"HSE {returnModel.NearMissTypeCode?.Description} #{returnModel.IncidentId}, {returnModel.NearMissTypeCode?.Code ?? "{Unknown Near Miss Code}"}, {returnModel.IncidentDate?.Year}-{returnModel.IncidentDate?.Month}-{returnModel.IncidentDate?.Day}";
                    emailBuilder.EMailFromAddress = "noreply@howcogroup.com";
                    if (EnvironmentSettings.CurrentEnvironment == Environment.Development)
                    {
                        emailBuilder.Recipients = new List<string>()
                        {
                            //"Isidro.Gallegos@howcogroup.com",
                            //"Marc.Pike@howcogroup.com",
                            "Shannen.Reese@howcogroup.com"
                        };

                    }
                    else if (EnvironmentSettings.CurrentEnvironment == Environment.QualityControl)
                    {
                        emailBuilder.Recipients = new List<string>()
                        {
                            "Isidro.Gallegos@howcogroup.com",
                            "Marc.Pike@howcogroup.com",
                            "Shannen.Reese@howcogroup.com",
                            "Denise.Walker@howcogroup.com"
                        };
                    }
                    else
                    {
                        emailBuilder.Recipients = new List<string>()
                        {
                            "HSE-IncidentNotification@howcogroup.com"
                        };
                    }
                    StringBuilder sb = new StringBuilder();

                    if (model.IncidentUrl != string.Empty)
                    {
                        sb.AppendLine("Click on the following link to view the Incident:");
                        sb.Append(model.IncidentUrl);
                    }

                    sb.AppendLine();
                    sb.AppendLine(
                        $"REPORTED BY: {returnModel.ReportedBy?.FirstName} {returnModel.ReportedBy?.LastName} - {returnModel.ReportedBy?.PayrollId}");
                    sb.AppendLine(
                        $"TYPE: {returnModel.NearMissTypeCode?.Description}");
                    sb.AppendLine(
                        $"TIME OF EVENT: {returnModel.IncidentDate?.ToShortDateString()} {returnModel.IncidentDate?.ToShortTimeString()}");
                    sb.AppendLine(
                        $"LOCATION: {returnModel.Location?.Office}");
                    sb.AppendLine(
                        $"SEVERITY: {returnModel.SeverityTypeCode.Code}");
                    sb.AppendLine($"DESCRIPTION OF {returnModel.NearMissTypeCode?.Description.ToUpper()}: {returnModel.AdditionalComments}");

                    emailBuilder.Body = sb.ToString();
                    emailBuilder.Send();
            }

            return returnModel;

        }


    }
}
