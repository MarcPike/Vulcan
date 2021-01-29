using DAL.HRS.Mongo.DocClass.Hse;

namespace DAL.HRS.Mongo.Models
{
    public class HseObservationSuggestionModel
    {
        public string Id { get; set; } 
        public string Suggestion { get; set; }
        public bool Validated { get; set; }
        public string Comments { get; set; }

        public HseObservationSuggestionModel() { }

        public HseObservationSuggestionModel(HseObservationSuggestion s)
        {
            Id = s.Id.ToString();
            Suggestion = s.Suggestion;
            Validated = s.Validated;
            Comments = s.Comments;
        }

    }
}