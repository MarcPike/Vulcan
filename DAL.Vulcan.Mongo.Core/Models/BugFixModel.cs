using System;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Version_History;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class BugFixModel 
    {
        public string Id;
        public DateTime ReportDate;
        public CrmUserRef ReportUser;
        public string Notes;
        public string Resolution;
        public bool Open;
        public int PercentComplete;

        public BugFixModel()
        {
        }

        public BugFixModel(BugFix fix) 
        {
            Id = fix.Id.ToString();
            ReportDate = fix.ReportDate;
            ReportUser = fix.ReportUser;
            Notes = fix.Notes;
            Resolution = fix.Resolution;
            Open = fix.Open;
            PercentComplete = fix.PercentComplete;
        }
    }
}