using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Queries;
using DAL.Vulcan.Mongo.Core.DocClass.CRM;
using DAL.Vulcan.Mongo.Core.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Core.DocClass.StockItemCacheLog
{
    public class StockItemCacheLogValue : BaseDocument
    {
        public static MongoRawQueryHelper<StockItemCacheLogValue> Helper = new MongoRawQueryHelper<StockItemCacheLogValue>();
        public CrmUserRef SalesPerson { get; set; }
        public StockItemsQueryModel StockItemsQueryModel { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime OccurredAt { get; set; } = DateTime.Now;
    }

}
