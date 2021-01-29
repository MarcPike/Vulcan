using System;
using System.Collections;
using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Models;
using DAL.HRS.SqlServer.Model;
using MongoDB.Driver;
using Employee = DAL.HRS.Mongo.DocClass.Employee.Employee;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperMedicalInfo
    {
        List<MedicalInfoGridModel> GetMedicalInfoGrid(HrsUser hrsUser);
        MedicalInfoModel GetMedicalInfo(string employeeId, HrsUserRef hrsUser);
        MedicalInfoModel SaveMedicalInfo(MedicalInfoModel model);
        EmployeeDrugTest GetNewDrugTest();
        EmployeeMedicalLeave GetNewMedicalLeave();
        MedicalExam GetNewMedicalExam();
        EmployeeOtherMedicalInfo GetNewOtherMedicalInfo();
        List<MedicalLeaveHistoryGridModel> GetMedicalLeaves(FilterDefinition<Employee> empFilter);
        //IEnumerable GetMedicalLeaves(HrsUser hrsUser);
    }
}