using DAL.Vulcan.Mongo.Base.Core.DocClass;
using DAL.Vulcan.Mongo.Base.Core.Queries;
using System.Collections.Generic;

namespace DAL.Vulcan.Mongo.Core.DocClass.Croz
{
    public class CrozGradeList : BaseDocument
    {
        public static MongoRawQueryHelper<CrozGradeList> Helper = new MongoRawQueryHelper<CrozGradeList>();
        public string Region { get; set; }
        public List<CrozGrade> Grades { get; set; } = new List<CrozGrade>();
    }
}