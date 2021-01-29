using System;
using DAL.Vulcan.Mongo.Base.DocClass;
using Microsoft.Exchange.WebServices.Data;
using MongoDB.Bson.Serialization.Attributes;

namespace DAL.Vulcan.Mongo.DocClass.Email
{
    [BsonIgnoreExtraElements]
    public class EmailRef: ReferenceObject<Email>
    {
        public EmailRef(Email email):base(email)
        {
        }

        public EmailRef()
        {
            
        }

        public Email AsEmail()
        {
            return ToBaseDocument();
        }
    }
}