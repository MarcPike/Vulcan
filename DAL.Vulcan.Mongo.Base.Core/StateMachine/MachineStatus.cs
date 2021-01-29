namespace DAL.Vulcan.Mongo.Base.Core.StateMachine
{
    public enum MachineStatus
    {
        Sleep,
        Active,
        Failed,
        Completed,
        Error,
        Stopped
    }
}