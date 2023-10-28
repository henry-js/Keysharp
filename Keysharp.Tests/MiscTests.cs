﻿using Keysharp.Core;
using NUnit.Framework;
using static Keysharp.Core.Misc;

namespace Keysharp.Tests
{
	public partial class Scripting
	{
		[NonParallelizable]
		[Test, Category("Misc")]
		public void SimplePass() => Assert.IsTrue(TestScript("misc-pass", false));

		[Test, Category("Misc")]
		public void MiscObject()
		{
			var a = Keysharp.Core.Misc.Array(10, 20, 30);
			var fo = a.GetMethod("Clear");
			_ = fo.Call();
			Assert.AreEqual(0L, a.Length);
			Assert.IsTrue(new KeysharpObject().HasBase(new Any()) == 1L);
			Assert.IsTrue(TestScript("misc-object", true));
		}

		[Test, Category("Misc")]
		public void MiscTimer() => Assert.IsTrue(TestScript("misc-timer", false));

		[Test, Category("Misc")]
		public void MiscIs()
		{
			var x = 1;
			var o = Keysharp.Core.Misc.Array(10, 20, 30);
			var map = Keysharp.Core.Misc.Map("one", 1, "two", 2, "three", 3);
			Assert.IsTrue(IsInteger(x) == 1);
			x = -1;
			Assert.IsTrue(IsInteger(x) == 1);
			var d = 1.234;
			Assert.IsTrue(IsInteger(d) == 0);
			var f = 1.234f;
			Assert.IsTrue(IsInteger(f) == 0);
			var m = 1.234m;
			Assert.IsTrue(IsInteger(m) == 0);
			var s = "1234";
			Assert.IsTrue(IsInteger(s) == 1);
			s = "-1234";
			Assert.IsTrue(IsInteger(s) == 1);
			s = "+1234";
			Assert.IsTrue(IsInteger(s) == 1);
			s = "1234.1234";
			Assert.IsTrue(IsInteger(s) == 0);
			s = "-1234.1234";
			Assert.IsTrue(IsInteger(s) == 0);
			s = "+1234.1234";
			Assert.IsTrue(IsInteger(s) == 0);
			Assert.IsTrue(IsInteger(o) == 0);
			//
			d = 1.234;
			Assert.IsTrue(IsFloat(d) == 1);
			d = -1.234;
			Assert.IsTrue(IsFloat(d) == 1);
			f = 1.234f;
			Assert.IsTrue(IsFloat(f) == 1);
			m = 1.234m;
			Assert.IsTrue(IsFloat(m) == 1);
			s = "1234";
			Assert.IsTrue(IsFloat(s) == 0);
			s = "-1234";
			Assert.IsTrue(IsFloat(s) == 0);
			s = "+1234";
			Assert.IsTrue(IsFloat(s) == 0);
			Assert.IsTrue(IsFloat(o) == 0);
			//
			Assert.IsTrue(IsNumber(0) == 1);
			Assert.IsTrue(IsNumber(1) == 1);
			Assert.IsTrue(IsNumber(-1) == 1);
			Assert.IsTrue(IsNumber(1.234) == 1);
			Assert.IsTrue(IsNumber(-1.234) == 1);
			Assert.IsTrue(IsNumber("1234") == 1);
			Assert.IsTrue(IsNumber("-1234") == 1);
			Assert.IsTrue(IsNumber("+1234") == 1);
			Assert.IsTrue(IsNumber("1.234") == 1);
			Assert.IsTrue(IsNumber("-1.234") == 1);
			Assert.IsTrue(IsNumber("+1.234") == 1);
			Assert.IsTrue(IsNumber(o) == 0);
			//
			Assert.IsTrue(IsObject(0) == 0);
			Assert.IsTrue(IsObject(1.234) == 0);
			Assert.IsTrue(IsObject("test") == 0);
			Assert.IsTrue(IsObject(o) == 1);
			Assert.IsTrue(IsObject(map) == 1);
			//
			Assert.IsTrue(IsDigit(1) == 1);
			Assert.IsTrue(IsDigit(-1) == 0);
			Assert.IsTrue(IsDigit(1.234) == 0);
			Assert.IsTrue(IsDigit("0123456789") == 1);
			Assert.IsTrue(IsDigit("1A") == 0);
			Assert.IsTrue(IsDigit("A1") == 0);
			Assert.IsTrue(IsDigit("0x01") == 0);
			Assert.IsTrue(IsDigit(o) == 0);
			Assert.IsTrue(IsDigit(m) == 0);
			//
			Assert.IsTrue(IsXDigit(1) == 1);
			Assert.IsTrue(IsXDigit(-1) == 0);
			Assert.IsTrue(IsXDigit(1.234) == 0);
			Assert.IsTrue(IsXDigit("0123456789") == 1);
			Assert.IsTrue(IsXDigit("1A") == 1);
			Assert.IsTrue(IsXDigit("0x01ABCdef") == 1);
			Assert.IsTrue(IsXDigit("0xg") == 0);
			Assert.IsTrue(IsXDigit(o) == 0);
			Assert.IsTrue(IsXDigit(m) == 0);
			//
			Assert.IsTrue(IsAlpha(1) == 0);
			Assert.IsTrue(IsAlpha(-1) == 0);
			Assert.IsTrue(IsAlpha(1.234) == 0);
			Assert.IsTrue(IsAlpha("0123456789") == 0);
			Assert.IsTrue(IsAlpha("ABC") == 1);
			Assert.IsTrue(IsAlpha("abc") == 1);
			Assert.IsTrue(IsAlpha("ABC123") == 0);
			Assert.IsTrue(IsAlpha(".") == 0);
			Assert.IsTrue(IsAlpha(o) == 0);
			Assert.IsTrue(IsAlpha(m) == 0);
			//
			Assert.IsTrue(IsUpper(1) == 0);
			Assert.IsTrue(IsUpper(-1) == 0);
			Assert.IsTrue(IsUpper(1.234) == 0);
			Assert.IsTrue(IsUpper("0123456789") == 0);
			Assert.IsTrue(IsUpper("ABC") == 1);
			Assert.IsTrue(IsUpper("abc") == 0);
			Assert.IsTrue(IsUpper("AbC123") == 0);
			Assert.IsTrue(IsUpper(".") == 0);
			Assert.IsTrue(IsUpper(o) == 0);
			Assert.IsTrue(IsUpper(m) == 0);
			//
			Assert.IsTrue(IsLower(1) == 0);
			Assert.IsTrue(IsLower(-1) == 0);
			Assert.IsTrue(IsLower(1.234) == 0);
			Assert.IsTrue(IsLower("0123456789") == 0);
			Assert.IsTrue(IsLower("ABC") == 0);
			Assert.IsTrue(IsLower("abc") == 1);
			Assert.IsTrue(IsLower("AbC123") == 0);
			Assert.IsTrue(IsLower(".") == 0);
			Assert.IsTrue(IsLower(o) == 0);
			Assert.IsTrue(IsLower(m) == 0);
			//
			Assert.IsTrue(IsAlnum(1) == 1);
			Assert.IsTrue(IsAlnum(-1) == 0);
			Assert.IsTrue(IsAlnum(1.234) == 0);
			Assert.IsTrue(IsAlnum("0123456789") == 1);
			Assert.IsTrue(IsAlnum("ABC") == 1);
			Assert.IsTrue(IsAlnum("abc") == 1);
			Assert.IsTrue(IsAlnum("AbC123") == 1);
			Assert.IsTrue(IsAlnum(".") == 0);
			Assert.IsTrue(IsAlnum(o) == 0);
			Assert.IsTrue(IsAlnum(m) == 0);
			//
			Assert.IsTrue(IsSpace(1) == 0);
			Assert.IsTrue(IsSpace(-1) == 0);
			Assert.IsTrue(IsSpace(1.234) == 0);
			Assert.IsTrue(IsSpace("0123456789") == 0);
			Assert.IsTrue(IsSpace("ABC") == 0);
			Assert.IsTrue(IsSpace("abc") == 0);
			Assert.IsTrue(IsSpace("AbC123") == 0);
			Assert.IsTrue(IsSpace(".") == 0);
			Assert.IsTrue(IsSpace(" \t\n\r\v\f") == 1);
			Assert.IsTrue(IsSpace(o) == 0);
			Assert.IsTrue(IsSpace(m) == 0);
			//
			Assert.IsTrue(IsTime("2021") == 1);
			Assert.IsTrue(IsTime("202106") == 1);
			Assert.IsTrue(IsTime("202199") == 0);//Unrepresentable
			Assert.IsTrue(IsTime("20211201") == 1);
			Assert.IsTrue(IsTime("20211299") == 0);
			Assert.IsTrue(IsTime("2021121513") == 1);
			Assert.IsTrue(IsTime("2021121555") == 0);
			Assert.IsTrue(IsTime("202112152033") == 1);
			Assert.IsTrue(IsTime("202112152099") == 0);
			Assert.IsTrue(IsTime("20211215203522") == 1);
			Assert.IsTrue(IsTime("20211215203599") == 0);
			Assert.IsTrue(IsTime(o) == 0);
			Assert.IsTrue(IsTime(m) == 0);
			//
			Assert.IsTrue(TestScript("misc-is", true));
		}
	}
}