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
            this.mEditFps = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mEditSx = new System.Windows.Forms.TextBox();
            this.mEditSy = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mPreviewPicture = new System.Windows.Forms.PictureBox();
            this.mProgressBar = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mPreviewPicture)).BeginInit();
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
            this.mStatusBar.Location = new System.Drawing.Point(0, 172);
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
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Dest File";
            // 
            // mButtonGenerate
            // 
            this.mButtonGenerate.Location = new System.Drawing.Point(6, 19);
            this.mButtonGenerate.Name = "mButtonGenerate";
            this.mButtonGenerate.Size = new System.Drawing.Size(75, 23);
            this.mButtonGenerate.TabIndex = 7;
            this.mButtonGenerate.Text = "Start";
            this.mButtonGenerate.UseVisualStyleBackColor = true;
            this.mButtonGenerate.Click += new System.EventHandler(this.mButtonGenerate_Click);
            // 
            // mEditFps
            // 
            this.mEditFps.Location = new System.Drawing.Point(46, 21);
            this.mEditFps.MaxLength = 10;
            this.mEditFps.Name = "mEditFps";
            this.mEditFps.Size = new System.Drawing.Size(53, 20);
            this.mEditFps.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "FPS";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.mEditSy);
            this.groupBox1.Controls.Add(this.mEditSx);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.mEditFps);
            this.groupBox1.Location = new System.Drawing.Point(12, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(114, 104);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // mEditSx
            // 
            this.mEditSx.Location = new System.Drawing.Point(46, 47);
            this.mEditSx.MaxLength = 10;
            this.mEditSx.Name = "mEditSx";
            this.mEditSx.Size = new System.Drawing.Size(53, 20);
            this.mEditSx.TabIndex = 10;
            // 
            // mEditSy
            // 
            this.mEditSy.Location = new System.Drawing.Point(46, 73);
            this.mEditSy.MaxLength = 10;
            this.mEditSy.Name = "mEditSy";
            this.mEditSy.Size = new System.Drawing.Size(53, 20);
            this.mEditSy.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Width";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Height";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.mProgressBar);
            this.groupBox2.Controls.Add(this.mPreviewPicture);
            this.groupBox2.Controls.Add(this.mButtonGenerate);
            this.groupBox2.Location = new System.Drawing.Point(132, 62);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(348, 104);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Generation";
            // 
            // mPreviewPicture
            // 
            this.mPreviewPicture.Location = new System.Drawing.Point(87, 19);
            this.mPreviewPicture.Name = "mPreviewPicture";
            this.mPreviewPicture.Size = new System.Drawing.Size(255, 79);
            this.mPreviewPicture.TabIndex = 8;
            this.mPreviewPicture.TabStop = false;
            // 
            // mProgressBar
            // 
            this.mProgressBar.Location = new System.Drawing.Point(6, 70);
            this.mProgressBar.Name = "mProgressBar";
            this.mProgressBar.Size = new System.Drawing.Size(75, 23);
            this.mProgressBar.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Progress:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(492, 194);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mPreviewPicture)).EndInit();
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

        private TextBox mEditFps;
        private Label label3;
        private GroupBox groupBox1;
        private Label label5;
        private Label label4;
        private TextBox mEditSy;
        private TextBox mEditSx;
        private GroupBox groupBox2;
        private Label label6;
        private ProgressBar mProgressBar;
        private PictureBox mPreviewPicture;
   }
}