﻿using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Keysharp.Tests
{
	public partial class SoundTests : TestRunner
	{
		[Test, Category("Sound")]
		public void SoundBeep()
		{
			Keysharp.Core.Sound.SoundBeep();
			Keysharp.Core.Sound.SoundBeep(700, 500);
			Keysharp.Core.Sound.SoundBeep(800, 500);
			Keysharp.Core.Sound.SoundBeep(900, 500);
			Keysharp.Core.Sound.SoundBeep(1000, 500);
			Assert.IsTrue(TestScript("sound-soundbeep", true));
		}
	}
}
