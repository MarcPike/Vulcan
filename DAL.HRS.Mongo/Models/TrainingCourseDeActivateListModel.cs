using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.HRS.Mongo.DocClass.Hrs_User;

namespace DAL.HRS.Mongo.Models
{
    public class TrainingCourseDeActivateListModel
    {
        public HrsUserRef UpdateUser { get; set; }
        public List<string> TrainingCourseIdList { get; set; } = new List<string>();
    }
}
