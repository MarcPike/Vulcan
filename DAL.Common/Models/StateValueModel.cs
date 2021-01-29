using DAL.Common.DocClass;

namespace DAL.Common.Models
{
    public class StateValueModel
    {
        public string StateName { get; set; }

        public StateValueModel()
        {
            
        }

        public StateValueModel(StateValue state)
        {
            StateName = state.StateName;
        }
    }
}