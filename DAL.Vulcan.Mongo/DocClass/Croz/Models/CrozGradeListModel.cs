using System.Collections.Generic;
using MongoDB.Driver;

namespace DAL.Vulcan.Mongo.DocClass.Croz.Models
{
    public class CrozGradeListModel
    {
        public string Region { get; set; } = string.Empty;
        public List<CrozGrade> Grades { get; set; } = new List<CrozGrade>();
        public string UserToken { get; set; }
        public CrozGradeListModel()
        {
            
        }

        public CrozGradeListModel(string userToken, CrozGradeList list)
        {
            Region = list.Region;
            Grades = list.Grades;
            UserToken = userToken;
        }

        public CrozGradeListModel(string userToken, string region)
        {
            Region = region;
            var value = CrozGradeList.Helper.
                Find(x => x.Region == region).FirstOrDefault();

            if (value != null)
            {
                Grades = value.Grades;
            }

            UserToken = userToken;
        }

        public static List<CrozGradeListModel> GetAll(string userToken)
        {
            var result = new List<CrozGradeListModel>();
            foreach (var crozGradeList in CrozGradeList.Helper.GetAll())
            {
                result.Add(new CrozGradeListModel(userToken, crozGradeList));
            }
            return result;
        }
    }
}