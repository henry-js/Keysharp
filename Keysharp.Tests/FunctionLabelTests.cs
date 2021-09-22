﻿using NUnit.Framework;

namespace Keysharp.Tests
{
	/// <summary>
	/// Function tests don't need to also be wrapped in a funciton, so pass false.
	/// </summary>
	public partial class Scripting
	{
		[Test, Category("Function")]
		public void AllGlobalInFunc() => Assert.IsTrue(TestScript("func-all-global", false));

		[Test, Category("Function")]
		public void AllLocalInFunc() => Assert.IsTrue(TestScript("func-all-local", false));

		[Test, Category("Function")]
		public void GlobalLocalInFunc() => Assert.IsTrue(TestScript("func-global-local", false));

		[Test, Category("Function")]
		public void GlobalStaticInFunc() => Assert.IsTrue(TestScript("func-global-static", false));

		[Test, Category("Function")]
		public void LocalStaticInFunc() => Assert.IsTrue(TestScript("func-local-static", false));

		[Test, Category("Function")]
		public void GlobalLocalStaticInFunc() => Assert.IsTrue(TestScript("func-global-local-static", false));

		[Test, Category("Function")]
		public void ParamsInFunc() => Assert.IsTrue(TestScript("func-params", false));

		[Test, Category("Function")]
		public void DynVarsInFunc() => Assert.IsTrue(TestScript("func-dyn-vars", false));

		[Test, Category("Function")]
		public void LabelInFunc() => Assert.IsTrue(TestScript("func-label", true));
	}
}