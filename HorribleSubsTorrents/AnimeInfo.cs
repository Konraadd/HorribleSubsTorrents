using System;
using System.Net;
using System.Windows.Forms;

namespace HorribleSubsTorrents

{
    [Serializable()]
    public class AnimeInfo : IComparable
    {
        public string url { get; set; }
        public int latestDownloadedEpisode { get; set; }
        public DateTime lastDownloadedDate { get; set; }

        public AnimeInfo(string url, int latest, DateTime lastDownloadDate)
        {
            this.url = url;
            this.latestDownloadedEpisode = latest;
            this.lastDownloadedDate = lastDownloadDate;
        }

        // necessary!!
        public AnimeInfo() { }
        
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            AnimeInfo animeInfo = obj as AnimeInfo;
            if (animeInfo != null)
            {
                return -this.lastDownloadedDate.CompareTo(animeInfo.lastDownloadedDate);
            }
            else
            {
                return 1;
            }
        }

    }
}
