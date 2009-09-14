
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
    public class TrackParser {

        public struct Dot {
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
            public int Index { get; private set; }
            public double StartTime { get; private set; }
            public double LapTime { get; private set; }
            public List<Dot> Dots { get; private set; }

            public Lap(int index, double startTime) {
                Index = index;
                StartTime = startTime;
                Dots = new List<Dot>();
            }

            public void addDot(Dot d) {
                Dots.Add(d);
                LapTime = d.mElapsedTime;
            }
        }

        public List<Lap> Laps { get; private set; }

        public TrackParser(XmlDocument doc) {

            Laps = new List<Lap>(); 

            double startTime = 0;
            Lap l = null;
            int n = 1;
            while ((l = parseLap(doc, startTime, n++)) != null) {
                startTime += l.LapTime;
            }
        }

        private Lap parseLap(XmlDocument doc, double startTime, int n) {

            try {
                XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("k", @"http://www.opengis.net/kml/2.2");

                string expr = String.Format("/k:kml/k:Document/k:Folder/k:Folder[k:styleUrl='#lap' and k:name='{0}']/k:Placemark", n);
                XmlNodeList nodes = doc.SelectNodes(expr, ns);

                if (nodes != null && nodes.Count > 0) {

                    Lap l = new Lap(n, startTime);

                    foreach (XmlNode node in nodes) {
                        XmlNode descNode = node.SelectSingleNode("k:description", ns);
                        XmlNode coordNode = node.SelectSingleNode("k:Point/k:coordinates", ns);

                        if (descNode != null && coordNode != null) {
                            Dot d = new Dot();
                            parseDesc(d, descNode);
                            parseCoords(d, coordNode);

                            l.addDot(d);
                        }
                    }

                    Laps.Add(l);
                    return l;
                }

            } catch (XPathException e) {
                // TODO use debug log
                System.Diagnostics.Debug.Print(e.Message + "\n" + e.StackTrace + "\n");
            }

            return null;
        }

        private void parseDesc(Dot d, XmlNode descNode) {
            System.Diagnostics.Debug.Assert(descNode != null);

            string t = descNode.InnerText.ToString().Trim();

            // Regexp:      ... <p> name        = value     </p> ...
            Regex r = new Regex(@">(?<1>[\w\s]+)=(?<2>[^<]+)<", RegexOptions.Singleline | RegexOptions.Compiled);

            for (Match m = r.Match(t); m.Success; m = m.NextMatch()) {
                string name  = m.Groups[1].Value;
                string value = m.Groups[2].Value;

                switch(name) {
                    case "accel":
                        d.mAccel = Convert.ToDouble(value);
                        break;
                    case "lateral_accel":
                        d.mLateralAccel = Convert.ToDouble(value);
                        break;
                    case "bearing":
                        d.mBearing = Convert.ToDouble(value);
                        break;
                    case "speed":
                        int space = value.IndexOf(' ');
                        string num = space > 0 ? value.Substring(0, space) : value;
                        d.mSpeed = Convert.ToDouble(num);
                        if (value.IndexOf("mph") > 0) {
                            d.mSpeed = mph2ms(d.mSpeed);
                        } else if (value.IndexOf("kmh") > 0) {
                            d.mSpeed = kmh2ms(d.mSpeed);
                        }
                        break;
                    case "elapsed time":
                        int col = value.IndexOf(':');
                        string min = value.Substring(0, col);
                        string sec = value.Substring(col + 1);
                        d.mElapsedTime = Convert.ToDouble(min) * 60 + Convert.ToDouble(sec);
                        break;
                }
            }
        }

        private void parseCoords(Dot d, XmlNode coordNode) {
            System.Diagnostics.Debug.Assert(coordNode != null);

            string t = coordNode.InnerText.ToString().Trim();

            string[] v = t.Split(',');

            d.mLongtiude = Convert.ToDouble(v[0]);
            d.mLatitude  = Convert.ToDouble(v[1]);
            d.mAccel     = ft2m(Convert.ToDouble(v[2]));
        }

        private double mph2ms(double mph) {
            return mph * 0.44704;
        }

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
