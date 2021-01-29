using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using MongoDB.Driver;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class BbsInfluenceBehaviorType : BaseDocument
    {
        public static MongoRawQueryHelper<BbsInfluenceBehaviorType> Helper = new MongoRawQueryHelper<BbsInfluenceBehaviorType>();
        public string Name { get; set; }
        public bool SendEmail { get; set; }

        public BbsInfluenceBehaviorTypeRef AsBbsInfluenceBehaviorTypeRef()
        {
            return new BbsInfluenceBehaviorTypeRef(this);
        }

        public static BbsInfluenceBehaviorTypeRef GetBbsInfluenceBehaviorTypeRefForName(string name)
        {
            var value = Helper.Find(x => x.Name == name).FirstOrDefault();
            if (value == null)
            {
                value = new BbsInfluenceBehaviorType()
                {
                    Name = name,
                    SendEmail = false
                };
                Helper.Upsert(value);
                //throw new Exception($"No Behavior Type found for Name: {name}");
            }

            return value.AsBbsInfluenceBehaviorTypeRef();
        }
    }

    public class BbsInfluenceBehaviorTypeRef : ReferenceObject<BbsInfluenceBehaviorType>
    {
        public string Name { get; set; }
        public bool SendEmail { get; set; }

        public BbsInfluenceBehaviorTypeRef()
        {
            
        }

        public BbsInfluenceBehaviorTypeRef(BbsInfluenceBehaviorType t)
        {
            Name = t.Name;
            SendEmail = t.SendEmail;
        }

        public BbsInfluenceBehaviorType AsBbsInfluenceBehaviorType()
        {
            return ToBaseDocument();
        }
    }
}
