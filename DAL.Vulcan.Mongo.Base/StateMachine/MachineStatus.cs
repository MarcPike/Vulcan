namespace DAL.Vulcan.Mongo.Base.StateMachine
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