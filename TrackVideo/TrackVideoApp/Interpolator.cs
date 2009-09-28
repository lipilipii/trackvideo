using System;
using System.Collections.Generic;
using System.Text;

namespace Alfray.TrackVideo.TrackVideoApp {
    class Interpolator {

        private TrackParser mTrackData;

        private TrackParser.Sample mCurrInterpol;

        private int mCurrIndex = -1;
        private TrackParser.Sample mCurrSample;

        public Interpolator(TrackParser trackData) {
            mTrackData = trackData;

            mCurrInterpol = new TrackParser.Sample();
        }

        public TrackParser.Sample Value {
            get {
                return mCurrInterpol;
            }
        }

        public TrackParser.Sample GoTo(double time) {

            if (mCurrSample == null ||
                    time < mCurrSample.mAbsStartTime ||
                    (!Double.IsNaN(mCurrSample.mAbsEndTime) && time >= mCurrSample.mAbsEndTime)) {
                findCurrSample(time);
            }

            interpolCurrent(time);
            return mCurrInterpol;
        }

        public double CurrLapTime {
            get {
                if (mCurrInterpol == null || mCurrInterpol.mLap == null) return 0;
                return mCurrInterpol.mAbsStartTime - mCurrInterpol.mLap.mAbsStartTime;
            }
        }

        public int CurrLapIndex {
            get {
                if (mCurrInterpol == null || mCurrInterpol.mLap == null) return 0;
                return mCurrInterpol.mLap.mIndex;
            }
        }

        public double LastLapDuration {
            get {
                if (mCurrInterpol == null || mCurrInterpol.mLap == null) return 0;
                int i = mCurrInterpol.mLap.mIndex;
                if (i > 0) {
                    return mTrackData.Laps[i - 1].mLapTime;
                }
                return 0;
            }
        }

        private void findCurrSample(double time) {

            List<TrackParser.Sample> samples = mTrackData.Samples;
            int n = samples.Count;
            int n1 = n - 1;

            if (time < 0) {
                // can't go before the first sample
                mCurrSample = samples[0];
                mCurrIndex = 0;
                return;
            } else if (time > samples[n1].mAbsStartTime) {
                // can't go past the last sample
                mCurrSample = samples[n1];
                mCurrIndex = n - 1;
                return;
            }

            // The most common case is that we need to advance to the next sample
            if (mCurrSample != null && mCurrIndex < n1) {
                TrackParser.Sample s = samples[mCurrIndex + 1];
                if (time >= s.mAbsStartTime &&
                        (Double.IsNaN(s.mAbsEndTime) || (!Double.IsNaN(s.mAbsEndTime) && time < s.mAbsEndTime))) {
                    mCurrIndex++;
                    mCurrSample = s;
                    return;
                }
            }

            // Going forward from current sample
            if (mCurrSample != null && mCurrIndex < n1 && time > mCurrSample.mAbsEndTime) {
                while (mCurrIndex < n1 && time > mCurrSample.mAbsEndTime) {
                    mCurrIndex++;
                    mCurrSample = samples[mCurrIndex];
                }
                return;
            }

            // Going backwards from current sample
            if (mCurrSample != null && mCurrIndex > 0 && time < mCurrSample.mAbsStartTime) {
                while (mCurrIndex > 0 && time < mCurrSample.mAbsStartTime) {
                    mCurrIndex--;
                    mCurrSample = samples[mCurrIndex];
                }
                return;
            }

            // The most unlikely case is that we're just from scratch going to
            // a random position. We could do a dichotomic search. We'll just start with the
            // obvious linear search and optimize later if really needed (very unlikely),
            // in which case we could combine the 2 last cases to do a dichotomic search
            // with a preset direction and a preset starting point. Yawn.
            for (int i = 0; i < n; i++) {
                TrackParser.Sample s = samples[mCurrIndex + 1];
                if (time >= s.mAbsStartTime &&
                        (Double.IsNaN(s.mAbsEndTime) || (!Double.IsNaN(s.mAbsEndTime) && time < s.mAbsEndTime))) {
                    mCurrIndex++;
                    mCurrSample = s;
                    return;
                }
            }

            System.Diagnostics.Debug.Fail("Time not found in samples");
        }

        private void interpolCurrent(double time) {
            TrackParser.Sample result = mCurrInterpol;
            TrackParser.Sample curr = mCurrSample;
            TrackParser.Sample next = mCurrSample;

            double progress = 0;
            if (!Double.IsNaN(curr.mAbsEndTime)) {
                progress = (time - curr.mAbsStartTime) / (curr.mAbsEndTime - curr.mAbsStartTime);
            }
            if (progress > 0 && mCurrIndex < mTrackData.Samples.Count - 1) {
                next = mTrackData.Samples[mCurrIndex + 1];
            }

            result.mLap = curr.mLap;

            result.mAbsStartTime = time;
            result.mAbsEndTime = curr.mAbsEndTime;
            result.mElapsedTime = interpolate(curr.mElapsedTime, next.mElapsedTime, progress);

            result.mSpeed = interpolate(curr.mSpeed, next.mSpeed, progress);
            result.mAccel = interpolate(curr.mAccel, next.mAccel, progress);
            result.mLateralAccel = interpolate(curr.mLateralAccel, next.mLateralAccel, progress);
            result.mLatitude = interpolate(curr.mLatitude, next.mLatitude, progress);
            result.mLongtiude = interpolate(curr.mLongtiude, next.mLongtiude, progress);
            result.mAltitude = interpolate(curr.mAltitude, next.mAltitude, progress);
            result.mBearing = interpolate(curr.mBearing, next.mBearing, progress);
        }

        private double interpolate(double value1, double value2, double progress) {
            return value1 + (value2 - value1) * progress;
        }

    }
}
