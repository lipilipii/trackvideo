
using System;
using System.Xml;
using System.Collections.Generic;
using System.Xml.XPath;

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
    public class TrackParser {

        public class Dot {
            public double mElapsedTime;
            public double mLatitude;
            public double mLongtiude;
            public double mAltitude;
            public double mBearing;
            public double mAccel;
            public double mLateralAccel;
            public double mSpeed;
        }

        public class Lap {
            public int mIndex;
            public double mStartTime;
            public double mLapTime;
            public List<Dot> mDots;
        }

        private List<Lap> mLaps;

        public List<Lap> Laps {
            get {
                return mLaps;
            }
        }

        public TrackParser(XmlDocument doc) {

            double startTime = 0;
            Lap l = null;
            int n = 1;
            while ((l = parseLap(doc, startTime, n++)) != null) {
                startTime += l.mLapTime;
            }
        }

        private Lap parseLap(XmlDocument doc, double startTime, int n) {
            string expr = String.Format("/kml/Document/Folder[styleUrl='#lap' and name='{0}']/Placemark", n);
            try {
                XmlNodeList nodes = doc.SelectNodes(expr);

                if (nodes != null && nodes.Count > 0) {

                    Lap l = new Lap();
                    l.mStartTime = startTime;
                    l.mIndex = n;
                    l.mDots = new List<Dot>();

                    foreach (XmlNode node in nodes) {
                        Dot d = new Dot();
                        parseDesc(d, node.SelectSingleNode("description"));
                        parseCoords(d, node.SelectSingleNode("Point/coordinates"));

                        l.mDots.Add(d);
                    }

                    mLaps.Add(l);
                    return l;
                }

            } catch (XPathException e) {
                // TODO use debug log
                System.Diagnostics.Debug.Print(e.Message + "\n" + e.StackTrace + "\n");
            }

            return null;
        }

        private void parseDesc(Dot d, XmlNode descNode) {
            throw new NotImplementedException();
        }

        private void parseCoords(Dot d, XmlNode coordNode) {
            System.Diagnostics.Debug.Assert(coordNode != null);

            string t = coordNode.InnerText.ToString().Trim();

            string[] ps = t.Split(',');

            d.mLongtiude = Convert.ToDouble(ps[0]);
            d.mLatitude  = Convert.ToDouble(ps[1]);
            d.mAccel     = ft2m(Convert.ToDouble(ps[2]));
        }

        /// <summary>
        /// Converts feet into meters
        /// </summary>
        private double ft2m(double ft) {
            return ft * 0.3048;
        }
    }
}
