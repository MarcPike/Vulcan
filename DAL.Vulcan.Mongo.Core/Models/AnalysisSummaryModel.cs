using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.Analysis;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class AnalysisSummaryModel
    {
        public AnalysisDetailModel Expired { get; set; }
        public AnalysisDetailModel Submitted { get; set; }
        public AnalysisDetailModel Won { get; set; }
        public AnalysisDetailModel Lost { get; set; }

        //public List<ProductWinLossHistory> Quotes
        //{
        //    get
        //    {
        //        var result = new List<ProductWinLossHistory>();
        //        result.AddRange(Submitted.Quotes);
        //        result.AddRange(Won.Quotes);
        //        result.AddRange(Lost.Quotes);
        //        result.AddRange(Expired.Quotes);
        //        return result;
        //    }
        //}

        public int Count
        {
            get { return Expired.Count + Submitted.Count + Won.Count + Lost.Count; }
        }

        public decimal TotalCost
        {
            get { return Expired.TotalCost + Submitted.TotalCost + Won.TotalCost + Lost.TotalCost; }
        }

        public decimal TotalPrice
        {
            get { return Expired.TotalPrice + Submitted.TotalPrice + Won.TotalPrice + Lost.TotalPrice; }
        }

        public decimal TotalAdditionalServiceCost
        {
            get
            {
                return Expired.TotalAdditionalServiceCost + Submitted.TotalAdditionalServiceCost +
                       Won.TotalAdditionalServiceCost + Lost.TotalAdditionalServiceCost;
            }
        }

        public decimal TotalAdditionalServicePrice
        {
            get
            {
                return Expired.TotalAdditionalServicePrice + Submitted.TotalAdditionalServicePrice +
                       Won.TotalAdditionalServicePrice + Lost.TotalAdditionalServicePrice;
            }
        }

        public decimal TotalKerfCost
        {
            get { return Expired.TotalKerfCost + Submitted.TotalKerfCost + Won.TotalKerfCost + Lost.TotalKerfCost; }
        }

        public decimal WinPercent
        {
            get
            {
                if (Count == 0) return 0;

                if (Won.Count == 0) return 0;

                return ((Count / Won.Count) * 100);
            }
        }
        public decimal LossPercent
        {
            get
            {
                if (Count == 0) return 0;

                if (Lost.Count == 0) return 0;

                return ((Count / Lost.Count) * 100);
            }
        }

        public decimal ReplenishCost => 0;
    }
}