using static Keysharp.Scripting.Keywords;

namespace Keysharp.Core
{
	public static class Misc
	{
		public static Keysharp.Core.Array Array(params object[] obj)
		{
			if (obj == null || obj.Length == 0)
			{
				var arr = new Keysharp.Core.Array
				{
					Capacity = 64
				};
				return arr;
			}
			else
				return new Keysharp.Core.Array(obj);
		}

		public static Keysharp.Core.Buffer Buffer(object obj0, object obj1 = null) => new (obj0, obj1);

		public static void Collect() => GC.Collect();

		public static Map Dictionary(object[] keys, object[] values)
		{
			var table = new Map();

			for (var i = 0; i < keys.Length; i++)
			{
				var name = keys[i];
				var entry = i < values.Length ? values[i] : null;

				if (entry == null)
				{
					if (table.Has(name))
						_ = table.Delete(name);
				}
				else
					table[name] = entry;
			}

			return table;
		}

		public static Error Error(params object[] obj) => new (obj);

		public static bool ErrorOccurred(Error err)
		{
			if (Script.onErrorHandlers != null)
			{
				foreach (var handler in Script.onErrorHandlers)
				{
					var result = handler.Call(err, err.ExcType);

					if (result.IsCallbackResultNonEmpty() && result.ParseLong(false) == 1L)
						return false;
				}
			}

			if (err.ExcType == Keyword_ExitApp)
				_ = Flow.ExitAppInternal(Flow.ExitReasons.Critical);

			return err.ExcType != Keywords.Keyword_Return;//Don't report an error if it was just an exit from a thread.
		}

		public static IFuncObj FuncObj(object obj0, object obj1 = null, object obj2 = null) => new FuncObj(obj0.As(), obj1, obj2);

		public static Gui Gui(object obj0 = null, object obj1 = null, object obj2 = null) => new (obj0, obj1, obj2);

		public static long HasBase(object obj0, object obj1) => obj1.GetType().IsAssignableFrom(obj0.GetType()) ? 1L : 0L;

		public static IndexError IndexError(params object[] obj) => new (obj);

		/// <summary>
		/// Differs in that locale is always considered.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static long IsAlnum(object obj)
		{
			var s = obj.As();
			return s?.Length == 0 || s.All(ch => char.IsLetter(ch) || char.IsNumber(ch)) ? 1 : 0;
		}

		/// <summary>
		/// Differs in that locale is always considered.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static long IsAlpha(object obj)
		{
			var s = obj.As();
			return s?.Length == 0 || s.All(char.IsLetter) ? 1 : 0;
		}

		public static long IsDate(object obj) => IsTime(obj);

		public static long IsDigit(object obj)
		{
			var s = obj.As();
			return s?.Length == 0 || s.All(char.IsDigit) ? 1 : 0;
		}

		public static long IsFloat(object obj)
		{
			var o = obj;

			if (o is double || o is float || o is decimal)
				return 1;

			var val = o.ParseDouble(false, true);
			return val.HasValue ? 1 : 0;
		}

		public static long IsFunc(object obj0, object obj1 = null) => Reflections.FindMethod(obj0.ToString(), obj1.Ai(-1)) is MethodPropertyHolder mph && mph.mi != null ? 1L : 0L;

		public static long IsInteger(object obj)
		{
			var o = obj;

			if (o is long || o is int || o is uint || o is ulong)
				return 1L;

			if (o is double || o is float || o is decimal)
				return 0L;

			var val = o.ParseLong(false);
			return val.HasValue ? 1L : 0L;
		}

		public static long IsLabel(object name) => throw new Error("C# does not allow querying labels at runtime.");

		/// <summary>
		/// Differs in that locale is always considered.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static long IsLower(object obj)
		{
			var s = obj.As();
			return s?.Length == 0 || s.All(ch => char.IsLetter(ch) && char.IsLower(ch)) ? 1 : 0;
		}

		public static long IsNumber(object obj) => IsInteger(obj) | IsFloat(obj);

		public static long IsObject(object obj) => obj is KeysharpObject ? 1 : 0;

		public static long IsSet(object obj) => obj != UnsetArg.Default&& obj != null ? 1 : 0;

		public static long IsSpace(object obj) => obj.ToString().AsSpan().IndexOfAnyExcept(SpacesSv) != -1 ? 0L : 1L;

		public static long IsTime(object obj)
		{
			var s = obj.As();
			DateTime dt;

			try
			{
				dt = Conversions.ToDateTime(s);

				if (dt == DateTime.MinValue)
					return 0L;
			}
			catch
			{
				return 0L;
			}

			int[] t = { DateTime.Now.Year / 100, DateTime.Now.Year % 100, 1, 1, 0, 0, 0, 0 };
			var tempdt = new DateTime(t[1], t[2], t[3], t[4], t[5], t[6], System.Globalization.CultureInfo.CurrentCulture.Calendar);//Will be wrong this if parsing totally failed.
			return dt != tempdt ? 1L : 0L;
		}

		/// <summary>
		/// Differs in that locale is always considered.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static long IsUpper(object obj)
		{
			var s = obj.As();
			return s?.Length == 0 || s.All(ch => char.IsLetter(ch) && char.IsUpper(ch)) ? 1 : 0;
		}

		public static long IsXDigit(object obj)
		{
			var s = obj.As();
			var sp = s.AsSpan();

			if (sp.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
				sp = sp.Slice(2);

			foreach (var ch in sp)
				if (!((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'F') || (ch >= 'a' && ch <= 'f')))
					return 0L;

			return 1L;
		}

		public static KeyError KeyError(params object[] obj) => new (obj);

		public static Map Map(params object[] obj) => Object(obj);

		public static MemberError MemberError(params object[] obj) => new (obj);

		public static MemoryError MemoryError(params object[] obj) => new (obj);

		public static Menu Menu() => new ();

		public static MenuBar MenuBar() => new ();

		public static object MenuFromHandle(object obj)
		{
			var handle = new IntPtr(obj.Al());
			var menu = System.Windows.Forms.Control.FromHandle(handle);

			if (menu != null)
				return menu;

			if ((menu = System.Windows.Forms.Control.FromHandle(handle)) != null)
				return menu;

			return "";
		}

		public static MethodError MethodError(params object[] obj) => new (obj);

		public static RefHolder Mrh(int i, object o, Action<object> r) => new RefHolder(i, o, r);

		public static object ObjGetCapacity(object obj) => obj is KeysharpObject kso ? kso.GetCapacity() : throw new Error($"Object of type {obj.GetType()} was not of type KeysharpObject.");

		//Make RefHolder.
		public static long ObjHasOwnProp(object obj0, object obj1) => obj0 is KeysharpObject kso ? kso.HasOwnProp(obj1) : 0L;

		public static long ObjOwnPropCount(object obj) => obj is KeysharpObject kso ? kso.OwnPropCount() : throw new Error($"Object of type {obj.GetType()} was not of type KeysharpObject.");

		public static object ObjOwnProps(object obj0, object obj1 = null) => obj0 is KeysharpObject kso ? kso.OwnProps(obj1) : throw new Error($"Object of type {obj0.GetType()} was not of type KeysharpObject.");

		public static void ObjSetBase(params object[] obj) => throw new Exception(Any.BaseExc);

		public static object ObjSetCapacity(object obj0, object obj1) => obj0 is KeysharpObject kso ? kso.SetCapacity(obj1) : throw new Error($"Object of type {obj0.GetType()} was not of type KeysharpObject.");

		public static void OnError(object obj0, object obj1 = null)
		{
			var e = obj0;
			var i = obj1.Al(1L);
			var del = Function.GetFuncObj(e, null, true);

			if (Script.onErrorHandlers == null)
				Script.onErrorHandlers = new List<IFuncObj>();

			Script.onErrorHandlers.ModifyEventHandlers(del, i);
		}

		public static OSError OSError(params object[] obj) => new (obj);

		public static PropertyError PropertyError(params object[] obj) => new (obj);

		public static Keysharp.Core.StringBuffer StringBuffer(object obj0, object obj1 = null) => new StringBuffer(obj0.As(), obj1.Ai(256));

		public static TargetError TargetError(params object[] obj) => new (obj);

		public static TimeoutError TimeoutError(params object[] obj) => new (obj);

		public static string Type(object t) => t.GetType().Name;

		public static TypeError TypeError(params object[] obj) => new (obj);

		public static UnsetItemError UnsetItemError(params object[] obj) => new (obj);

		public static ValueError ValueError(params object[] obj) => new (obj);

		public static ZeroDivisionError ZeroDivisionError(params object[] obj) => new (obj);

		internal static string PrintProps(object obj, string name, StringBuffer sbuf, ref int tabLevel)
		{
			var sb = sbuf.sb;
			var indent = new string('\t', tabLevel);
			var fieldType = obj != null ? obj.GetType().Name : "";

			if (obj is KeysharpObject kso)
			{
				kso.PrintProps(name, sbuf, ref tabLevel);
			}
			else if (obj != null)
			{
				if (obj is string vs)
				{
					var str = "\"" + vs + "\"";//Can't use interpolated string here because the AStyle formatter misinterprets it.
					_ = sb.AppendLine($"{indent}{name}: {str} ({fieldType})");
				}
				else
					_ = sb.AppendLine($"{indent}{name}: {obj} ({fieldType})");
			}
			else
				_ = sb.AppendLine($"{indent}{name}: null");

			return sb.ToString();
		}

		internal static bool TryCatch(Action action, bool pop)
		{
			try
			{
				action();
				return true;
			}
			catch (Keysharp.Core.Error kserr)
			{
				if (pop)
					Threads.EndThread(true);

				if (ErrorOccurred(kserr))
				{
					var (__pushed, __btv) = Keysharp.Core.Common.Threading.Threads.BeginThread();
					Keysharp.Core.Dialogs.MsgBox("Uncaught Keysharp exception:\r\n" + kserr, $"{Accessors.A_ScriptName}: Unhandled exception", "iconx");
					Keysharp.Core.Common.Threading.Threads.EndThread(__pushed);
				}

				return false;
			}
			catch (System.Exception mainex)
			{
				if (pop)
					Threads.EndThread(true);

				var ex = mainex.InnerException ?? mainex;

				if (ex is Keysharp.Core.Error kserr)
				{
					if (ErrorOccurred(kserr))
					{
						var (__pushed, __btv) = Keysharp.Core.Common.Threading.Threads.BeginThread();
						Keysharp.Core.Dialogs.MsgBox("Uncaught Keysharp exception:\r\n" + kserr, $"{Accessors.A_ScriptName}: Unhandled exception", "iconx");
						Keysharp.Core.Common.Threading.Threads.EndThread(__pushed);
					}
				}
				else
				{
					var (__pushed, __btv) = Keysharp.Core.Common.Threading.Threads.BeginThread();
					Keysharp.Core.Dialogs.MsgBox("Uncaught exception:\r\n" + "Message: " + ex.Message + "\r\nStack: " + ex.StackTrace, $"{Accessors.A_ScriptName}: Unhandled exception", "iconx");
					Keysharp.Core.Common.Threading.Threads.EndThread(__pushed);
				}

				return false;
			}
		}

		private static Map Object(params object[] obj)
		{
			if (obj.Length == 0)
				return new Map();

			var dkt = new Map();
			dkt.Set(obj);
			return dkt;
		}

		public delegate void SimpleDelegate();

		public delegate void VariadicAction(params object[] o);

		public delegate object VariadicFunction(params object[] args);
	}
}