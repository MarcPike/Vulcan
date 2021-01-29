using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.Models;
using DAL.Vulcan.Mongo.Base.Context;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.DocClass.Versioning
{
    public class VersionHistory : BaseDocument
    {
        public static MongoRawQueryHelper<VersionHistory> Helper = new MongoRawQueryHelper<VersionHistory>();
        public string VersionId { get; set; } 
        public List<string> Features { get; set; } = new List<string>();
        public string Environment { get; set; }
        
        public static void AddNewVersion(string versionId, List<string> features)
        {
            Helper.Upsert(new VersionHistory()
            {
                VersionId = versionId,
                Features = features,
                Environment = EnvironmentSettings.CurrentEnvironment.ToString(),

            });
        }

        public static VersionHistoryModel GetLatestVersion()
        {
            return Helper.GetAll().OrderBy(x => x.CreateDateTime).Select(x=> new VersionHistoryModel(x)).LastOrDefault();
        }
    }

}
