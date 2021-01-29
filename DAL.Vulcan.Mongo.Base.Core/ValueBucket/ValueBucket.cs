using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Core.DocClass;

namespace DAL.Vulcan.Mongo.Base.Core.ValueBucket
{
    public class ValueBucket<TBaseDocument, TGroupByType>: IDisposable
        where TBaseDocument : BaseDocument
    {
        public class BucketValue
        {
            public TGroupByType GroupBy { get; set; }
            public List<TBaseDocument> Documents { get; set; } = new List<TBaseDocument>();
        }

        private List<BucketValue> Results { get; } = new List<BucketValue>();

        public List<BucketValue> Execute(List<TBaseDocument> values, Func<TBaseDocument,TGroupByType> getValue)
        {
            Results.Clear();
            foreach (var baseDocument in values)
            {
                var bucketLabel = getValue.Invoke(baseDocument);
                var bucket = Results.SingleOrDefault(x => x.GroupBy.Equals(bucketLabel));
                if (bucket == null)
                {
                    bucket = new BucketValue()
                    {
                        GroupBy = bucketLabel
                    };
                    bucket.Documents.Add(baseDocument);
                    Results.Add(bucket);
                }
                else
                {
                    bucket.Documents.Add(baseDocument);
                }
            }
            
            return Results.OrderBy(x=>x.GroupBy).ToList();
        }

        public void Dispose()
        {
            Results.Clear();
        }
    }

}
