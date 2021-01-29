using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Driver;
using NUnit.Framework;

namespace DAL.HRS.Mongo.Tests.DirectReportsFix
{
    [TestFixture]
    public class FixAllDirectReports
    {
        [SetUp]
        public void SetUp()
        {
            EnvironmentSettings.HrsProduction();
        }

        [Test]
        public void Execute()
        {
            
            var filter = Employee.Helper.FilterBuilder.Where(x => x.DirectReports.Any());
           
            var projection = Employee.Helper.ProjectionBuilder.Expression(x => new { x.Id, x.DirectReports});
           
            var employeesWithDirectReports = Employee.Helper.FindWithProjection(filter, projection).ToList();

            //loop each record in employeesWithDirectReports
            foreach (var manager in employeesWithDirectReports)
            {
                List<EmployeeRef> removeDirectReports = new List<EmployeeRef>();

                //loop through each managers list of direct reports
                foreach (var directReport in manager.DirectReports.ToList())
                {
                    //look to find if an employee payrollId matches the direct reports payrollId
                    var foundEmpByPayrollId = Employee.Helper.Find(x => x.PayrollId == directReport.PayrollId).FirstOrDefault();

                  

                    if (foundEmpByPayrollId == null)  //if it's not matching, add it to the removeDirectReports List
                    {
                        removeDirectReports.Add(directReport);
                    }
                }

                if (removeDirectReports.Any())  
                {
                    var modifyEmployee = Employee.Helper.FindById(manager.Id);

                   
                    
                    modifyEmployee.DirectReports.RemoveAll(dr => removeDirectReports.Select(r => r.PayrollId.Trim()).Contains(dr.PayrollId.Trim()));

                    Employee.Helper.Upsert(modifyEmployee);
                }
            }

        }

       
    }
}
