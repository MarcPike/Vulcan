using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Core.Version_History;

namespace DAL.Vulcan.Mongo.Core.Models
{
    public class VersionHistoryModel
    {
        public string Application { get; set; }
        public string UserId { get; set; }

        public string Id { get; set; }
        public string Label { get; set; }
        public string ReleaseNotes { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public List<BugFixModel> BugFixes { get; set; }
        public List<FeatureModel> Features { get; set; }
        public DateTime WorkStartTime { get; set; }

        public VersionHistoryModel()
        {
        }

        public VersionHistoryModel(string application, string userId, VersionHistory hist)
        {
            Id = hist.Id.ToString();
            Label = hist.Label;
            ReleaseDate = hist.ReleaseDate;
            ReleaseNotes = hist.ReleaseNotes;
            WorkStartTime = hist.WorkStarTime;
            Features = hist.Features.Select(x=> new FeatureModel(x)).ToList();
            BugFixes = hist.BugFixes.Select(x => new BugFixModel(x)).ToList();

            Application = application;
            UserId = userId;
        }
    }
}
