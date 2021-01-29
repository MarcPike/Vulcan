using System;

namespace DAL.HRS.Mongo.DocClass.Hse
{
    public class HseObservationSuggestion
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Suggestion { get; set; }
        public bool Validated { get; set; }
        public string Comments { get; set; }
    }
}