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

using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Alfray.TrackVideo.TrackVideoApp {
    public partial class PreviewForm : Form {

        public bool CanClose { get; set; }
        
        public PreviewForm() {
            InitializeComponent();
            init();
        }

        private void init() {
            CanClose = false;

            // load all settings
            loadSettings();
        }

        private void terminate() {
            // save settings
            saveSettings();
        }

        private void loadSettings() {
            // load position & size of this window
            Rectangle r;
            if (MainModule.Pref.GetRect(PrefConstants.kPreviewForm, out r)) {
                this.Bounds = r;
                // tell window not to change this position
                this.StartPosition = FormStartPosition.Manual;
            }
        }

        private void saveSettings() {
            // save position & size of this window
            MainModule.Pref.SetRect(PrefConstants.kPreviewForm, this.Bounds);

            // save settings
            MainModule.Pref.Save();
        }

        private void onFormClosing(object sender, FormClosingEventArgs e) {
            if (!CanClose) {
                // Simply hide it
                e.Cancel = true;
                this.Visible = false;
            } else {
                terminate();
            }
        }

    }
}
