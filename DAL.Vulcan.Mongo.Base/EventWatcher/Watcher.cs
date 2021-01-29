using System;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.Base.EventWatcher
{
    public class Watcher
    {
        public Type Type { get; set; } 
        public WatcherDataOperation Operation { get; set; }
        public Action<BaseDocument> OnExecute { get; set; }

        public Watcher(Type type, WatcherDataOperation operation, Action<BaseDocument> onExecute)
        {
            if (!type.IsSubclassOf(typeof(BaseDocument)))
            {
                throw new Exception($"{type.FullName} is not a descendant of BaseDocument");
            }
            Type = type;
            Operation = operation;
            OnExecute = onExecute;
        }
    }
}