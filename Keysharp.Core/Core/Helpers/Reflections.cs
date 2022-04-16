using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Keysharp.Core
{
	public static class Reflections
	{
		private static readonly Dictionary<string, Dictionary<Type, MethodInfo>> stringToTypeBuiltInMethods = new Dictionary<string, Dictionary<Type, MethodInfo>>(sttcap, StringComparer.OrdinalIgnoreCase);
		private static readonly Dictionary<string, Dictionary<Type, MethodInfo>> stringToTypeLocalMethods = new Dictionary<string, Dictionary<Type, MethodInfo>>(sttcap / 10, StringComparer.OrdinalIgnoreCase);
		private static readonly Dictionary<string, Dictionary<Type, MethodInfo>> stringToTypeMethods = new Dictionary<string, Dictionary<Type, MethodInfo>>(sttcap, StringComparer.OrdinalIgnoreCase);
		private static readonly Dictionary<string, Dictionary<Type, PropertyInfo>> stringToTypeProperties = new Dictionary<string, Dictionary<Type, PropertyInfo>>(sttcap, StringComparer.OrdinalIgnoreCase);
		private static readonly int sttcap = 1000;
		private static readonly Dictionary<Type, Dictionary<string, MethodInfo>> typeToStringBuiltInMethods = new Dictionary<Type, Dictionary<string, MethodInfo>>(sttcap / 10);
		private static readonly Dictionary<Type, Dictionary<string, MethodInfo>> typeToStringLocalMethods = new Dictionary<Type, Dictionary<string, MethodInfo>>(sttcap / 10);
		private static readonly Dictionary<Type, Dictionary<string, MethodInfo>> typeToStringMethods = new Dictionary<Type, Dictionary<string, MethodInfo>>(sttcap / 5);
		private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> typeToStringProperties = new Dictionary<Type, Dictionary<string, PropertyInfo>>(sttcap / 5);
		//private static Dictionary<Guid, Dictionary<string, MethodInfo>> ExtensionMethods = new Dictionary<Guid, Dictionary<string, MethodInfo>>(sttcap / 20);

		static Reflections()
		{
			/*
			    CacheAllMethods();
			    CacheAllProperties();
			    //When we first started Keysharp, all methods were going to be extension methods, but we've since changed the design, so this isn't really needed anymore.
			    var types = new List<Type>(7000);//At the time of initial development, this will have 6,713 items in it.

			    foreach (var item in AppDomain.CurrentDomain.GetAssemblies())//Need this loop with try/catch because it throws an exception on System.ServiceModel.dll anyway.
			    {
			    try
			    {
			        types.AddRange(item.GetTypes());
			    }
			    catch// (Exception e)
			    {
			        //Keysharp.Core.Dialogs.MsgBox(e.Message);
			    }
			    }

			    foreach (var t in new Type[]//Our usual property and method searching does not go outside the boundaries of the assemblies in this solution. So manually add some of the properties
			    {
			    typeof(object),
			    typeof(object[]),
			    typeof(ArrayList),
			    typeof(IDictionary),
			    typeof(Dictionary<object, object>),
			    })
			    {
			    var meths = t.GetExtensionMethods(types);
			    var sel = meths.Select((meth) => new KeyValuePair<string, MethodInfo>(meth.Name, meth)).ToList();
			    ExtensionMethods.Add(t.GUID, new Dictionary<string, MethodInfo>(sel, StringComparer.OrdinalIgnoreCase));
			    typeToStringProperties.Add(t, new Dictionary<string, PropertyInfo>(
			                                   t.GetProperties().Select((prop) => new KeyValuePair<string, PropertyInfo>(prop.Name, prop)), StringComparer.OrdinalIgnoreCase));
			    }
			*/
		}

		/// <summary>
		/// This must be manually called before any program is run.
		/// Normally we'd put this kind of init in the static constructor, however it must be able to be manually called
		/// when running unit tests. Once upon init, then again within the unit test's auto generated program so it can find
		/// any locally declared methods inside.
		/// </summary>
		public static void Initialize()
		{
			CacheAllMethods();
			CacheAllProperties();
		}

		internal static void CacheAllMethods()
		{
			IEnumerable<Assembly> assemblies;

			if (AppDomain.CurrentDomain.FriendlyName == "testhost")//When running unit tests, the assembly names are changed for the auto generated program.
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
			else
				assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assy => assy.FullName.StartsWith("Keysharp."));

			foreach (var item in assemblies)
				foreach (var type in item.GetTypes())
					if (type.IsClass && type.IsPublic && type.Namespace != null && (type.Namespace.StartsWith("Keysharp.Core") || type.Namespace.StartsWith("Keysharp.Main") || type.Namespace.StartsWith("Keysharp.Tests")))//Allow tests so we can use function objects inside of unit tests.
						_ = FindAndCacheMethod(type, "");

			foreach (var typekv in typeToStringMethods)
				foreach (var methkv in typekv.Value)
				{
					_ = stringToTypeMethods.GetOrAdd(methkv.Key).GetOrAdd(typekv.Key, methkv.Value);

					if (typekv.Key.FullName.StartsWith("Keysharp.Main", StringComparison.OrdinalIgnoreCase) || typekv.Key.FullName.StartsWith("Keysharp.Tests", StringComparison.OrdinalIgnoreCase))//Need to include Tests so that unit tests will work.
					{
						_ = stringToTypeLocalMethods.GetOrAdd(methkv.Key).GetOrAdd(typekv.Key, methkv.Value);
						_ = typeToStringLocalMethods.GetOrAdd(typekv.Key, () => new Dictionary<string, MethodInfo>(typekv.Value.Count, StringComparer.OrdinalIgnoreCase)).GetOrAdd(methkv.Key, methkv.Value);
					}
					else
					{
						_ = stringToTypeBuiltInMethods.GetOrAdd(methkv.Key).GetOrAdd(typekv.Key, methkv.Value);
						_ = typeToStringBuiltInMethods.GetOrAdd(typekv.Key, () => new Dictionary<string, MethodInfo>(typekv.Value.Count, StringComparer.OrdinalIgnoreCase)).GetOrAdd(methkv.Key, methkv.Value);
					}
				}
		}

		internal static void CacheAllProperties()
		{
			foreach (var item in AppDomain.CurrentDomain.GetAssemblies().Where(assy => assy.FullName.StartsWith("Keysharp.Core,")))
				foreach (var type in item.GetTypes())
					if (type.IsClass && type.IsPublic && type.Namespace != null && type.Namespace.StartsWith("Keysharp.Core"))
						_ = FindAndCacheProperty(type, "");

			foreach (var typekv in typeToStringProperties)
				foreach (var propkv in typekv.Value)
					_ = stringToTypeProperties.GetOrAdd(propkv.Key).GetOrAdd(typekv.Key, propkv.Value);
		}

		internal static MethodInfo FindAndCacheMethod(Type t, string name)
		{
			try
			{
				do
				{
					if (typeToStringMethods.TryGetValue(t, out var dkt))
					{
					}
					else
					{
						var meths = (MethodInfo[])t.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);

						if (meths.Length > 0)
						{
							foreach (var meth in meths)
								typeToStringMethods.GetOrAdd(meth.DeclaringType, () => new Dictionary<string, MethodInfo>(meths.Length, StringComparer.OrdinalIgnoreCase)).Add(meth.Name, meth);
						}
						else//Make a dummy entry because this type has no methods. This saves us additional searching later on when we encounter a type derived from this one. It will make the first Dictionary lookup above return true.
						{
							typeToStringMethods[t] = dkt = new Dictionary<string, MethodInfo>(StringComparer.OrdinalIgnoreCase);
							t = t.BaseType;
							continue;
						}
					}

					if (dkt == null && !typeToStringMethods.TryGetValue(t, out dkt))
					{
						t = t.BaseType;
						continue;
					}

					if (dkt.TryGetValue(name, out var mi))//Since the Dictionary was created above with StringComparer.OrdinalIgnoreCase, this will be a case insensitive match.
						return mi;

					//foreach (var kv in dkt)//Case insensitive match.//This is probably not needed anymore because we use StringComparer.OrdinalIgnoreCase.//TODO
					//  if (kv.Value.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
					//      return kv.Value;
					t = t.BaseType;
				} while (t.Assembly == typeof(Any).Assembly);//Traverse down to the base, but only do it for types that are part of this library. Once a base crosses the library boundary, the loop stops.
			}
			catch (Exception e)
			{
				throw;
			}

			return null;
		}

		internal static PropertyInfo FindAndCacheProperty(Type t, string name)
		{
			try
			{
				do
				{
					if (typeToStringProperties.TryGetValue(t, out var dkt))
					{
					}
					else//Property on this type has not been used yet, so get all properties and cache.
					{
						var props = t.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);

						if (props.Length > 0)
						{
							foreach (var prop in props)
								typeToStringProperties.GetOrAdd(prop.DeclaringType, () => new Dictionary<string, PropertyInfo>(props.Length, StringComparer.OrdinalIgnoreCase)).Add(prop.Name, prop);
						}
						else//Make a dummy entry because this type has no properties. This saves us additional searching later on when we encounter a type derived from this one. It will make the first Dictionary lookup above return true.
						{
							typeToStringProperties[t] = dkt = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
							t = t.BaseType;
							continue;
						}
					}

					if (dkt == null && !typeToStringProperties.TryGetValue(t, out dkt))
					{
						t = t.BaseType;
						continue;
					}

					if (dkt.TryGetValue(name, out var pi))//Since the Dictionary was created above with StringComparer.OrdinalIgnoreCase, this will be a case insensitive match.
						return pi;

					//foreach (var kv in dkt)//Case insensitive match.//This is probably not needed anymore because we use StringComparer.OrdinalIgnoreCase.//TODO
					//  if (kv.Value.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
					//      return kv.Value;
					t = t.BaseType;
				} while (t != typeof(object));
			}
			catch (Exception)// e)
			{
				throw;
			}

			return null;
		}

		internal static MethodInfo FindBuiltInMethod(string name)
		{
			//foreach (var item in AppDomain.CurrentDomain.GetAssemblies().Where(assy => assy.FullName.StartsWith("Keysharp.Core,")))
			//  foreach (var type in item.GetTypes())
			//      if (type.IsClass && type.IsPublic && type.Namespace.StartsWith("Keysharp.Core"))
			//          if (FindMethod(type, name) is MethodInfo mi)
			//              return mi;
			if (stringToTypeBuiltInMethods.TryGetValue(name, out var meths))
				if (meths.Count > 0)
					return meths.Values.First();

			return null;
		}

		/*
		        internal static MethodInfo FindExtensionMethod(Type t, string meth)
		        {
		            //if (typeof(IDictionary).IsAssignableFrom(t))
		            //  if (ExtensionMethods.TryGetValue(typeof(IDictionary).GUID, out var idkt))
		            //      if (idkt.TryGetValue(meth, out var mi))
		            //          return mi;
		            if (ExtensionMethods.TryGetValue(t.GUID, out var dkt))
		                if (dkt.TryGetValue(meth, out var mi))
		                    return mi;

		            return null;
		        }
		*/

		internal static MethodInfo FindLocalMethod(string name)
		{
			if (stringToTypeLocalMethods.TryGetValue(name, out var meths))
				if (meths.Count > 0)
					return meths.Values.First();

			//var stack = new StackTrace(false).GetFrames();
			//for (var i = 0; i < stack.Length; i++)
			//{
			//  var type = stack[i].GetMethod().DeclaringType;
			//  if (type.FullName.StartsWith("Keysharp.Main", StringComparison.OrdinalIgnoreCase))
			//      return FindMethod(type, name);
			//}
			return null;
		}

		internal static MethodInfo FindLocalRoutine(string name) => FindLocalMethod(Keysharp.Scripting.Parser.LabelMethodName(name));

		internal static MethodInfo FindMethod(string name)
		{
			if (FindLocalMethod(name) is MethodInfo mi)
				return mi;

			return FindBuiltInMethod(name);
		}

		internal static string GetVariableInfo()
		{
			var sb = new StringBuilder(2048);
			var stack = new StackTrace(false).GetFrames();

			for (var i = stack.Length - 1; i >= 0; i--)
			{
				if (stack[i] != null &&
						stack[i].GetMethod() != null &&
						stack[i].GetMethod().DeclaringType.Attributes.HasFlag(TypeAttributes.Public))//Public is the script, everything else should be hidden.
				{
					if (stack[i].GetMethod().DeclaringType.Namespace != null &&
							stack[i].GetMethod().DeclaringType.Namespace.StartsWith("Keysharp"))
					{
						var meth = stack[i].GetMethod();
						_ = sb.AppendLine($"Class: {meth.ReflectedType.Name}");
						_ = sb.AppendLine();

						foreach (var v in meth.ReflectedType.GetFields(BindingFlags.Public | BindingFlags.Static))
						{
							var val = v.GetValue(null);
							var varstr = $"\t{val?.GetType()} {v.Name}: ";

							if (val is string s)
								varstr += $"[{s.Length}] {s.Substring(0, Math.Min(s.Length, 60))}";
							else if (val is Keysharp.Core.Array arr)
							{
								var ct = Math.Min(100, arr.Count);
								var tempsb = new StringBuilder(ct * 100);

								for (var a = 1; a <= ct; a++)
								{
									var tempstr = arr[a].ToString();
									_ = tempsb.Append(tempstr.Substring(0, Math.Min(tempstr.Length, 60)));

									if (a < ct)
										_ = tempsb.Append(", ");
								}

								varstr += tempsb.ToString();
							}
							else if (val is Keysharp.Core.Map map)
							{
								var ct = Math.Min(100, map.Count);
								var a = 0;
								var tempsb = new StringBuilder(ct * 100);
								_ = tempsb.Append('{');

								foreach (var kv in map.map)
								{
									var tempstr = kv.Value.ToString();
									_ = tempsb.Append($"{kv.Key} : {tempstr.Substring(0, Math.Min(tempstr.Length, 60))}");

									if (++a < ct)
										_ = tempsb.Append(", ");
								}

								_ = tempsb.Append('}');
								varstr += tempsb.ToString();
							}
							else
								varstr += val.ToString();

							_ = sb.AppendLine(varstr);
						}

						_ = sb.AppendLine("");
						_ = sb.AppendLine($"Method: {meth.Name}");
						var mb = stack[i].GetMethod().GetMethodBody();

						foreach (var lvi in mb.LocalVariables)
							_ = sb.AppendLine($"\t{lvi.LocalType}");

						_ = sb.AppendLine("--------------------------------------------------");
						_ = sb.AppendLine();
					}
				}
			}

			return sb.ToString();
		}

		/*
		    public MethodInfo BestMatch(string name, int length)
		    {
		    MethodInfo result = null;
		    var last = int.MaxValue;

		    foreach (var writer in this)
		    {
		        // find method with same name (case insensitive)
		        if (!name.Equals(writer.Name, StringComparison.OrdinalIgnoreCase))
		            continue;

		        var param = writer.GetParameters().Length;

		        if (param == length) // perfect match when parameter count is the same
		        {
		            return writer;
		        }
		        else if (param > length && param < last) // otherwise find a method with the next highest number of parameters
		        {
		            result = writer;
		            last = param;
		        }
		        else if (result == null) // return the first method with excess parameters as a last resort
		            result = writer;
		    }

		    return result;
		    }
		*/

		internal static T SafeGetProperty<T>(object item, string name) => (T)item.GetType().GetProperty(name, typeof(T))?.GetValue(item);

		internal static object SafeInvoke(string name, params object[] args)
		{
			var method = FindLocalRoutine(name);

			if (method == null)
				return null;

			try
			{
				return method.Invoke(null, new object[] { args });
			}
			catch { }

			return null;
		}

		internal static void SafeSetProperty(object item, string name, object value) => item.GetType().GetProperty(name, value.GetType())?.SetValue(item, value, null);

		/// <summary>
		/// This Methode extends the System.Type-type to get all extended methods. It searches hereby in all assemblies which are known by the current AppDomain.
		/// </summary>
		/// <remarks>
		/// Insired by Jon Skeet from his answer on http://stackoverflow.com/questions/299515/c-sharp-reflection-to-identify-extension-methods
		/// </remarks>
		/// <returns>returns MethodInfo[] with the extended Method</returns>
		private static List<MethodInfo> GetExtensionMethods(this Type t, List<Type> types)
		{
			var query = from type in types
						where type.IsSealed && /*!type.IsGenericType &&*/ !type.IsNested
						from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
						where method.IsDefined(typeof(ExtensionAttribute), false)
						where method.GetParameters().Length > 0 && method.GetParameters()[0].ParameterType.Name == t.Name
						select method;
			return query.Select(m => m.IsGenericMethod ? m.MakeGenericMethod(t.GenericTypeArguments) : m).ToList();
		}
	}
}