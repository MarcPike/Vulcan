using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Schema;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using MongoDB.Driver;
using CoshhProduct = DAL.HRS.Mongo.DocClass.Hse.CoshhProduct;
using CoshhRiskAssessment = DAL.HRS.Mongo.DocClass.Hse.CoshhRiskAssessment;
using GhsClassificationType = DAL.HRS.Mongo.DocClass.Hse.GhsClassificationType;
using HrsContext = DAL.HRS.SqlServer.Model.HrsContext;

namespace DAL.HRS.Import.ImportHse
{
    [TestFixture()]
    public class UpsertBbsObservations
    {
        private HseHelperMethods _helperMethods = new HseHelperMethods();

        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsQualityControl();
        }

        [Test]
        public void Execute()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SqlServer.Model.HrsContext context = new HrsContext())
            {
                var rep = new RepositoryBase<BbsObservation>();

                LoadBbsTake5Departments(context);
                LoadBbsDepartments(context);
                LoadTasks(context);
                LoadBbsPrecautionTypes(context);
                LoadBbsInfluenceBehaviorTypes(context);
                LoadBbsObservers(context);


                foreach (var obs in context.BBS_Observation.Where(x=>x.GCRecord == null).ToList())
                {

                    if (BbsObservation.Helper.Find(x=>x.ObservationId == obs.OID).Any()) continue;
                    
                    var newObservation = rep.AsQueryable().FirstOrDefault(x => x.OldHrsId == obs.OID) ??
                                         new BbsObservation()
                                         {
                                             OldHrsId = obs.OID
                                         };
                    newObservation.DateOf = obs.DateTime;
                    if (obs.BBS_Department1 != null)
                    {
                        newObservation.Department = BbsDepartment.GetRefForName(obs.BBS_Department1.Name);
                    }

                    if (obs.BBS_DepartmentSubCategory1 != null)
                    {
                        newObservation.DepartmentSubCategory = BbsDepartmentSubCategory.GetRefForName(obs.BBS_DepartmentSubCategory1.Name);
                    }

                    if (obs.BBS_TaskType != null)
                    {
                        newObservation.TaskType = BbsTask.GetRefForName(obs.BBS_TaskType.Name);
                    }

                    if (obs.BBS_TaskTypeSubCategory != null)
                    {
                        newObservation.TaskSubCategory = BbsTaskSubCategory.GetRefForName(obs.BBS_TaskTypeSubCategory.Name);
                    }

                    if (obs.BBS_EmployeeType != null)
                    {
                        newObservation.EmployeeType = PropertyBuilder.New(
                            "BbsEmployeeType",
                            "BBS Employee Type",
                            obs.BBS_EmployeeType.Name, string.Empty);
                    }

                    if (obs.BBS_Observer != null)
                    {
                        var observer = BbsObserver.GetBbsObserverRefByOldId(obs.BBS_Observer.OID);
                        if (observer != null)
                        {
                            newObservation.Observer = observer;
                        }
                    }

                    var location = GetLocationForHrsLocation(obs.Location1.Name);
                    if (location != null)
                    {
                        newObservation.Location = location;
                    }

                    if (obs.ObservationType != null)
                    {
                        newObservation.ObservationType = GetObservationType(obs.ObservationType, context);
                    }

                    if (obs.Department1 != null)
                    {
                        newObservation.Take5Department = PropertyBuilder.New("Take5Department", "Take 5 Department",
                            obs.Department1.DepartmentCode,
                            obs.Department1.DepartmentName);
                    }

                    newObservation.ObserverComments = obs.ObserverComments ?? string.Empty;
                    if (obs.PositionShiftType != null)
                    {
                        newObservation.ShiftType = PropertyBuilder.New(
                            "BbsPositionType",
                            "BBS Position Type",
                            obs.PositionShiftType.Name, string.Empty);

                    }
                    
                    newObservation.NumberOfPeopleObserved = obs.NumberOfPeopleObserved;
                    newObservation.ObservationId = obs.OID;
                    newObservation.WorkerComments = obs.WorkerComments;
                    
                    
                    foreach (var item in obs.BBS_ObservationItem)
                    {
                        var newBbsItem = new BbsObservationItem();

                        

                        if (item.BBS_InfluenceOnBehaviorType != null)
                        {
                            var influence = BbsInfluenceBehaviorType.GetBbsInfluenceBehaviorTypeRefForName(item.BBS_InfluenceOnBehaviorType
                                .Name);
                            newBbsItem.InfluenceBehaviorType = influence;
                        }

                        if (item.BBS_PrecautionType != null)
                        {
                            newBbsItem.BbsPrecautionType =
                                BbsPrecautionTypeLocation.GetPropertyValueRefForLocation(item.BBS_PrecautionType.Name,
                                    newObservation.Location, "Howco");
                        }

                       
                        newBbsItem.ConcernCount = item.ConcernCount ?? 0;
                        //newBbsItem.ReasonForConcernWhat = item.ReasonForConcernWhat ?? string.Empty;
                        //newBbsItem.ReasonForConcernWhy = item.ReasonForConcernWhy ?? string.Empty;
                        newBbsItem.SafeCount = item.SafeCount ?? 0;


                        newObservation.Items.Add(newBbsItem);
                    }
                    BbsObservation.Helper.Upsert(newObservation);
                }


            }

            sw.Stop();
            Console.WriteLine($"{BbsObservation.Helper.GetRowCount()} imported. Elapsed: {sw.Elapsed}");
        }

        private void LoadBbsObservers(HrsContext context)
        {
            BbsObserver.Helper.DeleteMany(BbsObserver.Helper.FilterBuilder.Empty);

            foreach (var bbsObserver in context.BBS_Observer.Where(x => x.GCRecord == null).ToList())
            {
                if (bbsObserver.Employee == null) continue;
                var empFilter = Employee.Helper.FilterBuilder.Where(x => x.OldHrsId == bbsObserver.Employee);
                var project = Employee.Helper.ProjectionBuilder.Expression(x => x.AsEmployeeRef());

                var employeeRef = Employee.Helper.FindWithProjection(empFilter, project).FirstOrDefault();
                if (employeeRef == null) continue;


                var newObserver = new BbsObserver()
                {
                    Employee = employeeRef,
                    IsActive = bbsObserver.IsActive ?? false,
                    OldHrsId = bbsObserver.OID
                };


                BbsObserver.Helper.Upsert(newObserver);
            }
        }

        private void LoadBbsInfluenceBehaviorTypes(HrsContext context)
        {
            foreach (var bbsInfluenceOnBehaviorType in context.BBS_InfluenceOnBehaviorType.Where(x=>x.GCRecord == null))
            {
                var newRow = new BbsInfluenceBehaviorType()
                {
                    Name = bbsInfluenceOnBehaviorType.Name,
                    SendEmail = bbsInfluenceOnBehaviorType.SendEmail ?? false
                };  
                
            }
        }

        [Test]
        public void LoadBbsPrecautionTypesOnly()
        {
            using (SqlServer.Model.HrsContext context = new HrsContext())
            {
                LoadBbsPrecautionTypes(context);
            }
        }

        private void LoadBbsPrecautionTypes(HrsContext context)
        {
            var locations = context.BBS_PrecautionType.Where(x => x.GCRecord == null).Select(x => x.Location1.Name).ToList().Distinct();
            foreach (var loc in locations)
            {
                LocationRef location = GetLocationForHrsLocation("<unknown>");
                if (loc != null) location = GetLocationForHrsLocation(loc);

                if (location == null) continue;

                var thisPrecaution = BbsPrecautionTypeLocation.Helper.Find(x => x.Location.Id == location.Id).FirstOrDefault();
                if (thisPrecaution == null)
                {
                    thisPrecaution = new BbsPrecautionTypeLocation()
                    {
                        Location = location
                    };
                }

                thisPrecaution.PrecautionTypeProperties.Clear();

                if (location.Office == "<unknown>")
                {
                    foreach (var bbsPrecautionType in context.BBS_PrecautionType.Where(x => (x.Location == null) && x.GCRecord == null).OrderBy(x => x.OrderBy))
                    {
                        thisPrecaution.PrecautionTypeProperties.Add(PropertyBuilder.New("BbsPrecautionTypeLocation", "Type of Bbs Precaution", bbsPrecautionType.Name, ""));
                    }
                }
                else
                {
                    foreach (var bbsPrecautionType in context.BBS_PrecautionType.Where(x => (x.Location1.Name == loc) && x.GCRecord == null).OrderBy(x => x.OrderBy))
                    {
                        thisPrecaution.PrecautionTypeProperties.Add(PropertyBuilder.New("BbsPrecautionTypeLocation", "Type of Bbs Precaution", bbsPrecautionType.Name, ""));
                    }
                }

                if (thisPrecaution.PrecautionTypeProperties.Any())
                {
                    BbsPrecautionTypeLocation.Helper.Upsert(thisPrecaution);
                }
            }

        }

        private PropertyValueRef GetObservationType(int? observationTypeId, HrsContext context)
        {
            var observationType = context.BBS_ObservationType.SingleOrDefault(x => x.OID == observationTypeId);
            return PropertyBuilder.New(
                "BbsObservationType",
                "BBS Observation Type",
                observationType?.Name ?? "(Not Specified)", string.Empty);
        }

        private static void LoadTasks(HrsContext context)
        {
            var repTaskType = new RepositoryBase<BbsTask>();
            foreach (var taskType in context.BBS_TaskType.ToList())
            {
                var newTaskType = repTaskType.AsQueryable().FirstOrDefault(x => x.Name == taskType.Name) ??
                                  new BbsTask()
                                  {
                                      Name = taskType.Name
                                  };
                newTaskType.SubCategories.Clear();
                var subCategories = new List<BbsTaskSubCategory>();
                foreach (var subCategory in taskType.BBS_TaskTypeSubCategory)
                {
                    var newSub = new BbsTaskSubCategory()
                    {
                        Name = subCategory.Name
                    };
                    BbsTaskSubCategory.Helper.Upsert(newSub);

                    subCategories.Add(newSub);

                }

                newTaskType.SubCategories = subCategories.Select(x => new BbsTaskSubCategoryRef(x)).ToList();
                repTaskType.Upsert(newTaskType);
            }
        }

        private static void LoadBbsTake5Departments(HrsContext context)
        {
            foreach (var department in context.Department)
            {
                PropertyBuilder.New("Take5Department", "Take 5 Department", department.DepartmentCode,
                    department.DepartmentName);
            }
        }

        private static void LoadBbsDepartments(HrsContext context)
        {
            BbsDepartment.Helper.DeleteMany(BbsDepartment.Helper.FilterBuilder.Empty);
            BbsDepartmentSubCategory.Helper.DeleteMany(BbsDepartmentSubCategory.Helper.FilterBuilder.Empty);
            var repDepartments = new RepositoryBase<BbsDepartment>();
            foreach (var department in context.BBS_Department.ToList())
            {
                var newDepartment = repDepartments.AsQueryable().FirstOrDefault(x => x.Name == department.Name) ??
                                    new BbsDepartment()
                                    {
                                        Name = department.Name
                                    };
                newDepartment.SubCategories.Clear();
                var subCategories = new List<BbsDepartmentSubCategory>();
                foreach (var subCategory in department.BBS_DepartmentSubCategory)
                {
                    var newSub = new BbsDepartmentSubCategory()
                    {
                        Name = subCategory.Name
                    };
                    BbsDepartmentSubCategory.Helper.Upsert(newSub);

                    subCategories.Add(newSub);
                }

                newDepartment.SubCategories = subCategories.Select(x => new BbsDepartmentSubCategoryRef(x)).ToList();
                repDepartments.Upsert(newDepartment);
            }
        }

        private LocationRef GetLocationForHrsLocation(string employeeLocation)
        {
            if (employeeLocation == "Canada")
            {
                employeeLocation = "Edmonton";
            }

            if (employeeLocation == "Emmott")
            {
                employeeLocation = "Emmott Road";
            }

            if ((employeeLocation == "Lafayette/South Bernard") || (employeeLocation == "Lafayette (Deleted)"))
            {
                employeeLocation = "Lafayette";
            }

            employeeLocation = employeeLocation.TrimEnd();
            var rep = new RepositoryBase<Location>();
            var location = rep.AsQueryable().FirstOrDefault(x => x.Office == employeeLocation) ??
                           rep.AsQueryable().First(x => x.Office == "<unknown>");

            return location.AsLocationRef();
        }

    }
}
