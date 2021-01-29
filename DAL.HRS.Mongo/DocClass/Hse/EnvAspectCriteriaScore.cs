using DAL.Vulcan.Mongo.Base.DocClass;
using System.Collections.Generic;
using DAL.Common.DocClass;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class EnvAspectCriteriaScore : BaseDocument
    {
        public int OldHrsId { get; set; }
        public string Comments { get; set; }
        public List<LocationRef> Locations { get; set; } = new List<LocationRef>();
        public List<Review> Reviews { get; set; } = new List<Review>();

        public AssignedWorkAreas WorkAreas = new AssignedWorkAreas();
        public AspectCode AspectCode { get; set; } = new AspectCode();
        public List<AspectScoreHistory> History { get; set; } = new List<AspectScoreHistory>();
        public List<AspectScore> Scores { get; set; } = new List<AspectScore>();
        public AspectType AspectType { get; set; } = new AspectType();
    }

    public class AspectType
    {
        public string Activity { get; set; }
        public string Description { get; set; }
        public string Impacts { get; set; }
        public string Other { get; set; }

    }
}
