using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Base.Core.Guid_Conversions
{
    public static class GuidConversions
    {
        public static Guid AsGuid(this ObjectId oid)
        {
            var bytes = oid.ToByteArray().Concat(new byte[] { 5, 5, 5, 5 }).ToArray();
            Guid gid = new Guid(bytes);
            return gid;
        }

        /// <summary>
        /// Only Use to convert a Guid that was once an ObjectId
        /// </summary>
        public static ObjectId AsObjectId(this Guid gid)
        {
            var bytes = gid.ToByteArray().Take(12).ToArray();
            var oid = new ObjectId(bytes);
            return oid;
        }
    }
}
