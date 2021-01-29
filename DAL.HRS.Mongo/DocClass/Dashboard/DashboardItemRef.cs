using System;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Dashboard
{
    public class DashboardItemRef : ReferenceObject<DashboardItem>
    {
        public DateTime DueDate { get; set; }
        public bool Dismissed { get; set; }
        public DashboardItemRef(DashboardItem d)
        {
            DueDate = d.DueDate;
            Dismissed = d.Dismissed;
        }

        public DashboardItem AsDashboardItem()
        {
            return ToBaseDocument();
        }
    }
}