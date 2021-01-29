using System.Collections.Generic;
using DAL.HRS.Mongo.DocClass.Training;

namespace DAL.HRS.Mongo.Models
{
    public class TrainingHoursModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<decimal> Values { get; set; }
        public TrainingHoursModel()
        {
        }

        public TrainingHoursModel(TrainingHours trainingHours)
        {
            Id = trainingHours.Id.ToString();
            Name = trainingHours.Name;
            Values = trainingHours.Values;
        }
    }
}