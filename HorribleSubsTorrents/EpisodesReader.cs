using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading.Tasks;

namespace HorribleSubsTorrents
{
    static class EpisodesReader
    {
        public static LinkedList<Episode> ReadEpisodes(string url, int latestEpisode)
        {
            LinkedList<Episode> episodes = new LinkedList<Episode>();
            int nextEpisodes = 0;
            string show_id = getShowId(url);
            string httpResponse = "";
            do
            {
                string api_url = "https://horriblesubs.info/api.php?method=getshows&type=show&showid=" + show_id + "&nextid=" + nextEpisodes;
                nextEpisodes++;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(api_url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    httpResponse = reader.ReadToEnd();
                    httpResponse = httpResponse.Replace("&", "&amp;");
                    httpResponse = "<div>" + httpResponse + "</div>";
                    XmlDocument xmlResponse = new XmlDocument();
                    xmlResponse.LoadXml(httpResponse);
                    XmlNode root = xmlResponse.FirstChild;
                    XmlNodeList nodeList = xmlResponse.SelectNodes("//div[contains(@id, '1080p')]");
                    // traverse each node with 'id=1080p'
                    foreach (XmlNode node in nodeList)
                    {
                        // extract episode id from id attribute
                        string episode_id = node.Attributes.GetNamedItem("id").Value;
                        Regex numbers = new Regex(@"\d+");
                        episode_id = numbers.Match(episode_id).Value;
                        // if the episode has already been downloaded return magnet links for other episodes
                        if (int.Parse(episode_id) <= latestEpisode)
                            return episodes;
                        // extract magnet link from href attribute
                        XmlNode torrent = node.SelectSingleNode(".//a[contains(@title, 'Magnet Link')]");
                        string torrentLink = torrent.Attributes.GetNamedItem("href").Value;
                        Episode episode = new Episode(int.Parse(episode_id), torrentLink);
                        episodes.AddLast(episode);
                    }
                }
            } while (!httpResponse.Contains("DONE"));

            return episodes;
        }

        private static string getShowId(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string httpResponse = reader.ReadToEnd();
                Regex regex = new Regex(@"hs_showid = \d+");
                Match match = regex.Match(httpResponse);
                string show_id = match.Value;
                show_id = show_id.Remove(0, 12);
                return show_id;
            }
        }
    }
}
