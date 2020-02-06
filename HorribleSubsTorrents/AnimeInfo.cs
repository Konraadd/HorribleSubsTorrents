using System;
using System.Net;
using System.Windows.Forms;

namespace HorribleSubsTorrents

{
    [Serializable()]
    public class AnimeInfo
    {
        public string url { get; set; }
        public int latestDownloadedEpisode { get; set; }

        public AnimeInfo(string url, int latest)
        {
            this.url = url;
            this.latestDownloadedEpisode = latest;
        }

        public AnimeInfo() { }
    }
}
