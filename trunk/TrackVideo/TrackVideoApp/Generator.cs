
using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

using ReneNyffenegger;

namespace Alfray.TrackVideo.TrackVideoApp {

    public delegate void OnGeneratorUpdate(int frame, int maxFrame, Image image);

    public class Generator : IDisposable {

        private bool mThreadMustStop;

        private int mFps;
        private int mSx;
        private int mSy;
        private Size mPreviewSize;
        private TrackParser mTrackData;
        private string mDestFilename;

        private AviWriter mAviWriter;
        private Bitmap mAviBmp;

        private Thread mThread;
        public event OnGeneratorUpdate mOnUpdateEvent;

        public Generator(int fps,
                        int sx,
                        int sy,
                        Size previewSize,
                        TrackParser trackData,
                        string destFilename) {
            mFps = fps;
            mSx  = sx;
            mSy  = sy;
            mPreviewSize  = previewSize;
            mTrackData    = trackData;
            mDestFilename = destFilename;
        }

        public void Start() {
            System.Diagnostics.Debug.Assert(mAviWriter == null);

            mAviWriter = new AviWriter();
            mAviBmp = mAviWriter.Open(mDestFilename, (uint)mFps, mSx, mSy);

            mThreadMustStop = false;

            mThread = new Thread(threadEntryPoint);
            mThread.Start();
        }

        public void Stop() {
            if (mThread != null) {
                mThreadMustStop = true;
                mThread.Join();
                mThread = null;
            }
        }

        public void Dispose() {
            mAviBmp = null;
            if (mAviWriter != null) mAviWriter.Dispose();
        }

        private void threadEntryPoint() {
            int fps = mFps;
            int sx = mSx;
            int sy = mSy;
            TrackParser trackData = mTrackData;

            using (Graphics g = Graphics.FromImage(mAviBmp)) {
                using (Brush gray = new SolidBrush(Color.Gray),
                             yellow = new SolidBrush(Color.Yellow)) {

                    // Test: 10 seconds at the given fps
                    int nbFrames = 10 * (int)fps;

                    for (int frame = 0, updateFps = 0; frame < nbFrames; frame++, updateFps++) {

                        double progress = (double)frame / (double)nbFrames;

                        g.FillRectangle(gray, 0, 0, sx, sy);

                        int x = (int)(20 + (sx - 40) * progress);
                        int y = (int)(sy - 20 - (sy - 40) * progress);

                        g.FillEllipse(yellow, x - 20, y - 20, 40, 40);

                        mAviWriter.AddFrame();

                        if (updateFps == fps) updateFps = 0;
                        if (updateFps == 0 && mPreviewSize != null) {
                            // Update status, progress, etc.
                            raiseUpdateEvent(frame, nbFrames, new Bitmap(mAviBmp, mPreviewSize));
                        }

                        if (mThreadMustStop) break;
                    }

                    // make sure to tell owner that the generator is done
                    raiseUpdateEvent(nbFrames, nbFrames, new Bitmap(mAviBmp, mPreviewSize));
                }
            } // using Graphics

            Dispose();
        }

        internal void raiseUpdateEvent(int frame, int maxFrame, Image image) {
            if (mOnUpdateEvent != null) {
                // *asynhronous* thread-safe event call
                // (will invoke the method from the main form in the context of the main thread)
                object[] args = { frame, maxFrame, image };
                MainModule.MainForm.BeginInvoke(mOnUpdateEvent, args);
            }
        }
    }
}
