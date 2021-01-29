using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Properties;
using DAL.HRS.Mongo.DocClass.Training;
using MongoDB.Bson;

namespace DAL.HRS.Mongo.Models
{
    public class QngTrainingInfoModel
    {
        public string PayrollId { get; set; }
        public string FullName { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public LocationRef Location { get; set; }
        public bool IsActive { get; set; }
        public string CourseType { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string VenueType { get; set; }
        public decimal TrainingHours { get; private set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CertificateExpiration { get; set; }
        public decimal Cost { get; set; }
        public decimal Reimbursement { get; set; }
        public string ExternalInstructor { get; set; }
        public EmployeeRef InternalInstructor { get; set; }

        public QngTrainingInfoModel()
        {

        }

        public static List<QngTrainingInfoModel> GetForEmployeeId(ObjectId employeeId)
        {
            var result = new List<QngTrainingInfoModel>();

            return result;
        }

        public static List<QngTrainingInfoModel> GetForTrainingEvent(TrainingEvent te)
        {
            var result = new List<QngTrainingInfoModel>();

            if (!te.Attendees.Any())
            {
                return result;
            }

            var employeeIds = te.Attendees.Where(x=>x.Employee != null).Select(
                x => new { EmployeeId = ObjectId.Parse(x.Employee.Id), x.Reimbursement}).ToList();

            //foreach (var a in te.Attendees)
            //{
                //if (a.Employee == null) continue;

                var filter = Employee.Helper.FilterBuilder.In(x => x.Id, employeeIds.Select(x=>x.EmployeeId));
                var project = Employee.Helper.ProjectionBuilder.Expression(x =>
                    new
                    {
                        x.Id,
                        x.PayrollId,
                        x.CostCenterCode,
                        x.TerminationDate,
                        x.FirstName,
                        x.PreferredName,
                        x.LastName,
                        x.MiddleName,
                        
                    });
                var employees = Employee.Helper.FindWithProjection(filter, project).ToList();

                foreach (var emp in employees)
                {
                    var trainingInfo = new QngTrainingInfoModel()
                    {
                        StartDate = te.StartDate,
                        EndDate = te.EndDate,
                        InternalInstructor = te.InternalInstructor,
                        ExternalInstructor = te.ExternalInstructor,
                        Location = te.Location,
                        Cost = te.Cost,
                        CertificateExpiration = te.CertificateExpiration,
                        TrainingHours = te.TrainingHours
                    };
                    if (te.TrainingCourse != null)
                    {
                        var trainingCourse = te.TrainingCourse.AsTrainingCourse();

                        trainingInfo.CourseType = trainingCourse.CourseType.Code;
                        trainingInfo.CourseName = trainingCourse.Name;
                        trainingInfo.CourseDescription = trainingCourse.Description;
                        trainingInfo.VenueType = trainingCourse.VenueType.Code;
                    }

                    trainingInfo.Reimbursement = employeeIds.First(x => x.EmployeeId == emp.Id).Reimbursement;

                    trainingInfo.PayrollId = emp.PayrollId;
                    trainingInfo.FullName = GetFullName(emp.FirstName, emp.LastName, emp.PreferredName, emp.MiddleName);

                    trainingInfo.DepartmentCode = emp.CostCenterCode.Code;
                    trainingInfo.DepartmentName = emp.CostCenterCode.Description;
                    trainingInfo.IsActive = (emp.TerminationDate == null || emp.TerminationDate > DateTime.Now);

                    result.Add(trainingInfo);
                }


                return result;
        }


        private static string GetFullName(string firstName, string lastName, string preferredName, string middleName)
        {
            if (!String.IsNullOrWhiteSpace(preferredName))
            {
                return $"{lastName}, {preferredName} {middleName}".TrimEnd();
            }
            return $"{lastName}, {firstName} {middleName}".TrimEnd();
        }

    }
}
