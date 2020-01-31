using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorribleSubsTorrents
{
    class Episode:IComparable<Episode>
    {
        // number of the episode
        public int number { get; }
        public string torrent { get; }
        
        public Episode(int number, string torrent)
        {
            this.number = number;
            this.torrent = torrent;
        }

        // function for IComparable interface
        public int CompareTo(Episode ep)
        {
            return (ep.number.CompareTo(this.number));
        }
    }
}
