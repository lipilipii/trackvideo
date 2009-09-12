//*******************************************************************
/*

	Solution:	TrackVideo
	Project:	TrackVideoApp
	File:		MainForm.Designer.cs

	Copyright 2009, ralfoide at gmail dot com.

	This file is part of TrackVideo.

	TrackVideo is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	TrackVideo is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with TrackVideo; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

*/
//*******************************************************************

using System;
using System.Windows.Forms;

namespace Alfray.TrackVideo.TrackVideoApp {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.MainMenu mMenuMain;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.StatusBar mStatusBar;
        private System.Windows.Forms.MenuItem mMenuItemConnect;
        private System.Windows.Forms.MenuItem mMenuItemDisconnect;
        private System.Windows.Forms.MenuItem mMenuItemQuit;
        private System.Windows.Forms.MenuItem mMenuItemUpdate;
        private System.Windows.Forms.MenuItem mMenuItemDebug;
        private System.Windows.Forms.MenuItem mMenuItemAbout;
        private System.Windows.Forms.MenuItem mMenuHelp;
        private System.Windows.Forms.MenuItem mMenuItemPreferences;
        private System.Windows.Forms.MenuItem menuItem3;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.mMenuMain = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.mMenuItemConnect = new System.Windows.Forms.MenuItem();
            this.mMenuItemDisconnect = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mMenuItemPreferences = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mMenuItemQuit = new System.Windows.Forms.MenuItem();
            this.mMenuHelp = new System.Windows.Forms.MenuItem();
            this.mMenuItemUpdate = new System.Windows.Forms.MenuItem();
            this.mMenuItemDebug = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.mMenuItemAbout = new System.Windows.Forms.MenuItem();
            this.mStatusBar = new System.Windows.Forms.StatusBar();
            this.label1 = new System.Windows.Forms.Label();
            this.mEditTrackFilename = new System.Windows.Forms.TextBox();
            this.mButtonBrowseTrack = new System.Windows.Forms.Button();
            this.mEditDestFilename = new System.Windows.Forms.TextBox();
            this.mButtonBrowseDest = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.mButtonGenerate = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mMenuMain
            // 
            this.mMenuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.mMenuHelp});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mMenuItemConnect,
            this.mMenuItemDisconnect,
            this.menuItem3,
            this.mMenuItemPreferences,
            this.menuItem4,
            this.mMenuItemQuit});
            this.menuItem1.Text = "File";
            // 
            // mMenuItemConnect
            // 
            this.mMenuItemConnect.Index = 0;
            this.mMenuItemConnect.Text = "Item 1";
            // 
            // mMenuItemDisconnect
            // 
            this.mMenuItemDisconnect.Index = 1;
            this.mMenuItemDisconnect.Text = "Item 2";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 2;
            this.menuItem3.Text = "-";
            // 
            // mMenuItemPreferences
            // 
            this.mMenuItemPreferences.Index = 3;
            this.mMenuItemPreferences.Text = "Preferences...";
            this.mMenuItemPreferences.Click += new System.EventHandler(this.mMenuItemPreferences_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 4;
            this.menuItem4.Text = "-";
            // 
            // mMenuItemQuit
            // 
            this.mMenuItemQuit.Index = 5;
            this.mMenuItemQuit.Text = "Quit";
            this.mMenuItemQuit.Click += new System.EventHandler(this.mMenuItemQuit_Click);
            // 
            // mMenuHelp
            // 
            this.mMenuHelp.Index = 1;
            this.mMenuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mMenuItemUpdate,
            this.mMenuItemDebug,
            this.menuItem10,
            this.mMenuItemAbout});
            this.mMenuHelp.Text = "Help";
            // 
            // mMenuItemUpdate
            // 
            this.mMenuItemUpdate.Index = 0;
            this.mMenuItemUpdate.Text = "Update...";
            // 
            // mMenuItemDebug
            // 
            this.mMenuItemDebug.Index = 1;
            this.mMenuItemDebug.Text = "Debug";
            this.mMenuItemDebug.Click += new System.EventHandler(this.mMenuItemDebug_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 2;
            this.menuItem10.Text = "-";
            // 
            // mMenuItemAbout
            // 
            this.mMenuItemAbout.Index = 3;
            this.mMenuItemAbout.Text = "About...";
            // 
            // mStatusBar
            // 
            this.mStatusBar.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.mStatusBar.Location = new System.Drawing.Point(0, 187);
            this.mStatusBar.Name = "mStatusBar";
            this.mStatusBar.Size = new System.Drawing.Size(492, 22);
            this.mStatusBar.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Track (.kml)";
            // 
            // mEditTrackFilename
            // 
            this.mEditTrackFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mEditTrackFilename.Location = new System.Drawing.Point(82, 9);
            this.mEditTrackFilename.Name = "mEditTrackFilename";
            this.mEditTrackFilename.Size = new System.Drawing.Size(317, 20);
            this.mEditTrackFilename.TabIndex = 2;
            // 
            // mButtonBrowseTrack
            // 
            this.mButtonBrowseTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonBrowseTrack.Location = new System.Drawing.Point(405, 7);
            this.mButtonBrowseTrack.Name = "mButtonBrowseTrack";
            this.mButtonBrowseTrack.Size = new System.Drawing.Size(75, 23);
            this.mButtonBrowseTrack.TabIndex = 3;
            this.mButtonBrowseTrack.Text = "Browse...";
            this.mButtonBrowseTrack.UseVisualStyleBackColor = true;
            this.mButtonBrowseTrack.Click += new System.EventHandler(this.mButtonBrowseTrack_Click);
            // 
            // mEditDestFilename
            // 
            this.mEditDestFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mEditDestFilename.Location = new System.Drawing.Point(82, 36);
            this.mEditDestFilename.Name = "mEditDestFilename";
            this.mEditDestFilename.Size = new System.Drawing.Size(317, 20);
            this.mEditDestFilename.TabIndex = 4;
            // 
            // mButtonBrowseDest
            // 
            this.mButtonBrowseDest.Location = new System.Drawing.Point(405, 34);
            this.mButtonBrowseDest.Name = "mButtonBrowseDest";
            this.mButtonBrowseDest.Size = new System.Drawing.Size(75, 23);
            this.mButtonBrowseDest.TabIndex = 5;
            this.mButtonBrowseDest.Text = "Browse...";
            this.mButtonBrowseDest.UseVisualStyleBackColor = true;
            this.mButtonBrowseDest.Click += new System.EventHandler(this.mButtonBrowseDest_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Dest File";
            // 
            // mButtonGenerate
            // 
            this.mButtonGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonGenerate.Location = new System.Drawing.Point(405, 158);
            this.mButtonGenerate.Name = "mButtonGenerate";
            this.mButtonGenerate.Size = new System.Drawing.Size(75, 23);
            this.mButtonGenerate.TabIndex = 7;
            this.mButtonGenerate.Text = "Generate";
            this.mButtonGenerate.UseVisualStyleBackColor = true;
            this.mButtonGenerate.Click += new System.EventHandler(this.mButtonGenerate_Click);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(492, 209);
            this.Controls.Add(this.mButtonGenerate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.mButtonBrowseDest);
            this.Controls.Add(this.mEditDestFilename);
            this.Controls.Add(this.mButtonBrowseTrack);
            this.Controls.Add(this.mEditTrackFilename);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mStatusBar);
            this.Menu = this.mMenuMain;
            this.Name = "MainForm";
            this.Text = "Track Video";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.RMainForm_Closing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private Label label1;
        private TextBox mEditTrackFilename;
        private Button mButtonBrowseTrack;
        private TextBox mEditDestFilename;
        private Button mButtonBrowseDest;
        private Label label2;
        private Button mButtonGenerate;
 


        //-------------------------------------------
        //----------- Private Callbacks -------------
        //-------------------------------------------


        private void mMenuItemQuit_Click(object sender, System.EventArgs e) {
            this.Close();
        }

        private void mMenuItemDebug_Click(object sender, System.EventArgs e) {
            showHideDebugWindow();
        }


        private void RMainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            terminate();
        }

        private void mMenuItemPreferences_Click(object sender, System.EventArgs e) {
            showHidePrefWindow();
        }

        private void MainForm_Activated(object sender, EventArgs e) {
            onFormActivated();
        }

        private void mButtonBrowseTrack_Click(object sender, EventArgs e) {
            onBrowseTrack();
        }

        private void mButtonBrowseDest_Click(object sender, EventArgs e) {
            onBrowseDest();
        }

        private void mButtonGenerate_Click(object sender, EventArgs e) {
            onGenerate();
        }
   }
}