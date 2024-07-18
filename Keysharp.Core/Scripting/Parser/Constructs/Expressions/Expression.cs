using static Keysharp.Scripting.Keywords;

namespace Keysharp.Scripting
{
	public partial class Parser
	{
		internal CodeExpression BinOpToSnippet(CodeObject co)
		{
			if (co.GetExpr() is CodeExpression expr)
			{
				if (expr is CodeBinaryOperatorExpression cboe)
				{
					if (cboe.Operator == CodeBinaryOperatorType.Assign)
					{
						var c2s = Ch.CodeToString(cboe);
						c2s = c2s.Substring(1, c2s.Length - 2);
						var cse = new CodeSnippetExpression($"{c2s}");
						cse.UserData["orig"] = cboe;
						cboe.UserData["snippet"] = cse;
						cboe.Left.UserData["snippet"] = cse;
						cboe.Right.UserData["snippet"] = cse;
						assignSnippets.Add(cse);
						return cse;
					}
					else
					{
						var c2s = Ch.CodeToString(cboe);
						var cse = new CodeSnippetExpression($"_ = ({c2s})");
						cse.UserData["orig"] = cboe;
						cboe.UserData["snippet"] = cse;
						cboe.Left.UserData["snippet"] = cse;
						cboe.Right.UserData["snippet"] = cse;
						assignSnippets.Add(cse);
						return cse;
					}
				}

				return expr;
			}

			return null;
		}

		internal void ReevaluateSnippet(CodeSnippetExpression cse)
		{
			var orig = cse.UserData["orig"] as CodeExpression;
			var c2s = Ch.CodeToString(orig);
			c2s = c2s.Substring(1, c2s.Length - 2);

			if (orig is CodeBinaryOperatorExpression cboe)
			{
				if (cboe.Operator == CodeBinaryOperatorType.Assign)
					cse.Value = $"{c2s}";
				else
					cse.Value = $"_ = ({c2s})";
			}
		}

		private static (List<int>, List<List<object>>) ParseArguments(List<object> paren)
		{
			var paramIndex = 0;
			var parenLevel = 0;
			var braceLevel = 0;
			var bracketLevel = 0;
			var lastisstar = paren.Count > 0 && paren.Last().ToString() == "*";
			List<int> refIndexes = new List<int>(paren.Count);//This was to handle parameters where the caller passed & to signify it's meant to be passed by reference, but we're no longer supporting reference parameters.
			List<object> arg = new List<object>();
			List<List<object>> args = new List<List<object>>(paren.Count);

			for (var i = 0; i < paren.Count; i++)
			{
				var p = paren[i].ToString();

				if (p.Contains('(') && !p.StartsWith('"'))//Indicates the start of a new expression as a parameter, so don't count it, because it'll be handled recursively on its own.
				{
					arg.Add(p);
					parenLevel++;
				}
				else if (p.Contains(')') && !p.StartsWith('"'))
				{
					arg.Add(p);
					parenLevel--;
				}
				else if (p == "[")
				{
					arg.Add(p);
					bracketLevel++;
				}
				else if (p == "]")
				{
					arg.Add(p);
					bracketLevel--;
				}
				else if (p == "{")
				{
					arg.Add(p);
					braceLevel++;
				}
				else if (p == "}")
				{
					arg.Add(p);
					braceLevel--;
				}
				else if (parenLevel == 0 && braceLevel == 0 && bracketLevel == 0)
				{
					if (p == ",")//Reached the end of a parameter, so increment parameter index.
					{
						if (arg.Count == 0)
							arg.Add("null");

						args.Add(arg);
						arg = new List<object>();
						paramIndex++;
					}
					else if (lastisstar && p == "*")//p can be * if the param is just a variable reference, or if it's a function call whose result is to be expanded. Ex: func(func2(val)*)
					{
						paren.RemoveAt(i);
						i--;
					}
					else if (p == "&" && (i == 0 || paren[i - 1].ToString() == ","))
					{
						refIndexes.Add(paramIndex);

						if (i < paren.Count - 1)
							arg.Add(paren[i + 1]);

						paren.RemoveAt(i);
					}
					else
						arg.Add(p);
				}
				else
					arg.Add(p);
			}

			if (arg.Count > 0 || (paren.Count > 0 && paren[paren.Count - 1].ToString() == ","))
			{
				if (arg.Count == 0)
					arg.Add("null");

				args.Add(arg);
			}

			return (refIndexes, args);
		}

		private static List<int> ParseArguments(CodeExpressionCollection parameters)
		{
			var refIndexes = new List<int>(parameters.Count);//This was to handle parameters where the caller passed & to signify it's meant to be passed by reference, but we're no longer supporting reference parameters.

			for (var i = 0; i < parameters.Count; i++)
			{
				var p = parameters[i];

				if (p is CodeDirectionExpression cde && cde.Direction == FieldDirection.Ref)
					refIndexes.Add(i);
				else if (p is CodeParameterDeclarationExpression cdpe && cdpe.Direction == FieldDirection.Ref)
					refIndexes.Add(i);
			}

			return refIndexes;
		}

		private CodeExpressionCollection ConvertDirectParamsToInvoke(CodeExpressionCollection parameters)
		{
			var newParams = new CodeExpressionCollection();

			for (int i1 = 0; i1 < parameters.Count; i1++)
			{
				var p = parameters[i1];

				if (p is CodeDirectionExpression cde)
				{
					var c2s = Ch.CodeToString(cde.Expression);

					if (cde.Expression is CodeMethodInvokeExpression cmie2)
					{
						if (cmie2.Method.MethodName == "Index")//It was an array access.
						{
							var p1s = Ch.CodeToString(cmie2.Parameters[0]);
							var p2s = string.Join(", ", cmie2.Parameters.Cast<CodeExpression>().Skip(1).Select(Ch.CodeToString));
							_ = newParams.Add(new CodeSnippetExpression($"Mrh({i1}, {c2s}, v => SetObject(v, {p1s}, {p2s}))"));
							continue;
						}
						else if (cmie2.Method.MethodName == "GetPropertyValue")
						{
							var p1s = Ch.CodeToString(cmie2.Parameters[0]);
							var p2s = Ch.CodeToString(cmie2.Parameters[1]);
							_ = newParams.Add(new CodeSnippetExpression($"Mrh({i1}, {c2s}, v => SetPropertyValue({p1s}, {p2s}, v))"));
							continue;
						}
						else
							throw new Error("Cannot create a reference to a value returned from a function call.");
					}

					_ = newParams.Add(new CodeSnippetExpression($"Mrh({i1}, {c2s}, v => {c2s} = v)"));
				}
				else
					_ = newParams.Add(p);
			}

			return newParams;
		}

		private void HandleInvokeParams(List<int> refIndexes, CodeExpression[] passed, CodeMethodInvokeExpression invoke, bool lastisstar, bool dynamic)
		{
			var passedLen = passed != null ? passed.Length : 0;

			if (passedLen > 0)
			{
				if (passed.Length == 1 && passed[0].WasCboe() is CodeBinaryOperatorExpression cbe && (cbe.Operator == CodeBinaryOperatorType.BooleanAnd || cbe.Operator == CodeBinaryOperatorType.BooleanOr))
				{
					_ = invoke.Parameters.Add(new CodeMethodInvokeExpression(passed[0], "ParseObject"));
				}
				else
				{
					if (lastisstar)
					{
						invoke.Parameters.AddRange(passed.Take(passed.Length - 1).ToArray());
						var combineExpr = LocalMethodInvoke("FlattenParam");
						_ = combineExpr.Parameters.Add(passed[passed.Length - 1]);
						_ = invoke.Parameters.Add(combineExpr);
					}
					else
						invoke.Parameters.AddRange(passed);

					for (var i1 = 0; i1 < invoke.Parameters.Count; i1++)
					{
						if (refIndexes.Contains(i1))
						{
							var p = invoke.Parameters[i1];
							invoke.Parameters[i1] = new CodeDirectionExpression(FieldDirection.Ref, p);
						}
					}
				}
			}

			if (dynamic)
			{
				if (invoke.Parameters[0] is CodeMethodInvokeExpression cmie)
					_ = cmie.Parameters.Add(new CodePrimitiveExpression(passedLen));
			}
		}

		private CodeAssignStatement ParseAssign(CodeLine codeLine, string code)
		{
			string name, value;
			var buf = new StringBuilder(code.Length);
			var i = 0;
			char sym;
			var bound = false;

			for (i = 0; i < code.Length; i++)
			{
				sym = code[i];

				if (IsIdentifier(sym) || sym == Resolve)
					_ = buf.Append(sym);
				else if (sym == Equal)
				{
					i++;
					bound = true;
					break;
				}
				else if (IsSpace(sym))
					break;
				else
					throw new ParseException(ExUnexpected, codeLine);
			}

			if (!bound)
			{
				while (i < code.Length)
				{
					sym = code[i];
					i++;

					if (sym == Equal)
					{
						bound = true;
						break;
					}
				}

				if (!bound)
					throw new ParseException(ExUnexpected, codeLine);
			}

			name = buf.ToString();
			buf.Length = 0;
			value = code.Substring(i);
			value = value.Length == 0 ? null : StripCommentSingle(value.Trim(Spaces));
			CodeExpression left;
			left = Reflections.flatPublicStaticProperties.TryGetValue(name, out var pi)
				   ? new CodeVariableReferenceExpression(pi.Name)//Using static declarations obviate the need for specifying the static class type.
				   : VarId(codeLine, name, true);
			var result = value == null ? nullPrimitive : IsExpressionParameter(value) ? ParseSingleExpression(codeLine, value.TrimStart(Spaces).Substring(2), false) : VarIdExpand(codeLine, value);
			return new CodeAssignStatement(left, result);
		}

		private CodeExpression ParseExpression(CodeLine codeLine, string code, List<object> parts, bool create)
		{
			RemoveExcessParentheses(codeLine, parts);
			var rescanned = false;
			start:
			var rescan = false;

			for (var i = 0; i < parts.Count; i++)
			{
				if (parts[i] is string part)
				{
					if (part[0] == ParenOpen)
					{
						var n = i + 1;
						var paren = ExtractRange(parts, n, Set(parts, i, codeLine));
						parts.RemoveAt(n);
						n -= 2;
						var call = n > -1 && parts[n] is CodeExpression && !(parts[n] is CodePrimitiveExpression);

						if (call && parts[n] is CodeMethodInvokeExpression cmie && cmie.Parameters[0] is CodeFieldReferenceExpression)
							call = false;

						if (call)
						{
							var invoke = (CodeMethodInvokeExpression)InternalMethods.Invoke;
							CodeMethodInvokeExpression tempinvoke = null;
							var scope = Scope.ToLower();

							//Distinguish between Index which is an array or dictionary lookup and GetMethodOrProperty which is getting a method or property to be called.
							if (parts[n] is CodeMethodInvokeExpression indexcmie)
							{
								if (indexcmie.Method.MethodName == "Index")
								{
									var tupleInvoke = (CodeMethodInvokeExpression)InternalMethods.MakeObjectTuple;
									tupleInvoke.Parameters.Clear();
									_ = tupleInvoke.Parameters.Add(nullPrimitive);
									_ = tupleInvoke.Parameters.Add(indexcmie);
									parts[n] = tupleInvoke;//Replace Index() with MakeObjectTuple(null, Index()).
								}
								else
								{
									//var tupleExpr = new CodeSnippetExpression($"(null, {Ch.CodeToString(indexcmie)})");
									//parts[n] = tupleExpr;
									tempinvoke = (CodeMethodInvokeExpression)InternalMethods.GetMethodOrProperty;
									tempinvoke.Parameters.Clear();
									tempinvoke.Parameters.AddRange(indexcmie.Parameters);
									parts[n] = tempinvoke;//Replace GetPropertyValue() with GetMethodOrProperty().
									getMethodCalls[typeStack.Peek()].GetOrAdd(scope).Add(tempinvoke);
								}
							}

							if (paren.Count > 0)
							{
								var lastisstar = paren.Last().ToString() == "*";
								var (refIndexes, args) = ParseArguments(paren);

								if (paren.Count != 0)
								{
									var passed = new List<CodeExpression>();
									var tempCode = TokensToCode(paren);
									var codeStrings = SplitStringBalanced(tempCode, ',');//Tricky here: we need the code, not the parsed tokens.

									//Each argument must be parsed as a single expression.
									//They cannot all be parsed together as a multi expression because the comma delimiter conflicts.
									if (args.Count == codeStrings.Count)
									{
										for (int i1 = 0; i1 < args.Count; i1++)
										{
											var arg = args[i1];
											var temp = codeStrings[i1].TrimStart('&');
											var expr = ParseExpression(codeLine, temp, arg, true/*create false*/);//Override the value of create with true because reference arguments passed to a function should always be created automatically.

											if (expr.WasCboe() is CodeBinaryOperatorExpression cboe)
											{
												_ = parent.Add(expr);
												passed.Add(cboe.Left);
											}
											else
												passed.Add(expr);
										}
									}
									else
										throw new ParseException($"Error parsing function call arguments.", codeLine);

									HandleInvokeParams(refIndexes, passed.ToArray(), invoke, lastisstar, false);

									if (refIndexes.Count > 0)
									{
										IList list = invoke.Parameters;

										//This code is to handle passing by reference to a class method call.
										//It's a gruesome, grisly hack, but there is no other way to do this in C#.
										for (int i1 = 0; i1 < refIndexes.Count; i1++)
										{
											var ri = refIndexes[i1];

											if (list[ri] is CodeDirectionExpression cde)
											{
												var c2s = Ch.CodeToString(cde.Expression);

												if (cde.Expression is CodeMethodInvokeExpression cmie2)
												{
													if (cmie2.Method.MethodName == "Index")//It was an array access.
													{
														var p1s = Ch.CodeToString(cmie2.Parameters[0]);
														var p2s = string.Join(", ", cmie2.Parameters.Cast<CodeExpression>().Skip(1).Select(Ch.CodeToString));
														invoke.Parameters[ri] = new CodeSnippetExpression($"Mrh({ri}, {c2s}, v => SetObject(v, {p1s}, {p2s}))");
														continue;
													}
													else if (cmie2.Method.MethodName == "GetPropertyValue")
													{
														var p1s = Ch.CodeToString(cmie2.Parameters[0]);
														var p2s = Ch.CodeToString(cmie2.Parameters[1]);
														invoke.Parameters[ri] = new CodeSnippetExpression($"Mrh({ri}, {c2s}, v => SetPropertyValue({p1s}, {p2s}, v))");
														continue;
													}
													else
														throw new Error("Cannot create a reference to a value returned from a function call.");
												}

												invoke.Parameters[ri] = new CodeSnippetExpression($"Mrh({ri}, {c2s}, v => {c2s} = v)");
											}
										}

										invoke.Method.MethodName = "InvokeWithRefs";//Change method name to handle refs. This is much less efficient.
									}
								}
							}

							invoke.Parameters.Insert(0, (CodeExpression)parts[n]);//Must come after HandleInvokeParams() due to the indexing there assuming param count is 0.

							if (tempinvoke != null)
								_ = tempinvoke.Parameters.Add(new CodePrimitiveExpression(invoke.Parameters.Count - 1));

							parts[i] = invoke;
							parts.RemoveAt(n);
							i--;//Do this to ensure we don't miss a token, particularly in the case of chained method calls such as: MyGui.Add("Button",, "Click Me").OnEvent("Click", "MenuHandler").
							allMethodCalls[typeStack.Peek()].GetOrAdd(scope).Add(invoke);
						}
						else
						{
							if (i < parts.Count - 1 && parts[i + 1] as string == "=>")
							{
								parts[i] = paren;
							}
							else if (paren.Count == 0)
							{
								parts.RemoveAt(i);
							}
							else
							{
								var tempCode = TokensToCode(paren);
								parts[i] = ParseExpression(codeLine, tempCode, paren, create);
							}
						}
					}
					else if (part[0] == ParenClose)
						rescan = true;
					else if (part.Length > 1 && part[0] == StringBound && part[part.Length - 1] == StringBound)
						parts[i] = new CodePrimitiveExpression(EscapedString(part.Substring(1, part.Length - 2), false));
					else if (PrimitiveToExpression(part) is CodeExpression result)
					{
						parts[i] = result;
						//if (result != null)
						//{
						//  var longresult = result.ParseLong(false);//Also supports hex.
						//
						//  if (longresult.HasValue)
						//      parts[i] = new CodeSnippetExpression($"{longresult.Value}L");
						//  else
						//      parts[i] = new CodePrimitiveExpression(result);
						//}
						//else
						//  parts[i] = new CodePrimitiveExpression(result);
					}
					else if (IsIdentifier(part, true) && !IsKeyword(part))
					{
						var s = i < parts.Count - 1 && parts[i + 1] is string s1 ? s1 : "";
						var varexpr = Reflections.flatPublicStaticProperties.TryGetValue(part, out var pi)
									  ? new CodeVariableReferenceExpression(pi.Name)//Using static declarations obviate the need for specifying the static class type.
									  //Check for function or property calls on an object, which only count as read operations.
									  : VarIdOrConstant(codeLine, part,
														(i == 0 || (s.EndsWith('=') && parts[i - 1] is CodeBinaryOperatorType cbot && cbot == CodeBinaryOperatorType.Assign)) && //This accounts for either the left side of a single assignment like x := 0 or a multi-assignment like x := y := 0, where we create x and y.
														(!s.StartsWith("[") || parts.Count == 1) &&
														(create || s.EndsWith('='))//Covers :=, +=, -= etc...
														, false);

						if (varexpr is CodeVariableReferenceExpression cvre && (string.Compare(cvre.VariableName, "this", true) == 0 || string.Compare(cvre.VariableName, "@this", true) == 0) && InClassDefinition())
						{
							varexpr = new CodeThisReferenceExpression();
						}

						parts[i] = varexpr;
					}
					else if (part.Length == 1 && part[0] == BlockOpen)
					{
						//This will prevent proper parsing of fat arrow functions when defining
						//them inside of a map passed to a function like DefineProp().
						//So temporarily remove it here and restore it below.
						var n = i + 1;
						var paren = ExtractRange(parts, n, Set(parts, i, codeLine));
						var invoke = (CodeMethodInvokeExpression)InternalMethods.Dictionary;
						ParseObject(codeLine, code, paren, out var keys, out var values, create);//Might want this to always be false, because it would seem weird to create variable here inside of the arguments passed to Map().
						_ = invoke.Parameters.Add(new CodeArrayCreateExpression(typeof(object), keys));
						_ = invoke.Parameters.Add(new CodeArrayCreateExpression(typeof(object), values));
						parts[i] = invoke;
						parts.RemoveAt(n);
						i--;
					}
					else if (part.Length > 0 && part == "[" || part == "[*")
					{
						var n = i + 1;

						if (part == "[*")//Special case for accessing a property. This is similar to the array construction one below, but different enough to warrant its own section.
						{
							//var closing = parts.FirstIndexOf(ob => ob.ToString() == "*]", n);
							var closing = parts.FindIndex(n, ob => ob.ToString() == "*]");

							if (closing != -1)
							{
								var proptokens = ExtractRange(parts, n, closing);
								parts.RemoveAt(n);

								if (i > 0 && parts[i - 1] is CodeExpression)
								{
									CodeMethodInvokeExpression invoke;

									if (n + 1 < parts.Count && parts[n].ToString() == ":=")//Detect if this was a property assignment by searching for a := then a token after it, which is the value to assign.
									{
										var valtokens = ExtractRange(parts, n + 1, parts.Count);
										var val = ParseMultiExpression(codeLine, code, valtokens.ToArray(), create);
										invoke = (CodeMethodInvokeExpression)InternalMethods.SetPropertyValue;
										n = i - 1;
										//if (parts[n] is CodeVariableReferenceExpression cvre && string.Compare(cvre.VariableName, "this", true) == 0)
										//{
										//  var cse = new CodeSnippetExpression("this");//Required because otherwise it will be @this, which fails to compile.
										//  parts[n] = cse;
										//  _ = invoke.Parameters.Add(cse);
										//}
										//else
										_ = invoke.Parameters.Add((CodeExpression)parts[n]);
										var index = ParseMultiExpression(codeLine, code, proptokens.ToArray(), create);

										if (index.Length > 1)
											throw new ParseException("Cannot have multipart expression in a property name.", codeLine);
										else if (index.Length == 0)
											throw new ParseException("Cannot access an empty property.", codeLine);
										else if (index[0] is CodeArrayIndexerExpression caie)
											_ = invoke.Parameters.Add(caie.Indices[0]);//Don't want to use Vars if the property was dynamically specified, rather use the expression that was returned as the index to Vars.
										else
											_ = invoke.Parameters.Add(index[0]);

										_ = invoke.Parameters.Add(val[0]);
										parts.RemoveAt(n);//Remove first element (done again below).
										setPropertyValueCalls[typeStack.Peek()].GetOrAdd(Scope.ToLower()).Add(invoke);
									}
									else
									{
										invoke = (CodeMethodInvokeExpression)InternalMethods.GetPropertyValue;
										n = i - 1;

										if (parts[n] is CodeVariableReferenceExpression cvre && string.Compare(cvre.VariableName, "this", true) == 0)
										{
											var cse = new CodeSnippetExpression("this");//Required because otherwise it will be @this, which fails to compile.
											parts[n] = cse;
											_ = invoke.Parameters.Add(cse);
										}
										else
											_ = invoke.Parameters.Add((CodeExpression)parts[n]);

										var index = ParseMultiExpression(codeLine, code, proptokens.ToArray(), create);

										if (index.Length > 1)
											throw new ParseException("Cannot have multipart expression in a property name.", codeLine);
										else if (index.Length == 0)
											throw new ParseException("Cannot access an empty property.", codeLine);
										else if (index[0] is CodeArrayIndexerExpression caie)
											_ = invoke.Parameters.Add(caie.Indices[0]);//Don't want to use Vars if the property was dynamically specified, rather use the expression that was returned as the index to Vars.
										else
											_ = invoke.Parameters.Add(index[0]);//The problem is that this will have quotes on it at this point and be treated as a string no matter what, even if it's a dynamic var.//TODO

										getPropertyValueCalls[typeStack.Peek()].GetOrAdd(Scope.ToLower()).Add(invoke);
									}

									parts[i] = invoke;
									parts.RemoveAt(n);
									i--;
								}
							}
							else
								throw new ParseException("[* did not have a closing *].", codeLine);//This should never happen because it's autogenerated by this parser, so a user would never normally enter it.
						}
						else
						{
							var span = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(parts);
							var paren = ExtractRange(parts, n, n + ParseBalancedArray(span.Slice(i)));
							parts.RemoveAt(n);

							if (i > 0 && parts[i - 1] is CodeExpression)
							{
								var invoke = (CodeMethodInvokeExpression)InternalMethods.Index;
								n = i - 1;
								_ = invoke.Parameters.Add((CodeExpression)parts[n]);
								var index = ParseMultiExpression(codeLine, code, paren.ToArray(), create);

								if (index.Length == 0)
									_ = invoke.Parameters.Add(new CodeSnippetExpression("System.Array.Empty<object>()"));
								else if (index[0].WasCboe() is CodeBinaryOperatorExpression cbe && (cbe.Operator == CodeBinaryOperatorType.BooleanAnd || cbe.Operator == CodeBinaryOperatorType.BooleanOr))
									_ = invoke.Parameters.Add(new CodeMethodInvokeExpression(index[0], "ParseObject"));
								else
									foreach (var p in index)
										_ = invoke.Parameters.Add(p);

								parts[i] = invoke;
								parts.RemoveAt(n);
								i--;
							}
							else
							{
								var argi = 0;
								var paramExprs = new List<CodeExpression>();
								var parenspan = System.Runtime.InteropServices.CollectionsMarshal.AsSpan(paren);

								while (argi < paren.Count)
								{
									var tempi = ParseBalanced(parenspan.Slice(argi), ",");
									var temprange = parenspan.Slice(argi, tempi);

									if (temprange.Length > 0)
									{
										var temparr = temprange.ToArray();
										var tempcode = string.Join("", temparr.Select((p) => p.ToString()));
										var tempexpr = ParseMultiExpression(codeLine, tempcode, temparr, create);
										paramExprs.AddRange(tempexpr);
									}

									argi += tempi + 1;//Account for the comma.
								}

								if (paren.Count > 0 && paren[paren.Count - 1].ToString() == ",")
									paramExprs.Add(nullPrimitive);

								parts[i] = new CodeObjectCreateExpression(typeof(Core.Array), new CodeArrayCreateExpression(typeof(object[]), paramExprs.ToArray()));
							}
						}
					}
					else if (part.Length > 1 && part[part.Length - 1] == ParenOpen)
					{
						var dynamic = false;
						var name = part.Substring(0, part.Length - 1);

						if (!IsIdentifier(name))
						{
							if (IsDynamicReference(name))
								dynamic = true;
							else
								throw new ParseException("Invalid function name.", codeLine);
						}
						else
							CheckPersistent(name);

						var n = i + 1;
						var paren = ExtractRange(parts, n, Set(parts, i, codeLine));
						parts.RemoveAt(n);
						CodeMethodInvokeExpression invoke = null;

						//Special processing is needed to convert an expression passed to a HotIf() to a separate function, then pass it with FuncObj().
						//Do not do this if a FuncObj is already being passed, whcih will be the case when #HotIf is found in the preprocessing stage.
						if (name == "HotIf" && paren.Count > 1 && !paren[0].ToString().StartsWith("FuncObj"))
						{
							var hotiffuncname = $"HotIf_{PreReader.NextHotIfCount}";
							var hotifexpr = ParseExpression(codeLine, code, paren, false);
							var hotifmethod = LocalMethod(hotiffuncname);
							_ = hotifmethod.Statements.Add(new CodeMethodReturnStatement(hotifexpr));
							methods[targetClass].Add(hotiffuncname, hotifmethod);
							invoke = LocalMethodInvoke(name);
							_ = invoke.Parameters.Add(new CodeSnippetExpression("FuncObj(\"" + hotiffuncname + "\")"));//Can't use interpolated string here because the AStyle formatter misinterprets it.
						}
						else//Any other function call aside from HotIf().
						{
							var origCode = code;
							var passed = new List<CodeExpression>();
							var lastisstar = paren.Count > 0 && paren.Last().ToString() == "*";
							var (refIndexes, args) = ParseArguments(paren);
							var tempCode = TokensToCode(paren);
							var codeStrings = SplitStringBalanced(tempCode, ',', true);//Tricky here: we need the code, not the parsed tokens.
							var scope = Scope.ToLower();

							//Each argument must be parsed as a single expression.
							//They cannot all be parsed together as a multi expression because the comma delimiter conflicts.
							if (args.Count == codeStrings.Count)
							{
								for (int i1 = 0; i1 < args.Count; i1++)
								{
									var arg = args[i1];
									tempCode = codeStrings[i1];
									var argExpr = ParseExpression(codeLine, tempCode, arg, false);//Override the value of create with false because the arguments passed into a function should never be created automatically.

									if (argExpr.WasCboe() is CodeBinaryOperatorExpression cboe)
									{
										_ = parent.Add(argExpr);
										argExpr = cboe.Left;
									}

									//Allow for the declaration of a variable at the same time it's passed to a function call.
									if (argExpr is CodeVariableReferenceExpression cvre)
									{
										if (!VarExistsAtCurrentOrParentScope(typeStack.Peek(), scope, cvre.VariableName)
												&& !Reflections.flatPublicStaticProperties.TryGetValue(cvre.VariableName, out _))
											allVars[typeStack.Peek()].GetOrAdd(scope)[cvre.VariableName] = emptyStringPrimitive;
									}

									passed.Add(argExpr);
								}
							}
							else
								throw new ParseException($"Error parsing function call arguments.", codeLine);

							if (dynamic)
							{
								invoke = (CodeMethodInvokeExpression)InternalMethods.Invoke;
								var getmethod = (CodeMethodInvokeExpression)InternalMethods.GetMethodOrProperty;
								var expand = VarIdExpand(codeLine, name);
								_ = getmethod.Parameters.Add(nullPrimitive);

								//The returned object is a call to Vars, but since we are doing a function call here
								//we need to call Invoke() instead, but we still want the arguments that
								//were passed to Vars, so extract them.
								if (expand is CodeArrayIndexerExpression caie)
									foreach (CodeExpression index in caie.Indices)
									{
										if (index is CodeVariableReferenceExpression cvre)
											_ = getmethod.Parameters.Add(new CodeSnippetExpression($"{cvre.VariableName}.ToString()"));
										else
											_ = getmethod.Parameters.Add(index);
									}

								_ = invoke.Parameters.Add(getmethod);
							}
							else
								invoke = LocalMethodInvoke(name);

							HandleInvokeParams(refIndexes, passed.ToArray(), invoke, lastisstar, dynamic);
							var needInvoke = false;

							//If a variable by the same name as the function exists at the current or parent scope,
							//assume it's a lambda. In that case, don't convert it to an invoke here.
							//Instead, do it in Parser.GeneratecompileUnit().
							if (!VarExistsAtCurrentOrParentScope(typeStack.Peek(), scope, name))
							{
								foreach (var ri in refIndexes)
								{
									if (invoke.Parameters[ri] is CodeDirectionExpression cde && (
												string.Compare(name, "dllcall", true) == 0 ||//These two are hacks, but they are special functions which take variadic arguments that can be refs.
												string.Compare(name, "comcall", true) == 0 ||
												cde.Expression is CodeMethodInvokeExpression cmie || cde.Expression is CodeArrayIndexerExpression))
									{
										needInvoke = true;
										break;
									}
								}
							}

							//Due to references to method call results, this method cannot be called directly and instead invoke must be used.
							if (needInvoke)
							{
								var oldInvoke = invoke;
								invoke = (CodeMethodInvokeExpression)InternalMethods.InvokeWithRefs;
								var objExpr = typeStack.Peek().Name == mainClassName ? new CodeSnippetExpression("null") : new CodeSnippetExpression("FindObjectForMethod(this, \"" + oldInvoke.Method.MethodName + $"\", {oldInvoke.Parameters.Count})");
								var gmop = (CodeMethodInvokeExpression)InternalMethods.GetMethodOrProperty;
								_ = gmop.Parameters.Add(objExpr);
								_ = gmop.Parameters.Add(new CodePrimitiveExpression(oldInvoke.Method.MethodName));
								_ = gmop.Parameters.Add(new CodePrimitiveExpression((long)oldInvoke.Parameters.Count));
								_ = invoke.Parameters.Add(gmop);
								invoke.Parameters.AddRange(ConvertDirectParamsToInvoke(oldInvoke.Parameters));
							}

							allMethodCalls[typeStack.Peek()].GetOrAdd(scope).Add(invoke);
						}

						parts[i] = invoke;
						invokes.Add(invoke);
					}
					//This is very strange and prevents single = from working in if statements when comparing variables such as if (x = y)
					//Not exactly sure what an "implicit assignment" is, since it's not mentioned anywhere in the documentation
					else if (IsAssignOp(part))// || IsImplicitAssignment(parts, i))
					{
						var n = i - 1;

						if (i > 0 && IsJsonObject(parts[n])) { }
						else if (n < 0 || !IsVarReference(parts[n]))
						{
							if (LaxExpressions)
							{
								if (parts[n] is CodePrimitiveExpression cpe)
								{
									if (cpe.Value is decimal decval)
										parts[n] = VarId(codeLine, decval.ToString(), i == 0 && create);
									else if (cpe.Value is double d)
										parts[n] = VarId(codeLine, d.ToString(), i == 0 && create);
								}
							}
							else
								throw new ParseException("Can only assign to a variable.", codeLine);
						}

						// (x += y) => (x = x + y)
						parts[i] = CodeBinaryOperatorType.Assign;

						if (part[0] != AssignPre && part.Length != 1)
						{
							parts.Insert(++i, ParenOpen.ToString());
							parts.Insert(++i, parts[i - 3]);

							if (part.Length > 1)
							{
								var opStr = part.Substring(0, part.Length - 1);
								var (opValid, op) = OperatorFromString(opStr);

								if (opValid)
								{
									parts.Insert(++i, op);
									parts.Insert(++i, ParenOpen.ToString());
									parts.Add(ParenClose.ToString());
								}
								else
									throw new ParseException($"Invalid operator: {opStr}", codeLine);
							}

							parts.Add(ParenClose.ToString());

							//Need to take special action if it was +=, -= etc... on a property.
							if (parts[0] is CodeMethodInvokeExpression cmie && cmie.Method.MethodName == "GetPropertyValue" && parts[1] is CodeBinaryOperatorType cbot && cbot == CodeBinaryOperatorType.Assign)
							{
								var propobj = cmie.Parameters[0];
								var propname = cmie.Parameters[1];
								var invoke = (CodeMethodInvokeExpression)InternalMethods.SetPropertyValue;
								_ = invoke.Parameters.Add(propobj);
								_ = invoke.Parameters.Add(propname);
								var extract = ExtractRange(parts, 2, parts.Count);
								var val = ParseMultiExpression(codeLine, code, extract.ToArray(), create);
								_ = invoke.Parameters.Add(val[0]);
								parts[0] = invoke;
								setPropertyValueCalls[typeStack.Peek()].GetOrAdd(Scope.ToLower()).Add(invoke);
								parts.RemoveAt(1);
							}
						}
					}
					else if (part.Length == 1 && part[0] == Multicast)
					{
						if (!LaxExpressions)
							throw new ParseException("Nested multipart expression not allowed.", codeLine);

						// implement as: + Dummy(expr..)
						int z = i + 1, l = parts.Count - z;
						var sub = new List<object>(l);

						for (; z < parts.Count; z++)
							sub.Add(parts[z]);

						parts.RemoveRange(i, parts.Count - i);
						var invoke = (CodeMethodInvokeExpression)InternalMethods.OperateZero;
						_ = invoke.Parameters.Add(ParseExpression(codeLine, code, sub, create));
						parts.Add(Script.Operator.Add);
						parts.Add(invoke);
					}
					else if (part == "=>")
					{
						var assignIndex = parts.FindIndex(0, parts.Count, o => o is CodeBinaryOperatorType cbot && cbot == CodeBinaryOperatorType.Assign);
						var cmieIndex = parts.FindIndex(0, parts.Count, o => o is CodeMethodInvokeExpression cmie);
						var ctrpaa = new CodeTypeReference(typeof(System.ParamArrayAttribute));
						var cad = new CodeAttributeDeclaration(ctrpaa);
						var cmd = new CodeMemberMethod
						{
							ReturnType = new CodeTypeReference(typeof(object)),
							Attributes = InClassDefinition() ? MemberAttributes.Public | MemberAttributes.Final : MemberAttributes.Public | MemberAttributes.Static
						};
						void AddParts(CodeMemberMethod cmd)
						{
							var extracted = ExtractRange(parts, assignIndex + 1, parts.Count);//This works even when assignIndex is -1.
							//var extracted = ExtractRange(parts, i - 1, parts.Count);//Start at the parameters collection which should be right before parts[i] which is "=>".
							parts.Add("FuncObj(");
							parts.Add("\"" + cmd.Name + "\"");
							parts.Add(",");
							//parts.Add(InClassDefinition() ? new CodeThisReferenceExpression() : "null");
							parts.Add(InClassDefinition() ? "this" : "null");
							parts.Add(",");
							parts.Add($"{cmd.Parameters.Count}");
							parts.Add(")");
							i = assignIndex;//Move i back to force parsing of the tokens we just added.
						}
						void MakeMethod(CodeMemberMethod cmd)
						{
							string retstr;
							var bodyParts = parts.Take(new Range(i + 1, parts.Count)).ToArray();
							var arrowIndex = code.IndexOf("=>");
							AddParts(cmd);
							var lineNumber = codeLines.IndexOf(codeLine) + 1;
							//Inefficient to recompose the parameters into a string, but unsure what else to do here.
							codeLines.Insert(lineNumber, new CodeLine(codeLine.FileName, lineNumber++, $"{cmd.Name}({string.Join(',', cmd.Parameters.Cast<CodeParameterDeclarationExpression>().Select(p => (p.Direction == FieldDirection.Ref ? "&" : "") + p.Name + (p.CustomAttributes.Cast<CodeAttributeDeclaration>().Any(c => c.AttributeType.BaseType == ctrpaa.BaseType) ? "*" : "")))})"));
							codeLines.Insert(lineNumber, new CodeLine(codeLine.FileName, lineNumber++, "{"));
							var subs = SplitStringBalanced(code.Substring(arrowIndex + 2), ',');
							var ct = subs.Count - 1;

							for (var sindex = 0; sindex < ct; sindex++)
								codeLines.Insert(lineNumber, new CodeLine(codeLine.FileName, lineNumber++, subs[sindex]));

							retstr = subs[ct];

							if (!retstr.StartsWith("return", StringComparison.OrdinalIgnoreCase))
								retstr = "return " + retstr;

							codeLines.Insert(lineNumber, new CodeLine(codeLine.FileName, lineNumber++, retstr));
							codeLines.Insert(lineNumber, new CodeLine(codeLine.FileName, lineNumber++, "}"));
							SetLineIndexes();
						}

						if (cmieIndex != -1 && cmieIndex <  i)//Named func member := memberfunc(a) => a * 2
						{
							var cmie = parts[cmieIndex] as CodeMethodInvokeExpression;
							cmd.Name = cmie.Method.MethodName;

							foreach (var p in cmie.Parameters)
							{
								if (p is CodeVariableReferenceExpression cvre)
								{
									_ = cmd.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), cvre.VariableName));
								}
								else if (p is CodeDirectionExpression cde)
								{
									var c2s = Ch.CodeToString(cde.Expression);
									_ = cmd.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), c2s) { Direction = FieldDirection.Ref });
								}
								//If a * was included, it will have been parsed above as a function argument, rather than a function parameter
								//and such arguments are passed as a call to FlattenParam(). So catch that here and create the appropriate variadic parameter.
								else if (p is CodeMethodInvokeExpression cmie2 && cmie2.Method.MethodName == "FlattenParam")
								{
									foreach (var p2 in cmie2.Parameters)
										if (p2 is CodeVariableReferenceExpression cvre2)
										{
											var cpde = new CodeParameterDeclarationExpression(typeof(object[]), cvre2.VariableName);
											_ = cpde.CustomAttributes.Add(cad);
											_ = cmd.Parameters.Add(cpde);
										}
								}
							}

							MakeMethod(cmd);

							//Now need to handle creating a function object and assigning.
							if (assignIndex != -1 && assignIndex < i)
								AddParts(cmd);
						}
						else if (assignIndex != -1 && assignIndex < i)
						{
							if (parts[assignIndex + 1] is List<object> lo)//Parens like member := (a) => a * 2
							{
								var funcParams = ParseFunctionParameters(string.Join("", lo), codeLine);
								cmd.Name = $"anonfunc_{labelCount++:X}";

								if (funcParams != null)
									foreach (var fp in funcParams)
										_ = cmd.Parameters.Add(fp);

								MakeMethod(cmd);
							}
							else if (parts[assignIndex + 1] is CodeVariableReferenceExpression cvre)//No parens like member := a => a * 2
							{
								cmd.Name = $"anonfunc_{labelCount++:X}";

								if (parts[assignIndex + 2] as string == "*")//Account for member4 := a* => (a[1] + a[2]) * 2
								{
									var cpde = new CodeParameterDeclarationExpression(typeof(object[]), cvre.VariableName);
									_ = cpde.CustomAttributes.Add(cad);
									_ = cmd.Parameters.Add(cpde);
								}
								else
									_ = cmd.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), cvre.VariableName));

								MakeMethod(cmd);
							}
						}
						else if (assignIndex == -1 && cmieIndex == -1)//Defined on the fly.
						{
							if (parts[0] is List<object> lo)
							{
								var funcParams = ParseFunctionParameters(string.Join("", lo), codeLine);
								cmd.Name = $"anonfunc_{labelCount++:X}";

								if (funcParams != null)
									foreach (var fp in funcParams)
										_ = cmd.Parameters.Add(fp);

								MakeMethod(cmd);
							}
						}
						else
							throw new Exception($"Unsupported tokens surrounding a fat arrow function definition at line {codeLine}");
					}
					else if (i == parts.Count - 1 || (i < parts.Count - 1 && parts[i + 1] as string != "=>"))//Allow for a single no parentheses fat arrow function variadic parameter declaration: x := a* => 123.
					{
						var (opValid, ops) = OperatorFromString(part);

						if (opValid && (ops == Script.Operator.TernaryA || ops == Script.Operator.TernaryB))
						{
							parts[i] = ops;
							var n = i + 1;
							var nextOp = parts.FindIndex(i, (p) =>
							{
								var ps = p.ToString();
								return ps == "?" || ps == ":";
							});

							if (nextOp == -1)
								nextOp = parts.Count;

							var ternaryParts = ExtractRange(parts, n, nextOp);

							while (ternaryParts[0].ToString() == "("
									&& ternaryParts[ternaryParts.Count - 1].ToString() != ")"
									&& !IsBalanced(ternaryParts, "(", ")"))
							{
								ternaryParts.RemoveAt(0);
							}

							while (ternaryParts[0].ToString() != "("
									&& !ternaryParts[0].ToString().EndsWith("(")
									&& ternaryParts[ternaryParts.Count - 1].ToString() == ")"
									&& !IsBalanced(ternaryParts, "(", ")"))
							{
								ternaryParts.RemoveAt(ternaryParts.Count - 1);
							}

							var ternaryCode = TokensToCode(ternaryParts);
							var ternaryExpr = ParseExpression(codeLine, ternaryCode, ternaryParts, false);
							parts.Insert(n, ternaryExpr);
						}
						else if (opValid && (ops == Script.Operator.Increment || ops == Script.Operator.Decrement))
						{
							int z = -1, x = i - 1, y = i + 1;
							var d = 1L;
							CodeMethodInvokeExpression shadow = null;

							if (x > -1 && parts[x] is CodeMethodInvokeExpression)
							{
								var sub = new List<object>(5)
								{
									parts[x],
										  CodeBinaryOperatorType.Assign,
										  parts[x],
										  Script.Operator.Add,
										  d
								};
								parts.RemoveAt(i);
								parts[x] = ParseExpression(codeLine, code, sub, create);
								i = x;
								continue;
							}

							if (LaxExpressions)
							{
								while (y < parts.Count)
								{
									var nextOps = Script.Operator.ValueEquality;

									if (parts[y] is Script.Operator so)
										nextOps = so;
									else if (parts[y] is string os)
									{
										var (nextOpValid, tempOp) = OperatorFromString(os);

										if (nextOpValid)
											nextOps = tempOp;
										else
											break;
									}
									else
										break;

									if (nextOps == Script.Operator.Increment)
										d++;
									else if (nextOps == Script.Operator.Decrement)
										d--;
									else
										break;

									parts.RemoveAt(y);
								}
							}

							if (x > -1 && (IsVarReference(parts[x]) || parts[x] is CodePropertyReferenceExpression))
								z = x;

							if (y < parts.Count && parts[y] is string sy && !IsOperator(sy))
							{
								if (z != -1)
								{
									if (LaxExpressions)
									{
										parts.Insert(y, Script.Operator.Concat);
										z = x;
									}
									else
										throw new ParseException("Cannot use both prefix and postfix operators on the same variable.", codeLine);
								}

								if (z == -1)
									z = y;

								if (LaxExpressions)
								{
									if (parts[z] is string psz && psz.Length == 1 && psz[0] == ParenOpen)
									{
										var zx = new[] { z + 1, z + 2 };

										if (zx[1] < parts.Count &&
												parts[zx[1]] is string zx1 && zx1.Length == 1 && zx1[0] == ParenClose &&
												(parts[zx[0]] is string zx0 && IsDynamicReference(zx0) || IsVarReference(parts[zx[0]])))
										{
											parts.RemoveAt(zx[1]);
											parts.RemoveAt(z);
										}
										else
										{
											parts.RemoveAt(i);
											i--;
											continue;
										}
									}
								}
							}

							if (z == -1)
							{
								if (LaxExpressions)
								{
									if ((x > 0 && ((parts[x] as CodeExpression).WasCboe() is CodeBinaryOperatorExpression || parts[x] is CodeMethodInvokeExpression || parts[x] is CodePrimitiveExpression)) ||
											(y < parts.Count && (parts[y] is string && !IsOperator(parts[y] as string) || parts[y] is Script.Operator)))
									{
										parts.RemoveAt(i);
										i--;
										continue;
									}
								}
								else
									throw new ParseException("Neither left or right hand side of operator is a variable.", codeLine);
							}

							if (parts[z] is string sz && sz.Length > 0 && sz[0] == StringBound)
							{
								parts.RemoveAt(Math.Max(i, z));
								parts.RemoveAt(Math.Min(i, z));
								continue;
							}

							if (LaxExpressions)
							{
								var w = z + (z == x ? 2 : 1);

								if (w < parts.Count && ((parts[w] is string sw
														 && IsAssignOp(sw)) || IsVarAssignment(parts[w])))
								{
									var l = parts.Count - w;
									var sub = new List<object>(l + 1)
									{
										parts[z]
									};

									for (var wx = w; wx < parts.Count; wx++)
										sub.Add(parts[wx]);

									shadow = (CodeMethodInvokeExpression)InternalMethods.OperateZero;
									_ = shadow.Parameters.Add(ParseExpression(codeLine, code, sub, create));
									parts.RemoveRange(w, l);
								}
							}

							var list = new List<object>(9)
							{
								parts[z],
									  "+=",
									  new CodeSnippetExpression($"{(ops == Script.Operator.Increment ? d : -d)}L")
							};

							if (shadow != null)
							{
								list.Add(Script.Operator.Add);
								list.Add(shadow);
							}

							if (z < i) // postfix, so adjust
							{
								list.Insert(0, ParenOpen.ToString());
								list.Add(ParenClose.ToString());
								list.Add(ops == Script.Operator.Increment ? Script.Operator.Minus : Script.Operator.Add);
								list.Add(new CodePrimitiveExpression(d));
							}

							x = Math.Min(i, z);
							y = Math.Max(i, z);
							parts[x] = ParseExpression(codeLine, code, list, create);
							parts.RemoveAt(y);
							i = x;
						}
						else if (part == "&" && i > 0 && parts[i - 1] is CodeBinaryOperatorType op && op == CodeBinaryOperatorType.Assign &&
								 i < parts.Count - 1 && parts[i + 1] is string s2)
						{
							//There was a lambda declared with no parens and the only parameter was a ref.
							//m := &a => a := (a * 2)
							//So hack it by surrounding the parameter with parens, and reparsing because that will parse correctly.
							parts[i] = "(";
							parts.Insert(i + 1, "&");
							parts.Insert(i + 3, ")");
							i--;
						}
						else
						{
							if (part.Length == 1 && part[0] == Dereference)
							{
								var deref = false;

								if (i == 0)
									deref = true;
								else
								{
									var x = i - 1;
									deref = parts[x] is Script.Operator || IsVarAssignment(parts[x]) ||
											(parts[x] is string sx && sx.Length == 1 && sx[0] == '(');
								}

								if (deref)
								{
									var y = i + 1;

									if (y < parts.Count && (IsVarReference(parts[y]) ||
															(parts[y] is string sy && IsIdentifier(sy) && !IsKeyword(sy))))
										ops = Script.Operator.Dereference;
								}
							}

							parts[i] = ops;
						}
					}
				}
			}

			if (rescan && !rescanned)
			{
				rescanned = true;//Prevent infinite loop.
				goto start;
			}

			for (var i = 1; i < parts.Count; i++)
			{
				if (parts[i] is Script.Operator op &&
						(
							parts[i - 1] is Script.Operator ||
							parts[i - 1] as CodeBinaryOperatorType? == CodeBinaryOperatorType.Assign || IsVarAssignment(parts[i - 1])
						) &&
						IsUnaryOperator(op))
				{
					int n = i + 1, m = n + 1;
					var u = n;

					while (u < parts.Count && parts[u] is Script.Operator sou && IsUnaryOperator(sou)) u++;

					if (u == parts.Count)
					{
						if (LaxExpressions)
						{
							u--;

							while (parts[u] is Script.Operator sou && (sou == Script.Operator.Add || sou == Script.Operator.Subtract))
								parts.RemoveAt(u--);

							if (u + 1 < n)
							{
								i = u;
								continue;
							}
						}

						throw new ParseException("Compounding unary operator with no operand.", codeLine);
					}

					if (u > n)
					{
						var sub = new List<object>(++u - n);

						for (var x = n; x < u; x++)
							sub.Add(parts[x]);

						parts.RemoveRange(n, u - n);
						parts.Insert(n, ParseExpression(codeLine, code, sub, create));
					}

					if (m + 1 < parts.Count && IsVarReference(parts[n]) && IsVarAssignment(parts[m]))
						MergeAssignmentAt(codeLine, parts, i + 2);

					if (m > parts.Count)
						throw new ParseException("Unary operator without operand.", codeLine);

					//var op = (Script.Operator)parts[i];

					//if (parts.Count > n + 1
					//      && parts[n + 1] is Script.Operator opp1
					//      && opp1 == Script.Operator.Power)
					//{
					//  Console.WriteLine("power op is next");
					//}

					if (parts[n] is CodePrimitiveExpression parent && op == Script.Operator.Subtract)
					{
						if (parent.Value is long ll)
							parent.Value = -ll;
						else if (parent.Value is double dd)
							parent.Value = -dd;
						else if (parent.Value is string ss)
							parent.Value = string.Concat(Minus.ToString(), ss);
						else if (parent.Value is int ii)
							parent.Value = -ii;
						else if (parent.Value is decimal mm)
							parent.Value = -mm;
						else
							throw new ParseException($"Value of {parent.Value} of type {parent.Value.GetType()} was not a type that can be used with the subtraction operator.", codeLine);

						parts.RemoveAt(i);
					}
					else if (parts[n] is CodeSnippetExpression cseparent && op == Script.Operator.Subtract)//Long requires snippet, since CodePrimitiveExpression does not append L.
					{
						cseparent.Value = cseparent.Value is string cses ? string.Concat(Minus.ToString(), cses) : throw new ParseException($"Value of {cseparent.Value} of type {cseparent.Value.GetType()} was not of type string.", codeLine);
						parts.RemoveAt(i);
					}
					else if (op == Script.Operator.Add)
					{
						parts.RemoveAt(i);
					}
					else
					{
						var invoke = (CodeMethodInvokeExpression)InternalMethods.OperateUnary;
						_ = invoke.Parameters.Add(OperatorAsFieldReference(op));
						_ = invoke.Parameters.Add(VarMixedExpr(codeLine, parts[n]));
						parts[i] = invoke;
						parts.RemoveAt(n);
					}
				}
			}

			var scan = true;
			var level = -1;

			//Support implicit comparison to empty string statements such as if (x !=)
			//by detecting that the last token was an operator with nothing following it.
			if (parts[parts.Count - 1] is Script.Operator opend)
			{
				parts.Add(emptyStringPrimitive);
			}

			//Unsure what this final loop actually does.
			while (scan)
			{
				scan = false;

				for (var i = 0; i < parts.Count; i++)
				{
					if (parts[i] is Script.Operator op && op != Script.Operator.Assign)
					{
						scan = true;

						if (OperatorPrecedence(op, codeLine) < level)
							continue;

						int x = i - 1, y = i + 1;
						var invoke = new CodeMethodInvokeExpression();

						if (i + 3 < parts.Count && IsVarReference(parts[i + 1]) && (parts[i + 2] as CodeBinaryOperatorType? ) == CodeBinaryOperatorType.Assign)
							MergeAssignmentAt(codeLine, parts, i + 2);

						if (op == Script.Operator.TernaryA)
						{
							if (x < 0)
							{
								if (LaxExpressions)
									return nullPrimitive;
								else
									throw new ParseException("Ternary with no condition.", codeLine);
							}

							var cond = VarMixedExpr(codeLine, parts[x]);
							var eval = (CodeMethodInvokeExpression)InternalMethods.IfElse;
							_ = eval.Parameters.Add(cond);
							int depth = 1, max = parts.Count - i, start = i;
							var branch = new[] { new List<object>(max), new List<object>(max) };

							for (i++; i < parts.Count; i++)
							{
								switch (parts[i] as Script.Operator? )
								{
									case Script.Operator.TernaryA:
										depth++;
										break;

									case Script.Operator.TernaryB:
										depth--;
										break;
								}

								if (depth == 0)
								{
									for (var n = i + 1; n < parts.Count; n++)
										branch[1].Add(parts[n]);

									break;
								}
								else
									branch[0].Add(parts[i]);
							}

							if (branch[0].Count == 0)
								throw new ParseException("Ternary operator must have at least one branch.", codeLine);

							if (branch[1].Count == 0)
								branch[1].Add(nullPrimitive);

							//No real way to recover the code here because all of the branches have been parsed to objects already.
							//It doesn't really matter though.
							//The branches have both already been parsed and should have been reduced to a single expression.
							var trueBranch = ParseExpression(codeLine, code, branch[0], create);
							var falseBranch = ParseExpression(codeLine, code, branch[1], create);
							//CodeDOM does not have built in support for ternary operators. So we must manually create the code string for the ternary,
							//then use a code snippet to hold the string. This is not ideal, but there is no other way.
							var evalstr = Ch.CodeToString(eval);
							var tbs = Ch.CodeToString(trueBranch);
							var fbs = Ch.CodeToString(falseBranch);
							parts[x] = new CodeSnippetExpression($"(_ = {evalstr} ? (object)({tbs}) : (object)({fbs}))");//Must explicitly cast both branches to object in case they are different types such as: x ? 1.0 : 1L (double and long).
							parts.RemoveRange(start, parts.Count - start);
							//i++;//Tried, but not needed.
							//continue;
						}
						else if (op == Script.Operator.NullAssign)
						{
							if (x < 0)
								throw new ParseException("Nullable assignment with no condition.", codeLine);

							var n = i + 1;

							if (n >= parts.Count)
								throw new ParseException("Nullable assignment with no right-hand operator.", codeLine);

							var orMaybe = (CodeMethodInvokeExpression)InternalMethods.OrMaybe;
							var leftExpr = VarMixedExpr(codeLine, parts[x]);
							var rightSlice = parts.Slice(n, parts.Count - n);
							var rightExpr = ParseExpression(codeLine, code, rightSlice, create);
							_ = orMaybe.Parameters.Add(leftExpr);
							_ = orMaybe.Parameters.Add(rightExpr);
							parts[x] = orMaybe;
							parts.RemoveRange(i, parts.Count - i);
						}
						else if (x == -1)
						{
							var z = y + 1;

							if (op == Script.Operator.LogicalNotEx && IsVarReference(parts[y]) && z < parts.Count)
								MergeAssignmentAt(codeLine, parts, z);

							if (LaxExpressions)
							{
								if (y > parts.Count - 1)
									return nullPrimitive;
							}

							invoke.Method = (CodeMethodReferenceExpression)InternalMethods.OperateUnary;
							_ = invoke.Parameters.Add(OperatorAsFieldReference(op));
							_ = invoke.Parameters.Add(VarMixedExpr(codeLine, parts[y]));
							parts[i] = invoke;
							parts.RemoveAt(y);
						}
						else
						{
							if (op == Script.Operator.BooleanAnd || op == Script.Operator.BooleanOr)
							{
								var boolean = new CodeBinaryOperatorExpression();
								boolean.Operator = op == Script.Operator.BooleanAnd ? CodeBinaryOperatorType.BooleanAnd : CodeBinaryOperatorType.BooleanOr;
								var iftest = (CodeMethodInvokeExpression)InternalMethods.IfElse;
								_ = iftest.Parameters.Add(VarMixedExpr(codeLine, parts[x]));
								boolean.Left = iftest;
								iftest = (CodeMethodInvokeExpression)InternalMethods.IfElse;
								var next = parts[y] as Script.Operator?;

								if (next == Script.Operator.BooleanAnd || next == Script.Operator.BooleanOr)
								{
									if (LaxExpressions)
										_ = iftest.Parameters.Add(new CodePrimitiveExpression(false));
									else
										throw new ParseException(ExInvalidExpression, codeLine);
								}
								else
								{
									_ = iftest.Parameters.Add(VarMixedExpr(codeLine, parts[y]));
									parts.RemoveAt(y);
								}

								boolean.Right = iftest;

								if (x > 0 && parts[x - 1] is CodeBinaryOperatorType cbot && cbot == CodeBinaryOperatorType.Assign)//z := x && y
									parts[x] = new CodeMethodInvokeExpression(boolean, "ParseObject");
								else if (x + 1 < parts.Count && parts[0] is CodeVariableReferenceExpression &&
										 parts[x + 1] is Script.Operator sop &&
										 (sop == Script.Operator.BooleanAnd || sop == Script.Operator.BooleanOr))//z := (x && y)
									parts[x] = new CodeMethodInvokeExpression(boolean, "ParseObject");
								else
									parts[x] = BinOpToSnippet(boolean);
							}
							else
							{
								if (LaxExpressions)
								{
									if (parts[x] is Script.Operator sox && sox == Script.Operator.TernaryA)
									{
										parts[x] = nullPrimitive;
										goto next;
									}

									if (y > parts.Count - 1)
										return nullPrimitive;
								}
								else
									throw new ParseException(ExInvalidExpression, codeLine);

								invoke.Method = (CodeMethodReferenceExpression)InternalMethods.Operate;
								_ = invoke.Parameters.Add(OperatorAsFieldReference(op));

								if (LaxExpressions && parts[i] is Script.Operator soi
										&& soi == Script.Operator.Concat
										&& parts[x] as CodeBinaryOperatorType? == CodeBinaryOperatorType.Assign)
									_ = invoke.Parameters.Add(new CodePrimitiveExpression(string.Empty));
								else
									_ = invoke.Parameters.Add(VarMixedExpr(codeLine, parts[x]));

								var py = parts[y];

								if (op == Script.Operator.Is)//When using "is", we pass the type as a string, which is then compared internally inside of IfLegacy().
								{
									if (py is CodeVariableReferenceExpression pycvre)
										_ = invoke.Parameters.Add(new CodePrimitiveExpression(pycvre.VariableName));
									else if (py is CodePrimitiveExpression pycpe)
										_ = invoke.Parameters.Add(new CodePrimitiveExpression(pycpe.Value.ToString()));
								}
								else
									_ = invoke.Parameters.Add(VarMixedExpr(codeLine, py));

								parts[x] = invoke;
								next:
								parts.RemoveAt(y);
							}

							parts.RemoveAt(i);
						}

						i--;
					}
					else if (parts[i] as CodeBinaryOperatorType? != CodeBinaryOperatorType.Assign && !(parts[i] is CodeSnippetStatement))
					{
						var x = i - 1;

						if (x > 0 && !(parts[x] is Script.Operator || parts[x] is CodeBinaryOperatorType || parts[x] is CodeSnippetStatement))
						{
							parts.Insert(i, Script.Operator.Concat);
							i--;
							continue;
						}
					}
				}

				level--;
			}

			for (var i = parts.Count - 1; i > 0; i--)
				MergeAssignmentAt(codeLine, parts, i);

			if (parts.Count > 1)
			{
				for (var i = 0; i < parts.Count; i++)
				{
					var typed = false;

					if (LaxExpressions)
						typed = IsVarAssignment(parts[i]) || IsVarReference(parts[i]);

					if (!(typed || parts[i] is CodeMethodInvokeExpression
							|| parts[i] is CodePrimitiveExpression
							|| parts[i] is CodeTernaryOperatorExpression
							|| parts[i] is CodeBinaryOperatorExpression
							|| parts[i] is CodePropertyReferenceExpression
							|| parts[i] is CodeSnippetExpression
						 ))
						throw new ParseException($"Value of {parts[i]} of type {parts[i].GetType()} was not a type that can be used with the concatenation operator.", codeLine);

					if (i % 2 == 1)
						parts.Insert(i, Script.Operator.Concat);
				}

				var concat = ParseExpression(codeLine, code, parts, create);
				parts.Clear();
				parts.Add(concat);
			}

			if (parts.Count != 1)
				throw new ParseException($"parts count of {parts.Count} was not 1.", codeLine);

			return BinOpToSnippet(parts[0] as CodeExpression);
		}

		private CodeExpressionStatement[] ParseMultiExpression(CodeLine codeLine, string code, bool create, List<List<object>> subs = null)
		=> ParseMultiExpressionWithoutTokenizing(codeLine, code, SplitTokens(codeLine, code).ToArray(), create, subs);

		private CodeExpression[] ParseMultiExpression(CodeLine codeLine, string code, object[] parts, bool create, List<List<object>> subs = null)
		{
			var fatArrow = false;
			var sub = new List<object>();
			var expr = new List<CodeExpression>();
			memberVarsStatic = false;

			for (var i = 0; i < parts.Length; i++)
			{
				if (!(parts[i] is string s) || s.Length == 0)
				{
					sub.Add(parts[i]);
					continue;
				}

				var part = s;

				if (i == 0 && part == FunctionStatic)
				{
					memberVarsStatic = true;
					continue;
				}

				var next = Set(parts, i, codeLine);

				if (next > 0)
				{
					for (; i < next; i++)
						sub.Add(parts[i]);

					i--;
					continue;
				}

				if (s == "=>")
					fatArrow = true;

				//This will properly handle inserting null when passing arguments like a,,c or ,b,c or a,b,
				//When not parsing arguments and instead just parsing regular expressions, it will cause
				//a compiler error because just saying "null" in the wrong place is a syntax error.
				//This is a good way to help catch syntax bugs in a script.
				if (s == "," && !fatArrow)
				{
					if (sub.Count != 0)
					{
						subs?.Add(sub.ToList());
						expr.Add(ParseExpression(codeLine, code, sub, create));
						sub.Clear();
					}
					else
					{
						expr.Add(nullPrimitive);
						//expr.Add(new CodeSnippetExpression("UnsetArg.Default"));
					}

					if (i == parts.Length - 1)
						expr.Add(nullPrimitive);

					//expr.Add(new CodeSnippetExpression("UnsetArg.Default"));
					continue;
				}
				else
					sub.Add(parts[i]);
			}

			if (sub.Count != 0)
			{
				subs?.Add(sub.ToList());
				expr.Add(ParseExpression(codeLine, code, sub, create));
			}

			return expr.ToArray();
		}

		private CodeExpressionStatement[] ParseMultiExpressionWithoutTokenizing(CodeLine codeLine, string code, object[] parts, bool create, List<List<object>> subs = null)
		{
			var result = ParseMultiExpression(codeLine, code, parts, create, subs);
			var statements = new CodeExpressionStatement[result.Length];

			for (var i = 0; i < result.Length; i++)
			{
				if (OptimizeLoneExpression(result[i]) is CodeExpression expr)
					statements[i] = new CodeExpressionStatement(BinOpToSnippet(expr));
				else
					statements[i] = new CodeExpressionStatement(result[i]);
			}

			return statements;
		}

		private CodeExpression ParseSingleExpression(CodeLine codeLine, string code, bool create)
		{
			var tokens = SplitTokens(codeLine, code);
			return ParseExpression(codeLine, code, tokens, create);
		}

		private CodeExpression PrimitiveToExpression(string code)
		{
			if (IsPrimitiveObject(code, out var result))
			{
				if (result != null)
				{
					if (result is long ll)
						return new CodeSnippetExpression($"{ll}L");
					else
						return new CodePrimitiveExpression(result);
				}
				else
					return new CodePrimitiveExpression(result);
			}

			return null;
		}
	}
}