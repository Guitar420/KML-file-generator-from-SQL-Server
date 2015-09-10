using System;
using System.Collections.Generic;
using System.Data.Spatial;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new SqlConnection("your connection string");  //connection string
            var cmd = new SqlCommand("SELECT location.ToString(), name FROM table", connection); //query
            connection.Open();

            var kml = $@"<?xml version=""1.0"" encoding=""UTF-8""?>"
+ @"<kml xmlns = ""http://www.opengis.net/kml/2.2"" xmlns:gx=""http://www.google.com/kml/ext/2.2"" xmlns:kml=""http://www.opengis.net/kml/2.2"" xmlns:atom=""http://www.w3.org/2005/Atom"">"
+ @"<Document><name>PP</name><Style id=""s_ylw-pushpin"">"
+ @"<IconStyle><scale>1.1</scale><Icon><href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href>"
+ @"</Icon><hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/></IconStyle>"
+ @"</Style><StyleMap id=""m_ylw-pushpin""><Pair><key>normal</key><styleUrl>#s_ylw-pushpin</styleUrl>"
+ @"</Pair><Pair><key>highlight</key><styleUrl>#s_ylw-pushpin_hl</styleUrl>"
+ @"</Pair></StyleMap><Style id=""s_ylw-pushpin_hl""><IconStyle><scale>1.3</scale><Icon>"
+ @"<href>http://maps.google.com/mapfiles/kml/pushpin/ylw-pushpin.png</href></Icon>"
+ @"<hotSpot x=""20"" y=""2"" xunits=""pixels"" yunits=""pixels""/></IconStyle></Style>"
+ @"{placemarks}</Document></kml>";

            var placemarks = string.Empty;

            using (var polygon = cmd.ExecuteReader())
            {
                while (polygon.Read())
                {
                    var geo = DbGeography.PointFromText(polygon[0].ToString(), 4326);
                    var placemark = "<Placemark><name>" + polygon[1].ToString()
                        + "</name><Point><gx:drawOrder>1</gx:drawOrder><coordinates>" + geo.Longitude + "," + geo.Latitude
                        + ",0</coordinates></Point></Placemark>";
                    placemarks += placemark;
                }
            }
            connection.Close();

            kml = kml.Replace("{placemarks}", placemarks);

            File.WriteAllText("data.kml", kml);
        }

    }
}

