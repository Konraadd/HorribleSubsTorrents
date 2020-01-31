using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace HorribleSubsTorrents
{
    static class AnimeManager
    {
        public static List<AnimeInfo> ReadAllAnime()
        {
            List<AnimeInfo> animeList = new List<AnimeInfo>();
            Stream stream = File.Open("Animes.xml", FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<AnimeInfo>));
            animeList = (List<AnimeInfo>) xmlSerializer.Deserialize(stream);
            stream.Close();

            return animeList;
        }

        public static void SaveAnimes(List<AnimeInfo> animes)
        {
            Stream stream = File.Open("Animes.xml", FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<AnimeInfo>));
            xmlSerializer.Serialize(stream, animes);
            stream.Close();
        }

        public static void AddAnime(AnimeInfo anime)
        {
            List<AnimeInfo> animes= ReadAllAnime();
            animes.Add(anime);
            SaveAnimes(animes);
        }
    }
}
