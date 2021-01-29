using System;

namespace DAL.Vulcan.Mongo.Base.Core.DocClass
{
    public class DocVersion
    {
        public string VersionId { get; set; } = "0.0";
        public DateTime ExecutedOn { get; set; } = DateTime.MinValue;
        public string Comments { get; set; } = string.Empty;

    }
}
