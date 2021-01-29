using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.HRS.Mongo.DocClass.Training
{
    public class TrainingHours : ValueCollection<decimal>
    {
        public TrainingHours()
        {
            Name = "TrainingHours";
        }
    }
}