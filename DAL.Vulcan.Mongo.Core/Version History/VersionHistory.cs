using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;

namespace DAL.Vulcan.Mongo.Core.Version_History
{
    public class VersionHistory : BaseDocument
    {
        public DateTime WorkStarTime { get; set; } = DateTime.Now.Date;
        public DateTime? ReleaseDate { get; set; } = null;
        public string Label { get; set; } = string.Empty;
        public string ReleaseNotes { get; set; } = string.Empty;
        public List<BugFix> BugFixes { get; set; } = new List<BugFix>();
        public List<Feature> Features { get; set; } = new List<Feature>();

    }

    public class BugFix
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime ReportDate { get; set; } = DateTime.Now.Date;
        public CrmUserRef ReportUser { get; set; }
        public string Notes { get; set; }
        public string Resolution { get; set; } = string.Empty;
        public bool Open { get; set; } = true;
        public int PercentComplete { get; set; } = 0;

        public BugFix()
        {

        }

        public BugFix(CrmUserRef user, string notes)
        {
            ReportUser = user;
            Notes = notes;
        }


    }

    public class Feature: BugFix
    {
        public Feature()
        {

        }

        public Feature(CrmUserRef user, string notes) : base(user, notes)
        {

        }
    }


}
