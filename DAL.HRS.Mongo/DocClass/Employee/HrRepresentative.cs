using DAL.Common.DocClass;
using DAL.Vulcan.Mongo.Base.DocClass;
using DAL.Vulcan.Mongo.Base.Queries;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DAL.HRS.Mongo.DocClass.Employee
{
    public class HrRepresentative : BaseDocument
    {
        public static MongoRawQueryHelper<HrRepresentative> Helper = new MongoRawQueryHelper<HrRepresentative>();
        public LocationRef Location { get; set; }
        public EmployeeRef Representative { get; set; }

        public static EmployeeRef GetRepresentativeForLocation(LocationRef location)
        {
            if (location == null)
                return null;

            return Helper.Find(x => x.Location.Id == location.Id).FirstOrDefault()?.Representative;
        }

        public static List<HrRepresentative> GetAll()
        {
            return Helper.GetAll();
        }
    }

}