using System.Collections.Generic;
using DAL.HRS.Mongo.Logger;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperPerformance
    {
        List<PerformanceGridModel> GetPerformanceGrid(string userId);
        (PerformanceModel Current, List<PerformanceModel> History) GetPerformanceModelsForEmployee(string employeeId);
        (PerformanceModel Current, List<PerformanceModel> History) SavePerformance(PerformanceModel model);
        PerformanceModel ModifyPerformanceHistory(PerformanceModel model);
    }
}