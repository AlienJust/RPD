using System;

namespace RPD.Supports
{
	static class EventHandlerExtensions
	{
		public static void SafeInvoke<T>(this EventHandler<T> evt, object sender, T e) where T : System.EventArgs
		{
			if (evt != null)
			{
				evt(sender, e);
			}
		}

		public static void SafeInvoke(this EventHandler evt, object sender, System.EventArgs e)
		{
			if (evt != null)
			{
				evt(sender, e);
			}
		}
	}
}
