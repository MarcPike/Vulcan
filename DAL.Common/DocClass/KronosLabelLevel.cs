using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Common.DocClass
{
    public class KronosLabelLevel : BaseDocument, ICommonDatabaseObject
    {
        public List<string> Values { get; set; } = new List<string>();

        public static MongoRawQueryHelper<KronosLabelLevel> Helper { get; set; } = new MongoRawQueryHelper<KronosLabelLevel>();
    }
}
