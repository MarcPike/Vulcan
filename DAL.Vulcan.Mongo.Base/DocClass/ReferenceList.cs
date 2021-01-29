using System.Collections.Generic;
using System.Linq;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public class ReferenceList<TBaseDocument, TReferenceObject> : List<TReferenceObject>
        where TBaseDocument : BaseDocument
        where TReferenceObject : ReferenceObject<TBaseDocument>
    {

        public void ResyncWithList(List<TReferenceObject> documents)
        {
            Clear();
            AddListOfReferenceObjects(documents);
        }


        public void AddListOfReferenceObjects(List<TReferenceObject> documents)
        {
            foreach (var doc in documents)
            {
                Add(doc);
            }
        }

        public void AddReferenceObject(TReferenceObject document)
        {
            if (this.All(x => x.Id != document.Id.ToString()))
            {
                this.Add(document);
            }
        }

        public void RemoveDocumentRef(TReferenceObject document)
        {
            var docToRemove = this.FirstOrDefault(x => x.Id == document.Id.ToString());
            if (docToRemove != null)
            {
                this.Remove(docToRemove);
            }
        }

        public List<TBaseDocument> AsListOfBaseDocuments()
        {
            var result = new List<TBaseDocument>();
            foreach (var referenceObject in this)
            {
                result.Add(referenceObject.ToBaseDocument());
            }
            return result;
        }

    }
}