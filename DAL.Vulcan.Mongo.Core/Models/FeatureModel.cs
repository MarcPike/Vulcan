using System;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Version_History;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class FeatureModel
    {
        public string Id;
        public DateTime ReportDate;
        public CrmUserRef ReportUser;
        public string Notes;
        public string Resolution;
        public bool Open;
        public int PercentComplete;

        public FeatureModel()
        {
        }

        public FeatureModel(Feature feature)
        {
            Id = feature.Id.ToString();
            ReportDate = feature.ReportDate;
            ReportUser = feature.ReportUser;
            Notes = feature.Notes;
            Resolution = feature.Resolution;
            Open = feature.Open;
            PercentComplete = feature.PercentComplete;
        }
    }
}