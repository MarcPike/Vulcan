using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.AuditTrails;
using DAL.HRS.Mongo.DocClass.Discipline;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperDiscipline: HelperBase, IHelperDiscipline
    {
        private HelperEmployee _helperEmployee = new HelperEmployee();
        private HelperUser _helperUser = new HelperUser();
        public List<DisciplineGridModel> GetDisciplineGrid(string userId)
        {
            var employees = _helperEmployee.GetAllMyEmployeeGridModelsForModule(userId, "Discipline", true, withDiscipline:true);

            var result = new List<DisciplineGridModel>();
            foreach (var employee in employees)
            {
                result.Add(new DisciplineGridModel(employee));   
            }

            return result.OrderBy(x=>x.LastName).ThenBy(x =>x.FirstName).ToList();

        }

        public (DisciplineModel Current, List<DisciplineModel> History) GetDisciplineModelsListForEmployee(string employeeId)
        {
            var employee = _helperEmployee.GetEmployee(employeeId);
            if (employee == null) throw new Exception("Employee not found");

            var employeeRef = employee.AsEmployeeRef();

            DisciplineModel currentResult = new DisciplineModel();
            List<DisciplineModel> historyResult = new List<DisciplineModel>();

            var current = employee.Discipline.SingleOrDefault(x => x.Locked == false);
            currentResult = new DisciplineModel(employeeRef, current);
            //if (current != null)
            //{
            //    currentResult = new DisciplineModel(employeeRef,current) {Id = Guid.NewGuid().ToString()};
            //}
            //else
            //{
            //    currentResult.Employee = employeeRef;
            //}
            foreach (var discipline in employee.Discipline.OrderByDescending(x=>x.DateOfAction).Where(x=>x.Locked))
            {
                historyResult.Add(new DisciplineModel(employeeRef, discipline));
            }

            return (currentResult, historyResult);

        }

        public (DisciplineModel Current, List<DisciplineModel> History) SaveDiscipline(DisciplineModel model)
        {
            if (model.IsDirty)
            {

                var rep = new RepositoryBase<Employee>();
                var emp = rep.Find(model.Employee.Id);

                var audit = new EmployeeAuditTrail(emp, model.ModifiedBy);

                var disc = new Discipline()
                {
                    Id = Guid.Parse(model.Id),
                    DateOfAction = model.DateOfAction,
                    DateOfActionAppeals = model.DateOfActionAppeals,
                    DateOfViolation = model.DateOfViolation,
                    DisciplinaryActionType = model.DisciplinaryActionType,
                    Employee = model.Employee,
                    EmployeeInAgreement = model.EmployeeInAgreement,
                    EmployeeStatement = model.EmployeeStatement,
                    Location = model.Location,
                    Manager = model.Manager,
                    ManagerStatement = model.ManagerStatement,
                    NatureOfViolationType = model.NatureOfViolationType,
                    RepresentativeName = model.RepresentativeName,
                    RepresentativePresent = model.RepresentativePresent,
                    Locked = false
                };


                foreach (var oldDisc in emp.Discipline.Where(x => x.Locked == false).ToList())
                {
                    oldDisc.Locked = true;
                }

                emp.Discipline.Add(disc);
                rep.Upsert(emp);
                audit.Save(emp);
            }

            var result = GetDisciplineModelsListForEmployee(model.Employee.Id);

            return (result.Current, result.History);
        }

        public DisciplineModel ModifyDisciplineHistory(DisciplineModel model)
        {
            var rep = new RepositoryBase<Employee>();
            var emp = rep.Find(model.Employee.Id);

            var disc = emp.Discipline.FirstOrDefault(x => x.Id == Guid.Parse(model.Id));
            if (disc == null) throw new Exception("Discipline History not found");

            if (!disc.Locked) throw new Exception("This cannot be performed on a current record, only historical items");
            
            disc.DateOfAction = model.DateOfAction;
            disc.DateOfActionAppeals = model.DateOfActionAppeals;
            disc.DateOfViolation = model.DateOfViolation;
            disc.DisciplinaryActionType = model.DisciplinaryActionType;
            disc.EmployeeInAgreement = model.EmployeeInAgreement;
            disc.EmployeeStatement = model.EmployeeStatement;
            disc.Location = model.Location;
            disc.Manager = model.Manager;
            disc.ManagerStatement = model.ManagerStatement;
            disc.NatureOfViolationType = model.NatureOfViolationType;
            disc.RepresentativeName = model.RepresentativeName;
            disc.RepresentativePresent = model.RepresentativePresent;

            rep.Upsert(emp);

            var result = GetDisciplineModelsListForEmployee(model.Employee.Id);
            return new DisciplineModel(emp.AsEmployeeRef(), disc);
        }

    }
}
