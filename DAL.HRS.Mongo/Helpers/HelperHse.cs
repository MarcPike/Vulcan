using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.DocClass.Hse;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;
using MongoDB.Bson;
using static DAL.HRS.Mongo.Models.IncidentSeverityModel;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperHse : IHelperHse
    {
        public List<HseObservationModel> GetAllHseObservations()
        {
            return new RepositoryBase<HseObservation>().AsQueryable().ToList().Select(x => new HseObservationModel(x))
                .OrderByDescending(x => x.DateOf).ToList();
        }

        public List<BbsDepartmentModel> GetAllBbsDepartmentModels()
        {
            return BbsDepartment.Helper.GetAll().Select(x => new BbsDepartmentModel(x)).OrderBy(x => x.Name).ToList();
        }

        public List<BbsTaskModel> GetAllBbsTaskModels()
        {
            return BbsTask.Helper.GetAll().Select(x => new BbsTaskModel(x)).OrderBy(x => x.Name).ToList();
        }

        public List<BbsObservationGridModel> GetBbsObservationGrid()
        {
            var filter = BbsObservation.Helper.FilterBuilder.Empty;
            var projection = BbsObservation.Helper.ProjectionBuilder.Expression(x =>
                new BbsObservationGridModel()
                {
                    DateOf = x.DateOf,
                    Department = x.Department,
                    DepartmentSubCategory = x.DepartmentSubCategory,
                    EmployeeType = x.EmployeeType,
                    Id = x.Id,
                    Location = x.Location,
                    NumberOfPeopleObserved = x.NumberOfPeopleObserved,
                    Observer = x.Observer,
                    ShiftType = x.ShiftType,
                    Take5Department = x.Take5Department,
                    TaskSubCategory = x.TaskSubCategory,
                    TaskType = x.TaskType,
                    ObservationId = x.ObservationId

                });

            return BbsObservation.Helper.FindWithProjection(filter, projection).ToList();
        }



        public List<BbsObservationModel> GetAllBbsObservationModels()
        {
            return BbsObservation.Helper.GetAll().Select(x => new BbsObservationModel(x)).ToList();
        }

        public BbsObservationModel GetBbsObservation(string id)
        {
            var observation = BbsObservation.Helper.FindById(id);

            //var emp = Employee.Helper.FindById(employeeId);

            //return new MedicalInfoModel(emp.AsEmployeeRef(), hrsUser);

            return new BbsObservationModel(observation);
        }

        public BbsObservationItem GetNewObservationItem()
        {
            return new BbsObservationItem();
        }

        public List<BbsDepartmentRef> GetAllBbsDepartmentRefs()
        {
            return BbsDepartment.Helper.GetAll().Select(x => new BbsDepartmentRef(x)).ToList();
        }

        public List<BbsTaskRef> GetAllBbsTaskRefs()
        {
            return BbsTask.Helper.GetAll().Select(x => new BbsTaskRef(x)).ToList();
        }

        public List<BbsDepartmentSubCategoryRef> GetAllBbsDepartmentSubCategoryRefs(string departmentId)
        {
            var dept = BbsDepartment.Helper.FindById(departmentId);
            if (dept == null) throw new Exception("Department not found");

            return dept.SubCategories.OrderBy(x => x.Name).ToList();
        }

        public List<BbsTaskSubCategoryRef> GetAllBbsTaskSubCategoryRefs(string taskId)
        {
            var task = BbsTask.Helper.FindById(taskId);
            if (task == null) throw new Exception("Task not found");

            return task.SubCategories.OrderBy(x => x.Name).ToList();
        }

        public List<BbsObserverRef> GetActiveObserverRefs()
        {
            var filter = BbsObserver.Helper.FilterBuilder.Where(x => x.IsActive);
            var project = BbsObserver.Helper.ProjectionBuilder.Expression(x => new BbsObserverRef(x));
            return BbsObserver.Helper.FindWithProjection(filter, project).ToList().OrderBy(x => x.Employee.LastName)
                .ThenBy(x => x.Employee.FirstName).ToList();
        }

        public List<BbsObserverModel> GetObserverModels()
        {
            var filter = BbsObserver.Helper.FilterBuilder.Empty;
            var project = BbsObserver.Helper.ProjectionBuilder.Expression(x => new BbsObserverModel(x));
            return BbsObserver.Helper.FindWithProjection(filter, project).ToList().OrderBy(x => x.Employee.LastName)
                .ThenBy(x => x.Employee.FirstName).ToList();
        }

        public BbsObserverModel SaveBbsObserver(BbsObserverModel model)
        {
            var obs = BbsObserver.Helper.FindById(model.Id) ?? new BbsObserver() { Id = ObjectId.Parse(model.Id) };
            obs.Employee = model.Employee;
            obs.IsActive = model.IsActive;
            BbsObserver.Helper.Upsert(obs);
            return new BbsObserverModel(obs);
        }

        public void RemoveBbsObserver(string id)
        {
            var obs = BbsObserver.Helper.FindById(id);
            if (obs != null)
            {
                BbsObserver.Helper.DeleteOne(obs.Id);
            }

        }

        public BbsDepartmentModel GetNewBbsDepartmentModel()
        {
            return new BbsDepartmentModel();
        }

        public BbsTaskModel GetNewBbsTaskModel()
        {
            return new BbsTaskModel();
        }

        public BbsObservationModel GetNewBbsObservationModel()
        {
            return new BbsObservationModel();
        }



        public BbsDepartmentModel SaveBbsDepartment(BbsDepartmentModel model)
        {
            var dept = BbsDepartment.Helper.FindById(model.Id) ??
                new BbsDepartment()
                {
                    Id = ObjectId.Parse(model.Id)
                };

            dept.Name = model.Name;
            dept.SubCategories = model.SubCategories;

            if (dept.SubCategories != null)
            {
                foreach (var subCategory in dept.SubCategories)
                {

                    // Add / Update existing
                    //var filter = BbsDepartmentSubCategory.Helper.FilterBuilder.Where(x => x.Id == ObjectId.Parse(subCategory.Id));


                    var subDept = BbsDepartmentSubCategory.Helper.FindById(subCategory.Id) ??
                        new BbsDepartmentSubCategory()
                        {
                            Id = ObjectId.GenerateNewId()
                        };

                    subDept.Name = subCategory.Name;
                    subCategory.Id = subDept.Id.ToString();

                    BbsDepartmentSubCategory.Helper.Upsert(subDept);



                }

            }

            BbsDepartment.Helper.Upsert(dept);

            return new BbsDepartmentModel(dept);
        }

        public BbsTaskModel SaveBbsTask(BbsTaskModel model)
        {
            var task = BbsTask.Helper.FindById(model.Id) ??
                       new BbsTask()
                       {
                           Id = ObjectId.Parse(model.Id)
                       };

            task.Name = model.Name;
            task.SubCategories = model.SubCategories;

            BbsTask.Helper.Upsert(task);

            return new BbsTaskModel(task);
        }

        public BbsObservationModel SaveBbsObservation(BbsObservationModel model, HrsUserRef modifiedByUser)
        {
            var obs = BbsObservation.Helper.FindById(model.Id) ??
                       new BbsObservation()
                       {
                           Id = ObjectId.Parse(model.Id),
                           CreatedByUserId = modifiedByUser.UserId
                       };

            obs.TaskType = model.TaskType;
            obs.TaskSubCategory = model.TaskSubCategory;
            obs.Department = model.Department;
            obs.DepartmentSubCategory = model.DepartmentSubCategory;
            obs.DateOf = model.DateOf;
            obs.EmployeeType = model.EmployeeType;
            obs.ModifiedByUserId = modifiedByUser.UserId;
            obs.WorkerComments = model.WorkerComments;
            obs.ObserverComments = model.ObserverComments;
            obs.Items = model.Items;
            obs.ObservationType = model.ObservationType;
            obs.Observer = model.Observer;
            obs.ShiftType = model.ShiftType;
            obs.NumberOfPeopleObserved = model.NumberOfPeopleObserved;
            obs.Location = model.Location;
            obs.TaskStopped = model.TaskStopped;
            obs.CorrectedOnJob = model.CorrectedOnJob;
            obs.RasiedWithSupIndiv = model.RasiedWithSupIndiv;
            obs.RasiedWithSupObsrv = model.RasiedWithSupObsrv;
            obs.TrainingNeeded = model.TrainingNeeded;




            BbsObservation.Helper.Upsert(obs);

            return new BbsObservationModel(obs);
        }

        public void RemoveBbsDepartment(string id)
        {
            var doc = BbsDepartment.Helper.FindById(id);
            if (doc == null) throw new Exception("Department not found");
            BbsDepartment.Helper.DeleteOne(id);
        }

        public void RemoveBbsTask(string id)
        {
            var doc = BbsTask.Helper.FindById(id);
            if (doc == null) throw new Exception("Task not found");
            BbsTask.Helper.DeleteOne(id);
        }

        public void RemoveBbsObservation(string id)
        {
            var doc = BbsObservation.Helper.FindById(id);
            if (doc == null) throw new Exception("Bbs Observation not found");
            BbsObservation.Helper.DeleteOne(id);
        }

        public List<IncidentSeverityModel> GetIncidentSeverityByLocation()
        {
            var result = new List<IncidentSeverityModel>();

            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Location.Entity.Name == "Howco" && x.IncidentDate <= DateTime.Now.AddMonths(-13));
            var project = queryHelper.ProjectionBuilder.Expression(x => new { x.Location.Office, x.SeverityTypeCode.Code });
            var incidents = queryHelper.FindWithProjection(filter, project).OrderBy(x => x.Office).ToList();


            var grouped = incidents.GroupBy(x => x.Office).ToList();

            foreach (var office in grouped)
            {
                var firstAid = office.Where(x => x.Code == "First Aid").ToList();
                var lostTime = office.Where(x => x.Code == "Lost Time").ToList();
                var medical = office.Where(x => x.Code == "Medical Treatment Only").ToList();
                var series = new List<IncidentSeverityTypeModel>
                    {
                        new IncidentSeverityTypeModel { Name = "First Aid", Value = firstAid.Count() },
                        new IncidentSeverityTypeModel { Name = "Lost Time", Value = lostTime.Count() },
                        new IncidentSeverityTypeModel { Name = "Medical Treatment Only", Value = medical.Count() }
                    };

                result.Add(new IncidentSeverityModel
                {
                    Name = office.Key,
                    Series = series.ToArray()

                });
            }

            return result;
        }

        public List<IncidentsYearToYearModel> GetIncidentsYearToYear()
        {
            var result = new List<IncidentsYearToYearModel>();
            var queryDate = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);

            var queryHelper = new MongoRawQueryHelper<EmployeeIncident>();
            var filter = queryHelper.FilterBuilder.Where(x => x.Location.Entity.Name == "Howco" && x.NearMissTypeCode.Description == "Incident" && x.IncidentDate >= queryDate);
            var project = queryHelper.ProjectionBuilder.Expression(x => x.IncidentDate );
            var incidentDates = queryHelper.FindWithProjection(filter, project);

           
            var results = incidentDates.Select(x => x.Value.Year).ToList();
            
            result.Add(new IncidentsYearToYearModel
            {
                Name = queryDate.Year.ToString(),
                Value = results.Where(x => x == queryDate.Year).Count()
            });

            result.Add(new IncidentsYearToYearModel
            {
                Name = DateTime.Now.Year.ToString(),
                Value = results.Where(x => x == DateTime.Now.Year).Count()
            });

            
            return result;
        }
    }
}
