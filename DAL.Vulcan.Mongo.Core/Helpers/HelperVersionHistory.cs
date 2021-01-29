using DAL.Vulcan.Mongo.Base.Core.Repository;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.Version_History;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public class HelperVersionHistory : HelperBase, IHelperVersionHistory
    {
        public VersionHistory GetCurrentVersionHistory(string application, string userId)
        {
            var rep = new RepositoryBase<VersionHistory>();
            var currentVersion = rep.AsQueryable().SingleOrDefault(x => x.ReleaseDate == null);
                
            if (currentVersion == null)
            {
                currentVersion = new VersionHistory()
                {
                    Label = "Ongoing Maintenance"
                };
                rep.Upsert(currentVersion);
            };
            return currentVersion;
        }

        public VersionHistoryModel GetCurrentVersionHistoryModel(string application, string userId)
        {
            var currentVersion = GetCurrentVersionHistory(application, userId);
            return new VersionHistoryModel(application, userId, currentVersion);
        }


        public List<VersionHistoryModel> GetAllVersionHistory(string application, string userId)
        {
            var rep = new RepositoryBase<VersionHistory>();
            var versionHistory = rep.AsQueryable().OrderByDescending(x=>x.ReleaseDate).ToList();
            var result = new List<VersionHistoryModel>();
            if (!versionHistory.Any())
            {
                versionHistory.Add(GetCurrentVersionHistory(application,userId)); 
            }

            result.AddRange(versionHistory.Select(x=> new VersionHistoryModel(application, userId, x)).OrderByDescending(x=>x.ReleaseDate));

            return result;
        }

        public void AddBugReport(string application, string userId, CrmUserRef user, string notes)
        {
            var currentVersion = GetCurrentVersionHistory(application, userId);

            var newBug = new BugFix(user, notes);
            currentVersion.BugFixes.Add(newBug);

            currentVersion.SaveToDatabase();
        }

        public void AddFeature(string application, string userId, CrmUserRef user, string notes)
        {
            var currentVersion = GetCurrentVersionHistory(application, userId);

            var feature = new Feature(user, notes);
            currentVersion.Features.Add(feature);

            currentVersion.SaveToDatabase();
        }


        public VersionHistoryModel SaveVersionHistory(VersionHistoryModel model)
        {
            var rep = new RepositoryBase<VersionHistory>();
            var versionHistory = rep.Find(model.Id);
            if (versionHistory == null) throw new Exception("Version History not found");

            foreach (var bugFixModel in model.BugFixes)
            {
                var bugFix = versionHistory.BugFixes.SingleOrDefault(x => x.Id == Guid.Parse(bugFixModel.Id));
                if (bugFix != null)
                {
                    bugFix.Notes = bugFixModel.Notes;
                    bugFix.Open = bugFixModel.Open;
                    bugFix.PercentComplete = bugFixModel.PercentComplete;
                    bugFix.Resolution = bugFixModel.Resolution;
                }
            }
            foreach (var featureModel in model.Features)
            {
                var feature = versionHistory.Features.SingleOrDefault(x => x.Id == Guid.Parse(featureModel.Id));
                if (feature != null)
                {
                    feature.Notes = featureModel.Notes;
                    feature.Open = featureModel.Open;
                    feature.PercentComplete = featureModel.PercentComplete;
                    feature.Resolution = featureModel.Resolution;
                }
            }

            versionHistory.Label = model.Label;
            versionHistory.ReleaseDate = model.ReleaseDate;
            versionHistory.ReleaseNotes = model.ReleaseNotes;
            versionHistory.WorkStarTime = model.WorkStartTime;
            rep.Upsert(versionHistory);

            return new VersionHistoryModel(model.Application, model.UserId, versionHistory);

        }

    }


}
