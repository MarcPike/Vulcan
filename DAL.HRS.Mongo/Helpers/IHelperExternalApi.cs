using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Common.Models;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperExternalApi
    {
        HrsSecurityModel GetHrsSecurityForUser(string activeDirectoryId);
        HseSecurityModel GetHseSecurityForUser(string activeDirectoryId);
        List<EmployeeDetailsGridModel> GetEmployeeList();
        List<QngEmployeeBasicDataModel> GetEmployeeDetailsForQng(DateTime dateOf, string activeDirectoryId);
        List<QngCompensationModel> GetCompensationForQng(DateTime dateOf, string activeDirectoryId);
        List<LocationModel> GetAllLocations();

        List<QngEmployeeIncidentVarDataModel> GetEmployeeIncidentsByVarDataField(string varDataField, DateTime minDate, DateTime maxDate,
            string activeDirectoryId);
        List<QngEmployeeIncidentModel> GetEmployeeIncidents(DateTime minDate, DateTime maxDate,
            string activeDirectoryId);
        List<QngTrainingInfoModel> GetTrainingInfo(DateTime minDate, DateTime maxDate, string activeDirectoryId);
    }
}