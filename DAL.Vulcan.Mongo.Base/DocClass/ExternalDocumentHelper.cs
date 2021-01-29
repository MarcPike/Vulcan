using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public class ExternalDocumentHelper<TBaseDocument, T> where TBaseDocument : BaseDocument
    {
        public List<T> GetDocumentsFor(TBaseDocument doc, string name)
        {
            var result = new List<T>();
            var docList = doc.GetExternalDocumentList(name);
            if (docList == null) return result;

            foreach (var bsonDocument in docList.Documents)
            {
                result.Add(BsonSerializer.Deserialize<T>(bsonDocument));
            }

            return result;
        }

        public void SaveDocumentsFor(TBaseDocument doc, string name, List<T> saveDocs)
        {
            RemoveDocumentsFor(doc, name);
            var externalDocs = new ExternalDocumentList() {Name = name};

            foreach (var saveDoc in saveDocs)
            {
                externalDocs.Documents.Add(saveDoc.ToBsonDocument());
            }
            doc.ExternalDocuments.Add(externalDocs);
        }

        public void RemoveDocumentsFor(TBaseDocument doc, string name)
        {
            doc.RemoveExternalDocumentList(name);
        }

    }

}