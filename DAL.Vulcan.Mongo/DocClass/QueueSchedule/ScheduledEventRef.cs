﻿using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.QueueSchedule
{
    [BsonIgnoreExtraElements]
    public class ScheduledEventRef : ReferenceObject<ScheduledEvent>
    {
        public ScheduledEventRef()
        {
            
        }

        public ScheduledEventRef(ScheduledEvent doc) : base(doc)
        {
        }

        public ScheduledEvent AScheduledEvent()
        {
            return ToBaseDocument();
        }
    }
}