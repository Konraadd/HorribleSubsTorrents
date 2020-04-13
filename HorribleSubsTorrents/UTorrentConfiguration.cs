using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;


namespace HorribleSubsTorrents
{
    public class  UTorrentConfiguration
    {
        public string password { get; set; }
        public string username { get; set; }
        public int port { get; set; }

        // needed for serialization
        public UTorrentConfiguration() { }

        public void readCredentials()
        {
            Stream stream = File.Open("Credentials.xml", FileMode.OpenOrCreate);
            if (stream.Length <= 0)
            {
                stream.Close();
                return;
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UTorrentConfiguration));
            UTorrentConfiguration creds = (UTorrentConfiguration) xmlSerializer.Deserialize(stream);
            this.username = creds.username;
            this.password = creds.password;
            this.port = (creds.port == 0)? 8080 : creds.port;
            stream.Close();
        }

        public static void saveCredentials(string username, string password, int port)
        {
            UTorrentConfiguration config = new UTorrentConfiguration();
            config.password = password;
            config.username = username;
            config.port = port;
            Stream stream = File.Open("Credentials.xml", FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UTorrentConfiguration));
            xmlSerializer.Serialize(stream, config);
            stream.Close();
        }
    }
}
