﻿//*******************************************************************
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
using System.IO;
using System.Xml;
using System.Data;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Alfray.LibUtils2.Misc;

using Ionic.Zip;
using ReneNyffenegger;
using System.Threading;

namespace Alfray.TrackVideo.TrackVideoApp {
    public partial class MainForm : Form, ILog {

        private DebugForm mDebugForm;
        private PrefForm mPrefForm;
        private bool mIsFirstFormActivated = true;
        private bool mRequestUserStop = false;
        private Generator mGenerator;
        private TrackParser mTrackData;

        private delegate void reloadTrackData();

        private class ListBoxDoubleBuffer : ListBox {
            public ListBoxDoubleBuffer() : base() {
                DoubleBuffered = true;
            }
        }

        public MainForm() {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // Add any constructor code after InitializeComponent call
            //

            init();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing) {

            stopGenerator();

            if (disposing) {
                if (components != null) {
                    components.Dispose();
                    components = null;
                }
            }

            base.Dispose(disposing);
        }

        #region RILog Members

        public void Log(object o) {
            Log(o.ToString());
        }

        public void Log(string s) {
            if (mDebugForm == null)
                createDebugWindow(false);
            if (mDebugForm != null)
                mDebugForm.Log(s);
        }

        #endregion

        public void ReloadPrefs() {
            reloadPrefs();
        }

        private void init() {

            // load all settings
            loadSettings();

            // apply defaults, update UI
            reloadPrefs();
        }

        /// <summary>
        /// Invoked when the window is closed by the user
        /// </summary>
        private void terminate() {

            stopGenerator();

            // close windows
            closePrefWindow();
            closeDebugWindow();

            // save settings
            saveSettings();
        }

        /// <summary>
        /// Loads settings specific to this window.
        /// Done only once when the window is created.
        /// </summary>
        private void loadSettings() {
            Pref p = MainModule.Pref;

            // load settings
            p.Load();

            // tell Windows not to change this position automatically
            this.StartPosition = FormStartPosition.Manual;

            // load position of this window
            Rectangle r;
            if (p.GetRect(PrefConstants.kMainForm, out r)) {
                // RM 20050307 No longer change the size of the window.
                // This is because the window cannot be resized manually,
                // instead it adapts to the size of the inner video preview.
                this.Location = r.Location;
            }

            // restore last dir path
            mEditTrackFilename.Text = p[PrefConstants.kLastKmxPath ];
            mEditDestFilename.Text  = p[PrefConstants.kLastDestPath];

            // restore generate options with default values
            mEditFps.Text     = p.getString(PrefConstants.kLastFps,  "30");
            mEditMovieSx.Text = p.getString(PrefConstants.kLastMovieSx, "1280");
            mEditMovieSy.Text = p.getString(PrefConstants.kLastMovieSy,  "720");
            mEditTrackSx.Text = p.getString(PrefConstants.kLastTrackSx,  "500");
            mEditTrackSy.Text = p.getString(PrefConstants.kLastTrackSy,  "200");

            // <insert other setting stuff here>
        }

        private void saveSettings() {

            Pref p = MainModule.Pref;

            // Save current dir path
            p[PrefConstants.kLastKmxPath ] = mEditTrackFilename.Text;
            p[PrefConstants.kLastDestPath] = mEditDestFilename.Text;

            // save position & size of this window
            p.SetRect(PrefConstants.kMainForm, this.Bounds);

            // save sizes
            p[PrefConstants.kLastFps] = mEditFps.Text;
            p[PrefConstants.kLastMovieSx] = mEditMovieSx.Text;
            p[PrefConstants.kLastMovieSy] = mEditMovieSy.Text;
            p[PrefConstants.kLastTrackSx] = mEditTrackSx.Text;
            p[PrefConstants.kLastTrackSy] = mEditTrackSy.Text;

            // save settings
            p.Save();
        }

        /// <summary>
        /// (Re)Loads app-wide preferences.
        /// Done anytime the user applies changes to the preference window
        /// or once at startup.
        /// </summary>
        private void reloadPrefs() {
            updateButtons();

            //--Log("Prefs reloaded");
        }

        private void createDebugWindow(bool visible) {
            if (mDebugForm == null) {
                mDebugForm = new DebugForm();
                mDebugForm.Show();
            }
        }

        private void closeDebugWindow() {
            if (mDebugForm != null) {
                mDebugForm.CanClose = true;
                mDebugForm.Close();
                mDebugForm = null;
            }
        }

        private void showHideDebugWindow() {
            if (mDebugForm == null)
                createDebugWindow(true);
            else
                mDebugForm.Visible = !mDebugForm.Visible;
        }

        private void createPrefWindow(bool visible) {
            if (mPrefForm == null) {
                mPrefForm = new PrefForm();
                mPrefForm.Show();
            }
        }

        private void closePrefWindow() {
            if (mPrefForm != null) {
                mPrefForm.CanClose = true;
                mPrefForm.Close();
                mPrefForm = null;
            }
        }

        private void showHidePrefWindow() {
            if (mPrefForm == null)
                createPrefWindow(true);
            else
                mPrefForm.Visible = !mPrefForm.Visible;
        }

        private void updateButtons() {

            mButtonReloadTrack.Enabled = File.Exists(mEditTrackFilename.Text);

            mButtonGenerate.Text = (mGenerator == null) ? "Start" : "Stop";
            mButtonGenerate.Enabled = mTrackData != null &&
                                      mEditDestFilename.Text != "";
            mProgressBar.Enabled = mButtonGenerate.Enabled;
        }

        /// <summary>
        /// Actions to perform the first time the form is activated,
        /// i.e. it has been created and has been made visible for the first time.
        /// We load settings and do some time-consuming inits that require some
        /// user feedback.
        /// </summary>
        private void onFormActivated() {
            if (mIsFirstFormActivated) {
                mIsFirstFormActivated = false;
                loadSettings();
                updateButtons();

                mLabelTrackDataLoadResults.Text = "Loading initial track data, please wait.";
                new Thread(delegate() {
                    Thread.Sleep(100 /*ms*/);
                    MainModule.MainForm.BeginInvoke(new reloadTrackData(onReloadTrack));
                }).Start();
            }
        }

        private void onBrowseTrack() {
            OpenFileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = mEditTrackFilename.Text;
            fd.Filter = "Google Earth files (.kml, kmz)|*.km?|All files (*.*)|*.*";
            fd.FilterIndex = 1;

            if (fd.ShowDialog() == DialogResult.OK) {
                if (File.Exists(fd.FileName)) {
                    mEditTrackFilename.Text = fd.FileName;
                    onReloadTrack();
                }
            }

            updateButtons();
        }

        private void onReloadTrack() {
            parseKmxOrKmz(mEditTrackFilename.Text);
            updateButtons();
        }

        private void onBrowseDest() {
            SaveFileDialog fd = new SaveFileDialog();
            fd.InitialDirectory = mEditDestFilename.Text;
            fd.Filter = "AVI files (.avi)|*.avi|All files (*.*)|*.*";
            fd.FilterIndex = 1;

            if (fd.ShowDialog() == DialogResult.OK) {
                mEditDestFilename.Text = fd.FileName;
            }

            updateButtons();
        }

        private void onGenerate() {
            if (mGenerator != null) {
                mRequestUserStop = true;
                return;
            }

            String dest = mEditDestFilename.Text;
            if (File.Exists(dest)) {
                if (MessageBox.Show(
                        mButtonGenerate,
                        String.Format("File {0} already exists. Do you want to overwrite it?", dest),
                        "Generate Movie",
                        MessageBoxButtons.YesNo) == DialogResult.No) {
                    return;
                }                    
            }

            mStatusBar.Text = "Parsing KML...";
            TrackParser trackData = mTrackData;

            mGenerator = new Generator(
                    Convert.ToInt32(mEditFps.Text),
                    Convert.ToInt32(mEditMovieSx.Text),
                    Convert.ToInt32(mEditMovieSy.Text),
                    Convert.ToInt32(mEditTrackSx.Text),
                    Convert.ToInt32(mEditTrackSy.Text),
                    mPreviewPicture.Size,
                    trackData,
                    dest);
            mGenerator.mOnUpdateCallback = onUpdateCallback;
            mRequestUserStop = false;
            mGenerator.Start();

            updateButtons();
        }

        private void stopGenerator() {
            if (mGenerator != null) {
                mGenerator.Dispose();
                mGenerator = null;
            }
        }

        private bool onUpdateCallback(int frame, int maxFrame, Image image) {
            if (frame == 0) {
                // This signals the first call of the generator
                mStatusBar.Text = "Generating video...";
                mProgressBar.Maximum = maxFrame;
            } else if (frame == maxFrame) {
                // This signals the generator is done.
                mStatusBar.Text = "Done";
                stopGenerator();
                updateButtons();
            } else {
                mStatusBar.Text = String.Format("{0}/{1}", frame, maxFrame);
            }

            mProgressBar.Value = frame;

            if (image != null) mPreviewPicture.Image = image;

            return mRequestUserStop;
        }

        /// <summary>
        /// Loads the given KMZ or KMX and store it in mTrackData.
        /// </summary>
        private void parseKmxOrKmz(string kmxPath) {
            mTrackData = null;

            try {
                XmlDocument doc = new XmlDocument();

                if (ZipFile.IsZipFile(kmxPath)) {
                    using (ZipFile zf = ZipFile.Read(kmxPath)) {
                        foreach (ZipEntry ze in zf) {
                            // We take the first name that ends with .kmx
                            if (ze.FileName.EndsWith(".kml")) {
                                mLabelTrackDataLoadResults.Text = "Parsing " + ze.FileName;
                                Application.DoEvents();

                                MemoryStream ms = new MemoryStream();
                                ze.Extract(ms);
                                ms.Seek(0, SeekOrigin.Begin);

                                doc.Load(ms);
                                mTrackData = new TrackParser(doc);
                            }
                        }
                    }
                } else {
                    // Read the kml file directly
                    mLabelTrackDataLoadResults.Text = "Parsing " + Path.GetFileName(kmxPath);
                    Application.DoEvents();

                    doc.Load(kmxPath);
                    mTrackData = new TrackParser(doc);
                }

                mLabelTrackDataLoadResults.Text = mTrackData.Summary;
            } catch (FileNotFoundException) {
                mLabelTrackDataLoadResults.Text = "File not found";
            } catch (Exception e) {
                mLabelTrackDataLoadResults.Text = "Error: " + e.Message;
                Log("Failed to load " + kmxPath + ": " + e.Message + "\n" + e.StackTrace);
            }

            mStatusBar.Text = null;
        }
    }
}