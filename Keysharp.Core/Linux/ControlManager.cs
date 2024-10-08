﻿#if LINUX

namespace Keysharp.Core.Linux
{
	/// <summary>
	/// Concrete implementation of ControlManager for the linux platfrom.
	/// </summary>
	internal class ControlManager : ControlManagerBase
	{
		internal override long ControlAddItem(string str, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var res = 0L;
				var ctrl2 = Control.FromHandle(item.Handle);

				if (ctrl2 is ComboBox cb)
					res = cb.Items.Add(str);
				else if (ctrl2 is ListBox lb)
					res = lb.Items.Add(str);
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}

				WindowItemBase.DoControlDelay();
				return res + 1L;
			}

			return 0L;
		}

		internal override void ControlChooseIndex(int n, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var ctrl2 = Control.FromHandle(item.Handle);
				n--;

				if (ctrl2 is ComboBox cb)
				{
					if (n >= 0)
						cb.SelectedIndex = n;
					else
						cb.SelectedIndex = -1;
				}
				else if (ctrl2 is ListBox lb)
				{
					if (n >= 0)
					{
						lb.SelectedIndex = n;

						if (lb.GetGuiControl() is GuiControl gc)
							gc._control_DoubleClick(lb, new EventArgs());
					}
					else
						lb.SelectedIndex = -1;
				}
				else if (ctrl2 is TabControl tc)
				{
					tc.SelectedIndex = n;
				}
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}

				WindowItemBase.DoControlDelay();
			}
		}

		internal override long ControlChooseString(string str, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			var index = 0L;

			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var ctrl2 = Control.FromHandle(item.Handle);

				if (ctrl2 is ComboBox cb)
				{
					index = cb.FindString(str);
					cb.SelectedIndex = (int)index;
				}
				else if (ctrl2 is ListBox lb)
				{
					index = lb.FindString(str);
					lb.SelectedIndex = (int)index;

					if (index >= 0)
					{
						if (lb.GetGuiControl() is GuiControl gc)
							gc._control_DoubleClick(lb, new EventArgs());
					}
				}
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}

				WindowItemBase.DoControlDelay();
			}

			return index;
		}

		internal override void ControlClick(object ctrlorpos, object title, string text, string whichButton, int clickCount, string options, string excludeTitle, string excludeText)
		{
		}

		internal override void ControlDeleteItem(int n, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var ctrl2 = Control.FromHandle(item.Handle);
				n--;

				if (ctrl2 is ComboBox cb)
				{
					cb.Items.RemoveAt(n);
					cb.SelectedIndex = -1;//On linux, if the selected item is deleted, it will throw an exception the next time the dropdown is clicked if SelectedIndex is not set to -1.
				}
				else if (ctrl2 is ListBox lb)
				{
					lb.Items.RemoveAt(n);
				}
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}

				WindowItemBase.DoControlDelay();
			}
		}

		internal override long ControlFindItem(string str, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var ctrl2 = Control.FromHandle(item.Handle);

				if (ctrl2 is ComboBox cb)
					return cb.Items.IndexOf(str) + 1L;
				else if (ctrl2 is ListBox lb)
					return lb.Items.IndexOf(str) + 1L;
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}
			}

			return 0L;
		}

		internal override void ControlFocus(object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				if (Control.FromHandle(item.Handle) is Control ctrl2)
					ctrl2.Focus();
				else
					item.Active = true;//Will not work for X11.//TODO
			}
		}

		internal override long ControlGetChecked(object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var ctrl2 = Control.FromHandle(item.Handle);

				if (ctrl2 is CheckBox cb)
					return cb.Checked ? 1L : 0L;
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}
			}

			return 0L;
		}

		internal override string ControlGetChoice(object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var ctrl2 = Control.FromHandle(item.Handle);

				if (ctrl2 is ComboBox cb)
					return cb.SelectedItem != null ? cb.SelectedItem.ToString() : "";
				else if (ctrl2 is ListBox lb)
					return lb.SelectedItem != null ? lb.SelectedItem.ToString() : "";
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}

				WindowItemBase.DoControlDelay();
			}

			return "";
		}

		internal override long ControlGetExStyle(object ctrl, object title, string text, string excludeTitle, string excludeText) => 1;

		internal override long ControlGetFocus(object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchWindow(new object[] { title, text, excludeTitle, excludeText }, true) is WindowItem item)
			{
				if (Control.FromHandle(item.Handle) is Form form)
				{
					if (form.ActiveControl != null)
						return form.ActiveControl.Handle.ToInt64();
				}
			}
			return 0L;
		}

		internal override long ControlGetIndex(object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			long index = -1;

			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var ctrl2 = Control.FromHandle(item.Handle);

				if (ctrl2 is ComboBox cb)
					index = cb.SelectedIndex;
				else if (ctrl2 is ListBox lb)
					index = lb.SelectedIndex;
				else if (ctrl2 is TabControl tc)
					index = tc.SelectedIndex;
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}
			}

			return index + 1L;
		}

		internal override Keysharp.Core.Array ControlGetItems(object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var ctrl2 = Control.FromHandle(item.Handle);

				if (ctrl2 is ComboBox cb)
					return new Keysharp.Core.Array(cb.Items.Cast<object>().Select(item => (object)item.ToString()).ToList());
				else if (ctrl2 is ListBox lb)
					return new Keysharp.Core.Array(lb.Items.Cast<object>().Select(item => (object)item.ToString()).ToList());
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}
			}

			return new Keysharp.Core.Array();
		}

		internal override void ControlGetPos(ref object outX, ref object outY, ref object outWidth, ref object outHeight, object ctrl = null, string title = null, string text = null, string excludeTitle = null, string excludeText = null)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				if (Control.FromHandle(item.Handle) is Control ctrl2)
				{
					outX = ctrl2.Left;
					outY = ctrl2.Top;
					outWidth = ctrl2.Width;
					outHeight = ctrl2.Height;
				}
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}

				return;
			}

			outX = 0L;
			outY = 0L;
			outWidth = 0L;
			outHeight = 0L;
		}

		internal override long ControlGetStyle(object ctrl, object title, string text, string excludeTitle, string excludeText) => 1;

		internal override string ControlGetText(object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			var val = "";

			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
				val = Control.FromHandle(item.Handle) is Control ctrl2 ? ctrl2.Text : item.Title;

			return val;
		}

		internal override void ControlHideDropDown(object ctrl, object title, string text, string excludeTitle, string excludeText) =>
		DropdownHelper(false, ctrl, title, text, excludeTitle, excludeText);

		internal override void ControlSend(string str, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
		}

		internal override void ControlSendText(string str, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
		}

		internal override void ControlSetChecked(object val, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var onoff = Conversions.ConvertOnOffToggle(val);
				var ctrl2 = Control.FromHandle(item.Handle);

				if (ctrl2 is CheckBox cb)
					cb.Checked = onoff == ToggleValueType.Toggle ? !cb.Checked : onoff == ToggleValueType.On;
				else if (ctrl2 is RadioButton rb)
					rb.Checked = onoff == ToggleValueType.Toggle ? !rb.Checked : onoff == ToggleValueType.On;
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}
			}
		}

		internal override void ControlSetEnabled(object val, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var onoff = Conversions.ConvertOnOffToggle(val);

				if (Control.FromHandle(item.Handle) is Control ctrl2)
					ctrl2.Enabled = onoff == ToggleValueType.Toggle ? !ctrl2.Enabled : onoff == ToggleValueType.On;
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}

				WindowItemBase.DoControlDelay();
			}
		}

		internal override void ControlSetExStyle(object val, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
		}

		internal override void ControlSetStyle(object val, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
		}

		internal override void ControlShowDropDown(object ctrl, object title, string text, string excludeTitle, string excludeText) =>
		DropdownHelper(true, ctrl, title, text, excludeTitle, excludeText);

		internal override long EditGetCurrentCol(object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var ctrl2 = Control.FromHandle(item.Handle);

				if (ctrl2 is TextBoxBase txt)
					return txt.SelectionStart + 1;
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}
			}

			return 0L;
		}

		internal override long EditGetCurrentLine(object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var ctrl2 = Control.FromHandle(item.Handle);

				if (ctrl2 is TextBoxBase txt)
					return txt.GetLineFromCharIndex(txt.SelectionStart);//On linux the line index is 1-based, so don't add 1 to it.
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}
			}

			return 0L;
		}

		internal override string EditGetLine(int n, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var ctrl2 = Control.FromHandle(item.Handle);
				n--;

				if (ctrl2 is TextBoxBase txt)
				{
					var lines = txt.Lines;

					if (n >= lines.Length)
						throw new ValueError($"Requested line of {n + 1} is greater than the number of lines ({lines.Length}) in the text box in window with criteria: title: {title}, text: {text}, exclude title: {excludeTitle}, exclude text: {excludeText}");

					return lines[n];
				}
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}
			}

			return "";
		}

		internal override long EditGetLineCount(object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				if (Control.FromHandle(item.Handle) is TextBoxBase txt)
				{
					var val = txt.Lines.LongLength;
					return val == 0L ? 1L : val;
				}
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}
			}

			return 0L;
		}

		internal override string EditGetSelectedText(object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				if (Control.FromHandle(item.Handle) is TextBoxBase ctrl2)
					return ctrl2.SelectedText;
			}

			return "";
		}

		internal override void EditPaste(string str, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				if (Control.FromHandle(item.Handle) is TextBox ctrl2)
					ctrl2.Paste(str);
			}
		}

		internal override object ListViewGetContent(string options, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			object ret = null;

			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				var focused = false;
				var count = false;
				var sel = false;
				var countcol = false;
				var col = int.MinValue;
				var opts = Options.ParseOptions(options);

				foreach (var opt in opts)
				{
					if (string.Compare(opt, "focused", true) == 0) { focused = true; }
					else if (string.Compare(opt, "count", true) == 0) { count = true; }
					else if (string.Compare(opt, "selected", true) == 0) { sel = true; }
					else if (string.Compare(opt, "col", true) == 0) { countcol = true; }
					else if (Options.TryParse(opt, "col", ref col)) { col--; }
				}

				if (Control.FromHandle(item.Handle) is ListView lv)
				{
					if (count && sel)
						ret = (long)lv.SelectedItems.Count;
					else if (count && focused)
						ret = lv.FocusedItem is ListViewItem lvi ? lvi.Index + 1L : (object)0L;
					else if (count && countcol)
						ret = (long)lv.Columns.Count;
					else if (count)
						ret = (long)lv.Items.Count;
					else
					{
						var sb = new StringBuilder(1024);
						var items = new List<ListViewItem>();

						if (focused)
						{
							if (lv.FocusedItem is ListViewItem lvi)
								items.Add(lvi);
						}
						else if (sel)
							items.AddRange(lv.SelectedItems.Cast<ListViewItem>());
						else
							items.AddRange(lv.Items.Cast<ListViewItem>());

						if (col >= 0)
						{
							if (col >= lv.Columns.Count)
								throw new ValueError($"Column ${col + 1} is greater than list view column count of {lv.Columns.Count} in window with criteria: title: {title}, text: {text}, exclude title: {excludeTitle}, exclude text: {excludeText}");

							items.ForEach(templvi => sb.AppendLine(templvi.SubItems[col].Text));
						}
						else
							items.ForEach(templvi => sb.AppendLine(string.Join('\t', templvi.SubItems.Cast<ListViewItem.ListViewSubItem>().Select(x => x.Text))));

						ret = sb.ToString();
					}
				}
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}

				WindowItemBase.DoControlDelay();
			}

			return ret;
		}

		internal override void MenuSelect(object title, string text, string menu, string sub1, string sub2, string sub3, string sub4, string sub5, string sub6, string excludeTitle, string excludeText)
		{
			if (Window.SearchWindow(new object[] { title, text, excludeTitle, excludeText }, true) is WindowItem win)
			{
				if (Control.FromHandle(win.Handle) is Form form)
				{
					if (form.MainMenuStrip is MenuStrip strip)
					{
						if (GetMenuItem(strip, menu, sub1, sub2, sub3, sub4, sub5, sub6) is ToolStripMenuItem item)
							item.PerformClick();
						else
							throw new ValueError($"Could not find menu.", $"{title}, {text}, {menu}, {sub1}, {sub2}, {sub3}, {sub4}, {sub5}, {sub6}, {excludeTitle}, {excludeText}");
					}
				}
			}
		}

		internal override void PostMessage(int msg, int wparam, int lparam, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
		}

		internal override long SendMessage(int msg, object wparam, object lparam, object ctrl, object title, string text, string excludeTitle, string excludeText, int timeout) => 1;

		private static void DropdownHelper(bool val, object ctrl, object title, string text, string excludeTitle, string excludeText)
		{
			if (Window.SearchControl(ctrl, title, text, excludeTitle, excludeText) is WindowItem item)
			{
				if (Control.FromHandle(item.Handle) is ComboBox ctrl2)
				{
					ctrl2.DroppedDown = val;
				}
				else
				{
					//How to do the equivalent of what the Windows derivation does, but on linux?
				}

				WindowItemBase.DoControlDelay();
			}
		}
	}
}

#endif