using DAL.HRS.Mongo.DocClass.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.HRS.Mongo.Models
{
    public class TrainingEventAttendee
    {
        public Guid Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string LocationOffice { get; set; }
        public string AttendeeName { get; set; }
        public string PayrollId { get; set; }
        public PropertyValueRef GroupClassification { get; set; }
        public DateTime DueDate { get; set; }
        //public int DaysRemaining { get; set; }

        public int DaysRemaining
        {
            get
            {
                if (DueDate != null)
                {
                    return (int)(DueDate - DateTime.Now).Days;
                }
                else
                {
                    return 0;
                }
            }
        }

    }
}
