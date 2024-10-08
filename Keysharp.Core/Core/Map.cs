﻿namespace Keysharp.Core
{
	public class CaseEqualityComp : IEqualityComparer<object>
	{
		private StringComparison compType;

		public CaseEqualityComp(eCaseSense caseSense)
		{
			if (caseSense == eCaseSense.On)
				compType = StringComparison.Ordinal;
			else if (caseSense == eCaseSense.Off)
				compType = StringComparison.OrdinalIgnoreCase;
			else
				compType = StringComparison.CurrentCulture;
		}

		public new bool Equals(object x, object y)// => x is not null&& y is not null&& string.Compare(x.ToString(), y.ToString(), compType) == 0;
		{
			if (x is string ls && y is string rs)
				return string.Compare(ls, rs, compType) == 0;

			if (ReferenceEquals(x, y))
				return true;

			return x.Equals(y);
		}

		public int GetHashCode(object obj) => obj is string s ? s.ToLower().GetHashCode() : obj.GetHashCode();
	}

	public class Map : KeysharpObject, IEnumerable<(object, object)>, ICollection
	{
		internal Dictionary<object, object> map;

		private eCaseSense caseSense = eCaseSense.On;

		public long Capacity
		{
			get => map.EnsureCapacity(0);
			set => map.EnsureCapacity((int)value);
		}

		public string CaseSense
		{
			get => caseSense.ToString();

			set
			{
				if (Count > 0)
					throw new PropertyError("Attempted to change case sensitivity of a map which was not empty.");

				var oldval = caseSense;
				var str = value.ToLower();
				var val = Options.OnOff(str);

				if (val != null)
					caseSense = val.IsTrue() ? eCaseSense.On : eCaseSense.Off;
				else if (str == "locale")
					caseSense = eCaseSense.Locale;

				if (caseSense != oldval)
					map = new Dictionary<object, object>(new CaseEqualityComp(caseSense));
			}
		}

		public int Count => map.Count;
		public object Default { get; set; }
		bool ICollection.IsSynchronized => ((ICollection)map).IsSynchronized;

		object ICollection.SyncRoot => ((ICollection)map).SyncRoot;

		public Map(params object[] obj) => _ = __New(obj);

		public IEnumerator<(object, object)> __Enum() => ((IEnumerable<(object, object)>)this).GetEnumerator();

		public override object __New(params object[] values)
		{
			if (values == null || values.Length == 0)
			{
				map = new Dictionary<object, object>();
			}
			else
			{
				if (values.Length == 1)
				{
					if (values[0] is Map m)
					{
						map = m.map;
						caseSense = m.caseSense;
						return "";
					}
					else if (values[0] is Dictionary<object, object> dkt)
					{
						map = dkt;
						return "";
					}
				}

				Set(values);
			}

			return "";
		}

		public void Clear() => map.Clear();

		public override object Clone()

		{
			var newmap = new Map()
			{
				Default = Default,
				Capacity = Capacity,
				CaseSense = CaseSense
							//Need to copy ownprops here too.//TODO
			};

			foreach (var kv in map)
				newmap[kv.Key] = kv.Value;

			return newmap;
		}

		public bool Contains(object item) => map.ContainsKey(item);

		public void CopyTo(System.Array array, int index)
		{
			//foreach (var kv in map)
			//array[index] = kv;
		}

		public object Delete(object obj)
		{
			var key = obj is string s && caseSense != eCaseSense.On ? s.ToLower() : obj;
			return map.Remove(key, out var val)
				   ? val
				   : throw new KeyError($"Key {key} was not present in the map.");
		}

		public object Get(object obj0, object obj1 = null)
		{
			var key = obj0;
			var def = obj1;

			if (TryGetValue(key, out var val))
				return val;

			if (def != null)
				return def;

			if (Default != null)
				return Default;

			throw new KeyError($"Key {key} was not present in the map.");
		}

		public IEnumerator<(object, object)> GetEnumerator() => new MapKeyValueIterator(map);

		public bool Has(object obj) => TryGetValue(obj, out _);

		public object MaxIndex()
		{
			var val = long.MinValue;

			foreach (var el in map)
			{
				var temp = el.Key.Al();

				if (temp > val)
					val = temp;
			}

			return val != long.MinValue ? val : string.Empty;
		}

		public object MinIndex()
		{
			var val = long.MaxValue;

			foreach (var el in map)
			{
				var temp = el.Key.Al();

				if (temp < val)
					val = temp;
			}

			return val != long.MaxValue ? val : string.Empty;
		}

		public override void PrintProps(string name, StringBuffer sbuf, ref int tabLevel)
		{
			var sb = sbuf.sb;
			var indent = new string('\t', tabLevel);

			if (map.Count > 0)
			{
				var i = 0;

				if (name.Length == 0)
					_ = sb.Append($"{indent}\t{{");
				else
					_ = sb.Append(indent + name + ": " + "\t {");//Need to put this in multiple steps because the AStyle formatter misinterprets it.

				foreach (var kv in map)
				{
					string key;

					if (kv.Key is string ks)
						key = "\"" + ks + "\"";//Can't use interpolated string here because the AStyle formatter misinterprets it.
					else if (kv.Key is KeysharpObject kso)
					{
						var tempsb = new StringBuffer();
						tabLevel++;
						_ = sb.AppendLine();
						kso.PrintProps("", tempsb, ref tabLevel);
						key = tempsb.ToString().TrimEnd(Keywords.CrLf);
						tabLevel--;
					}
					else
						key = kv.Key.ToString();

					string val;

					if (kv.Value is string vs)
						val = "\"" + vs + "\"";//Can't use interpolated string here because the AStyle formatter misinterprets it.
					else if (kv.Value is KeysharpObject kso)
					{
						var tempsb = new StringBuffer();
						tabLevel++;
						indent = new string('\t', tabLevel);
						key = indent + key;//Indent the line if it's an object.
						_ = sb.AppendLine();
						kso.PrintProps("", tempsb, ref tabLevel);
						val = tempsb.ToString().TrimEnd(Keywords.CrLf);
						tabLevel--;
					}
					else if (kv.Value is null)
						val = "null";
					else
						val = kv.Value.ToString();

					if (i < map.Count - 1)
						_ = sb.Append($"{key}: {val}, ");
					else
						_ = sb.Append($"{key}: {val}");

					i++;
				}
				_ = sb.AppendLine($"}} ({GetType().Name})");
			}
			else
			{
				if (name.Length == 0)
					_ = sb.Append($"{indent} {{}} ({GetType().Name})");
				else
					_ = sb.AppendLine($"{indent}{name}: {{}} ({GetType().Name})");
			}
			var opi = (OwnPropsIterator)OwnProps(true, false, true);
			tabLevel++;
			indent = new string('\t', tabLevel);

			while (opi.MoveNext())
			{
				var (propName, val) = opi.Current;
				var fieldType = val != null ? val.GetType().Name : "";

				if (val is KeysharpObject kso2)
				{
					kso2.PrintProps(propName.ToString(), sbuf, ref tabLevel);
				}
				else if (val != null)
				{
					if (val is string vs)
					{
						var str = "\"" + vs + "\"";//Can't use interpolated string here because the AStyle formatter misinterprets it.
						_ = sb.AppendLine($"{indent}{propName}: {str} ({fieldType})");
					}
					else
						_ = sb.AppendLine($"{indent}{propName}: {val} ({fieldType})");
				}
				else
					_ = sb.AppendLine($"{indent}{propName}: null");
			}
			tabLevel--;
		}

		public void Set(params object[] values)
		{
			if (values.Length == 1)
			{
				if (values[0] is Array temp)
				{
					var count = (temp.Count / 2) * 2;//Do not flatten here because the caller may want a map of maps, or a map of arrays.
					_ = map.EnsureCapacity(count);

					for (var i = 0; i < count - 1; i += 2)
						Insert(temp.array[i], temp.array[i + 1]);//Access the underlying ArrayList directly for performance.
				}
				else if (values[0] is Dictionary<string, object> tempm)
				{
					_ = map.EnsureCapacity(tempm.Count);

					foreach (var kv in tempm)
						Insert(kv.Key, kv.Value);
				}
				else
					throw new ValueError($"Improper object type of {values[0].GetType()} passed to Map constructor.");
			}
			else
			{
				var count = (values.Length / 2) * 2;

				for (var i = 0; i < count; i += 2)
					Insert(values[i], values[i + 1]);
			}
		}
		public override string ToString()
		{
			if (map.Count > 0)
			{
				var sb = new StringBuilder(map.Count * 10);
				_ = sb.Append('{');
				var i = 0;

				foreach (var kv in map)
				{
					string key;

					if (kv.Key is string ks)
						key = "\"" + ks + "\"";//Can't use interpolated string here because the AStyle formatter misinterprets it.
					else
						key = kv.Key.ToString();

					string val;

					if (kv.Value is string vs)
						val = "\"" + vs + "\"";//Can't use interpolated string here because the AStyle formatter misinterprets it.
					else
						val = kv.Value.ToString();

					if (i < map.Count - 1)
						_ = sb.Append($"{key}: {val}, ");
					else
						_ = sb.Append($"{key}: {val}");

					i++;
				}

				_ = sb.Append('}');
				return sb.ToString();
			}
			else
				return "{}";
		}
		IEnumerator IEnumerable.GetEnumerator() => __Enum();
		private void Insert(object key, object value)
		{
			if (caseSense != eCaseSense.On && key is string s)
				map[s.ToLower()] = value;
			else
				map[key] = value;
		}
		private bool TryGetValue(object key, out object value)
		{
			if (caseSense != eCaseSense.On && key is string s)
			{
				if (caseSense == eCaseSense.Off)
				{
					if (map.TryGetValue(s.ToLower(), out var val))
					{
						value = val;
						return true;
					}
				}
				else if (caseSense == eCaseSense.Locale)//By far the slowest.
				{
					foreach (var kv in map)
					{
						if (string.Compare(s, kv.Key.ToString(), StringComparison.CurrentCultureIgnoreCase) == 0)
						{
							value = kv.Value;
							return true;
						}
					}
				}
			}
			else
				return map.TryGetValue(key, out value);

			value = null;
			return false;
		}
		public object this[object key]
		{
			get
			{
				if (TryGetValue(key, out var val))
					return val;

				return Default ?? throw new UnsetItemError($"Key {key} was not present in the map.");
			}

			set => Insert(key, value);
		}
	}

	public class MapKeyValueIterator : IEnumerator<(object, object)>
	{
		private IEnumerator<KeyValuePair<object, object>> iter;
		private Dictionary<object, object> map;

		public (object, object) Current
		{
			get
			{
				try
				{
					var kv = iter.Current;
					return (kv.Key, kv.Value);
				}
				catch (IndexOutOfRangeException)
				{
					throw new InvalidOperationException();
				}
			}
		}

		object IEnumerator.Current => Current;

		public MapKeyValueIterator(Dictionary<object, object> m)
		{
			map = m;
			iter = map.GetEnumerator();
		}

		public void Call(ref object obj0) => (obj0, _) = Current;

		public void Call(ref object obj0, ref object obj1) => (obj0, obj1) = Current;

		public void Dispose() => Reset();

		public bool MoveNext() => iter.MoveNext();

		public void Reset() => iter = map.GetEnumerator();

		private IEnumerator<(object, object)> GetEnumerator() => this;
	}

	public enum eCaseSense
	{
		On,
		Off,
		Locale
	}
}