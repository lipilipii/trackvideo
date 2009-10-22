
using System;
using System.Xml;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Text.RegularExpressions;

/*
 * Details on the KML structure we're interested in (using XPath notations):
 * 
 * - /kml/Document/Folder[/styleUrl=#lap]  => one lap data
 *      - ./name=N (lap #)
 *      - ./Placemark
 *          - ./name=split=N (at split point) or data=N (sequence #)
 *          - ./description: text. Split on <p>, ignore </p>:
 *              data=N (sequence #)
 *              time=YYYY-MM-DDTHH:MM:SS.mmm-ZZZZ
 *              split time=M:SS.mmm (if at a split point)
 *              elapsed time=M:SS.mmm
 *              latitude=-nn.nnnnnn
 *              longitude=-nn.nnnnnn
 *              altitude=nn ft (prolly has m too)
 *              bearing=nn.nn
 *              accel=n.nnn
 *              lateral_accel=n.nn
 *              speed=nn.nn mph (prolly has kmh)
 *          - ./Point/coordinates=-nnn.nnnn,nnn.nnnn,nn.nn (long/lat/alt, more precise than in desc)
 *          - ./TimeStamp/when=UTC time, less precise than in desc (doesn't have millis)
 *          - ./styleUrl=#startFlag|#pathDot|#splitMarker|#endFlag
 * 
 * Our workflow:
 * - find the N laps (/kml/Document/Folder[/styleUrl=#lap]/name=N for N in 1..max)
 * - for each lap find all the (@Folder)/Placemark nodes
 * - for each placemark, use ./Point/coordinates to get the long/lat/alt and /description for the rest.
 * - internally formalize units to meters (altitude) and meters/second (speed)
*/


namespace Alfray.TrackVideo.TrackVideoApp {

    /// <summary>
    /// Information loaded from a track data file.
    /// A track parser contains track data, organized in laps.
    /// Each lap contains a number of sample.
    /// 
    /// Conventions:
    /// - AbsStartTime: an absolute time relative to the very first sample of the first lap.
    /// - LapDuration: the duration of a given lap (what is commonly called "a lap time").
    /// - ElapstedTime: the time of a sample relative to the start of its lap.
    /// </summary>
    public class TrackParser {

        /// <summary>
        /// A sample of track parameters at a given time in a lap.
        /// </summary>
        public class Sample {
            /// <summary>
            /// The lap that contains this sample.
            /// </summary>
            public Lap mLap;

            /// <summary>
            /// The absolute start time of the sample (relative to the start of the first lap)
            /// </summary>
            public double mAbsStartTime;

            /// <summary>
            /// The absolute start time of the <em>next</em> sample.
            /// It's NaN for the last sample of the last lap.
            /// </summary>
            public double mAbsEndTime = Double.NaN;

            /// <summary>
            /// The time of the sample relative to the start of this lap.
            /// </summary>
            public double mElapsedTime;

            public double mLatitude;
            public double mLongtiude;

            /// <summary>
            /// Altitude in meters
            /// </summary>
            public double mAltitude;

            public double mBearing;

            /// <summary>
            /// Acceleration in g
            /// </summary>
            public double mAccel;

            /// <summary>
            /// Lateral acceleration in g
            /// </summary>
            public double mLateralAccel;

            /// <summary>
            /// Speed in m/s
            /// </summary>
            public double mSpeed;
        }

        /// <summary>
        /// Information on a given lap.
        /// </summary>
        public class Lap {
            /// <summary>
            /// The lap number. First lap starts at 1.
            /// </summary>
            public int mIndex;

            /// <summary>
            /// The absolute start time of the lap (absolute to the first dot of the first lap)
            /// </summary>
            public double mAbsStartTime;

            /// <summary>
            /// The duration of this lap.
            /// </summary>
            public double mLapTime;
        }

        /// <summary>
        /// All the samples, for all the laps, in increasing time order.
        /// </summary>
        public List<Sample> Samples { get; private set; }

        /// <summary>
        /// Returns true if this track data has at least one sample.
        /// </summary>
        public bool HasSamples {
            get {
                return Samples.Count > 0;
            }
        }

        /// <summary>
        /// All the laps, in increasing number/time order.
        /// </summary>
        public List<Lap> Laps   { get; private set; }

        /// <summary>
        /// Total time of all the laps, e.g. simply the absolute start time
        /// of the last sample of the last lap.
        /// </summary>
        public double TotalTime { get; private set; }


        public TrackParser(XmlDocument doc) {

            Samples = new List<Sample>();
            Laps = new List<Lap>();

            parseLaps(doc);

            if (Samples.Count > 0) TotalTime = Samples[Samples.Count - 1].mAbsStartTime;
        }

        /// <summary>
        /// Returns a text summary of this track data: number of laps, points, etc.
        /// </summary>
        public string Summary {
            get {
                return String.Format("Loaded {1} points for {0} laps, {2}:{3,2}.{4}",
                    Laps.Count,
                    Samples.Count,
                    (int)(TotalTime / 60),                      // minutes
                    (int)(TotalTime % 60),                      // seconds
                    100 * (int)(TotalTime - (int)TotalTime));   // centiseconds
            }
        }

        private void parseLaps(XmlDocument doc) {

            try {
                XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("k", @"http://www.opengis.net/kml/2.2");

                double startTime = 0;
                double lapTime = 0;
                Sample prevSample = null;
                string prevTimeString = null;

                for (int n = 1; ; n++) {

                    string expr = String.Format("/k:kml/k:Document/k:Folder/k:Folder[k:styleUrl='#lap' and k:name='{0}']/k:Placemark", n);
                    XmlNodeList nodes = doc.SelectNodes(expr, ns);

                    if (nodes == null || nodes.Count == 0) {
                        break;
                    }

                    Lap l = new Lap();
                    l.mIndex = n;
                    l.mAbsStartTime = startTime;
                    Laps.Add(l);

                    foreach (XmlNode node in nodes) {
                        XmlNode descNode = node.SelectSingleNode("k:description", ns);
                        XmlNode coordNode = node.SelectSingleNode("k:Point/k:coordinates", ns);

                        if (descNode != null && coordNode != null) {
                            Sample s = new Sample();
                            s.mLap = l;

                            string timeString = parseDesc(s, descNode);
                            parseCoords(s, coordNode);

                            // If the ISO time code is the same as the one of the last sample
                            // then discard the sample as a dup. With TrackMaster data this
                            // happens for the first sample of each lap.
                            if (prevTimeString != null && prevTimeString == timeString) continue;
                            prevTimeString = timeString;

                            Samples.Add(s);

                            double elapsed = s.mElapsedTime;
                            s.mAbsStartTime = startTime + elapsed;
                            lapTime = elapsed;
                            if (prevSample != null) {
                                prevSample.mAbsEndTime = s.mAbsStartTime;
                            }
                            prevSample = s;

                        }
                    } // foreach node sample

                    l.mLapTime = lapTime;
                    startTime += lapTime;
                } // for n lap

                TotalTime = startTime;

            } catch (XPathException e) {
                // TODO use debug log
                System.Diagnostics.Debug.Print(e.Message + "\n" + e.StackTrace + "\n");
            }
        }

        private string parseDesc(Sample s, XmlNode descNode) {
            System.Diagnostics.Debug.Assert(descNode != null);

            string t = descNode.InnerText.ToString().Trim();

            // Regexp:      ... <p> name        = value     </p> ...
            Regex r = new Regex(@">(?<1>[\w\s]+)=(?<2>[^<]+)<", RegexOptions.Singleline | RegexOptions.Compiled);

            string timeCode = "";

            for (Match m = r.Match(t); m.Success; m = m.NextMatch()) {
                string name  = m.Groups[1].Value;
                string value = m.Groups[2].Value;

                switch(name) {
                    case "time":
                        // This is an ISO time code with actually enough milliseconds to be interesting.
                        // We have the same info in elapsed_time so we're not going to bother decoding
                        // the ISO time string. However we want the value to quickly discard dups.
                        timeCode = value;
                        break;
                    case "accel":
                        s.mAccel = Convert.ToDouble(value);
                        break;
                    case "lateral_accel":
                        s.mLateralAccel = Convert.ToDouble(value);
                        break;
                    case "bearing":
                        s.mBearing = Convert.ToDouble(value);
                        break;
                    case "speed":
                        int space = value.IndexOf(' ');
                        string num = space > 0 ? value.Substring(0, space) : value;
                        s.mSpeed = Convert.ToDouble(num);
                        if (value.IndexOf("mph") > 0) {
                            s.mSpeed = mph2ms(s.mSpeed);
                        } else if (value.IndexOf("kmh") > 0) {
                            s.mSpeed = kmh2ms(s.mSpeed);
                        }
                        break;
                    case "elapsed time":
                        int col = value.IndexOf(':');
                        string min = value.Substring(0, col);
                        string sec = value.Substring(col + 1);
                        s.mElapsedTime = Convert.ToDouble(min) * 60 + Convert.ToDouble(sec);
                        break;
                }
            }

            return timeCode;
        }

        private void parseCoords(Sample s, XmlNode coordNode) {
            System.Diagnostics.Debug.Assert(coordNode != null);

            string t = coordNode.InnerText.ToString().Trim();

            string[] v = t.Split(',');

            s.mLongtiude = Convert.ToDouble(v[0]);
            s.mLatitude  = Convert.ToDouble(v[1]);
            s.mAltitude  = ft2m(Convert.ToDouble(v[2]));
        }

        /// <summary>
        /// Converts mph in m/s
        /// </summary>
        private double mph2ms(double mph) {
            return mph * 0.44704;
        }

        /// <summary>
        /// Converts km/h in m/s
        /// </summary>
        private double kmh2ms(double kmh) {
            return kmh * 0.277777778;
        }

        /// <summary>
        /// Converts feet into meters
        /// </summary>
        private double ft2m(double ft) {
            return ft * 0.3048;
        }
    }
}
