using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.CRM
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