﻿using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Keysharp.Tests
{
	/// <summary>
	/// Function tests don't need to also be wrapped in a funciton, so pass false.
	/// </summary>
	public class FunctionAndLabelTests : TestRunner
	{
		[NonParallelizable]
		[Test, Category("Function")]
		public void AllGlobalInFunc() => Assert.IsTrue(TestScript("func-all-global", false));

		[NonParallelizable]
		[Test, Category("Function")]
		public void AllLocalInFunc() => Assert.IsTrue(TestScript("func-all-local", false));

		[NonParallelizable]
		[Test, Category("Function")]
		public void BoundFunc() => Assert.IsTrue(TestScript("func-bound", false));

		[NonParallelizable]
		[Test, Category("Function")]
		public void DynVarsInFunc() => Assert.IsTrue(TestScript("func-dyn-vars", false));

		[NonParallelizable]
		[Test, Category("Function")]
		public void FatArrowFunc() => Assert.IsTrue(TestScript("func-fat-arrow", false));

		[NonParallelizable]
		[Test, Category("Function")]
		public void GlobalLocalInFunc() => Assert.IsTrue(TestScript("func-global-local", false));

		[NonParallelizable]
		[Test, Category("Function")]
		public void GlobalLocalStaticInFunc() => Assert.IsTrue(TestScript("func-global-local-static", false));

		[NonParallelizable]
		[Test, Category("Function")]
		public void GlobalStaticInFunc() => Assert.IsTrue(TestScript("func-global-static", false));

		[NonParallelizable]
		[Test, Category("Function")]
		public void LabelInFunc() => Assert.IsTrue(TestScript("func-label", true));

		[NonParallelizable]
		[Test, Category("Function")]
		public void LocalStaticInFunc() => Assert.IsTrue(TestScript("func-local-static", false));

		[NonParallelizable]
		[Test, Category("Function")]
		public void ParamsInFunc() => Assert.IsTrue(TestScript("func-params", false));

		[NonParallelizable]
		[Test, Category("Function")]
		public void RefParamsInFunc() => Assert.IsTrue(TestScript("func-ref-params", false));

		[NonParallelizable]
		[Test, Category("Function")]
		public void ReturnFunc() => Assert.IsTrue(TestScript("func-return", false));
		
		[SetUp]
		public void Setup()
		{
			//For some reason, RefParamsInFunc() will not succeed on linux without these when run with all other tests in the group.
			//It runs fine on its own though. Add these two statements to make group testing succeed.
			Reflections.Clear();
			Reflections.Initialize();
		}
	}
}