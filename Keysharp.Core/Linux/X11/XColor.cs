﻿#if LINUX
namespace Keysharp.Core.Linux.X11
{
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	internal struct XColor
	{
		internal IntPtr pixel;
		internal ushort red;
		internal ushort green;
		internal ushort blue;
		internal byte flags;
		internal byte pad;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct XStandardColormap
	{
		internal IntPtr colormap;
		internal IntPtr red_max;
		internal IntPtr red_mult;
		internal IntPtr green_max;
		internal IntPtr green_mult;
		internal IntPtr blue_max;
		internal IntPtr blue_mult;
		internal IntPtr base_pixel;
		internal IntPtr visualid;
		internal IntPtr killid;
	}

	[Flags]
	internal enum ColorFlags
	{
		DoRed = 1 << 0,
		DoGreen = 1 << 1,
		DoBlue = 1 << 2
	}
}
#endif