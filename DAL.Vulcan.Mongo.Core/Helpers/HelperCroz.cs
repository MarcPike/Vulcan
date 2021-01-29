using DAL.Vulcan.Mongo.Core.DocClass.Croz;
using DAL.Vulcan.Mongo.Core.DocClass.Croz.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Core.DocClass.Quotes;
using DAL.Vulcan.Mongo.Core.Models;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public class HelperCroz : IHelperCroz
    {
        public CrozGradeListModel GetCrozGradeListForRegion(string userToken, string region)
        {
            return new CrozGradeListModel(userToken, region);
        }

        public List<CrozGradeListModel> GetAllCrozGradeLists(string userToken)
        {
            return CrozGradeListModel.GetAll(userToken);
        }

        public CrozGradeListModel SaveCrozGradeList(CrozGradeListModel model)
        {
            var gradeList =
                CrozGradeList.Helper.Find(x => x.Region == model.Region).FirstOrDefault() ??
                new CrozGradeList()
                {
                    Region = model.Region
                };
            gradeList.Grades = model.Grades;
            CrozGradeList.Helper.Upsert(gradeList);
            return new CrozGradeListModel(model.UserToken, gradeList);
        }

        public void RemoveCrozGradeList(string region)
        {
            var gradeList =
                CrozGradeList.Helper.Find(x => x.Region == region).FirstOrDefault();
            if (gradeList != null)
            {
                CrozGradeList.Helper.DeleteOne(gradeList.Id);
            }
        }

        public CrozProductionCostListModel GetCrozProductionCostListForRegion(string userToken, string region)
        {
            return new CrozProductionCostListModel(userToken, region);
        }

        public List<CrozProductionCostListModel> GetAllCrozProductionCostLists(string userToken)
        {
            return CrozProductionCostListModel.GetAll(userToken);
        }

        public CrozProductionCostListModel SaveCrozProductionCostList(CrozProductionCostListModel model)
        {
            var costList =
                CrozProductionCostList.Helper.Find(x => x.Region == model.Region).FirstOrDefault() ??
                new CrozProductionCostList()
                {
                    Region = model.Region
                };
            costList.ProductionCosts = model.ProductionCosts;
            CrozProductionCostList.Helper.Upsert(costList);
            return new CrozProductionCostListModel(model.UserToken, model.Region);
        }

        public CrozCalcItemModel CreateNewCalcItemModel(string application, string userId, string coid, string displayCurrency)
        {
            return CrozCalcItemModel.CreateNew(application, userId, coid, displayCurrency);
        }

        public CrozCalcItemModel SaveCalcItemModel(CrozCalcItemModel model)
        {
            return CrozCalcItem.Save(model);
        }

        public void RemoveCrozProductionCostList(string region)
        {
            var gradeList =
                CrozProductionCostList.Helper.Find(x => x.Region == region).FirstOrDefault();
            if (gradeList != null)
            {
                CrozProductionCostList.Helper.DeleteOne(gradeList.Id);
            }
        }
    }
}
