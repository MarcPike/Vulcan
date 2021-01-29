﻿using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class LostReasonUpdate: BaseDocument
    {
        public static MongoRawQueryHelper<LostReasonUpdate> Helper = new MongoRawQueryHelper<LostReasonUpdate>();

        public ObjectId QuoteId { get; set; }
        public ObjectId QuoteItemId { get; set; }
        public ObjectId OldLostReasonId { get; set; } 
        public string OldLostReason { get; set; }
        public ObjectId NewLostReasonId { get; set; }
        public string NewLostReason { get; set; }
    }
}