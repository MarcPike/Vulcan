using DAL.Vulcan.Mongo.Base.DocClass;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BI.DAL.Mongo.BiUserObjects;
using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace BI.DAL.Mongo.BiQueries
{
    public class BiQueryBase : BaseDocument
    {
        public static MongoRawQueryHelper<BiQueryBase> Helper = new MongoRawQueryHelper<BiQueryBase>();
        public string QueryType { get; set; }
        public string Name { get; set; }
        public BiUserRef User {get;set;}
        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();
        public List<string> CoidList { get; set; } = new List<string>();
        public List<string> Warehouses { get; set; } = new List<string>();
        public List<string> MetalTypes { get; set; } = new List<string>();
        public List<string> ProductCodes { get; set; } = new List<string>();
        public DateTime MinDate { get; set; } = DateTime.MinValue;
        public DateTime MaxDate { get; set; } = DateTime.MaxValue;
        public Dictionary<string,string> AdditionalStringValues { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, int> AdditionalIntegerValues { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, DateTime> AdditionalDateValues { get; set; } = new Dictionary<string, DateTime>();

    }
}
