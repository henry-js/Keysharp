﻿using static Keysharp.Core.Accessors;
using static Keysharp.Core.Core;
//using static Keysharp.Core.Common.Window.WindowItemBase;
using static Keysharp.Core.Common.Keyboard.HotstringDefinition;
using static Keysharp.Core.Dialogs;
using static Keysharp.Core.Dir;
using static Keysharp.Core.Drive;
using static Keysharp.Core.DllHelper;
using static Keysharp.Core.Env;
using static Keysharp.Core.File;
using static Keysharp.Core.Flow;
using static Keysharp.Core.Function;
using static Keysharp.Core.GuiHelper;
using static Keysharp.Core.Images;
using static Keysharp.Core.ImageLists;
using static Keysharp.Core.Ini;
using static Keysharp.Core.Keyboard;
using static Keysharp.Core.KeysharpObject;
using static Keysharp.Core.Loops;
using static Keysharp.Core.Maths;
using static Keysharp.Core.Menu;
using static Keysharp.Core.Misc;
using static Keysharp.Core.Monitor;
using static Keysharp.Core.Mouse;
using static Keysharp.Core.Network;
using static Keysharp.Core.Options;
using static Keysharp.Core.Processes;
using static Keysharp.Core.Registrys;
using static Keysharp.Core.Screen;
using static Keysharp.Core.Security;
using static Keysharp.Core.SimpleJson;
using static Keysharp.Core.Sound;
using static Keysharp.Core.Strings;
using static Keysharp.Core.ToolTips;
using static Keysharp.Core.Window;
using static Keysharp.Core.Windows.WindowsAPI;
using static Keysharp.Scripting.Script;
using static Keysharp.Scripting.Script.Operator;
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.CLSCompliantAttribute(true)]
[assembly: Keysharp.Scripting.AssemblyBuildVersionAttribute("0.0.0.1")]

namespace Keysharp.Main
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Data;
	using System.IO;
	using System.Reflection;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Windows.Forms;
	using Keysharp.Core;
	using Keysharp.Scripting;
	using Array = Keysharp.Core.Array;
	using Buffer = Keysharp.Core.Buffer;


	public sealed class Program
	{

		[System.STAThreadAttribute()]
		public static int Main(string[] args)
		{
			try
			{
				string name = @"*";
				Keysharp.Scripting.Script.Variables.InitGlobalVars();
				Keysharp.Scripting.Script.SetName(name);
				HandleSingleInstance(name, eScriptInstance.Off);
				HandleCommandLineParams(args);
				SetProcessDPIAware();
				CreateTrayMenu();

				if (IfTest(DirExist("./FileGetShortcut")))
				{
					DirDelete("./FileGetShortcut", true);
				}

				if (IfTest(FileExist("./testshortcut.lnk")))
				{
					FileDelete("./testshortcut.lnk");
				}

				path = "../../../Keysharp.Tests/Code/";
				dir = Operate(Concat, path, "DirCopy");
				DirCopy(dir, "./FileGetShortcut/");
				fullpath = FileDirName("./FileGetShortcut/file1.txt");
				FileCreateShortcut("./FileGetShortcut/file1.txt", "./testshortcut.lnk", fullpath, "", "TestDescription", "../../../Keysharp.ico", "");

				if (IfTest(FileExist("./testshortcut.lnk")))
				{
					FileAppend("pass", "*");
				}
				else
				{
					FileAppend("fail", "*");
				}

				shortcut = FileGetShortcut("./testshortcut.lnk");

				if (IfTest(Operate(IdentityEquality, FileFullPath("./FileGetShortcut/file1.txt"), GetPropertyValue(shortcut, "OutTarget"))))
				{
					FileAppend("pass", "*");
				}
				else
				{
					FileAppend("fail", "*");
				}

				if (IfTest(Operate(IdentityEquality, fullpath, GetPropertyValue(shortcut, "OutDir"))))
				{
					FileAppend("pass", "*");
				}
				else
				{
					FileAppend("fail", "*");
				}

				if (IfTest(Operate(IdentityEquality, "TestDescription", GetPropertyValue(shortcut, "OutDescription"))))
				{
					FileAppend("pass", "*");
				}
				else
				{
					FileAppend("fail", "*");
				}

				if (IfTest(Operate(IdentityEquality, "", Index(shortcut, "OutArgs"))))
				{
					FileAppend("pass", "*");
				}
				else
				{
					FileAppend("fail", "*");
				}

				if (IfTest(Operate(IdentityEquality, FileFullPath("../../../Keysharp.ico"), GetPropertyValue(shortcut, "OutIcon"))))
				{
					FileAppend("pass", "*");
				}
				else
				{
					FileAppend("fail", "*");
				}

				if (IfTest(Operate(IdentityEquality, "0", GetPropertyValue(shortcut, "OutIconNum"))))
				{
					FileAppend("pass", "*");
				}
				else
				{
					FileAppend("fail", "*");
				}

				if (IfTest(Operate(IdentityEquality, "1", GetPropertyValue(shortcut, "OutRunState"))))
				{
					FileAppend("pass", "*");
				}
				else
				{
					FileAppend("fail", "*");
				}

				if (IfTest(DirExist("./FileGetShortcut")))
				{
					DirDelete("./FileGetShortcut", true);
				}

				if (IfTest(FileExist("./testshortcut.lnk")))
				{
					FileDelete("./testshortcut.lnk");
				}

				ExitApp(0);
				return 0;
			}
			catch (Keysharp.Core.Error kserr)
			{
				if (ErrorOccurred(kserr))
				{
					MsgBox("Uncaught Keysharp exception:\r\n" + kserr);
				}

				ExitApp(1);
				return 1;
			}
			catch (System.Exception mainex)
			{
				MsgBox("Uncaught exception:\r\n" + "Message: " + mainex.Message + "\r\nStack: " + mainex.StackTrace);
				ExitApp(1);
				return 1;
			}
		}

		public static object dir;

		public static object fullpath;

		public static object path;

		public static object shortcut;
	}
}
