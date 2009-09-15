﻿
using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

using ReneNyffenegger;

namespace Alfray.TrackVideo.TrackVideoApp {

    /// <summary>
    /// Event invoked synchronously on the main UI thread to update the UI. It is invoked
    /// once per second of video processed. Frame is guaranteed to be 0 for the init and to
    /// be maxFrame for the last call. Image could be null.
    /// Should return True if the user requested the generator to abort.
    /// </summary>
    public delegate bool OnGeneratorUpdate(int frame, int maxFrame, Image image);

    public class Generator : IDisposable {

        private bool mThreadMustStop;

        private int mFps;
        private int mMovieSx;
        private int mMovieSy;
        private int mTrackSx;
        private int mTrackSy;
        private Size mPreviewSize;
        private TrackParser mTrackData;
        private string mDestFilename;

        /// <summary>
        /// All access to the AVI Writer must be done from the main UI thread
        /// </summary>
        private AviWriter mAviWriter;
        private Bitmap mAviBmp;

        private Thread mThread;
        public OnGeneratorUpdate mOnUpdateCallback;

        private delegate void AddFrame();
        private AddFrame mAddFrame;

        public Generator(int fps,
                        int movieSx,
                        int movieSy,
                        int trackSx,
                        int trackSy,
                        Size previewSize,
                        TrackParser trackData,
                        string destFilename) {
            mFps = fps;
            mMovieSx = movieSx;
            mMovieSy = movieSy;
            mTrackSx = trackSx;
            mTrackSy = trackSy;
            mPreviewSize = previewSize;
            mTrackData    = trackData;
            mDestFilename = destFilename;
        }

        public void Start() {
            System.Diagnostics.Debug.Assert(mAviWriter == null);

            mAddFrame = doAddFrame;
            mAviWriter = new AviWriter();
            mAviBmp = mAviWriter.Open(mDestFilename, (uint)mFps, mMovieSx, mMovieSy);

            mThreadMustStop = false;

            mThread = new Thread(threadEntryPoint);
            mThread.Start();
        }

        public void Stop() {
            Thread t = null;
            lock (this) {
                t = mThread;
                mThread = null;
                if (t != null) mThreadMustStop = true; ;
            }
            if (t != null) t.Join();
        }

        public void Dispose() {
            mAviBmp = null;
            if (mAviWriter != null) mAviWriter.Dispose();
        }

        private void threadEntryPoint() {
            int fps = mFps;
            int msx = mMovieSx;
            int msy = mMovieSy;
            int tsx = mTrackSx;
            int tsy = mTrackSy;
            TrackParser td = mTrackData;

            using (Graphics g = Graphics.FromImage(mAviBmp)) {
                using (Brush chromaColor = new SolidBrush(Color.Blue),
                             bgColor = new SolidBrush(Color.Gray),
                             trackColor = new SolidBrush(Color.Yellow),
                             posColor = new SolidBrush(Color.Red),
                             textColor = new SolidBrush(Color.Orange)) {

                    Rectangle bgRect = new Rectangle(msx - tsx, msy - tsy, tsx, tsy);

                    int nbFrames = (int)(mTrackData.TotalTime * fps);

                    // prepare track drawing (map GPS coord => screen coord: offset + scale)

                    // prepare g-meter pos

                    // prepare text positions

                    PointF[] textPos;
                    using (Font textFont = prepareText(bgRect, out textPos)) {

                        double currSpeed = 0,
                               currAccel = 0,
                               currLatAccel = 0,
                               currGpsLat = 0,
                               currGpsLong = 0,
                               currTime = 0,
                               currLapTime = 0,
                               currLastLap = -1;    // no "last lap" time at first
                        int currLapNum = 0;

                        TrackParser.Lap lap = null;
                        int maxLapNum = td.Laps.Count;
                        double nextLapFrame = 0;
                        double startLapFrame = 0;

                        TrackParser.Dot currDot = null;
                        TrackParser.Dot nextDot = null;
                        int dotIndex = 0;
                        int maxDotIndex = 0;
                        double startDotFrame = 0;
                        double nextDotFrame = 0;
                        double deltaDotFrame = 0;

                        double invFps = 1 / (double)fps;

                        bool userStopRequested = false;

                        for (int frame = 0, updateFps = 0;
                             frame < nbFrames && !userStopRequested;
                             frame++, updateFps++) {

                            // keep track of current lap
                            if (frame >= nextLapFrame && currLapNum < maxLapNum) {
                                if (lap != null) {
                                    currLapTime = lap.LapTime;
                                    currLapNum++;
                                }
                                lap = td.Laps[currLapNum];
                                startLapFrame = frame;
                                dotIndex = 0;
                                maxDotIndex = lap.Dots.Count;
                                nextDotFrame = startLapFrame;
                                if (currLapNum < maxLapNum) {
                                    // get the absolute next start time (to avoid precision errors)
                                    nextLapFrame = td.Laps[currLapNum + 1].StartTime * fps;
                                } else {
                                    // no more lap after
                                    nextLapFrame = nbFrames + 1;
                                }
                            }

                            currTime = (double)frame * invFps;
                            currLapTime = (double)(frame - startLapFrame) * invFps;

                            if (frame >= nextDotFrame && dotIndex < maxDotIndex) {
                                currDot = lap.Dots[dotIndex++];
                                startDotFrame = frame;

                                if (dotIndex < maxDotIndex) {
                                    nextDot = lap.Dots[dotIndex];
                                    nextDotFrame = startLapFrame + nextDot.mElapsedTime * fps;
                                } else {
                                    // no more dot for this lap
                                    nextDot = currDot;
                                    nextDotFrame = nextLapFrame;
                                }

                                deltaDotFrame = 1 / (nextDotFrame - startDotFrame);
                            }

                            // compute the "progress" inside this dot and interpolate values
                            double progress = 0;
                            if (currDot != nextDot && nextDotFrame > startDotFrame) {
                                progress = (frame - startDotFrame) * deltaDotFrame;
                            }

                            currSpeed = interpolate(currDot.mSpeed, nextDot.mSpeed, progress);
                            currAccel = interpolate(currDot.mAccel, nextDot.mAccel, progress);
                            currLatAccel = interpolate(currDot.mLateralAccel, nextDot.mLateralAccel, progress);
                            currGpsLat = interpolate(currDot.mLatitude, nextDot.mLatitude, progress);
                            currGpsLong = interpolate(currDot.mLongtiude, nextDot.mLongtiude, progress);
                               
                            // keep track of current dot & interpolate between dots

                            // draw background

                            g.FillRectangle(chromaColor, 0, 0, msx, msy);

                            g.FillRectangle(bgColor, bgRect);

                            // draw track + current pos

                            // draw text
                            drawText(g, textColor, textFont, textPos,
                                currSpeed, currAccel, currLatAccel, currTime,
                                currLapTime, currLapNum, currLastLap);


                            //--g.FillEllipse(textColor, x - 20, y - 20, 40, 40);

                            // finally dump frame and update preview/progress
                            MainModule.MainForm.Invoke(mAddFrame);

                            if (updateFps == fps) updateFps = 0;
                            if (updateFps == 0 && mPreviewSize != null) {
                                // Update status, progress, etc.
                                userStopRequested = syncUpdate(frame,
                                    nbFrames,
                                    new Bitmap(mAviBmp, mPreviewSize));
                            }

                            if (mThreadMustStop) break;
                        }
                    } // using Font

                    // Make sure to tell owner that the generator is done
                    // This one must be async -- this thread will quit and the owner will
                    // try to join to wait for the thread to finish.
                    lock (this) {
                        mThread = null;
                    }
                    asyncUpdate(nbFrames, nbFrames, new Bitmap(mAviBmp, mPreviewSize));
                } // using Brush
            } // using Graphics

            Dispose();
        }

        // this is invoked in the main UI thread just above
        private void doAddFrame() {
            mAviWriter.AddFrame();
        }

        private double interpolate(double value1, double value2, double progress) {
            return value1 + (value2 - value1) * progress;
        }

        private void drawText(Graphics g, Brush textColor, Font textFont, PointF[] textPos,
            double speed, double accel, double latAccel,
            double time, double lapTime, int lapNum, double lastLap) {

            int k = 0;

            //--
            g.DrawString("Speed", textFont, textColor, textPos[k++]);

            string s = String.Format("{0,3:f0} mph", ms2mph(speed));
            g.DrawString(s, textFont, textColor, textPos[k++]);

            //--
            g.DrawString("Time", textFont, textColor, textPos[k++]);

            s = formatTime(time);
            g.DrawString(s, textFont, textColor, textPos[k++]);

            //--
            g.DrawString("Accel", textFont, textColor, textPos[k++]);

            s = String.Format("{0,6:f3} g", accel);
            g.DrawString(s, textFont, textColor, textPos[k++]);

            //--
            g.DrawString("Lap", textFont, textColor, textPos[k++]);

            s = formatTime(lapTime) + String.Format(" (#{0})", lapNum);
            g.DrawString(s, textFont, textColor, textPos[k++]);

            //--
            g.DrawString("Lat", textFont, textColor, textPos[k++]);

            s = String.Format("{0,6:f3} g", latAccel);
            g.DrawString(s, textFont, textColor, textPos[k++]);

            //--
            g.DrawString("Last", textFont, textColor, textPos[k++]);

            s = lastLap < 0 ? "--" : formatTime(lastLap);
            g.DrawString(s, textFont, textColor, textPos[k++]);
        }

        private string formatTime(double time) {
            int min = (int)(time / 60);
            time -= min * 60;
            int sec = (int)time;
            int centis = (int)(100 * (time - sec));

            return String.Format("{0,2:d}:{1,2:d}.{2,2:d}", min, sec, centis);
        }

        private Font prepareText(Rectangle rect, out PointF[] textPos) {
            textPos = new PointF[12];

            int x = rect.X;
            int y = rect.Y;
            int w = rect.Width;
            int h = rect.Height;

            int tx = x + w * 2 / 5;         // beginning of text in tsx
            int xh =     w * 3 / 5 / 2;     // width of the two half columns
            int xl = xh * 1 / 3;            // label is 1/3 of each column

            int ty = y;                     // text starts at top of tsy
            int yl = h / 2 / 3;             // 3 lines use the upper half of tsy

            Font f = new Font(FontFamily.GenericSansSerif, (float)(yl * 0.4));
            int fh = f.Height / 2;

            int k = 0;
            for (int j = 0; j < 3; j++) {
                for (int i = 0; i < 2; i++) {
                    PointF p = new PointF(tx + xh * i, ty + yl * j + fh);
                    textPos[k++] = p;

                    p = new PointF(p.X + xl, p.Y);
                    textPos[k++] = p;
                }
            }

            return f;
        }

        /// <summary>
        /// Invokes the update synchronously in the context of the main UI thread.
        /// </summary>
        private bool syncUpdate(int frame, int maxFrame, Image image) {
            if (mOnUpdateCallback != null) {
                object[] args = { frame, maxFrame, image };
                
                object ret = MainModule.MainForm.Invoke(mOnUpdateCallback, args);
                
                if (ret is Boolean) {
                    return Convert.ToBoolean((Boolean)ret);
                }
            }

            return false;
        }

        /// <summary>
        /// Invokes the update event asynchronously in the context of the main UI thread.)
        /// </summary>
        private void asyncUpdate(int frame, int maxFrame, Image image) {
            if (mOnUpdateCallback != null) {
                // *asynhronous* thread-safe event call
                // (will invoke the method from the main form in the context of the main thread)
                object[] args = { frame, maxFrame, image };
                MainModule.MainForm.Invoke(mOnUpdateCallback, args);
            }
        }

        /// <summary>
        /// Converts m/s in mph
        /// </summary>
        private double ms2mph(double ms) {
            return ms / 0.44704;
        }

        /// <summary>
        /// Converts m/s in km/h
        /// </summary>
        private double ms2kmh(double ms) {
            return ms / 0.277777778;
        }

    }
}
