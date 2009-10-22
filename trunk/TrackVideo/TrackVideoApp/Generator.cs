
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
        private TrackParser mTrackData;
        private string mDestFilename;

        private const int kTrackBorder = 5;
        private const int kTrackWidth = 5;
        private const int kTrackPosRadius = 5;

        /// <summary>
        /// All access to the AVI Writer must be done from the main UI thread
        /// </summary>
        private AviWriter mAviWriter;
        private Bitmap mAviBmp;

        private Thread mThread;
        public OnGeneratorUpdate mOnUpdateCallback;

        private delegate void AddFrame();
        private AddFrame mAddFrameFunc;

        private CPointF mTempPointF = new CPointF();

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

        private class CPointF {
            public float mX;
            public float mY;
        }

        /// <summary>
        /// Creates a new generator.
        /// Note that for a preview you just use destFilename=null.
        /// </summary>
        /// <param name="fps">The desired frames per second, e.g. 30</param>
        /// <param name="movieSx">Movie container width in pixels</param>
        /// <param name="movieSy">Movie container height in pixels</param>
        /// <param name="trackSx">Track rendering width in pixels</param>
        /// <param name="trackSy">Track rendering height in pixels</param>
        /// <param name="trackData">Track data</param>
        /// <param name="destFilename">AVI destination filename. Null for a preview.</param>
        public Generator(int fps,
                        int movieSx,
                        int movieSy,
                        int trackSx,
                        int trackSy,
                        TrackParser trackData,
                        string destFilename) {
            mFps = fps;
            mMovieSx = movieSx;
            mMovieSy = movieSy;
            mTrackSx = trackSx;
            mTrackSy = trackSy;
            mTrackData    = trackData;
            mDestFilename = destFilename;
        }

        /// <summary>
        /// Starts rendering in a thread. Each StartAsync() call *must* be balanced
        /// by a Dispose() call or bad things will happen.
        /// </summary>
        public void StartAsync() {
            System.Diagnostics.Debug.Assert(mAviWriter == null);
            System.Diagnostics.Debug.Assert(mThread == null);
            if (mThread != null) return;  // for sanity

            mAddFrameFunc = doAddFrame;
            if (mDestFilename != null) {
                mAviWriter = new AviWriter();
                mAviBmp = mAviWriter.Open(mDestFilename, (uint)mFps, mMovieSx, mMovieSy);
            } else {
                mAviBmp = new Bitmap(mMovieSx, mMovieSy);
            }

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
                    using (Font labelFont = prepareText(bgRect, out textPos),
                                numFont = new Font(FontFamily.GenericMonospace, labelFont.Size, FontStyle.Bold)) {

                        Interpolator interp = new Interpolator(td);

                        double invFps = 1 / (double)fps;

                        bool userStopRequested = false;

                        for (int frame = 0, updateFps = 0;
                             frame < nbFrames && !userStopRequested;
                             frame++, updateFps++) {

                            double currTime = (double)frame * invFps;

                            TrackParser.Sample currSample = interp.GoTo(currTime);

                            double currLapTime = interp.CurrLapTime;

                            // keep track of current dot & interpolate between dots

                            // draw background

                            g.FillRectangle(chromaColor, 0, 0, msx, msy);

                            g.FillRectangle(bgColor, bgRect);

                            // draw track + current pos
                            drawTrackPos(g, posColor, trackRect, trackBmp, trackProj,
                                currSample.mLongtiude, currSample.mLatitude);

                            // draw bearing

                            // TODO

                            // draw text
                            drawText(g, textColor, labelFont, numFont, textPos,
                                currSample.mSpeed, currSample.mAccel, currSample.mLateralAccel,
                                currTime,
                                currLapTime, interp.CurrLapIndex, interp.LastLapDuration);

                            // finally dump frame and update preview/progress
                            MainModule.MainForm.Invoke(mAddFrameFunc);

                            if (updateFps == fps) updateFps = 0;
                            if (updateFps == 0) {
                                // Update status, progress, etc.
                                userStopRequested = syncUpdate(frame, nbFrames, new Bitmap(mAviBmp));
                            }

                            if (mThreadMustStop) break;
                        }
                    } // using Font

                    // Make sure to tell owner that the generator is done
                    // This one must be async -- this thread will quit and the owner will
                    // try to join to wait for the thread to finish.
                    asyncUpdate(nbFrames, nbFrames, new Bitmap(mAviBmp));
                } // using Brush
            } // using Graphics
        }

        private double interpolate(double value1, double value2, double progress) {
            return value1 + (value2 - value1) * progress;
        }


        // --- Update & threading ----

        // this is invoked in the main UI thread from the main loop
        private void doAddFrame() {
            if (mAviWriter != null) mAviWriter.AddFrame();
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
            if (!td.HasSamples) {
                trackRect = Rectangle.Empty;
                return null;
            }

            double minLong = Double.PositiveInfinity,
                   maxLong = Double.NegativeInfinity,
                   minLat  = Double.PositiveInfinity,
                   maxLat  = Double.NegativeInfinity;

            foreach (TrackParser.Sample d in td.Samples) {
                minLong = Math.Min(minLong, d.mLongtiude);
                maxLong = Math.Max(maxLong, d.mLongtiude);
                minLat  = Math.Min(minLat , d.mLatitude);
                maxLat  = Math.Max(maxLat , d.mLatitude);
            }

            // we want to map the min..maxLong(+X)..Lat(-Y) to the given rect
            // rect top left corner (it's x/y base) corresponds to minLong/maxLat.

            float x = rect.X;
            float y = rect.Y;
            float w = rect.Width;
            float h = rect.Height;

            // currently use a square part of the dest rect
            if (w > h) w = h;
            else h = w;

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
            CPointF outPoint) {
            double px = (longitude - proj.mGpsOffsetX) * proj.mGpsScaleX + proj.mPixelOffsetX;
            double py = (latitude - proj.mGpsOffsetY) * proj.mGpsScaleY + proj.mPixelOffsetY;
            outPoint.mX = (float)px;
            outPoint.mY = (float)py;
        }

        private Bitmap prepareTrackBmp(Rectangle trackRect, Gps2PixelProjection trackProj, TrackParser td) {
            // We need at least a lap to draw something
            if (!td.HasSamples || trackProj == null) return null;

            // We'll draw one of the laps. Just get the first lap.
            TrackParser.Lap currLap = td.Laps[0];

            int w = trackRect.Width;
            int h = trackRect.Height;
            Bitmap bmp = new Bitmap(w, h);

            using (Graphics g = Graphics.FromImage(bmp)) {
                using (Brush bgColor = new SolidBrush(Color.Gray),
                             trackColor = new SolidBrush(Color.Yellow)) {
                    using (Pen trackPen = new Pen(trackColor, kTrackWidth)) {

                        g.FillRectangle(bgColor, 0, 0, w, h);

                        List<TrackParser.Sample> samples = td.Samples;
                        TrackParser.Sample s = samples[0];
                        CPointF last = new CPointF();
                        CPointF next = new CPointF();

                        transformGpsCoord(s.mLongtiude, s.mLatitude, trackProj, last);

                        float px = (float) trackProj.mPixelOffsetX;
                        float py = (float) trackProj.mPixelOffsetY;

                        last.mX -= px;
                        last.mY -= py;

                        for (int i = 1, n = samples.Count; i < n; i++) {
                            s = samples[i];
                            if (s.mLap != currLap) continue;

                            transformGpsCoord(s.mLongtiude, s.mLatitude, trackProj, next);
                            next.mX -= px;
                            next.mY -= py;

                            g.DrawLine(trackPen, last.mX, last.mY, next.mX, next.mY);

                            last.mX = next.mX;
                            last.mY = next.mY;
                        }
                    }
                }
            }

            return bmp;
        }

        private void drawTrackPos(Graphics g, Brush posColor,
            Rectangle trackRect, Bitmap trackBmp,
            Gps2PixelProjection trackProj,
            double longitude, double latitude) {

            if (trackProj == null) return;

            g.DrawImageUnscaled(trackBmp, trackRect);

            transformGpsCoord(longitude, latitude, trackProj, mTempPointF);

            g.FillEllipse(posColor,
                mTempPointF.mX - kTrackPosRadius, mTempPointF.mY - kTrackPosRadius,
                kTrackPosRadius * 2, kTrackPosRadius * 2);
        }


        // --- Text Stats ---

        private Font prepareText(Rectangle rect, out PointF[] textPos) {
            textPos = new PointF[12];

            int x = rect.X;
            int y = rect.Y;
            int w = rect.Width;
            int h = rect.Height;

            int tx = x + w * 2 / 5;         // beginning of text in tsx
            int xh = w * 3 / 5 / 2;         // width of the two half columns
            int xl = xh * 1 / 3;            // label is 1/3 of each column

            int ty = y;                     // text starts at top of tsy
            int yl = h / 2 / 3;             // 3 lines use the upper half of tsy

            Font f = new Font(FontFamily.GenericSansSerif, (float)(yl * 0.4), FontStyle.Bold);
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

        private void drawText(Graphics g, Brush textColor,
            Font labelFont, Font numFont,
            PointF[] textPos,
            double speed, double accel, double latAccel,
            double time, double lapTime, int lapNum, double lastLap) {

            int k = 0;

            //--
            g.DrawString("Speed", labelFont, textColor, textPos[k++]);

            string s = String.Format("{0,3:f0} mph", ms2mph(speed));
            g.DrawString(s, numFont, textColor, textPos[k++]);

            //--
            g.DrawString("Time", labelFont, textColor, textPos[k++]);

            s = formatTime(time);
            g.DrawString(s, numFont, textColor, textPos[k++]);

            //--
            g.DrawString("Accel", labelFont, textColor, textPos[k++]);

            s = String.Format("{0,6:f3} g", accel);
            g.DrawString(s, numFont, textColor, textPos[k++]);

            //--
            s = String.Format("Lap {0}", lapNum + 1);
            g.DrawString(s, labelFont, textColor, textPos[k++]);

            s = formatTime(lapTime);
            g.DrawString(s, numFont, textColor, textPos[k++]);

            //--
            g.DrawString("Lat", labelFont, textColor, textPos[k++]);

            s = String.Format("{0,6:f3} g", latAccel);
            g.DrawString(s, numFont, textColor, textPos[k++]);

            //--
            g.DrawString("Last", labelFont, textColor, textPos[k++]);

            s = lastLap < 0 ? "--" : formatTime(lastLap);
            g.DrawString(s, numFont, textColor, textPos[k++]);
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
