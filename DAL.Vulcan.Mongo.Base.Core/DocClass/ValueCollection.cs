using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Base.Core.DocClass
{
    public class ValueCollection<T> : BaseDocument
    {
        public string Name { get; set; }
        public List<T> Values { get; set; } = new List<T>();


    }
}