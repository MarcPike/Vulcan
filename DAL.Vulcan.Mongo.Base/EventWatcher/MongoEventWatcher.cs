using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Vulcan.Mongo.Base.DocClass;

namespace DAL.Vulcan.Mongo.Base.EventWatcher
{
    public static class MongoEventWatcher
    {
        public static List<Watcher> Watchers { get; set; } = new List<Watcher>();

        public static void AddWatcher(Type type, WatcherDataOperation operation, Action<BaseDocument> action)
        {
            if (Watchers.Any(x => x.Type == type && x.Operation == operation)) return;

            var watcher = new Watcher(type, operation, action);
            Watchers.Add(watcher);
        }

        public static void RemoveWatcher(Type type, WatcherDataOperation operation)
        {
            var watcher = Watchers.SingleOrDefault(x => x.Type == type && x.Operation == operation);
            if (watcher != null)
                Watchers.Remove(watcher);
        }



        public static void InvokeFor(Type type, WatcherDataOperation operation, BaseDocument document)
        {
            var watcher = (Watchers.FirstOrDefault(x => x.Type == type && x.Operation == operation));
            if (watcher == null) return;
            try
            {
                watcher.OnExecute(document);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}