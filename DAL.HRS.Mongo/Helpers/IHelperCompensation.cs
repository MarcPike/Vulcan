using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.AuditTrails;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperCompensation
    {
        CompensationModel GetCompensationForEmployee(string employeeId);
        List<CompensationGridModel> GetCompensationGrid(string userId);
        CompensationModel SaveCompensation(CompensationModel model, bool newRow=false, EmployeeAuditTrail audit = null, HrsUserRef hrsUser = null);
        CompensationModel RemoveCompensationHistory(string employeeId, string historyId);
        CompensationModel RemoveBonusHistory(string employeeId, string historyId);
        CompensationModel RemovePayGradeHistory(string employeeId, string historyId);
    }
}