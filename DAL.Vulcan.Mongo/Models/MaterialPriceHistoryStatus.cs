using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Models
{
    public class MaterialPriceHistoryStatus
    {
        public List<MaterialPriceHistoryValue> AllItems { get; set; } = new List<MaterialPriceHistoryValue>();
        public List<MaterialPriceHistoryValue> DraftItems { get; set; } = new List<MaterialPriceHistoryValue>();
        public List<MaterialPriceHistoryValue> PendingItems { get; set; } = new List<MaterialPriceHistoryValue>();
        public List<MaterialPriceHistoryValue> WonItems { get; set; } = new List<MaterialPriceHistoryValue>();
        public List<MaterialPriceHistoryValue> LostItems { get; set; } = new List<MaterialPriceHistoryValue>();

        public decimal AveragePricePerKilogram => (AllItems.Count == 0) ? 0 : AllItems.Average(x => x.PricePerKilogram);
        public decimal AveragePricePerPound => (AllItems.Count == 0) ? 0 : AllItems.Average(x => x.PricePerPound);
        public decimal AveragePricePerFoot => (AllItems.Count == 0) ? 0 : AllItems.Average(x => x.PricePerFoot);
        public decimal AveragePricePerInch => (AllItems.Count == 0) ? 0 : AllItems.Average(x => x.PricePerInch);

        public MaterialPriceHistoryStatus(List<MaterialPriceHistoryValue> values)
        {
            AllItems.AddRange(values.OrderByDescending(x=>x.ReportDate));
            DraftItems.AddRange(values.Where(x=>x.Status == PipelineStatus.Draft).OrderByDescending(x=>x.ReportDate).ToList());
            PendingItems.AddRange(values.Where(x => x.Status == PipelineStatus.Submitted).OrderByDescending(x => x.ReportDate).ToList());
            WonItems.AddRange(values.Where(x => x.Status == PipelineStatus.Won).OrderByDescending(x => x.ReportDate).ToList());
            LostItems.AddRange(values.Where(x => x.Status == PipelineStatus.Loss).OrderByDescending(x => x.ReportDate).ToList());
        }

        public MaterialPriceHistoryStatus()
        {
        }

        public void AddItem(MaterialPriceHistoryValue value)
        {
            AllItems.Add(value);
            switch (value.Status
)
            {
                case PipelineStatus.Draft:
                    DraftItems.Add(value);
                    break;
                case PipelineStatus.Submitted:
                    PendingItems.Add(value);
                    break;
                case PipelineStatus.Won:
                    WonItems.Add(value);
                    break;
                case PipelineStatus.Loss:
                    LostItems.Add(value);
                    break;
            }
        }

    }
}