using System;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.Base.StateMachine
{
    public class MachineState: BaseDocument
    {
        private readonly StateMachine _stateMachine;
        
        public MachineStateType StateType = MachineStateType.WorkProcess;

        public string Label { get; set; }
        public DateTime? StartedOn { get; set; }
        public DateTime? StoppedOn { get; set; }
        public DateTime? CompletedOn { get; set; } 
        public DateTime? FailedOn { get; set; }
        public StateAction Action { get; set; } = StateAction.None;

        public delegate bool EventCheckIfCanComplete(MachineState state);
        public delegate bool EventCheckIfCanStart(MachineState state);

        public delegate void EventCompleteAfterThis(MachineState state);

        public delegate void EventOnWorkDone(string workMessage);

        public delegate void EventOnFailed(MachineState state);

        public EventCheckIfCanComplete OnCheckIfCanComplete { get; set; }
        public EventCheckIfCanStart OnCheckIfCanStart { get; set; }
        public EventCompleteAfterThis OnCompleteAfterThis { get; set; }
        public EventOnWorkDone OnWorkDone { get; set; }
        public EventOnWorkDone OnFailed { get; set; }

        public MachineState OnFailGoto { get; set; }
        public MachineState OnRetryGoto { get; set; }

        public MachineState(StateMachine stateMachine, string label)
        {
            _stateMachine = stateMachine;
            Label = label;
        }

        public void Work(string workMessage)
        {
            OnWorkDone?.Invoke(workMessage);
            _stateMachine.ActivityLog.Add(new MachineStateLog(this, workMessage));

        }

        public void Start()
        {
            if (OnCheckIfCanStart != null)
            {
                if (!OnCheckIfCanStart(this)) return;
            }
            Action = StateAction.Start;
            StartedOn = DateTime.Now;
            _stateMachine.Next(this);
            _stateMachine.ActivityLog.Add(new MachineStateLog(this,"Started"));
        }

        public void Stop()
        {
            Action = StateAction.Stop;
            StoppedOn = DateTime.Now;
            _stateMachine.MachineStatus = MachineStatus.Stopped;
            _stateMachine.ActivityLog.Add(new MachineStateLog(this, "Stopped"));
        }

        public void Complete()
        {
            if (OnCheckIfCanComplete != null)
            {
                if (!OnCheckIfCanComplete(this)) return;
            }
            OnCompleteAfterThis?.Invoke(this);

            Action = StateAction.Complete;
            CompletedOn = DateTime.Now;
            _stateMachine.Next(this);
            _stateMachine.ActivityLog.Add(new MachineStateLog(this, "Completed"));

        }

        public void Fail(string message)
        {
            Action = StateAction.Fail;
            FailedOn = DateTime.Now;
            
            _stateMachine.ActivityLog.Add(new MachineStateLog(this, "Failed"));
            if (OnFailGoto != null) _stateMachine.Goto(OnFailGoto);
            OnFailed?.Invoke(message);
        }

        public void Reset()
        {
            Action = StateAction.None;
            StartedOn = null;
            CompletedOn = null;
            FailedOn = null;
            _stateMachine.ActivityLog.Add(new MachineStateLog(this, "Reset"));
        }

        public void Previous()
        {
            Reset();
            _stateMachine.Previous();
            _stateMachine.ActivityLog.Add(new MachineStateLog(this, "Previous"));
        }

        public void Goto()
        {
            Reset();
            _stateMachine.Goto(this);
            _stateMachine.ActivityLog.Add(new MachineStateLog(this, "Goto"));
        }
    }
}