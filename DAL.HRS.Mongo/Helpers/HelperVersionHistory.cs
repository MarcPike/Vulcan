using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Versioning;
using DAL.HRS.Mongo.Models;

namespace DAL.HRS.Mongo.Helpers
{
    public class HelperVersionHistory : IHelperVersionHistory
    {
        public List<VersionHistoryModel> GetAllHistory()
        {
            return VersionHistory.Helper.GetAll().OrderBy(x => x.CreateDateTime)
                .Select(x => new VersionHistoryModel(x)).ToList();
        }

        public VersionHistoryModel GetCurrentVersion()
        {
            return VersionHistory.GetLatestVersion();
        }
    }
}
