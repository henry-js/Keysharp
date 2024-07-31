﻿#if LINUX
namespace Keysharp.Core.Linux.X11
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct XDestroyWindowEvent
	{
		internal XEventName type;
		internal IntPtr serial;
		internal bool send_event;
		internal IntPtr display;
		internal IntPtr xevent;
		internal uint window;
	}
}
#endif