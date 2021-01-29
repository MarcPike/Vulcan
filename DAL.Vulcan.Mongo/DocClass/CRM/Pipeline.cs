using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class Pipeline: BaseDocument
    {
        public string Label { get; set; }
        public List<PipelineStage> Stages { get; set; } = new List<PipelineStage>();

        public static void InitializeWithDefaults()
        {
            var rep = new RepositoryBase<Pipeline>();
            var sales = rep.AsQueryable().SingleOrDefault(x => x.Label == "Sales");
            if (sales == null)
            {
                sales = new Pipeline()
                {
                    Label = "Sales",
                    Stages = new List<PipelineStage>()
                    {
                        new PipelineStage() {Label = "Qualified", WinProbability = 5},
                        new PipelineStage() {Label = "Follow-up", WinProbability = 10},
                        new PipelineStage() {Label = "Presentation", WinProbability = 20},
                        new PipelineStage() {Label = "CrmQuote Sent", WinProbability = 40},
                        new PipelineStage() {Label = "Negotiation", WinProbability = 80},
                    }
                };
                rep.Upsert(sales);
            }

            var business = rep.AsQueryable().SingleOrDefault(x => x.Label == "Business Development");
            if (business == null)
            {

                business = new Pipeline()
                {
                    Label = "Business Development",
                    Stages = new List<PipelineStage>()
                    {
                        new PipelineStage() {Label = "First Meeting", WinProbability = 10},
                        new PipelineStage() {Label = "Partner Meeting", WinProbability = 25},
                        new PipelineStage() {Label = "Negotiation", WinProbability = 50},
                        new PipelineStage() {Label = "Term Sheet", WinProbability = 75},
                    }
                };

                rep.Upsert(business);
            }

        }
    }

    public class PipelineStage
    {
        public string Label { get; set; }
        public int WinProbability { get; set; }
    }
}
