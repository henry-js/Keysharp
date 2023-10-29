﻿using System;
using System.Runtime.InteropServices;

namespace Keysharp.Core.Linux.X11.Events
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct XKeyEvent
	{
		internal XEventName type;
		internal IntPtr serial;
		internal bool send_event;
		internal IntPtr display;
		internal IntPtr window;
		internal IntPtr root;
		internal IntPtr subwindow;
		internal IntPtr time;
		internal int x;
		internal int y;
		internal int x_root;
		internal int y_root;
		internal int state;
		internal XKeys keycode;
		internal bool same_screen;
	}
}