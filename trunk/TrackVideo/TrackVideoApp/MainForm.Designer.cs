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
        private System.Windows.Forms.MenuItem menuItem10;
        private System.Windows.Forms.StatusBar mStatusBar;
        private System.Windows.Forms.MenuItem mMenuItemConnect;
        private System.Windows.Forms.MenuItem mMenuItemDisconnect;
        private System.Windows.Forms.MenuItem mMenuItemQuit;
        private System.Windows.Forms.MenuItem mMenuItemUpdate;
        private System.Windows.Forms.MenuItem mMenuItemDebug;
        private System.Windows.Forms.MenuItem mMenuItemAbout;
        private System.Windows.Forms.MenuItem mMenuHelp;
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
            this.mButtonGenerate = new System.Windows.Forms.Button();
            this.mEditFps = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.mEditTrackSy = new System.Windows.Forms.TextBox();
            this.mEditTrackSx = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mEditMovieSy = new System.Windows.Forms.TextBox();
            this.mEditMovieSx = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mRadioRender = new System.Windows.Forms.RadioButton();
            this.mProgressBar = new System.Windows.Forms.ProgressBar();
            this.mRadioPreview = new System.Windows.Forms.RadioButton();
            this.mPreviewPicture = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.mButtonReloadTrack = new System.Windows.Forms.Button();
            this.mLabelTrackDataLoadResults = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mPreviewPicture)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
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
            // mMenuItemQuit
            // 
            this.mMenuItemQuit.Index = 3;
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
            this.mStatusBar.Location = new System.Drawing.Point(0, 405);
            this.mStatusBar.Name = "mStatusBar";
            this.mStatusBar.Size = new System.Drawing.Size(565, 22);
            this.mStatusBar.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Track (.kml)";
            // 
            // mEditTrackFilename
            // 
            this.mEditTrackFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mEditTrackFilename.Location = new System.Drawing.Point(75, 22);
            this.mEditTrackFilename.Name = "mEditTrackFilename";
            this.mEditTrackFilename.Size = new System.Drawing.Size(379, 20);
            this.mEditTrackFilename.TabIndex = 2;
            this.mEditTrackFilename.TextChanged += new System.EventHandler(this.mEditTrackFilename_TextChanged);
            // 
            // mButtonBrowseTrack
            // 
            this.mButtonBrowseTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonBrowseTrack.Location = new System.Drawing.Point(460, 20);
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
            this.mEditDestFilename.Location = new System.Drawing.Point(101, 42);
            this.mEditDestFilename.Name = "mEditDestFilename";
            this.mEditDestFilename.Size = new System.Drawing.Size(353, 20);
            this.mEditDestFilename.TabIndex = 4;
            // 
            // mButtonBrowseDest
            // 
            this.mButtonBrowseDest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonBrowseDest.Location = new System.Drawing.Point(460, 40);
            this.mButtonBrowseDest.Name = "mButtonBrowseDest";
            this.mButtonBrowseDest.Size = new System.Drawing.Size(75, 23);
            this.mButtonBrowseDest.TabIndex = 5;
            this.mButtonBrowseDest.Text = "Browse...";
            this.mButtonBrowseDest.UseVisualStyleBackColor = true;
            this.mButtonBrowseDest.Click += new System.EventHandler(this.mButtonBrowseDest_Click);
            // 
            // mButtonGenerate
            // 
            this.mButtonGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonGenerate.Location = new System.Drawing.Point(460, 68);
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
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.mEditTrackSy);
            this.groupBox1.Controls.Add(this.mEditTrackSx);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.mEditMovieSy);
            this.groupBox1.Controls.Add(this.mEditMovieSx);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.mEditFps);
            this.groupBox1.Location = new System.Drawing.Point(12, 204);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(114, 188);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(5, 117);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(44, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "Track:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(5, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Container:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Height";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 136);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Width";
            // 
            // mEditTrackSy
            // 
            this.mEditTrackSy.Location = new System.Drawing.Point(46, 159);
            this.mEditTrackSy.MaxLength = 10;
            this.mEditTrackSy.Name = "mEditTrackSy";
            this.mEditTrackSy.Size = new System.Drawing.Size(53, 20);
            this.mEditTrackSy.TabIndex = 15;
            // 
            // mEditTrackSx
            // 
            this.mEditTrackSx.Location = new System.Drawing.Point(46, 133);
            this.mEditTrackSx.MaxLength = 10;
            this.mEditTrackSx.Name = "mEditTrackSx";
            this.mEditTrackSx.Size = new System.Drawing.Size(53, 20);
            this.mEditTrackSx.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Height";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Width";
            // 
            // mEditMovieSy
            // 
            this.mEditMovieSy.Location = new System.Drawing.Point(46, 89);
            this.mEditMovieSy.MaxLength = 10;
            this.mEditMovieSy.Name = "mEditMovieSy";
            this.mEditMovieSy.Size = new System.Drawing.Size(53, 20);
            this.mEditMovieSy.TabIndex = 11;
            // 
            // mEditMovieSx
            // 
            this.mEditMovieSx.Location = new System.Drawing.Point(46, 63);
            this.mEditMovieSx.MaxLength = 10;
            this.mEditMovieSx.Name = "mEditMovieSx";
            this.mEditMovieSx.Size = new System.Drawing.Size(53, 20);
            this.mEditMovieSx.TabIndex = 10;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.mRadioRender);
            this.groupBox2.Controls.Add(this.mProgressBar);
            this.groupBox2.Controls.Add(this.mRadioPreview);
            this.groupBox2.Controls.Add(this.mButtonGenerate);
            this.groupBox2.Controls.Add(this.mEditDestFilename);
            this.groupBox2.Controls.Add(this.mButtonBrowseDest);
            this.groupBox2.Location = new System.Drawing.Point(12, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(541, 101);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Preview && Create Movie";
            // 
            // mRadioRender
            // 
            this.mRadioRender.AutoSize = true;
            this.mRadioRender.Location = new System.Drawing.Point(9, 43);
            this.mRadioRender.Name = "mRadioRender";
            this.mRadioRender.Size = new System.Drawing.Size(86, 17);
            this.mRadioRender.TabIndex = 9;
            this.mRadioRender.TabStop = true;
            this.mRadioRender.Text = "Also save to:";
            this.mRadioRender.UseVisualStyleBackColor = true;
            // 
            // mProgressBar
            // 
            this.mProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mProgressBar.Location = new System.Drawing.Point(9, 68);
            this.mProgressBar.Name = "mProgressBar";
            this.mProgressBar.Size = new System.Drawing.Size(445, 23);
            this.mProgressBar.TabIndex = 9;
            // 
            // mRadioPreview
            // 
            this.mRadioPreview.AutoSize = true;
            this.mRadioPreview.Location = new System.Drawing.Point(9, 19);
            this.mRadioPreview.Name = "mRadioPreview";
            this.mRadioPreview.Size = new System.Drawing.Size(85, 17);
            this.mRadioPreview.TabIndex = 8;
            this.mRadioPreview.TabStop = true;
            this.mRadioPreview.Text = "Preview only";
            this.mRadioPreview.UseVisualStyleBackColor = true;
            // 
            // mPreviewPicture
            // 
            this.mPreviewPicture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mPreviewPicture.Location = new System.Drawing.Point(251, 204);
            this.mPreviewPicture.Name = "mPreviewPicture";
            this.mPreviewPicture.Size = new System.Drawing.Size(302, 189);
            this.mPreviewPicture.TabIndex = 8;
            this.mPreviewPicture.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.mButtonReloadTrack);
            this.groupBox3.Controls.Add(this.mLabelTrackDataLoadResults);
            this.groupBox3.Controls.Add(this.mButtonBrowseTrack);
            this.groupBox3.Controls.Add(this.mEditTrackFilename);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(541, 79);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Track Data";
            // 
            // mButtonReloadTrack
            // 
            this.mButtonReloadTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonReloadTrack.Location = new System.Drawing.Point(460, 49);
            this.mButtonReloadTrack.Name = "mButtonReloadTrack";
            this.mButtonReloadTrack.Size = new System.Drawing.Size(75, 23);
            this.mButtonReloadTrack.TabIndex = 5;
            this.mButtonReloadTrack.Text = "Reload";
            this.mButtonReloadTrack.UseVisualStyleBackColor = true;
            this.mButtonReloadTrack.Click += new System.EventHandler(this.mButtonReloadTrack_Click);
            // 
            // mLabelTrackDataLoadResults
            // 
            this.mLabelTrackDataLoadResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mLabelTrackDataLoadResults.Location = new System.Drawing.Point(9, 54);
            this.mLabelTrackDataLoadResults.Name = "mLabelTrackDataLoadResults";
            this.mLabelTrackDataLoadResults.Size = new System.Drawing.Size(445, 16);
            this.mLabelTrackDataLoadResults.TabIndex = 4;
            this.mLabelTrackDataLoadResults.Text = "(placeholder for load results)";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Location = new System.Drawing.Point(132, 204);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(113, 188);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Templates (TODO)";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(565, 427);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.mPreviewPicture);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mStatusBar);
            this.Menu = this.mMenuMain;
            this.MinimumSize = new System.Drawing.Size(500, 330);
            this.Name = "MainForm";
            this.Text = "Track Video";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.RMainForm_Closing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mPreviewPicture)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion


        private Label label1;
        private TextBox mEditTrackFilename;
        private Button mButtonBrowseTrack;
        private TextBox mEditDestFilename;
        private Button mButtonBrowseDest;
        private Button mButtonGenerate;

        private TextBox mEditFps;
        private Label label3;
        private GroupBox groupBox1;
        private Label label5;
        private Label label4;
        private TextBox mEditMovieSy;
        private TextBox mEditMovieSx;
        private GroupBox groupBox2;
        private ProgressBar mProgressBar;
        private PictureBox mPreviewPicture;
        private Label label7;
        private Label label8;
        private TextBox mEditTrackSy;
        private TextBox mEditTrackSx;
        private Label label10;
        private Label label9;
 


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

        private void MainForm_Activated(object sender, EventArgs e) {
            onFormActivated();
        }

        private void mButtonBrowseTrack_Click(object sender, EventArgs e) {
            onBrowseTrack();
        }

        private void mButtonReloadTrack_Click(object sender, EventArgs e) {
            onReloadTrack();
        }

        private void mButtonBrowseDest_Click(object sender, EventArgs e) {
            onBrowseDest();
        }

        private void mButtonGenerate_Click(object sender, EventArgs e) {
            onGenerate();
        }

        private void mEditTrackFilename_TextChanged(object sender, EventArgs e) {
            updateButtons();
        }

        private GroupBox groupBox3;
        private Label mLabelTrackDataLoadResults;
        private Button mButtonReloadTrack;
        private RadioButton mRadioRender;
        private RadioButton mRadioPreview;
        private GroupBox groupBox4;
        private Button button2;
        private Button button1;
   }
}