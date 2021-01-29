using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Hrs_User;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperRequiredActivity
    {
        RequiredActivityModel ChangeRequiredActivityStatus(string requiredActivityId, string status);
        void CreateRequiredActivitiesForJobTitle(EmployeeRef employee, JobTitleRef oldJobTitle, JobTitleRef newJobTitle);
        //List<RequiredActivityModel> GetAllRequiredActivitiesDueLessThanSixMonthsFromNow(string status, string type, HrsUser hrsUser);
        List<RequiredActivityModel> GetAllRequiredActivitiesDueLessThanSixMonthsFromNow(HrsUser hrsUser);

        RequiredActivityModel SaveRequiredActivity(RequiredActivityModel model);
    }
}