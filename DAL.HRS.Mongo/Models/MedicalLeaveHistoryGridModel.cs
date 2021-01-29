using DAL.Common.DocClass;
using DAL.HRS.Mongo.DocClass.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.Models
{
    public class MedicalLeaveHistoryGridModel : EmployeeMedicalLeaveHistory
    {
        private LocationRef location;
        private List<EmployeeMedicalLeaveHistory> leaveHistory;

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }

        public DateTime? FromDateConverted { get; set; }
       

        public int DaysAway
        {
            get
            {
                try
                {
                    
                    DateTime now = DateTime.Today;
                    int days = (now - FromDateConverted.Value).Days;

                    return days;

                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }


        public MedicalLeaveHistoryGridModel()
        {

        }



        public MedicalLeaveHistoryGridModel(string firstName, string lastName, LocationRef location)
        {
            FirstName = firstName;
            LastName = lastName;
            this.location = location;
        }            
    }
}
