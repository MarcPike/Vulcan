using System;
using log4net;
using MongoDB.Driver.Core.Events;

namespace DAL.Vulcan.Mongo.Base.Core.Context
{
    public class Log4NetMongoEvents : IEventSubscriber
	{
		public static ILog CommandStartedLog = LogManager.GetLogger(typeof(string));

		private readonly ReflectionEventSubscriber _subscriber;

		public Log4NetMongoEvents()
		{
			_subscriber = new ReflectionEventSubscriber(this);
		}

		public bool TryGetEventHandler<TEvent>(out Action<TEvent> handler)
		{
			return _subscriber.TryGetEventHandler(out handler);
		}

		public void Handle(CommandStartedEvent started)
		{
			CommandStartedLog.Info(new
			{
				started.Command,
				started.CommandName,
				started.ConnectionId,
				started.DatabaseNamespace,
				started.OperationId,
				started.RequestId
			});
		}

		public void Handle(CommandSucceededEvent succeeded)
		{
			
		}
	}
}