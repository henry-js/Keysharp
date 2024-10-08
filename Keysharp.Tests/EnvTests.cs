﻿using Assert = NUnit.Framework.Legacy.ClassicAssert;

namespace Keysharp.Tests
{
	public class EnvTests : TestRunner
	{
		[Test, Category("Env"), NonParallelizable]
#if WINDOWS
		[Apartment(ApartmentState.STA)]
#endif
		public void ClipboardAll()
		{
			Accessors.A_Clipboard = "Asdf";
			var arr = Env.ClipboardAll();
			var clip = Accessors.A_Clipboard as string;
			Assert.AreEqual("Asdf", clip);
			Accessors.A_Clipboard = "";
			clip = Accessors.A_Clipboard as string;
			Assert.AreEqual("", clip);
			Accessors.A_Clipboard = arr;
			clip = Accessors.A_Clipboard as string;
			Assert.AreEqual("Asdf", clip);
		}

		/// <summary>
		/// This can fail periodically, but mostly works.
		/// If it fails in a batch, try running on its own.
		/// </summary>
		[Test, Category("Env"), NonParallelizable]
#if WINDOWS
		[Apartment(ApartmentState.STA)]
#endif
		public void ClipWait()
		{
			Clipboard.Clear();
			var dt = DateTime.Now;
			var b = Env.ClipWait(0.5);
			var dt2 = DateTime.Now;
			var ms = (dt2 - dt).TotalMilliseconds;
			Assert.AreEqual(false, b);//Will have timed out, so ErrorLevel will be 1.
			Assert.IsTrue(ms >= 500 && ms <= 3000);
			var tcs = new TaskCompletionSource<bool>();
			var thread = new Thread(() =>
			{
				Thread.Sleep(100);
				Clipboard.SetText("test text");
				tcs.SetResult(true);
			});
#if WINDOWS
			thread.SetApartmentState(ApartmentState.STA);
#endif
			thread.Start();
			dt = DateTime.Now;
			b = Env.ClipWait(null, true);//Will wait indefinitely for any type.
			dt2 = DateTime.Now;
			ms = (dt2 - dt).TotalMilliseconds;
			//Seems to take much longer than 100ms, but it's not too important.
			//Assert.IsTrue(ms >= 500 && ms <= 1100);
			tcs.Task.Wait();
			Assert.AreEqual(true, b);//Will have detected clipboard data, so ErrorLevel will be 0.
			//Now test with file paths.
			Clipboard.Clear();
			tcs = new TaskCompletionSource<bool>();
			thread = new Thread(() =>
			{
				Thread.Sleep(100);
				Clipboard.SetFileDropList(new System.Collections.Specialized.StringCollection
				{
					"./testfile1.txt",
					"./testfile2.txt",
					"./testfile3.txt",
				});
				tcs.SetResult(true);
			});
#if WINDOWS
			thread.SetApartmentState(ApartmentState.STA);
#endif
			thread.Start();
			dt = DateTime.Now;
			b = Env.ClipWait();//Will wait indefinitely for only text or file paths.
			dt2 = DateTime.Now;
			ms = (dt2 - dt).TotalMilliseconds;
			tcs.Task.Wait();
			Assert.AreEqual(true, b);//Will have detected clipboard data, so ErrorLevel will be 0.
			//Wait specifically for text/files, and copy an image. This should time out.
			Clipboard.Clear();
			var bitmap = new Bitmap(640, 480);
			tcs = new TaskCompletionSource<bool>();
			thread = new Thread(() =>
			{
				Thread.Sleep(100);
				Clipboard.SetImage(bitmap);
				tcs.SetResult(true);
			});
#if WINDOWS
			thread.SetApartmentState(ApartmentState.STA);
#endif
			thread.Start();
			dt = DateTime.Now;
			b = Env.ClipWait(1);//Will wait for one second for only text or file paths.
			dt2 = DateTime.Now;
			ms = (dt2 - dt).TotalMilliseconds;
			tcs.Task.Wait();
			Assert.AreEqual(false, b);//Will have timed out, so ErrorLevel will be 1.
			Assert.IsTrue(TestScript("env-clipwait", true));//For this to work, the bitmap from above must be on the clipboard.
		}

		[Test, Category("Env"), NonParallelizable]
		public void EnvGet()
		{
			var path = Env.EnvGet("PATH");
			Assert.AreNotEqual(path, string.Empty);
			path = Env.EnvGet("dummynothing123");
			Assert.AreEqual(path, string.Empty);
			Assert.IsTrue(TestScript("env-envget", true));
		}

		[Test, Category("Env"), NonParallelizable]
		public void EnvSet()
		{
			var key = "dummynothing123";
			var s = "a test value";
			Env.EnvSet(key, s);//Add the variable.
			var val = Env.EnvGet(key);
			Assert.AreEqual(val, s);
			Env.EnvSet(key, null);//Delete the variable.
			val = Env.EnvGet(key);//Ensure it's deleted.
			Assert.AreEqual(val, string.Empty);
			Assert.IsTrue(TestScript("env-envset", true));
		}

		[Test, Category("Env"), NonParallelizable]
		public void EnvUpdate()
		{
			Env.EnvUpdate();
			Assert.IsTrue(TestScript("env-envupdate", true));
		}

		[Test, Category("Env"), NonParallelizable]
		public void SysGet()
		{
			//Monitors
			var val = Env.SysGet(80);
			Assert.IsTrue(val.Ai() > 0);
			//Mouse buttons
			val = Env.SysGet(43);
			Assert.IsTrue(val.Ai() > 0);
			//Mouse present
			val = Env.SysGet(19);
			Assert.IsTrue(val.Ai() > 0);
			//Mouse wheel present
			val = Env.SysGet(75);
			Assert.IsTrue(val.Ai() > 0);
			Assert.IsTrue(TestScript("env-sysget", true));
		}
	}
}
