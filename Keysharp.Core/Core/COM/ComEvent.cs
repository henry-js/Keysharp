﻿#if WINDOWS
namespace Keysharp.Core.COM
{
	internal class ComEvent
	{
		internal Dispatcher dispatcher;
		internal KeysharpObject sinkObj;
		internal object[] thisArg;
		private bool logAll;
		private Dictionary<string, MethodPropertyHolder> methodMapper = new Dictionary<string, MethodPropertyHolder>(10, StringComparer.OrdinalIgnoreCase);
		private string prefix;

		internal ComEvent(Dispatcher disp, object sink, bool log)
		{
			dispatcher = disp;
			thisArg = new object[1] { this };
			logAll = log;

			if (sink is string s)
			{
				prefix = s;

				foreach (var kv in Reflections.stringToTypeLocalMethods)
				{
					if (string.Compare(kv.Key, "Main", true) != 0 &&
							string.Compare(kv.Key, "UserMainCode", true) != 0)
					{
						if (kv.Key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
						{
							methodMapper[kv.Key.Remove(0, prefix.Length)] = kv.Value.First().Value.First().Value;
						}
					}
				}

				if (methodMapper.Count > 0)
					dispatcher.EventReceived += Dispatcher_EventReceivedGlobalFunc;
				else
					Keysharp.Scripting.Script.OutputDebug($"No suitable global methods were found with the prefix {prefix} which could be used as COM event handlers. No COM event handlers will be triggered.");
			}
			else if (sink is KeysharpObject ko)
			{
				if (!Reflections.typeToStringMethods.TryGetValue(ko.GetType(), out var methDkt))
				{
					_ = Reflections.FindAndCacheMethod(ko.GetType(), "", 0);
					_ = Reflections.typeToStringMethods.TryGetValue(ko.GetType(), out methDkt);
				}

				if (methDkt != null)
				{
					foreach (var methkv in methDkt)
					{
						foreach (var meth in methkv.Value)
						{
							methodMapper[methkv.Key] = methkv.Value.First().Value;
						}
					}

					sinkObj = ko;
				}

				if (methodMapper.Count > 0)
					dispatcher.EventReceived += Dispatcher_EventReceivedObjectMethod;
				else
					Keysharp.Scripting.Script.OutputDebug($"No suitable methods were found on the passed in object of type {sink.GetType()} which could be used as COM event handlers. No COM event handlers will be triggered.");
			}
			else
				throw new ValueError($"The passed in sink object of type {sink.GetType()} was not either a string or a Keysharp object.");
		}

		public void Unwire()
		{
			thisArg[0] = null;
			dispatcher.EventReceived -= Dispatcher_EventReceivedGlobalFunc;
			dispatcher.EventReceived -= Dispatcher_EventReceivedObjectMethod;
		}

		private static void FixArgs(object[] args)
		{
			for (var i = 0; i < args.Length; i++)
			{
				var arg = args[i];

				if (arg is long || arg is double || arg is string)
					continue;

				if (arg is int ii)
					args[i] = (long)ii;
				else if (arg is uint ui)
					args[i] = (long)ui;
				else if (arg is float f)
					args[i] = (double)f;
				else if (arg is short s)
					args[i] = (long)s;
				else if (arg is ushort us)
					args[i] = (long)us;
				else if (arg is char c)
					args[i] = (long)c;
				else if (arg is byte b)
					args[i] = (long)b;
				else if (arg is IntPtr ip)
					args[i] = ip.ToInt64();
			}
		}

		private void Dispatcher_EventReceivedGlobalFunc(object sender, DispatcherEventArgs e)
		{
			if (logAll)
				Keysharp.Scripting.Script.OutputDebug($"Dispatch ID {e.DispId}: {e.Name} received to be dispatched to a global function with {e.Arguments.Length} + 1 args.");

			var thisObj = thisArg[0];

			if (thisObj != null && methodMapper.TryGetValue(e.Name, out var mph))
			{
				FixArgs(e.Arguments);
				var result = mph.callFunc(null, e.Arguments.Concat(thisArg));

				if (result is ComObject co)
					result = co.Ptr;

				e.Result = result;
			}
		}

		private void Dispatcher_EventReceivedObjectMethod(object sender, DispatcherEventArgs e)
		{
			if (logAll)
				Keysharp.Scripting.Script.OutputDebug($"Dispatch ID {e.DispId}: {e.Name} received to be dispatched to an object method with {e.Arguments.Length} + 1 args.");

			var thisObj = thisArg[0];

			if (thisObj != null && methodMapper.TryGetValue(e.Name, out var mph))
			{
				FixArgs(e.Arguments);
				var result = mph.callFunc(sinkObj, e.Arguments.Concat(thisArg));

				if (result is ComObject co)
					result = co.Ptr;

				e.Result = result;
			}
		}
	}
}
#endif