using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DAL.Vulcan.Mongo.DocClass.CRM;
using DAL.Vulcan.Mongo.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.StockItemCacheLog
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
