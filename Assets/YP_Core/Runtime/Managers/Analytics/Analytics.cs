using System;
using System.Collections.Generic;
using YP.Internal;

namespace YP
{
	public class Analytics : Manager
	{
		private static Analytics instance;
		private static AnalyticsService service => instance.supportedService as AnalyticsService;

		protected override string managerName => "YP Analytics";

		protected override void OnInitialized()
		{
			instance = this;
			Log(Core.Message.Initialized(managerName));
		}

		public static void SendEvent(string eventName, Dictionary<string, object> parameters = null)
		{
			service.SendEvent(eventName, parameters);

			string message = "Event sent: " + eventName;

			if (parameters != null)
			{
				foreach (var parameter in parameters)
					message += "\n" + parameter.Key + ": " + parameter.Value.ToString();
			}

			instance.Log(message);
		}
	}
}