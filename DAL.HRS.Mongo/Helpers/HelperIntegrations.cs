using System;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.Models;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperIntegrations : IHelperIntegrations
    {
        public List<KronosExportModel> KronosExport(string entityName)
        {
            FilterDefinition<Employee> entityFilter = Employee.Helper.FilterBuilder.Empty;
            if (entityName.ToUpper() == "HOWCO")
            {
                entityFilter = Employee.Helper.FilterBuilder.Where(x => x.Entity.Name == "Howco");
            }
            else if (entityName.ToUpper() == "EDGEN MURRAY")
            {
                entityFilter = Employee.Helper.FilterBuilder.Where(x => x.Entity.Name == "Edgen Murray");
            }

            var filter = Employee.Helper.FilterBuilder.Where(x=>x.TerminationDate == null || x.TerminationDate >= DateTime.Parse("1/1/2019"));
            var project = Employee.Helper.ProjectionBuilder.Expression(
                x =>
                    new KronosExportModel(x.Birthday, x.CompanyNumberCode, x.GenderCode, x.MaritalStatusCode, x.BusinessUnitCode,
                        x.PayrollId, x.CostCenterCode, x.KronosDepartmentCode, x.Location, x.GovernmentId, x.Status1Code, x.Status2Code,
                        x.WorkAreaCode, x.FirstName, x.LastName, x.JobTitle, x.ContactCountry, x.Address1, x.Address2, x.Address3,
                        x.City, x.State, x.PostalCode, x.OriginalHireDate, x.LastRehireDate, x.KronosManager, x.PhoneNumbers, x.EmailAddresses,
                        (x.TerminationDate == null || x.TerminationDate > DateTime.Now), x.Compensation, x.LdapUser, x.Login, x.TerminationDate, x.DeviceGroup)
            );
            return Employee.Helper.FindWithProjection(filter & entityFilter, project).ToList();
           
        }

        public List<HalogenExportModel> HalogenExport(string entityName)
        {
            FilterDefinition<Employee> entityFilter = Employee.Helper.FilterBuilder.Empty;
            if (entityName.ToUpper() == "HOWCO")
            {
                entityFilter = Employee.Helper.FilterBuilder.Where(x => x.Entity.Name == "Howco");
            }
            else if (entityName.ToUpper() == "EDGEN MURRAY")
            {
                entityFilter = Employee.Helper.FilterBuilder.Where(x => x.Entity.Name == "Edgen Murray");
            }

            var hrRepCache = HrRepresentative.GetAll();

            var filter = Employee.Helper.FilterBuilder.Where(x => (x.TerminationDate == null || x.TerminationDate >= DateTime.Parse("1/1/2019") ) &&  x.Status1Code.Code != "Contract" && x.Status1Code.Code != "Contract Labor" && x.Status1Code.Code != "Temporary");
            var project = Employee.Helper.ProjectionBuilder.Expression(
                x =>
                    new HalogenExportModel(x.Login, x.Manager, x.FirstName, x.LastName, x.EmailAddresses, 
                    x.PayrollId, x.OriginalHireDate, x.JobTitle, x.PerformanceEvaluationType, x.CostCenterCode, x.KronosDepartmentCode,
                    x.Location, x.Lms, x.TerminationDate, hrRepCache));

            return Employee.Helper.FindWithProjection(filter & entityFilter, project).ToList();
        }
    }

   
}
