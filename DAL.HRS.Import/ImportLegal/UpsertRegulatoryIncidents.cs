using DAL.HRS.Import.ImportHse;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Legal;
using DAL.HRS.SqlServer;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using HrsContext = DAL.HRS.SqlServer.Model.HrsContext;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Common.DocClass;

namespace DAL.HRS.Import.ImportLegal
{
    [TestFixture()]
    class UpsertRegulatoryIncidents
    {
        private HseHelperMethods _helperMethods = new HseHelperMethods();

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsDevelopment();
        }

        [Test]
        public void Execute()
        {
            var rep = new RepositoryBase<RegulatoryIncident>();
            var repEmployee = new RepositoryBase<Employee>();
            var incidents = rep.AsQueryable().ToList();
            var encryption = Encryption.NewEncryption;

            var onRow = 0;
            var newRows = 0;

            using (SqlServer.Model.HrsContext context = new HrsContext())
            {
                foreach (var incident in context.RegulatoryIncident.AsNoTracking().Where(x => x.GCRecord == null).ToList())
                {
                    onRow++;
                    var newRow = incidents.FirstOrDefault(x => x.OldHrsId == incident.OID);

                    if (newRow == null)
                    {
                        newRows++;
                        newRow = new RegulatoryIncident()
                        {
                            OldHrsId = incident.OID
                        };
                    }

                    if (incident.Complainant != null)
                    {

                        var employeeRef = repEmployee.AsQueryable()
                            .FirstOrDefault(x => x.OldHrsId == (incident.Complainant))?.AsEmployeeRef();
                        if (employeeRef != null)
                        {
                            newRow.Complainant = employeeRef;
                        }
                    }


                    newRow.IncidentId = incident.OID;
                    newRow.ComplainantThirdParty = incident.ComplainantThirdParty;

                    if (incident.ReportedTo != null)
                    {

                        var employeeRef = repEmployee.AsQueryable()
                            .FirstOrDefault(x => x.OldHrsId == (incident.ReportedTo))?.AsEmployeeRef();
                        if (employeeRef != null)
                        {
                            newRow.ReportedTo = employeeRef;
                        }
                    }

                    newRow.ReportedToThirdParty = incident.ReportedToThirdParty;

                    if (incident.Accused != null)
                    {

                        var employeeRef = repEmployee.AsQueryable()
                            .FirstOrDefault(x => x.OldHrsId == (incident.Accused))?.AsEmployeeRef();
                        if (employeeRef != null)
                        {
                            newRow.Accused = employeeRef;
                        }
                    }

                    newRow.AccusedThirdParty = incident.AccusedThirdParty;
                    newRow.IncidentDate = incident.IncidentDate;
                    newRow.ReportedOnDate = incident.ReportedOnDate;

                    newRow.Status = PropertyBuilder.New("RegulatoryIncidentStatus", "Type of regulatory incident status",
                       incident.RegulatoryIncidentStatus?.Name ?? "<unspecified>", "");

                    if (incident.Location1 != null)
                    {
                        newRow.Location = _helperMethods.GetLocationForHrsLocation(incident.Location1.Name);
                    }

                    newRow.IncidentType = PropertyBuilder.New("RegulatoryIncidentType", "Type of regulatory incident",
                      incident.RegulatoryIncidentType?.Name ?? "<unspecified>", "");

                    newRow.RegulatoryType = PropertyBuilder.New("RegulatoryType", "Type of regulatory",
                     incident.RegulatoryType1?.Name ?? "<unspecified>", "");

                    newRow.IncidentDescription = incident.IncidentDescription;
                    newRow.ImmediateAction = incident.ImmediateActionTaken;
                    newRow.CorrectiveAction = incident.CorrectiveAction;
                    newRow.PreventativeAction = incident.PreventativeAction;
                    newRow.Recommendation = incident.Recommendation;
                    newRow.Conclusion = incident.Conclusion;
                    newRow.FollowUp = incident.FollowUp;

                    newRow.ActionsTakenAccuserNotifiedDate = incident.ActionsTakenAccuserNotifiedDate;
                    newRow.ActionsTakenAccusedNotifiedDate = incident.ActionsTakenAccusedNotifiedDate;

                    if (incident.ApprovedBy != null)
                    {

                        var employeeRef = repEmployee.AsQueryable()
                            .FirstOrDefault(x => x.OldHrsId == (incident.ApprovedBy))?.AsEmployeeRef();
                        if (employeeRef != null)
                        {
                            newRow.ApprovedBy = employeeRef;
                        }
                    }

                    newRow.ApprovedByDate = incident.ApprovedByDate;

                    newRow.IIQCompanyPolicyViolated = incident.IIQ_CompanyPolicyViolated ?? false;
                    newRow.IIQCompanyPolicyViolatedComment = incident.IIQ_CompanyPolicyViolated_Comment;

                    newRow.IIQDisciplinaryActionTaken = incident.IIQ_DisciplinaryActionTaken ?? false;
                    newRow.IIQDisciplinaryActionTakenComment = incident.IIQ_DisciplinaryActionTaken_Comment;

                    newRow.IIQAccusedWasInvolved = incident.IIQ_AccusedWasInvolved ?? false;
                    newRow.IIQAccusedWasInvolvedComment = incident.IIQ_AccusedWasInvolved_Comment;

                    newRow.IIQAccusedNamedInPriorReportsPastYear = incident.IIQ_AccusedNamedInPriorReportsPastYear ?? false;
                    newRow.IIQAccusedNamedInPriorReportsPastYearComment = incident.IIQ_AccusedNamedInPriorReportsPastYear_Comment;

                    newRow.IIQBenefitsAwarded = incident.IIQ_BenefitsAwarded ?? false;
                    newRow.IIQBenefitsAwardedComment = incident.IIQ_BenefitsAwarded_Comment;

                    newRow.IIQEventWitness = incident.IIQ_EventWitness ?? false;
                    newRow.IIQEventWitnessComment = incident.IIQ_EventWitness_Comment;


                    newRow.InvestigatedBy = new List<RegulatoryIncidentInvestigator>();

                    foreach (var investigatorId in incident.RegulatoryIncidentInvestigator)
                    {
                        var investId = investigatorId.Investigator;
                        var employeeRef = repEmployee.AsQueryable().FirstOrDefault(x => x.OldHrsId == investId)?.AsEmployeeRef();
                        if (employeeRef != null)
                        {
                            var investigatedBy = incident.RegulatoryIncidentInvestigator.First(x => x.Investigator == investId);

                            newRow.InvestigatedBy.Add(new RegulatoryIncidentInvestigator
                            {
                                InvestigationDate = investigatedBy.InvestigationDate,
                                Investigator = employeeRef
                            });
                          
                        }
                    }


                    newRow.Interviews = new List<RegulatoryIncidentWitness>();

                    foreach (var witnessId in incident.RegulatoryIncidentWitness)
                    {
                        var witnessOid = witnessId.Employee1.OID;
                        var employeeRef = repEmployee.AsQueryable().FirstOrDefault(x => x.OldHrsId == witnessOid)?.AsEmployeeRef();
                        if (employeeRef != null)
                        {
                            var witnessedBy = incident.RegulatoryIncidentWitness.First(x => x.Employee1.OID == witnessOid);

                            newRow.Interviews.Add(new RegulatoryIncidentWitness
                            {
                                StatementDate = witnessedBy.StatementDate,
                                Statement = witnessedBy.Statement,
                                ThirdParty = witnessedBy.ThirdParty,
                                Employee = employeeRef
                            });

                        }
                    }
                    

                    rep.Upsert(newRow);
                }



            }
        }
    }
}
