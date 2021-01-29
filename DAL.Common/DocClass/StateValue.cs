namespace DAL.Common.DocClass
{
    public class StateValue
    {
        public string StateName { get; set; }

        public StateValue()
        {
            
        }

        public StateValue(string stateName)
        {
            StateName = stateName;
        }
    }
}