using System;
using DAL.Vulcan.Mongo.Base.DocClass;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.CRM
{
    [BsonIgnoreExtraElements]

    public class ActionRef : ReferenceObject<Action>
    {
        public string Label { get; set; }
        public int PercentComplete { get; set; }
        public string Type { get; set; }
        public DateTime DueDate { get; set; }

        public ActionRef()
        {
            
        }

        public ActionRef(Action action) : base(action)
        {
            Type = action.ActionType.ToString();
        }

        public Action AsAction()
        {
            return ToBaseDocument();
        }
    }
}
