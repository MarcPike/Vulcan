using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.Performance;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.Models
{
    //public class PerformanceRateOutcomeModel
    //{
    //    public string Id { get; set; } 
    //    public string Comment { get; set; }
    //    public PropertyValueRef RatingOutcomeType { get; set; }

    //    public PerformanceRateOutcomeModel()
    //    {
    //    }

    //    public PerformanceRateOutcomeModel(PerformanceRateOutcome outcome)
    //    {
    //        Id = outcome.Id.ToString();
    //        Comment = outcome.Comment;
    //        RatingOutcomeType = outcome.RatingOutcomeType;

    //        //UpdatePropertyReferences.Execute(this);

    //    }

    //    public static List<PerformanceRateOutcomeModel> ConvertList(List<PerformanceRateOutcome> list)
    //    {
    //        var result = new List<PerformanceRateOutcomeModel>();

    //        if (list != null)
    //        {
    //            foreach (var i in list)
    //            {
    //                result.Add(new PerformanceRateOutcomeModel(i));
    //            }
    //        }

    //        return result;
    //    }

    //}
}