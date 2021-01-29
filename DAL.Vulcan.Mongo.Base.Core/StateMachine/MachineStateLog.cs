using System;

namespace DAL.Vulcan.Mongo.Base.Core.StateMachine
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