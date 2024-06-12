﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Keysharp.Scripting;

namespace Keysharp.Core.Common.Platform
{
	internal class PlatformManagerBase
	{
		/// <summary>
		/// aX and aY are interpreted according to the current coord mode.  If necessary, they are converted to
		/// screen coordinates based on the position of the active window's upper-left corner (or its client area).
		/// </summary>
		/// <param name="aX"></param>
		/// <param name="aY"></param>
		/// <param name="aWhichMode"></param>
		internal void CoordToScreen(ref int aX, ref int aY, CoordMode modeType)
		{
			var coordMode = Mouse.Coords.GetCoordMode(modeType);

			if (coordMode == CoordModeType.Screen)
				return;

			var activeWindow = Script.windowManager.ActiveWindow;

			if (activeWindow.Handle != IntPtr.Zero && !activeWindow.IsIconic)
			{
				if (coordMode == CoordModeType.Window)
				{
					var rect = activeWindow.Location;
					aX += rect.Left;
					aY += rect.Top;
				}
				else // (coord_mode == CoordModeType.Window.Client)
				{
					var pt = activeWindow.ClientToScreen();
					aX += pt.X;
					aY += pt.Y;
				}
			}

			//else no active window per se, so don't convert the coordinates.  Leave them as-is as desired
			// by the caller.  More details:
			// Revert to screen coordinates if the foreground window is minimized.  Although it might be
			// impossible for a visible window to be both foreground and minimized, it seems that hidden
			// windows -- such as the script's own main window when activated for the purpose of showing
			// a popup menu -- can be foreground while simultaneously being minimized.  This fixes an
			// issue where the mouse will move to the upper-left corner of the screen rather than the
			// intended coordinates (v1.0.17).
		}
	}
}
