using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public class GuidList<T>: List<T>, ISupportInitialize
        where T : IObjectWithGuidForId
    {

        [BsonExtraElements]
        public IDictionary<string, object> ExtraElements { get; set; }


        public void Save(T value)
        {
            var noteFound = this.SingleOrDefault(x => x.Id == value.Id);
            if (noteFound == null)
            {
                this.Add(value);
            }
            else
            {
                var indexOf = IndexOf(noteFound);
                this[indexOf] = value;
            }
        }

        public void ResynchWithList(List<T> modelValues)
        {
            if (modelValues == null)
            {
                return;
            }

            foreach (var value in modelValues)
            {
                Save(value);
            }

            foreach (var value in this.ToList())
            {
                if (modelValues.All(x => x.Id != value.Id))
                {
                    Remove(value);
                }
            }

        }

        public virtual void BeginInit()
        {
        }

        public virtual void EndInit()
        {
        }
    }
}
