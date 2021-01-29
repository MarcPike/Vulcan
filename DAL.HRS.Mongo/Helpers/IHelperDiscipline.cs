using System.Collections.Generic;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperDiscipline
    {
        List<DisciplineGridModel> GetDisciplineGrid(string userId);
        (DisciplineModel Current, List<DisciplineModel> History) GetDisciplineModelsListForEmployee(string employeeId);
        (DisciplineModel Current, List<DisciplineModel> History) SaveDiscipline(DisciplineModel model);
        DisciplineModel ModifyDisciplineHistory(DisciplineModel model);
    }
}