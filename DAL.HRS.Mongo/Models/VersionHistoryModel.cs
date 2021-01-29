using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Versioning;
using DAL.Vulcan.Mongo.Base.Context;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.HRS.Mongo.Models
{
    public class VersionHistoryModel
    {
        public string VersionId { get; set; }
        public List<string> Features { get; set; } = new List<string>();
        public string Environment { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Pubished { get; set; }


        public VersionHistoryModel()
        {

        }

        public VersionHistoryModel(VersionHistory ver)
        {
            VersionId = ver.VersionId;
            Features = ver.Features;
            Environment = EnvironmentSettings.CurrentEnvironment.ToString();
            Pubished = ver.CreateDateTime;
        }
    }
}
