#if WINDOWS
namespace Keysharp.Core.Windows
{
	/// <summary>
	/// Concrete implementation of WindowManager for the Windows platfrom.
	/// </summary>
	internal class WindowManager : WindowManagerBase
	{

		internal override WindowItemBase ActiveWindow => new WindowItem(WindowsAPI.GetForegroundWindow());

		/// <summary>
		/// Return all top level windows. This does not recurse into child windows.
		/// </summary>
		internal override IEnumerable<WindowItemBase> AllWindows
		{
			get
			{
				var windows = new List<WindowItemBase>(64);
				var doHidden = ThreadAccessors.A_DetectHiddenWindows;
				_ = WindowsAPI.EnumWindows(delegate (IntPtr hwnd, int lParam)
				{
					if (doHidden || Windows.WindowsAPI.IsWindowVisible(hwnd))
						windows.Add(new WindowItem(hwnd));

					return true;
				}, 0);
				return windows;
			}
		}

		internal WindowManager() => Processes.CurrentThreadID = WindowsAPI.GetCurrentThreadId();

		internal override WindowItemBase CreateWindow(IntPtr id) => new WindowItem(id);

		internal override IEnumerable<WindowItemBase> FilterForGroups(IEnumerable<WindowItemBase> windows)
		{
			return windows.Where((w) =>
			{
				var style = w.Style;
				var exstyle = w.ExStyle;
				return w.Enabled &&
					   (exstyle & WindowsAPI.WS_EX_TOPMOST) == 0 &&
					   (exstyle & WindowsAPI.WS_EX_NOACTIVATE) == 0 &&
					   (exstyle & (WindowsAPI.WS_EX_TOOLWINDOW | WindowsAPI.WS_EX_APPWINDOW)) != WindowsAPI.WS_EX_TOOLWINDOW &&
					   WindowsAPI.IsWindowVisible(w.Handle) &&
					   WindowsAPI.GetLastActivePopup(w.Handle) != w.Handle &&
					   WindowsAPI.GetWindow(w.Handle, WindowsAPI.GW_OWNER) == IntPtr.Zero &&
					   WindowsAPI.GetShellWindow() != w.Handle &&
					   !WindowsAPI.IsWindowCloaked(w.Handle);
			});
		}

		internal override uint GetFocusedCtrlThread(ref IntPtr apControl, IntPtr aWindow)
		{
			// Determine the thread for which we want the keyboard layout.
			// When no foreground window, the script's own layout seems like the safest default.
			var thread_id = 0u;

			if (aWindow == IntPtr.Zero)
				aWindow = WindowsAPI.GetForegroundWindow();

			if (aWindow != IntPtr.Zero)
			{
				// Get thread of aWindow (which should be the foreground window).
				thread_id = WindowsAPI.GetWindowThreadProcessId(aWindow, out var _);
				// Get focus.  Benchmarks showed this additional step added only 6% to the time,
				// and the total was only around 4�s per iteration anyway (on a Core i5-4460).
				// It is necessary for UWP apps such as Microsoft Edge, and any others where
				// the top-level window belongs to a different thread than the focused control.
				var thread_info = GUITHREADINFO.Default;

				if (WindowsAPI.GetGUIThreadInfo(thread_id, out thread_info) && thread_info.hwndFocus != IntPtr.Zero)
				{
					// Use the focused control's thread.
					thread_id = WindowsAPI.GetWindowThreadProcessId(thread_info.hwndFocus, out var _);

					if (apControl != IntPtr.Zero)
						apControl = thread_info.hwndFocus;
				}
			}

			return thread_id;
		}

		internal override IntPtr GetForeGroundWindowHwnd() => WindowsAPI.GetForegroundWindow();

		internal override bool IsWindow(IntPtr handle) => WindowsAPI.IsWindow(handle);

		internal override void MinimizeAll()
		{
			var window = FindWindow(new SearchCriteria { ClassName = "Shell_TrayWnd" });
			_ = WindowsAPI.PostMessage(window.Handle, WindowsAPI.WM_COMMAND, new IntPtr(419), IntPtr.Zero);
			WindowItemBase.DoWinDelay();
		}

		internal override void MinimizeAllUndo()
		{
			var window = FindWindow(new SearchCriteria { ClassName = "Shell_TrayWnd" });
			_ = WindowsAPI.PostMessage(window.Handle, WindowsAPI.WM_COMMAND, new IntPtr(416), IntPtr.Zero);
			WindowItemBase.DoWinDelay();
		}

		internal override void MaximizeAll()
		{
			foreach (var window in AllWindows)
				window.WindowState = FormWindowState.Maximized;
		}

		internal override WindowItemBase WindowFromPoint(Point location)
		{
			var ctrl = WindowsAPI.WindowFromPoint(location);

			if (ctrl != IntPtr.Zero)
				return new WindowItem(ctrl);

			return null;
		}
	}
}
#endif