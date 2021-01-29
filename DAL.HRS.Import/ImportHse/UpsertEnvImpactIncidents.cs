using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.SqlServer.Model;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using HrsContext = DAL.HRS.SqlServer.Model.HrsContext;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee ;


namespace DAL.HRS.Import.ImportHse
{
    [TestFixture()]
    public class UpsertEnvImpactIncidents
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
            var rep = new RepositoryBase<EnvImpactIncident>();
            var repEmployee = new RepositoryBase<Employee>();
            using (HrsContext context = new HrsContext())
            {
                foreach (var incident in context.EnvironmentalImpactIncident.ToList())
                {
                    var newRow = rep.AsQueryable().FirstOrDefault(x => x.OldHrsId == incident.OID) ??
                                 new EnvImpactIncident()
                                 {
                                     OldHrsId = incident.OID
                                 };

                    newRow.ApprovedDate = incident.ApprovedByDate;
                    newRow.DescriptionOfEvent = incident.DescriptionOfEvent;
                    newRow.CorrectiveAction = incident.CorrectiveAction;
                    newRow.OldHrsId = incident.OID;
                    newRow.EmployeeInvoked = incident.EmployeeInvolved ?? false;
                    newRow.IncidentDate = incident.IncidentDate;
                    newRow.Location = _helperMethods.GetLocationForHrsLocation(incident.Location1.Name);
                    newRow.OneDayNotificationSent = incident.OneDayNotificationSent ?? false;
                    newRow.TenDayNotificationSent = incident.TenDayNotificationSent ?? false;
                    newRow.Osha = incident.OSHA ?? false;
                    newRow.Recommendation = incident.Recommendation;
                    newRow.Riddor = incident.RIDDOR ?? false;
                    newRow.SeverityType = PropertyBuilder.New("EnvironmentImpactSeverityType",
                        "Severity Type of Environment Impact", incident.EnvironmentalImpactSeverityType.Name, "");
                    newRow.SeverityTypeNotes = incident.SeverityTypeNotes;
                    newRow.WorkBeingPerformed = incident.WorkBeingPerformed;
                    newRow.Status = PropertyBuilder.New("EnvironmentImpactStatus",
                        "Status of Environment Impact", incident.Status1.StatusName, "");
                    GetInvestigators(newRow, incident);
                    GetWitnesses(newRow, incident);
                    GetEmployee(incident, repEmployee, newRow);
                    GetManager(incident, repEmployee, newRow);
                    GetApprovedBy(incident, repEmployee, newRow);
                    GetRecordedBy(incident, repEmployee, newRow);
                    GetJobFactors(incident, newRow);
                    GetImpactEvents(incident, newRow);
                    GetLackOfControl(newRow, incident);
                    GetNatureOfEvents(newRow, incident);
                    GetPersonalFactors(newRow, incident);
                    GetRootCauses(newRow, incident);
                    GetImpactTypes(newRow, incident);
                    rep.Upsert(newRow);

                }
            }
        }

        private static void GetImpactTypes(EnvImpactIncident newRow, EnvironmentalImpactIncident incident)
        {
            newRow.ImpactTypes.Clear();
            foreach (var type in incident.EnvironmentalImpactType)
            {
                newRow.ImpactTypes.Add(new EnvImpactImpactType()
                {
                    Comment = type.Comment,
                    Type = PropertyBuilder.New("EnvironmentImpactImpactType",
                        "Impact Type of Environment Impact", type.EnvironmentalImpactTypeType1.Name, "")
                });
            }
        }

        private static void GetRootCauses(EnvImpactIncident newRow, EnvironmentalImpactIncident incident)
        {
            newRow.RootCauses.Clear();
            foreach (var root in incident.EnvironmentalImpactRootCause)
            {
                newRow.RootCauses.Add(new EnvImpactRootCause()
                {
                    Comment = root.Comment,
                    Type = PropertyBuilder.New("EnvironmentImpactRootCauseType",
                        "Root Cause Type", root.EnvironmentalImpactRootCauseType1.Name, "")
                });
            }
        }

        private static void GetPersonalFactors(EnvImpactIncident newRow, EnvironmentalImpactIncident incident)
        {
            newRow.PersonalFactors.Clear();
            foreach (var factor in incident.EnvironmentalImpactPersonalFactor)
            {
                newRow.PersonalFactors.Add(new EnvImpactPersonalFactor()
                {
                    Comment = factor.Comment,
                    Type = PropertyBuilder.New("EnvironmentImpactPersonalFactorType",
                        "Personal Factor Type", factor.EnvironmentalImpactPersonalFactorType1.Name, "")
                });
            }
        }

        private static void GetNatureOfEvents(EnvImpactIncident newRow, EnvironmentalImpactIncident incident)
        {
            newRow.NatureOfEvent.Clear();
            foreach (var nature in incident.EnvironmentalImpactNatureOfEvent)
            {
                newRow.NatureOfEvent.Add(new EnvImpactNatureOfEvent()
                {
                    Comment = nature.Comment,
                    Type = PropertyBuilder.New("EnvironmentImpactNatureOfEventType",
                        "Nature of Event Type", nature.EnvironmentalImpactNatureOfEventType.Name, "")
                });
            }
        }

        private static void GetLackOfControl(EnvImpactIncident newRow, EnvironmentalImpactIncident incident)
        {
            newRow.LackOfControl.Clear();
            foreach (var lackOfControl in incident.EnvironmentalImpactLackOfControl)
            {
                newRow.LackOfControl.Add(new EnvImpactLackOfControl()
                {
                    Comment = lackOfControl.Comment,
                    Type = PropertyBuilder.New("EnvironmentImpactLackOfControlType",
                        "Lack of Control Type", lackOfControl.EnvironmentalImpactLackOfControlType1.Name, "")
                });
            }
        }

        private static void GetImpactEvents(EnvironmentalImpactIncident incident, EnvImpactIncident newRow)
        {
            newRow.ImpactEvents.Clear();
            foreach (var anEvent in incident.EnvironmentalImpactEvent)
            {
                newRow.ImpactEvents.Add(new EnvImpactEvent()
                {
                    Comment = anEvent.Comment,
                    
                    Type = PropertyBuilder.New("EnvironmentImpactEventType",
                        "Type of Environment Impact Event", anEvent.EnvironmentalImpactEventType1.Name, "")
                });
            }
        }

        private static void GetJobFactors(EnvironmentalImpactIncident incident, EnvImpactIncident newRow)
        {
            newRow.JobFactors.Clear();
            foreach (var impactJobFactor in incident.EnvironmentalImpactJobFactor)
            {
                newRow.JobFactors.Add(new EnvImpactJobFactor()
                {
                    Comment = impactJobFactor.Comment,
                    Type = PropertyBuilder.New("Environmental Impact Type",
                        "Environmental Impact Type",
                        impactJobFactor.EnvironmentalImpactJobFactorType1.Name, "")
                });
            }
        }

        private static void GetManager(EnvironmentalImpactIncident incident, RepositoryBase<Employee> repEmployee,
            EnvImpactIncident newRow)
        {
            if (incident.Manager != 0)
            {
                var mgr = repEmployee.AsQueryable().FirstOrDefault(x => x.OldHrsId == incident.Manager);
                if (mgr != null)
                {
                    newRow.Manager = mgr.AsEmployeeRef();
                }
            }
        }

        private static void GetEmployee(EnvironmentalImpactIncident incident, RepositoryBase<Employee> repEmployee,
            EnvImpactIncident newRow)
        {
            if (incident.Employee != 0)
            {
                var emp = repEmployee.AsQueryable().FirstOrDefault(x => x.OldHrsId == incident.Employee);
                if (emp != null)
                {
                    newRow.Employee = emp.AsEmployeeRef();
                }
            }
        }

        private static void GetApprovedBy(EnvironmentalImpactIncident incident, RepositoryBase<Employee> repEmployee,
            EnvImpactIncident newRow)
        {
            if (incident.ApprovedBy != 0)
            {
                var approvedBy = repEmployee.AsQueryable().FirstOrDefault(x => x.OldHrsId == incident.ApprovedBy);
                if (approvedBy != null)
                {
                    newRow.ApprovedBy = approvedBy.AsEmployeeRef();
                }
            }
        }

        private static void GetRecordedBy(EnvironmentalImpactIncident incident, RepositoryBase<Employee> repEmployee,
            EnvImpactIncident newRow)
        {
            if (incident.RecordedBy != 0)
            {
                var recordedBy = repEmployee.AsQueryable().FirstOrDefault(x => x.OldHrsId == incident.RecordedBy);
                if (recordedBy != null)
                {
                    newRow.RecordedBy = recordedBy.AsEmployeeRef();
                }
            }
        }

        private static void GetInvestigators(EnvImpactIncident newRow, EnvironmentalImpactIncident incident)
        {
            newRow.Investigators.Clear();
            var investigators = new List<EnvImpactIncidentInvestigator>();
            foreach (var investigatedBy in incident.EnvironmentalImpactInvestigatedBy.ToList())
            {
                investigators.Add(new EnvImpactIncidentInvestigator()
                {
                    InvestigatedDate = investigatedBy.InvestigatedDate,
                    InvestigatorName = investigatedBy.InvestigatorName
                });
            }

            newRow.Investigators = investigators;
        }

        private static void GetWitnesses(EnvImpactIncident newRow, EnvironmentalImpactIncident incident)
        {
            newRow.Witnesses.Clear();
            var witnesses = new List<EnvImpactIncidentWitness>();
            foreach (var witness in incident.EnvironmentalImpactWitness.ToList())
            {
                witnesses.Add(new EnvImpactIncidentWitness()
                {
                    WitnessDate = witness.Date,
                    WitnessName = witness.Name
                });
            }

            newRow.Witnesses = witnesses;
        }

    }
}
