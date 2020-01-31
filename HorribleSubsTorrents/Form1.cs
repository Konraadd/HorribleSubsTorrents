using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HorribleSubsTorrents
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadAnimesToList(AnimeManager.ReadAllAnime());
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            List<Episode> newEpisodes = new List<Episode>();
            List<AnimeInfo> animesInfo = new List<AnimeInfo>();
            List<AnimeInfo> updatedAnimeInfo = new List<AnimeInfo>();
            var listItems = listView1.Items;
            // get info about downloaded episodes
            animesInfo = loadAnimesFromList();
            // show progress bar
            progressBar1.Visible = true;
            progressBar1.Step = 1;
            progressBar1.Minimum = 1;
            progressBar1.PerformStep();
            progressBar1.Maximum = animesInfo.Count * 2;
            // scan the site for new episodes
            foreach (AnimeInfo info in animesInfo)
            {
                int latest_ep = info.latestDownloadedEpisode;
                LinkedList<Episode> temp = EpisodesReader.ReadEpisodes(info.url, info.latestDownloadedEpisode);
                // if new episodes were found, update latest downloaded
                if (temp.Count > 0)
                {
                    latest_ep = temp.First().number;
                }
                updatedAnimeInfo.Add(new AnimeInfo(info.url, latest_ep));
                newEpisodes.AddRange(temp);
                progressBar1.PerformStep();
            }
            AnimeManager.SaveAnimes(updatedAnimeInfo);
            UTorrentManager torrentManager = new UTorrentManager("admin", "Irtkan123");
            // update max value for progress bar
            progressBar1.Maximum = progressBar1.Maximum - animesInfo.Count + newEpisodes.Count;
            foreach (Episode ep in newEpisodes) {
                torrentManager.AddTorrent(ep.torrent);
                progressBar1.PerformStep();
            }
            // load updated animes to list view
            loadAnimesToList(AnimeManager.ReadAllAnime());
            progressBar1.Visible = false;
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NewAnimePrompt.showNewAnimeDialog();
            // load updated animes to list view
            loadAnimesToList(AnimeManager.ReadAllAnime());
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
            // save the newly modified list to memomory
            AnimeManager.SaveAnimes(loadAnimesFromList());
        }

        private List<AnimeInfo> loadAnimesFromList()
        {
            List<AnimeInfo> animeList = new List<AnimeInfo>();
            foreach (ListViewItem item in listView1.Items)
            {
                animeList.Add(new AnimeInfo(item.SubItems[0].Text, int.Parse(item.SubItems[1].Text)));
            }
            return animeList;
        }

        private void loadAnimesToList(List<AnimeInfo> animeInfos)
        {
            listView1.Items.Clear();
            foreach (AnimeInfo info in animeInfos)
            {
                string[] temp = { info.url, info.latestDownloadedEpisode.ToString() };
                listView1.Items.Add(new ListViewItem(temp));
            }
            listView1.View = View.Details;
            return;
        }
    }
}
