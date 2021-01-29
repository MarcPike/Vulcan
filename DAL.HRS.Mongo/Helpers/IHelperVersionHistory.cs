using System.Collections.Generic;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public interface IHelperVersionHistory
    {
        List<VersionHistoryModel> GetAllHistory();
        VersionHistoryModel GetCurrentVersion();
    }
}