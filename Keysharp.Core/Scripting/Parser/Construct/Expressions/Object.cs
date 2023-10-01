using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Keysharp.Core;
using static Keysharp.Core.Core;

namespace Keysharp.Scripting
{
	public partial class Parser
	{
		private bool IsArrayExtension(object item) => item is CodeMethodInvokeExpression cmie&& cmie.Method.MethodName == InternalMethods.ExtendArray.MethodName;

		private bool IsJsonObject(object item) => item is CodeMethodInvokeExpression cmie&& cmie.Method.MethodName == InternalMethods.Index.MethodName;

		private int ParseBalanced(Span<object> parts, string delim)
		{
			var parenLevel = 0;
			var braceLevel = 0;
			var bracketLevel = 0;
			var i = 0;

			for (; i < parts.Length; i++)
			{
				if (parts[i] is string s)
				{
					if (s.EndsWith('('))//Either it's a ( or a function call which will end with a (.
					{
						parenLevel++;
					}
					else if (s == ")")
					{
						parenLevel--;
					}
					else if (s == "{")
					{
						braceLevel++;
					}
					else if (s == "}")
					{
						braceLevel--;
					}
					else if (s == "[")
					{
						bracketLevel++;
					}
					else if (s == "]")
					{
						bracketLevel--;
					}
					else if (parenLevel == 0 && braceLevel == 0 && bracketLevel == 0 && s == delim)
					{
						return i;
					}
				}
			}

			return i;
		}

		private int ParseBalancedArray(Span<object> parts)
		{
			var bracketLevel = 0;
			var i = 0;

			for (; i < parts.Length; i++)
			{
				if (parts[i] is string s)
				{
					if (s == "[")
					{
						bracketLevel++;
					}
					else if (s == "]")
					{
						bracketLevel--;
					}

					if (bracketLevel == 0)
					{
						break;
					}
				}
			}

			return i - 1;
		}

		private void ParseObject(CodeLine line, string code, List<object> parts, out CodeExpression[] keys, out CodeExpression[] values, bool create)
		{
			var names = new List<CodeExpression>();
			var entries = new List<CodeExpression>();

			for (var i = 0; i < parts.Count; i++)
			{
				var hadResolve = false;
				CodeExpression value = null;

				if (!(parts[i] is string name))//Each token shouldn't be anything else other than a string.
					throw new ParseException($"{ExUnexpected} at line {line}");

				if (name.Length > 2)
				{
					if (name[0] == StringBound && name[name.Length - 1] == StringBound)
						name = name.Substring(1, name.Length - 2).ToLower();//If enclosed in quotes, just use as is, which will be a string in quotes.
					else if (name[0] == DefaultResolve)
						hadResolve = true;//If enclosed in percent signs, remove them and parse the expression.
				}

				if (hadResolve)//Remove percent signs and reparse.
				{
					var startPos = i;
					name = "";

					while (i < parts.Count)
					{
						var str = parts[i].ToString();

						if (str.Length > 0 && str[0] == AssignPre)
							break;

						if (str.StartsWith(DefaultResolve) || str.EndsWith(DefaultResolve))
							parts[i] = str = str.Trim(DefaultResolve);

						if (parts[i].ToString() == "")
						{
							parts.RemoveAt(i);
							i--;
						}

						if (str == "[*")
							name += '.';
						else if (str != "*]")
							name += str;

						i++;
					}

					//Need to reparse so that something like map.one can be properly interpreted.
					//This is because it would have been incorrectly parsed earlier because it was enclosed with %%.
					var tokens = SplitTokens(name);
					_ = ExtractRange(parts, startPos, i);
					i = startPos - 1; //i++ below will reset back to zero.
					var expr = ParseExpression(line, code, tokens, false);
					names.Add(expr);
				}
				else
					names.Add(new CodePrimitiveExpression(name.ToLower()));//Add as a quoted lowercase string.

				i++;//Ensure the next token is a : char.

				if (i == parts.Count)
					goto collect;

				if (!(parts[i] is string assign))
					throw new ParseException($"{ExUnexpected} at line {line}");

				if (assign.Length == 1 && assign[0] == Multicast)
					goto collect;

				if (!(assign.Length == 1 && (assign[0] == Equal || assign[0] == HotkeyBound)))//Should be an = or : char.
					throw new ParseException($"{ExUnexpected} at line {line}");

				i++;

				if (i == parts.Count)
					goto collect;

				//Now get the value portion, which comes after the : char.
				var subs = new List<List<object>>();
				var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(parts);
				var tempparts = ParseObjectValue(span.Slice(i));
				var valcode = string.Join("", tempparts.Select((p) => p.ToString()));
				var exprs = ParseMultiExpression(line, valcode, tempparts, create, subs);

				if (exprs.Length > 0)
				{
					value = exprs[0];
					i += subs[0].Count;
				}

				if (i == parts.Count)
					goto collect;

				if (!(parts[i] is string delim))
					throw new ParseException($"{ExUnexpected} at line {line}");

				if (!(delim.Length == 1 && delim[0] == Multicast))
					throw new ParseException($"{ExUnexpected} at line {line}");

				collect:
				entries.Add(value ?? new CodePrimitiveExpression(null));
			}

			keys = names.ToArray();
			values = entries.ToArray();
		}

		private object[] ParseObjectValue(Span<object> parts)
		{
			var i = ParseBalanced(parts, ":");

			if (i < parts.Length)//If we hit the end, just return it.
			{
				i--;

				while (i > 0 && ((parts[i] as string) != ","))
					i--;
			}

			return parts.Slice(0, i).ToArray();
		}

		private List<string> SplitStringBalanced(string s, char delim)
		{
			var parenLevel = 0;
			var braceLevel = 0;
			var bracketLevel = 0;
			var i = 0;
			var parts = new List<string>();
			var sb = new StringBuilder();

			foreach (var ch in s)
			{
				if (ch == '(')//Either it's a ( or a function call which will end with a (.
				{
					parenLevel++;
					_ = sb.Append(ch);
				}
				else if (ch == ')')
				{
					if (parenLevel > 0)
						_ = sb.Append(ch);

					parenLevel--;
				}
				else if (ch == '{')
				{
					braceLevel++;
					_ = sb.Append(ch);
				}
				else if (ch == '}')
				{
					if (braceLevel > 0)
						_ = sb.Append(ch);

					braceLevel--;
				}
				else if (ch == '[')
				{
					_ = sb.Append(ch);
					bracketLevel++;
				}
				else if (ch == ']')
				{
					if (bracketLevel > 0)
						_ = sb.Append(ch);

					bracketLevel--;
				}
				else if (parenLevel == 0 && braceLevel == 0 && bracketLevel == 0 && ch == delim)//Assuming delim is != to any of the above characters.
				{
					parts.Add(sb.ToString());
					_ = sb.Clear();
				}
				else
					_ = sb.Append(ch);
			}

			if (sb.Length > 0)
			{
				parts.Add(sb.ToString());
			}

			return parts;
		}
	}
}