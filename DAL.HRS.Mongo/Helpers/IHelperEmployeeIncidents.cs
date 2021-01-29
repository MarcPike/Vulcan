using System.Collections.Generic;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperEmployeeIncidents
    {
        List<EmployeeIncidentGridModel> GetEmployeeIncidentGridRows();
        List<EmployeeIncidentModel> GetAllEmployeeIncidents();
        //DrugTestModel GetDrugTestModel(string employeeIncidentId, string drugTestId);
        //DrugTestModel GetNewDrugTestModel();
        EmployeeIncidentModel GetNewEmployeeIncidentModel();
        EmployeeIncidentModel GetEmployeeIncidentModel(string id);
        void RemoveEmployeeIncident(string id);
        EmployeeIncidentModel SaveEmployeeIncident(EmployeeIncidentModel model);
    }
}