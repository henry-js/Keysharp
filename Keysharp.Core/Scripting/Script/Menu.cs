﻿namespace Keysharp.Scripting
{
	public partial class Script
	{
		internal static ToolStripMenuItem openMenuItem;
		internal static ToolStripMenuItem suspendMenuItem;
		internal static NotifyIcon Tray;
		internal static Keysharp.Core.Menu trayMenu;

		public static void CreateTrayMenu()
		{
			var trayIcon = Tray = new NotifyIcon { ContextMenuStrip = new ContextMenuStrip(), Text = Accessors.A_ScriptName.Substring(0, Math.Min(Accessors.A_ScriptName.Length, 64)) };//System tray icon tooltips have a limit of 64 characters.
			Processes.mainContext = System.Threading.SynchronizationContext.Current;//This must happen after the icon is created.

			if (NoTrayIcon)
				return;

			trayMenu = new Keysharp.Core.Menu(Tray.ContextMenuStrip);
			trayMenu.AddStandard();
			trayIcon.Tag = trayMenu;
			trayIcon.MouseClick += TrayIcon_MouseClick;
			trayIcon.MouseDoubleClick += TrayIcon_MouseDoubleClick;
			Icon icon = null;
#if WINDOWS
			icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
#endif

			if (Accessors.A_IsCompiled || icon == null)//For some reason this is needed when running as a compiled exe.
				icon = Core.Properties.Resources.Keysharp_ico;

			if (icon != null)
			{
				trayIcon.Icon = icon;
				trayIcon.Visible = true;
			}

			pausedIcon = Core.Properties.Resources.Keysharp_p_ico;//Pause isn't really needed since pausing is not supported. Perhaps it can be used for something else some day.
			suspendedIcon = Core.Properties.Resources.Keysharp_s_ico;
		}

		internal static void SuspendHotkeys()
		{
			mainWindow.CheckedInvoke(() =>
			{
				Flow.Suspended = !Flow.Suspended;
				HotstringDefinition.SuspendAll(Flow.Suspended);//Must do this prior to ManifestAllHotkeysHotstringsHooks() to avoid incorrect removal of hook.
				HotkeyDefinition.ManifestAllHotkeysHotstringsHooks();//Update the state of all hotkeys based on the complex interdependencies hotkeys have with each another.
				suspendMenuItem.Checked = Flow.Suspended;
				mainWindow.SuspendHotkeysToolStripMenuItem.Checked = Flow.Suspended;
			}, false);
		}

		private static void TrayIcon_MouseClick(object sender, MouseEventArgs e)
		{
			if (sender is NotifyIcon ni && ni.Tag is Keysharp.Core.Menu mnu)
				if (mnu.ClickCount == 1)
					if (mnu.defaultItem is ToolStripItem tsi)
						mnu.Tsmi_Click(tsi, new EventArgs());
		}

		private static void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (sender is NotifyIcon ni && ni.Tag is Keysharp.Core.Menu mnu)
				if (mnu.ClickCount > 1)
					if (mnu.defaultItem is ToolStripItem tsi)
						mnu.Tsmi_Click(tsi, new EventArgs());
		}
	}
}