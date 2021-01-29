using System;
using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    public class Note : IObjectWithGuidForId
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime NoteDate { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;
        public List<string> SearchTags { get; set; } = new List<string>();

    }
}