using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.Base.DocClass
{
    public class BaseValueObject: ISupportInitialize
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public DocVersion Version { get; set; }

        [BsonExtraElements]
        public IDictionary<string, object> ExtraElements { get; set; }

        public virtual void BeginInit()
        {
        }

        public virtual void EndInit()
        {
        }

    }

    public class BaseUtils
    {
    }

}
