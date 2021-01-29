using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.Core.DocClass;
using MongoDB.Bson;

namespace DAL.Vulcan.Mongo.Base.Core.StateMachine
{
    public class StateMachine: BaseDocument
    {
        public MachineStatus MachineStatus { get; set; } = MachineStatus.Sleep;
        private MachineState _currentState;
        public List<MachineState> States { get; set; } = new List<MachineState>();

        public List<MachineState> FailureStates { get; set; } = new List<MachineState>();

        public List<MachineStateLog> ActivityLog { get; set; } = new List<MachineStateLog>();

        public delegate void EventStarted(MachineState state);
        public delegate bool EventTransition(MachineState from, MachineState to);
        public delegate void EventFailed(MachineState state);
        public delegate void EventStopped(MachineState state);
        public delegate void EventGoto(MachineState state);

        public delegate void EventCompleted(StateMachine machine);

        public event EventTransition OnTransition;
        public event EventFailed OnFail;
        public event EventCompleted OnCompleted;
        public event EventStopped OnStopped;
        public event EventStarted OnStarted;
        public event EventGoto OnGoto;

        public MachineState CurrentState => _currentState;

        //public override StateMachine Save()
        //{
        //    return this.Save();
        //}

        public void Previous()
        {
            if (IsFirstState(CurrentState))
            {
                Reset();
                return;
            }

            _currentState = States.Single(x => x.Id == _currentState.Id);
            var indexOf = States.IndexOf(_currentState);
            _currentState = States[indexOf - 1];
            _currentState.Reset();
        }

        public MachineState FindStateByLabel(string label)
        {
            return States.SingleOrDefault(x => x.Label == label);
        }

        public MachineState FindStateById(ObjectId id)
        {
            return States.SingleOrDefault(x => x.Id == id);
        }

        public MachineState AddNewMachineState(string label, MachineState onFailGoto = null)
        {
            var state = new MachineState(this, label)
            {
                OnFailGoto = onFailGoto,
                StateType = MachineStateType.WorkProcess
            };
            States.Add(state);
            return state;
        }

        public MachineState AddNewFailureMachineState(string label, MachineState onRetryGoto)
        {
            var state = new MachineState(this, label)
            {
                OnRetryGoto = onRetryGoto,
                StateType = MachineStateType.Failure
            };
            States.Add(state);
            return state;
        }

        public int IndexOf(MachineState state)
        {
            var stateFound = States.SingleOrDefault(x => x.Id == state.Id);
            if (stateFound == null) return -1;
            return States.IndexOf(stateFound);
        }

        public void Goto(MachineState state)
        {
            _currentState = state;
            if (IsLastState(state)) return;

            var indexOf = IndexOf(state);

            for (int i = indexOf+1; i <= States.Count-1; i++)
            {
                States[i].Reset();
            }

            OnGoto?.Invoke(state);
        }

        public void Next(MachineState state)
        {
            if (state.Action == StateAction.Start)
            {
                _currentState = state;
                MachineStatus = MachineStatus.Active;
                OnStarted?.Invoke(state);
            }

            if (state.Action == StateAction.Complete)
            {
                var indexOf = States.IndexOf(state);

                if (IsLastState(state))
                {
                    MachineStatus = MachineStatus.Completed;
                    OnCompleted?.Invoke(this);
                    _currentState = null;
                }
                else
                {
                    var nextState = States[indexOf + 1];

                    if (OnTransition != null)
                    {
                        if (!OnTransition(_currentState, nextState))
                        {
                            //MachineStatus = MachineStatus.Error;
                            return;
                        }
                    }
                    _currentState = nextState;
                    _currentState.Start();
                    MachineStatus = MachineStatus.Active;
                }
            }

            if (state.Action == StateAction.Fail)
            {
                MachineStatus = MachineStatus.Failed;
                _currentState = null;
                OnFail?.Invoke(state);
            }
        }

        public void Reset()
        {
            MachineStatus = MachineStatus.Sleep;
            foreach (var state in States)
            {
                state.Reset();
            }
        }

        public bool IsFirstState(MachineState state)
        {
            return States.First().Id == state.Id;
        }

        public bool IsLastState(MachineState state)
        {
            return States.Last(x => x.StateType == MachineStateType.WorkProcess).Id == state.Id;
        }

        public MachineState Start()
        {
            if (MachineStatus == MachineStatus.Active) return _currentState;
            foreach (var state in States)
            {
                if (state.Action == StateAction.None)
                {
                    state.Start();
                    return state;
                }
            }
            return null;
        }
        public MachineState Stop()
        {
            foreach (var state in States)
            {
                if (state.Action == StateAction.Start)
                {
                    state.Stop();
                    OnStopped?.Invoke(state);
                    return state;
                }
            }
            return null;
        }

    }
}
