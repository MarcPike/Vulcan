using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Linq;
using HrsContext = DAL.HRS.SqlServer.Model.HrsContext;

namespace DAL.HRS.Import.ImportHse
{
    [TestFixture()]
    public class UpsertHseObservations
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
            var rep = new RepositoryBase<HseObservation>();
            var repEmployee = new RepositoryBase<Employee>();
            var observations = rep.AsQueryable().ToList();
            var defaultEmployeeRef = repEmployee.AsQueryable()
                .First(x => x.PreferredName == "Denise" && x.LastName == "Walker").AsEmployeeRef();
            using (HrsContext context = new HrsContext())
            {
                foreach (var hseObservation in context.HSE_Observation.ToList())
                {
                    var newRow = observations.FirstOrDefault(x => x.OldHrsId == hseObservation.OID) ??
                                 new HseObservation()
                                 {
                                     OldHrsId = hseObservation.OID
                                 };
                    newRow.Cost = hseObservation.Cost ?? 0;
                    newRow.DateOf = hseObservation.DateTime;
                    newRow.Description = hseObservation.Description;
                    if (hseObservation.Employee != null)
                    {

                        var employeeRef = repEmployee.AsQueryable()
                            .FirstOrDefault(x => x.OldHrsId == (hseObservation.Employee))?.AsEmployeeRef();
                        if (employeeRef == null) continue;

                        newRow.Employee = employeeRef;

                    }

                    //if (hseObservation.Employee1 != null)
                    //{
                    //    var employeeRef = repEmployee.AsQueryable()
                    //        .FirstOrDefault(x => x.OldHrsId == (hseObservation.Employee1.OID))?.AsEmployeeRef();
                    //    if (employeeRef == null) continue;

                    //    newRow.Employee = employeeRef;
                    //}
                    //if (hseObservation.Employee2 != null)
                    //{
                    //    var employeeRef = repEmployee.AsQueryable()
                    //        .FirstOrDefault(x => x.OldHrsId == (hseObservation.Employee2.OID))?.AsEmployeeRef();
                    //    if (employeeRef == null) continue;

                    //    newRow.Employee2 = employeeRef;
                    //}
                    if (hseObservation.Manager != null)
                    {
                        var employeeRef = repEmployee.AsQueryable()
                            .FirstOrDefault(x => x.OldHrsId == (hseObservation.Manager))?.AsEmployeeRef();
                        if (employeeRef == null) continue;

                        newRow.Manager = employeeRef;
                    }

                    if (hseObservation.Location1 != null)
                    {
                        newRow.Location = _helperMethods.GetLocationForHrsLocation(hseObservation.Location1.Name);
                    }


                    newRow.PhysicalLocation = hseObservation.PhysicalLocation;
                    newRow.ObservationClass = PropertyBuilder.CreatePropertyValue("HseObservationClass",
                        "Hse Observation Class", hseObservation.HSE_ObservationClass.Name, "").AsPropertyValueRef();
                    newRow.ObservationType = PropertyBuilder.CreatePropertyValue("HseObservationType",
                        "Hse Observation Type", hseObservation.HSE_ObservationType.Name, "").AsPropertyValueRef();
                    newRow.Status = PropertyBuilder.CreatePropertyValue("HseObservationStatus",
                        "Hse Observation Status", hseObservation.HSE_ObservationStatus.Name, "").AsPropertyValueRef();
                    newRow.Notifications.EmployeeNotified = hseObservation.EmployeeNotified ?? false;
                    newRow.Notifications.ManagerNotified = hseObservation.ManagerNotified ?? false;
                    newRow.Notifications.GlobalNotificationRequired =
                        hseObservation.GlobalNotificationRequired ?? false;
                    newRow.Notifications.GlobalNotificationCompleted =
                        hseObservation.GlobalNotificationCompleted ?? false;
                    foreach (var hseObservationAction in hseObservation.HSE_ObservationAction)
                    {
                        newRow.ObservationActions.Add(new HseObservationAction()
                        {
                            ActionRequired = hseObservationAction.ActionRequired,
                            AssignedTo = hseObservationAction.AssignedTo,
                            CompletionDate = hseObservationAction.CompletionDate,
                            DueDate = hseObservationAction.DueDate
                        });
                    }
                    newRow.Suggestions.Add(new HseObservationSuggestion()
                    {
                        Suggestion = hseObservation.SuggestionValidatedNote,
                        Validated = hseObservation.SuggestionValidated ?? false,
                        Comments = string.Empty
                    });

                    rep.Upsert(newRow);
                }

            }
        }


    }

}
