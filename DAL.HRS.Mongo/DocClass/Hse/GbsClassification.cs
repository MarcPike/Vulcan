using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class GhsClassificationType : BaseDocument
    {
        public int OldHrsId { get; set; }
        public string ImageName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCommentRequired { get; set; }

        public GhsClassificationTypeRef AsGhsClassificationTypeRef()
        {
            return new GhsClassificationTypeRef(this);
        }
    }
}
