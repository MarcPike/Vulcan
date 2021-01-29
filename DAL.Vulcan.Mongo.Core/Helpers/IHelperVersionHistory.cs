using System.Collections.Generic;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;
using DAL.Vulcan.Mongo.Core.Version_History;

namespace DAL.Vulcan.Mongo.Core.Helpers
{
    public interface IHelperVersionHistory
    {
        void AddBugReport(string application, string userId, CrmUserRef user, string notes);
        void AddFeature(string application, string userId, CrmUserRef user, string notes);
        List<VersionHistoryModel> GetAllVersionHistory(string application, string userId);
        VersionHistory GetCurrentVersionHistory(string application, string userId);
        VersionHistoryModel GetCurrentVersionHistoryModel(string application, string userId);
        VersionHistoryModel SaveVersionHistory(VersionHistoryModel model);
    }
}