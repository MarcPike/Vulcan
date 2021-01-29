using DAL.Vulcan.Mongo.DocClass.Croz.Models;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Models;

namespace DAL.Vulcan.Mongo.Helpers
{
    public interface IHelperCroz
    {
        List<CrozGradeListModel> GetAllCrozGradeLists(string userToken);
        List<CrozProductionCostListModel> GetAllCrozProductionCostLists(string userToken);
        CrozGradeListModel GetCrozGradeListForRegion(string userToken, string region);
        CrozProductionCostListModel GetCrozProductionCostListForRegion(string userToken, string region);
        void RemoveCrozGradeList(string region);
        void RemoveCrozProductionCostList(string region);
        CrozGradeListModel SaveCrozGradeList(CrozGradeListModel model);
        CrozProductionCostListModel SaveCrozProductionCostList(CrozProductionCostListModel model);

        CrozCalcItemModel CreateNewCalcItemModel(string application, string userId, string coid,
            string displayCurrency);

        CrozCalcItemModel SaveCalcItemModel(CrozCalcItemModel model);
    }
}