using System;
using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using DAL.HRS.Mongo.DocClass.Training;

namespace DAL.HRS.Mongo.Models
{
    public class ExpiringEventModel
    {
        public DateTime DueDate;
        public EmployeeRef Employee;
        public LocationRef Location;
        public TrainingCourseRef TrainingCourse;
        public string Type;
        public int DaysRemaining => (DueDate - DateTime.Now.Date).Days;
    }
}