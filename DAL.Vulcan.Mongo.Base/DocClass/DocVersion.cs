using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Repository;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public class DocVersion
    {
        public string VersionId { get; set; } = "0.0";
        public DateTime ExecutedOn { get; set; } = DateTime.MinValue;
        public string Comments { get; set; } = string.Empty;

    }
}
