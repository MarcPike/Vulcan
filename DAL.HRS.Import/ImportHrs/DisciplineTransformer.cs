using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.SqlServer;
using DAL.HRS.SqlServer.Model;
using DAL.Vulcan.Mongo.Base.Repository;
using DAL.Common.DocClass;
using System.Linq;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;

namespace DAL.HRS.Import.ImportHrs
{
    public static class DisciplineTransformer
    {
        public static void TransformDiscipline(Employee employee, DisciplineHrs disc)
        {
            if (disc.DisciplineHistory != null)
            {
                foreach (var history in disc.DisciplineHistory)
                {
                    TransformDisciplineHistory(employee, history);
                }
            }


            var location = new RepositoryBase<Common.DocClass.Location>().AsQueryable()
                .FirstOrDefault(x => x.Office == disc.LocationBranch)?.AsLocationRef();

            var manager = new RepositoryBase<DAL.HRS.Mongo.DocClass.Employee.Employee>().AsQueryable()
                .FirstOrDefault(x => x.OldHrsId == disc.ManagerId)?.AsEmployeeRef();

            PropertyValueRef representativePresent = PropertyBuilder.CreatePropertyValue("RepresentativePresent",
                "Was Representative Present", "Unknown", "Undetermined").AsPropertyValueRef();
            if (disc.RepresentativePresent != null)
            {
                if (disc.RepresentativePresent == true)
                {
                    representativePresent = PropertyBuilder.CreatePropertyValue("RepresentativePresent",
                        "Was Representative Present", "Yes", "Yes Representative was present").AsPropertyValueRef();
                }
                else
                {
                    representativePresent = PropertyBuilder.CreatePropertyValue("RepresentativePresent",
                        "Was Representative Present", "No", "No Representative was not present").AsPropertyValueRef();
                }
            }


            var newDiscipline = new DAL.HRS.Mongo.DocClass.Discipline.Discipline()
            {
                DateOfAction = disc.DateOfAction,
                DateOfActionAppeals = disc.DateOfActionAppeals,
                DateOfViolation = disc.DateOfViolation,
                DisciplinaryActionType = PropertyBuilder.CreatePropertyValue("DisciplinaryActionType",
                    "Disciplinary Action Type",
                    disc.DisciplinaryActionType, "Disciplinary Action Type").AsPropertyValueRef(),
                EmployeeInAgreement = PropertyBuilder.CreatePropertyValue("EmployeeInAgreement",
                    "Employee in Agreement",
                    disc.EmployeeInAgreement, "Employee in Agreement").AsPropertyValueRef(),
                EmployeeStatement = disc.EmployeeStatement,
                Location = location,
                Manager = manager,
                ManagerStatement = disc.ManagerStatement,
                NatureOfViolationType = PropertyBuilder.CreatePropertyValue("NatureOfViolationType",
                    "Nature of Violation Type",
                    disc.NatureOfViolationType, "Nature of Violation").AsPropertyValueRef(),
                RepresentativeName = disc.RepresentativeName,
                RepresentativePresent = representativePresent,
                Locked = false
            };

            employee.Discipline.Add(newDiscipline);

        }

        public static void TransformDisciplineHistory(Employee employee, DisciplineHistoryHrs disc)
        {
            var locationHist = new RepositoryBase<DAL.Common.DocClass.Location>().AsQueryable()
                .FirstOrDefault(x => x.Office == disc.LocationBranch)?.AsLocationRef();

            var managerHist = new RepositoryBase<DAL.HRS.Mongo.DocClass.Employee.Employee>().AsQueryable()
                .FirstOrDefault(x => x.OldHrsId == disc.ManagerId)?.AsEmployeeRef();

            var oldDiscipline = new DAL.HRS.Mongo.DocClass.Discipline.Discipline()
            {
                NatureOfViolationType = PropertyBuilder.CreatePropertyValue("NatureOfViolationType",
                    "Nature of Violation Type",
                    disc.NatureOfViolationType, "Nature of Violation").AsPropertyValueRef(),
                DateOfAction = disc.DateOfAction,
                DateOfViolation = disc.DateOfViolation,
                DisciplinaryActionType = PropertyBuilder.CreatePropertyValue("DisciplinaryActionType",
                    "Disciplinary Action Type",
                    disc.DisciplinaryActionType, "Disciplinary Action Type").AsPropertyValueRef(),
                EmployeeInAgreement = PropertyBuilder.CreatePropertyValue("EmployeeInAgreement",
                    "Employee in Agreement",
                    disc.EmployeeInAgreement, "Employee in Agreement").AsPropertyValueRef(),
                EmployeeStatement = disc.EmployeeStatement,
                Location = locationHist,
                Manager = managerHist,
                ManagerStatement = disc.ManagerStatement,
                RepresentativeName = "Unknown",
                RepresentativePresent = null,
                Locked = true
            };
            employee.Discipline.Add(oldDiscipline);

        }
    }

}







