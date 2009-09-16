
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

    /// <summary>
    /// Video overlay generator.
    /// </summary>
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

        private static const int kTrackBorder = 5;
        private static const int kTrackWidth = 5;

        /// <summary>
        /// All access to the AVI Writer must be done from the main UI thread
        /// </summary>
        private AviWriter mAviWriter;
        private Bitmap mAviBmp;

        private Thread mThread;
        public OnGeneratorUpdate mOnUpdateCallback;

        private delegate void AddFrame();
        private AddFrame mAddFrame;

        /// <summary>
        /// Projection to convert a gps coordinate in a pixel.
        /// We'll simplify and naively assume the gps coordinates are "flat" and "square".
        /// So pixel = (gps_coord - gps_offset) * gps_scale + pixel_offset;
        /// </summary>
        private class Gps2PixelProjection {
            public double mGpsOffsetX;
            public double mGpsOffsetY;
            public double mGpsScaleX;
            public double mGpsScaleY;
            public double mPixelOffsetX;
            public double mPixelOffsetY;
        }

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

        /// <summary>
        /// Starts rendering in a thread. Each Start() call *must* be balanced
        /// by a Dispose() call or bad things will happen.
        /// </summary>
        public void Start() {
            System.Diagnostics.Debug.Assert(mAviWriter == null);
            System.Diagnostics.Debug.Assert(mThread == null);
            if (mThread != null) return;  // for sanity

            mAddFrame = doAddFrame;
            mAviWriter = new AviWriter();
            mAviBmp = mAviWriter.Open(mDestFilename, (uint)mFps, mMovieSx, mMovieSy);

            mThreadMustStop = false;

            mThread = new Thread(threadEntryPoint);
            mThread.Start();
        }

        /// <summary>
        /// Stops the rendering thread, if present.
        /// Disposes the AVI writer resources.
        /// </summary>
        public void Dispose() {
            if (mThread != null) {
                mThreadMustStop = true;
                mThread.Join();
                mThread = null;
            }

            mAviBmp = null;
            if (mAviWriter != null) {
                mAviWriter.Dispose();
                mAviWriter = null;
            }
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
                             posColor = new SolidBrush(Color.Red),
                             textColor = new SolidBrush(Color.Orange)) {

                    Rectangle bgRect = new Rectangle(msx - tsx, msy - tsy, tsx, tsy);

                    int nbFrames = (int)(mTrackData.TotalTime * fps);

                    // prepare track drawing (map GPS coord => screen coord: offset + scale)
                    Rectangle trackRect;
                    Gps2PixelProjection trackProj = prepareTrackProj(bgRect, td, out trackRect);
                    Bitmap trackBmp = prepareTrackBmp(trackRect, trackProj, td);

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

                            drawTrackPos(g, trackRect, trackBmp, currGpsLong, currGpsLat);

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
                    asyncUpdate(nbFrames, nbFrames, new Bitmap(mAviBmp, mPreviewSize));
                } // using Brush
            } // using Graphics
        }

        private double interpolate(double value1, double value2, double progress) {
            return value1 + (value2 - value1) * progress;
        }


        // --- Update & threading ----

        // this is invoked in the main UI thread from the main loop
        private void doAddFrame() {
            mAviWriter.AddFrame();
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
                MainModule.MainForm.BeginInvoke(mOnUpdateCallback, args);
            }
        }


        // --- GPS to track rect & bitmap ---

        private Gps2PixelProjection prepareTrackProj(Rectangle rect, TrackParser td, out Rectangle trackRect) {
            // we need at least one point to do something
            if (td.Laps.Count == 0 || td.Laps[0].Dots.Count == 0) {
                trackRect = Rectangle.Empty;
                return null;
            }

            double minLong = Double.PositiveInfinity,
                   maxLong = Double.NegativeInfinity,
                   minLat  = Double.PositiveInfinity,
                   maxLat  = Double.NegativeInfinity;

            foreach (TrackParser.Lap l in td.Laps) {
                foreach (TrackParser.Dot d in l.Dots) {
                    minLong = Math.Min(minLong, d.mLongtiude);
                    maxLong = Math.Max(maxLong, d.mLongtiude);
                    minLat  = Math.Min(minLat , d.mLatitude);
                    maxLat  = Math.Max(maxLat , d.mLatitude);
                }
            }

            // we want to map the min..maxLong(+X)..Lat(-Y) to the given rect
            // rect top left corner (it's x/y base) corresponds to minLong/maxLat.

            float x = rect.X;
            float y = rect.Y;
            float w = rect.Width;
            float h = rect.Height;

            // currently use a square part of the dest rect based on its height
            w = h;

            // adjust for track border
            x += kTrackBorder;
            y += kTrackBorder;
            w -= 2 * kTrackBorder;
            h -= 2 * kTrackBorder;

            trackRect = new Rectangle((int)x, (int)y, (int)w, (int)h);

            // Compute offset and scaling to transform a coord point into a screen point

            Gps2PixelProjection proj = new Gps2PixelProjection();

            proj.mGpsOffsetX = minLong;
            proj.mGpsOffsetY = maxLat;

            proj.mPixelOffsetX = x;
            proj.mPixelOffsetY = y;

            proj.mGpsScaleX = (maxLong > minLong) ? w / (maxLong - minLong) : 0;
            proj.mGpsScaleY = (maxLat > minLat) ? h / (minLat - maxLat) : 0;

            return proj;
        }

        private void transformGpsCoord(double longitude, double latitude,
            Gps2PixelProjection proj,
            PointF outPoint) {
            double px = (longitude - proj.mGpsOffsetX) * proj.mGpsScaleX + proj.mPixelOffsetX;
            double py = (latitude - proj.mGpsOffsetY) * proj.mGpsScaleY + proj.mPixelOffsetY;
            outPoint.X = (float)px;
            outPoint.Y = (float)py;
        }

        private Bitmap prepareTrackBmp(Rectangle trackRect, Gps2PixelProjection trackProj, TrackParser td) {
            // We need at least a lap to draw something
            if (td.Laps.Count == 0) return null;

            // We'll draw one of the laps. We try to avoid the first and the last one and we try to pick
            // the lap with the most dots.
            TrackParser.Lap currLap = null;
            int maxDots = -1;
            for (int i = 0, n = td.Laps.Count; i < n; i++) {
                // remap [0..n-3..n-2..n-1] to [1..n-2..0..n-1], that is we start at the second track
                // and we do process the first track just before the last one.
                int j = i + 1;
                if (j == n - 1) j = 0;
                else if (j == n) j = n - 1;

                TrackParser.Lap l = td.Laps[j];
                int nd = l.Dots.Count;
                if (nd > n) {
                    currLap = l;
                    maxDots = nd;
                }
            }

            // We should have a lap now. But it should also have more than one point to be useful.
            System.Diagnostics.Debug.Assert(currLap != null);
            if (currLap == null && maxDots > 1) return null;

            int w = trackRect.Width;
            int h = trackRect.Height;
            Bitmap bmp = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(bmp)) {
                using (Brush bgColor = new SolidBrush(Color.Gray),
                             trackColor = new SolidBrush(Color.Yellow)) {
                    using (Pen trackPen = new Pen(trackColor, kTrackWidth)) {
 
                        g.FillRectangle(bgColor, 0, 0, w, h);

                        List<TrackParser.Dot> dots = currLap.Dots;
                        TrackParser.Dot d = dots[0];
                        PointF last = new PointF();
                        PointF next = new PointF();

                        transformGpsCoord(d.mLongtiude, d.mLatitude, trackProj, last);

                        for (int i = 1, n = dots.Count; i <= n; i++) {
                            d = dots[i == n ? 0 : i];
                            transformGpsCoord(d.mLongtiude, d.mLatitude, trackProj, next);

                            g.DrawLine(trackPen, last, next);

                            last.X = next.X;
                            last.Y = next.Y;
                        }
                    }
                }
            }

            return bmp;
        }

        private void drawTrackPos(Graphics g, Rectangle trackRect, Bitmap trackBmp, double currGpsLong, double currGpsLat) {

            Color backColor = trackBmp.GetPixel(0, 0);
            trackBmp.MakeTransparent(backColor);

            g.DrawImageUnscaled(trackBmp, trackRect);


            TODO continue here
        }


        // --- Text Stats ---

        private Font prepareText(Rectangle rect, out PointF[] textPos) {
            textPos = new PointF[12];

            int x = rect.X;
            int y = rect.Y;
            int w = rect.Width;
            int h = rect.Height;

            int tx = x + w * 2 / 5;         // beginning of text in tsx
            int xh = w * 3 / 5 / 2;     // width of the two half columns
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
