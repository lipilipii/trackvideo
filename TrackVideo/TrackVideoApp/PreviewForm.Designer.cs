//*******************************************************************
/*

	Solution:	TrackVideo
	Project:	TrackVideoApp
	File:		MainForm.cs

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


namespace Alfray.TrackVideo.TrackVideoApp {
    partial class PreviewForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mPreviewPicture = new System.Windows.Forms.PictureBox();
            this.mNumFrame = new System.Windows.Forms.NumericUpDown();
            this.mButtonStop = new System.Windows.Forms.Button();
            this.mProgressBar = new System.Windows.Forms.ProgressBar();
            this.mButtonPlay = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mPreviewPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mNumFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // mPreviewPicture
            // 
            this.mPreviewPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mPreviewPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mPreviewPicture.Location = new System.Drawing.Point(12, 12);
            this.mPreviewPicture.Name = "mPreviewPicture";
            this.mPreviewPicture.Size = new System.Drawing.Size(450, 253);
            this.mPreviewPicture.TabIndex = 9;
            this.mPreviewPicture.TabStop = false;
            // 
            // mNumFrame
            // 
            this.mNumFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mNumFrame.Location = new System.Drawing.Point(12, 274);
            this.mNumFrame.Name = "mNumFrame";
            this.mNumFrame.Size = new System.Drawing.Size(120, 20);
            this.mNumFrame.TabIndex = 10;
            // 
            // mButtonStop
            // 
            this.mButtonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonStop.Location = new System.Drawing.Point(398, 271);
            this.mButtonStop.Name = "mButtonStop";
            this.mButtonStop.Size = new System.Drawing.Size(29, 23);
            this.mButtonStop.TabIndex = 11;
            this.mButtonStop.Text = "[]";
            this.mButtonStop.UseVisualStyleBackColor = true;
            // 
            // mProgressBar
            // 
            this.mProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.mProgressBar.Location = new System.Drawing.Point(138, 271);
            this.mProgressBar.Name = "mProgressBar";
            this.mProgressBar.Size = new System.Drawing.Size(254, 23);
            this.mProgressBar.TabIndex = 12;
            // 
            // mButtonPlay
            // 
            this.mButtonPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonPlay.Location = new System.Drawing.Point(433, 271);
            this.mButtonPlay.Name = "mButtonPlay";
            this.mButtonPlay.Size = new System.Drawing.Size(29, 23);
            this.mButtonPlay.TabIndex = 13;
            this.mButtonPlay.Text = "|>";
            this.mButtonPlay.UseVisualStyleBackColor = true;
            // 
            // PreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 306);
            this.Controls.Add(this.mButtonPlay);
            this.Controls.Add(this.mProgressBar);
            this.Controls.Add(this.mButtonStop);
            this.Controls.Add(this.mNumFrame);
            this.Controls.Add(this.mPreviewPicture);
            this.Name = "PreviewForm";
            this.Text = "Preview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.onFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.mPreviewPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mNumFrame)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.PictureBox mPreviewPicture;
        private System.Windows.Forms.NumericUpDown mNumFrame;
        private System.Windows.Forms.Button mButtonStop;
        private System.Windows.Forms.ProgressBar mProgressBar;
        private System.Windows.Forms.Button mButtonPlay;
    }
}