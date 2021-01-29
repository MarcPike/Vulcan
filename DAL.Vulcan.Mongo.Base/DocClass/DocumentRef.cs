using System;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public class DocumentRef
    {
        public ObjectId Id { get; private set; }
        public Type Type { get; private set; }
        public string TypeFullName { get; private set; }

        public DocumentRef(BaseDocument doc)
        {
            Id = doc.Id;
            Type = doc.GetType();
            TypeFullName = Type.FullName;
        }
    }
}