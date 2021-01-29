using DAL.Vulcan.Mongo.DocClass.CRM;

namespace DAL.Vulcan.Mongo.DocClass.Notifications
{
    public class NotificationBuilderForMilestoneAddedToGoal : NotificationBuilder
    {
        private readonly Milestone _milestone;
        private readonly Goal _goal;

        public NotificationBuilderForMilestoneAddedToGoal(Goal goal, Milestone milestone)
        {
            _goal = goal;
            _milestone = milestone;
        }
        public override Notification GetNotification()
        {
            var notification = new Notification()
            {
                ActionType = NotificationActionType.Added,
                PrimaryObjectType = NotificationObjectType.Milestone,
                SecondaryObjectType = NotificationObjectType.Goal,
                Label = $"Milestone: [{_milestone.Label}] added to Goal: {_goal.Label}",
                Goal = _goal.AsGoalRef()
            };
            notification = Repository.Upsert(notification);
            return notification;
        }
    }
}