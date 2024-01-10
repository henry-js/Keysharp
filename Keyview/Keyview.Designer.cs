﻿using ScintillaNET;

namespace Keyview
{
	partial class Keyview
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Keyview));
			this.FileName = new System.Windows.Forms.Label();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.selectLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.indentSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.outdentSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.uppercaseSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lowercaseSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			//this.goToLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.wordWrapItem = new System.Windows.Forms.ToolStripMenuItem();
			this.indentGuidesItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hiddenCharactersItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoom100ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.collapseAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.expandAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PanelSearch = new System.Windows.Forms.Panel();
			this.BtnNextSearch = new System.Windows.Forms.Button();
			this.BtnPrevSearch = new System.Windows.Forms.Button();
			this.BtnCloseSearch = new System.Windows.Forms.Button();
			this.TxtSearch = new System.Windows.Forms.TextBox();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.txtIn = new ScintillaNET.Scintilla();
			this.txtOut = new ScintillaNET.Scintilla();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tslCodeStatus = new System.Windows.Forms.ToolStripLabel();
			this.tssCode = new System.Windows.Forms.ToolStripSeparator();
			this.tslCodeCompile = new System.Windows.Forms.ToolStripLabel();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.PanelSearch.SuspendLayout();
			this.SuspendLayout();
			//
			// splitContainer
			//
			this.splitContainer.Cursor = System.Windows.Forms.Cursors.VSplit;
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 50);
			this.splitContainer.Margin = new System.Windows.Forms.Padding(0);
			this.splitContainer.Name = "splitContainer";
			//
			// splitContainer.Panel1
			//
			this.splitContainer.Panel1.Controls.Add(this.txtIn);
			this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(1);
			//
			// splitContainer.Panel2
			//
			this.splitContainer.Panel2.Controls.Add(this.txtOut);
			this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(1);
			this.splitContainer.Size = new System.Drawing.Size(976, 405);
			this.splitContainer.SplitterDistance = 500;
			this.splitContainer.TabIndex = 0;
			this.splitContainer.DoubleClick += new System.EventHandler(this.splitContainer_DoubleClick);
			//
			// txtIn
			//
			this.txtIn.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtIn.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.txtIn.Location = new System.Drawing.Point(0, 0);
			this.txtIn.Margin = new System.Windows.Forms.Padding(0);
			this.txtIn.Name = "txtIn";
			this.txtIn.Size = new System.Drawing.Size(456, 403);
			this.txtIn.TabIndex = 1;
			this.txtIn.Text = "";
			this.txtIn.TextChanged += new System.EventHandler(this.txtIn_TextChanged);
			this.txtIn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIn_KeyDown);
			//
			// txtOut
			//
			this.txtOut.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtOut.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.txtOut.Location = new System.Drawing.Point(43, 1);
			this.txtOut.Margin = new System.Windows.Forms.Padding(0);
			this.txtOut.Name = "txtOut";
			this.txtOut.ReadOnly = true;
			this.txtOut.Size = new System.Drawing.Size(428, 403);
			this.txtOut.TabIndex = 2;
			this.txtOut.Text = "";
			this.txtOut.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtOut_KeyDown);
			//
			// toolStrip1
			//
			this.toolStrip1.AutoSize = false;
			this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			//this.toolStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.tslCodeStatus,
				this.tssCode,
				this.tslCodeCompile
			});
			this.toolStrip1.Location = new System.Drawing.Point(0, 405);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(976, 45);
			this.toolStrip1.TabIndex = 10;
			//
			// tslCodeStatus
			//
			this.tslCodeStatus.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tslCodeStatus.AutoSize = false;
			this.tslCodeStatus.Name = "tslCodeStatus";
			this.tslCodeStatus.Size = new System.Drawing.Size(300, 39);
			this.tslCodeStatus.Text = "------------------------";
			//
			// tssCode
			//
			this.tssCode.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tssCode.Name = "tssCode";
			this.tssCode.Size = new System.Drawing.Size(6, 45);
			//
			// tslCodeCompile
			//
			this.tslCodeCompile.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.tslCodeCompile.Name = "tslCodeCompile";
			this.tslCodeCompile.Size = new System.Drawing.Size(145, 39);
			this.tslCodeCompile.Text = "Code compile:";
			//
			// menuStrip1
			//
			this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Top;
			this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.fileToolStripMenuItem,
				this.editToolStripMenuItem,
				this.searchToolStripMenuItem,
				this.viewToolStripMenuItem
			});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(769, 31);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			//
			// fileToolStripMenuItem
			//
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.openToolStripMenuItem
			});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(47, 27);
			this.fileToolStripMenuItem.Text = "File";
			//
			// openToolStripMenuItem
			//
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(134, 28);
			this.openToolStripMenuItem.Text = "Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			//
			// editToolStripMenuItem
			//
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.cutToolStripMenuItem,
				this.copyToolStripMenuItem,
				this.pasteToolStripMenuItem,
				this.toolStripSeparator1,
				this.selectLineToolStripMenuItem,
				this.selectAllToolStripMenuItem,
				this.clearSelectionToolStripMenuItem,
				this.toolStripSeparator2,
				this.indentSelectionToolStripMenuItem,
				this.outdentSelectionToolStripMenuItem,
				this.toolStripSeparator3,
				this.uppercaseSelectionToolStripMenuItem,
				this.lowercaseSelectionToolStripMenuItem
			});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(51, 27);
			this.editToolStripMenuItem.Text = "Edit";
			//
			// cutToolStripMenuItem
			//
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+X";
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(228, 28);
			this.cutToolStripMenuItem.Text = "Cut";
			this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
			//
			// copyToolStripMenuItem
			//
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(228, 28);
			this.copyToolStripMenuItem.Text = "Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			//
			// pasteToolStripMenuItem
			//
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+V";
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(228, 28);
			this.pasteToolStripMenuItem.Text = "Paste";
			this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
			//
			// toolStripSeparator1
			//
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(225, 6);
			//
			// selectLineToolStripMenuItem
			//
			this.selectLineToolStripMenuItem.Name = "selectLineToolStripMenuItem";
			this.selectLineToolStripMenuItem.Size = new System.Drawing.Size(228, 28);
			this.selectLineToolStripMenuItem.Text = "Select Line";
			this.selectLineToolStripMenuItem.Click += new System.EventHandler(this.selectLineToolStripMenuItem_Click);
			//
			// selectAllToolStripMenuItem
			//
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+A";
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(228, 28);
			this.selectAllToolStripMenuItem.Text = "Select All";
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
			//
			// clearSelectionToolStripMenuItem
			//
			this.clearSelectionToolStripMenuItem.Name = "clearSelectionToolStripMenuItem";
			this.clearSelectionToolStripMenuItem.Size = new System.Drawing.Size(228, 28);
			this.clearSelectionToolStripMenuItem.Text = "Clear Selection";
			this.clearSelectionToolStripMenuItem.Click += new System.EventHandler(this.clearSelectionToolStripMenuItem_Click);
			//
			// toolStripSeparator2
			//
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(225, 6);
			//
			// indentSelectionToolStripMenuItem
			//
			this.indentSelectionToolStripMenuItem.Name = "indentSelectionToolStripMenuItem";
			this.indentSelectionToolStripMenuItem.ShortcutKeyDisplayString = "Tab";
			this.indentSelectionToolStripMenuItem.Size = new System.Drawing.Size(228, 28);
			this.indentSelectionToolStripMenuItem.Text = "Indent";
			this.indentSelectionToolStripMenuItem.Click += new System.EventHandler(this.indentSelectionToolStripMenuItem_Click);
			//
			// outdentSelectionToolStripMenuItem
			//
			this.outdentSelectionToolStripMenuItem.Name = "outdentSelectionToolStripMenuItem";
			this.outdentSelectionToolStripMenuItem.ShortcutKeyDisplayString = "Shift+Tab";
			this.outdentSelectionToolStripMenuItem.Size = new System.Drawing.Size(228, 28);
			this.outdentSelectionToolStripMenuItem.Text = "Outdent";
			this.outdentSelectionToolStripMenuItem.Click += new System.EventHandler(this.outdentSelectionToolStripMenuItem_Click);
			//
			// toolStripSeparator3
			//
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(225, 6);
			//
			// uppercaseSelectionToolStripMenuItem
			//
			this.uppercaseSelectionToolStripMenuItem.Name = "uppercaseSelectionToolStripMenuItem";
			this.uppercaseSelectionToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+U";
			this.uppercaseSelectionToolStripMenuItem.Size = new System.Drawing.Size(228, 28);
			this.uppercaseSelectionToolStripMenuItem.Text = "Uppercase";
			this.uppercaseSelectionToolStripMenuItem.Click += new System.EventHandler(this.uppercaseSelectionToolStripMenuItem_Click);
			//
			// lowercaseSelectionToolStripMenuItem
			//
			this.lowercaseSelectionToolStripMenuItem.Name = "lowercaseSelectionToolStripMenuItem";
			this.lowercaseSelectionToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+L";
			this.lowercaseSelectionToolStripMenuItem.Size = new System.Drawing.Size(228, 28);
			this.lowercaseSelectionToolStripMenuItem.Text = "Lowercase";
			this.lowercaseSelectionToolStripMenuItem.Click += new System.EventHandler(this.lowercaseSelectionToolStripMenuItem_Click);
			//
			// searchToolStripMenuItem
			//
			this.searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.findToolStripMenuItem,
				//this.toolStripSeparator7,
				//this.goToLineToolStripMenuItem
			});
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(73, 27);
			this.searchToolStripMenuItem.Text = "Search";
			//
			// findToolStripMenuItem
			//
			this.findToolStripMenuItem.Name = "findToolStripMenuItem";
			this.findToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+F";
			this.findToolStripMenuItem.Size = new System.Drawing.Size(283, 28);
			this.findToolStripMenuItem.Text = "Quick Find...";
			this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
			//
			// toolStripSeparator7
			//
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(280, 6);
			//
			// goToLineToolStripMenuItem
			//
			//this.goToLineToolStripMenuItem.Name = "goToLineToolStripMenuItem";
			//this.goToLineToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+G";
			//this.goToLineToolStripMenuItem.Size = new System.Drawing.Size(283, 28);
			//this.goToLineToolStripMenuItem.Text = "Go To Line...";
			//
			// viewToolStripMenuItem
			//
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.wordWrapItem,
				this.indentGuidesItem,
				this.hiddenCharactersItem,
				this.toolStripSeparator4,
				this.zoomInToolStripMenuItem,
				this.zoomOutToolStripMenuItem,
				this.zoom100ToolStripMenuItem,
				this.toolStripSeparator5,
				this.collapseAllToolStripMenuItem,
				this.expandAllToolStripMenuItem
			});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(58, 27);
			this.viewToolStripMenuItem.Text = "View";
			//
			// wordWrapItem
			//
			this.wordWrapItem.Name = "wordWrapItem";
			this.wordWrapItem.Size = new System.Drawing.Size(254, 28);
			this.wordWrapItem.Text = "Word Wrap";
			this.wordWrapItem.Click += new System.EventHandler(this.wordWrapToolStripMenuItem1_Click);
			//
			// indentGuidesItem
			//
			this.indentGuidesItem.Checked = true;
			this.indentGuidesItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.indentGuidesItem.Name = "indentGuidesItem";
			this.indentGuidesItem.Size = new System.Drawing.Size(254, 28);
			this.indentGuidesItem.Text = "Show Indent Guides";
			this.indentGuidesItem.Click += new System.EventHandler(this.indentGuidesToolStripMenuItem_Click);
			//
			// hiddenCharactersItem
			//
			this.hiddenCharactersItem.Name = "hiddenCharactersItem";
			this.hiddenCharactersItem.Size = new System.Drawing.Size(254, 28);
			this.hiddenCharactersItem.Text = "Show Whitespace";
			this.hiddenCharactersItem.Click += new System.EventHandler(this.hiddenCharactersToolStripMenuItem_Click);
			//
			// toolStripSeparator4
			//
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(251, 6);
			//
			// zoomInToolStripMenuItem
			//
			this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
			this.zoomInToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Plus";
			this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(254, 28);
			this.zoomInToolStripMenuItem.Text = "Zoom In";
			this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
			//
			// zoomOutToolStripMenuItem
			//
			this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
			this.zoomOutToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Minus";
			this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(254, 28);
			this.zoomOutToolStripMenuItem.Text = "Zoom Out";
			this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
			//
			// zoom100ToolStripMenuItem
			//
			this.zoom100ToolStripMenuItem.Name = "zoom100ToolStripMenuItem";
			this.zoom100ToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+0";
			this.zoom100ToolStripMenuItem.Size = new System.Drawing.Size(254, 28);
			this.zoom100ToolStripMenuItem.Text = "Zoom 100%";
			this.zoom100ToolStripMenuItem.Click += new System.EventHandler(this.zoom100ToolStripMenuItem_Click);
			//
			// toolStripSeparator5
			//
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(251, 6);
			//
			// collapseAllToolStripMenuItem
			//
			this.collapseAllToolStripMenuItem.Name = "collapseAllToolStripMenuItem";
			this.collapseAllToolStripMenuItem.Size = new System.Drawing.Size(254, 28);
			this.collapseAllToolStripMenuItem.Text = "Collapse All";
			this.collapseAllToolStripMenuItem.Click += new System.EventHandler(this.collapseAllToolStripMenuItem_Click);
			//
			// expandAllToolStripMenuItem
			//
			this.expandAllToolStripMenuItem.Name = "expandAllToolStripMenuItem";
			this.expandAllToolStripMenuItem.Size = new System.Drawing.Size(254, 28);
			this.expandAllToolStripMenuItem.Text = "Expand All";
			this.expandAllToolStripMenuItem.Click += new System.EventHandler(this.expandAllToolStripMenuItem_Click);
			//
			// PanelSearch
			//
			this.PanelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.PanelSearch.Dock = System.Windows.Forms.DockStyle.Top;
			this.PanelSearch.Margin = new System.Windows.Forms.Padding(5);
			this.PanelSearch.BackColor = System.Drawing.Color.White;
			this.PanelSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PanelSearch.Padding = new System.Windows.Forms.Padding(5);
			this.PanelSearch.Controls.Add(this.BtnNextSearch);
			this.PanelSearch.Controls.Add(this.BtnPrevSearch);
			this.PanelSearch.Controls.Add(this.BtnCloseSearch);
			this.PanelSearch.Controls.Add(this.TxtSearch);
			this.PanelSearch.Location = new System.Drawing.Point(0, 0);
			this.PanelSearch.Name = "PanelSearch";
			this.PanelSearch.Size = new System.Drawing.Size(292, 45);
			this.PanelSearch.TabIndex = 10;
			this.PanelSearch.Visible = false;
			//
			// BtnNextSearch
			//
			this.BtnNextSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnNextSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnNextSearch.ForeColor = System.Drawing.Color.White;
			this.BtnNextSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnNextSearch.Image")));
			this.BtnNextSearch.Location = new System.Drawing.Point(233, 4);
			this.BtnNextSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.BtnNextSearch.Name = "BtnNextSearch";
			this.BtnNextSearch.Size = new System.Drawing.Size(25, 30);
			this.BtnNextSearch.TabIndex = 9;
			this.BtnNextSearch.Tag = "Find next (Enter)";
			this.BtnNextSearch.UseVisualStyleBackColor = true;
			this.BtnNextSearch.Click += new System.EventHandler(this.BtnNextSearch_Click);
			//
			// BtnPrevSearch
			//
			this.BtnPrevSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnPrevSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnPrevSearch.ForeColor = System.Drawing.Color.White;
			this.BtnPrevSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnPrevSearch.Image")));
			this.BtnPrevSearch.Location = new System.Drawing.Point(205, 4);
			this.BtnPrevSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.BtnPrevSearch.Name = "BtnPrevSearch";
			this.BtnPrevSearch.Size = new System.Drawing.Size(25, 30);
			this.BtnPrevSearch.TabIndex = 8;
			this.BtnPrevSearch.Tag = "Find previous (Shift+Enter)";
			this.BtnPrevSearch.UseVisualStyleBackColor = true;
			this.BtnPrevSearch.Click += new System.EventHandler(this.BtnPrevSearch_Click);
			//
			// BtnCloseSearch
			//
			this.BtnCloseSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnCloseSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.BtnCloseSearch.ForeColor = System.Drawing.Color.White;
			this.BtnCloseSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnCloseSearch.Image")));
			this.BtnCloseSearch.Location = new System.Drawing.Point(261, 4);
			this.BtnCloseSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.BtnCloseSearch.Name = "BtnCloseSearch";
			this.BtnCloseSearch.Size = new System.Drawing.Size(25, 30);
			this.BtnCloseSearch.TabIndex = 7;
			this.BtnCloseSearch.Tag = "Close (Esc)";
			this.BtnCloseSearch.UseVisualStyleBackColor = true;
			this.BtnCloseSearch.Click += new System.EventHandler(this.BtnClearSearch_Click);
			//
			// TxtSearch
			//
			this.TxtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									 | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TxtSearch.Dock = System.Windows.Forms.DockStyle.Top;
			this.TxtSearch.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TxtSearch.Location = new System.Drawing.Point(0, 0);
			this.TxtSearch.Margin = new System.Windows.Forms.Padding(4);
			this.TxtSearch.Name = "TxtSearch";
			this.TxtSearch.Size = new System.Drawing.Size(189, 35);
			this.TxtSearch.TabIndex = 6;
			this.TxtSearch.TextChanged += new System.EventHandler(this.TxtSearch_TextChanged);
			this.TxtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtSearch_KeyDown);
			//
			// openFileDialog
			//
			this.openFileDialog.DefaultExt = "txt";
			this.openFileDialog.FileName = "New File";
			this.openFileDialog.Filter = "All files|*.*";
			//
			// Keyview
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.MainMenuStrip = this.menuStrip1;
			this.ClientSize = new System.Drawing.Size(976, 450);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.PanelSearch);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Name = "Keyview";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Keyview";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Keyview_FormClosing);
			this.Load += new System.EventHandler(this.Keyview_Load);
			this.ResizeEnd += new System.EventHandler(this.Keyview_ResizeEnd);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.PanelSearch.ResumeLayout(false);
			this.PanelSearch.PerformLayout();
			this.ResumeLayout(false);
		}

		#endregion
		private System.Windows.Forms.Label FileName;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem wordWrapItem;
		private System.Windows.Forms.ToolStripMenuItem hiddenCharactersItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoom100ToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem collapseAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem expandAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem indentSelectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem outdentSelectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem uppercaseSelectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lowercaseSelectionToolStripMenuItem;
		private System.Windows.Forms.Panel PanelSearch;
		private System.Windows.Forms.Button BtnNextSearch;
		private System.Windows.Forms.Button BtnPrevSearch;
		private System.Windows.Forms.Button BtnCloseSearch;
		private System.Windows.Forms.TextBox TxtSearch;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ToolStripMenuItem selectLineToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem clearSelectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		//private System.Windows.Forms.ToolStripMenuItem goToLineToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripMenuItem indentGuidesItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel tslCodeCompile;
		private System.Windows.Forms.ToolStripSeparator tssCode;
		private System.Windows.Forms.ToolStripLabel tslCodeStatus;
		private ScintillaNET.Scintilla txtIn;
		private ScintillaNET.Scintilla txtOut;
	}
}

