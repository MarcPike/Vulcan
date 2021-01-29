using System;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.Base.StateMachine
{
    public class MachineStateLog
    {
        public string Label { get; set; }
        public DateTime OccurredAt { get; set; }
        public string LogMessage { get; set; }
        public MachineStateLog(MachineState state, string message)
        {
            Label = state.Label;
            OccurredAt = DateTime.Now;
            LogMessage = message;
        }
    }
}