using System.Collections.Generic;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;

namespace DAL.Vulcan.Mongo.DocClass.Croz
{
    public class CrozGradeList : BaseDocument
    {
        public static MongoRawQueryHelper<CrozGradeList> Helper = new MongoRawQueryHelper<CrozGradeList>();
        public string Region { get; set; }
        public List<CrozGrade> Grades { get; set; } = new List<CrozGrade>();
    }
}