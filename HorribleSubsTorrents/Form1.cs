﻿using System;
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
                DateTime last_download_date = info.lastDownloadedDate;
                LinkedList<Episode> temp = EpisodesReader.ReadEpisodes(info.url, info.latestDownloadedEpisode);
                // if new episodes were found, update latest downloaded
                if (temp.Count > 0)
                {
                    latest_ep = temp.First().number;
                    last_download_date = DateTime.Now;
                }
                updatedAnimeInfo.Add(new AnimeInfo(info.url, latest_ep, last_download_date));
                newEpisodes.AddRange(temp);
                progressBar1.PerformStep();
            }
            AnimeManager.SaveAnimes(updatedAnimeInfo);
            UTorrentManager torrentManager = new UTorrentManager();
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
            Prompts.showNewAnimeDialog();
            // load updated animes to list view
            loadAnimesToList(AnimeManager.ReadAllAnime());
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
            }
            catch (ArgumentOutOfRangeException)
            {
                // clicked somewhere random on the list i guess?
                return;
            }
            // save the newly modified list to memomory
            AnimeManager.SaveAnimes(loadAnimesFromList());
        }

        private List<AnimeInfo> loadAnimesFromList()
        {
            List<AnimeInfo> animeList = new List<AnimeInfo>();
            foreach (ListViewItem item in listView1.Items)
            {
                DateTime last_download_time;
                if (!DateTime.TryParse(item.SubItems[2].Text, out last_download_time)) {
                    last_download_time = DateTime.MinValue;
                }
                animeList.Add(new AnimeInfo(item.SubItems[0].Text, int.Parse(item.SubItems[1].Text), last_download_time));
            }
            return animeList;
        }

        private void loadAnimesToList(List<AnimeInfo> animeInfos)
        {
            if (animeInfos == null)
                return;

            listView1.Items.Clear();
            animeInfos.Sort();
            foreach (AnimeInfo info in animeInfos)
            {
                string last_download_date = (info.lastDownloadedDate == DateTime.MinValue) ? "" : info.lastDownloadedDate.ToLongDateString();
                string[] temp = {
                    info.url,
                    info.latestDownloadedEpisode.ToString(),
                    last_download_date
                };
                listView1.Items.Add(new ListViewItem(temp));
            }
            listView1.View = View.Details;
            return;
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UTorrentConfiguration config = new UTorrentConfiguration();
            config.readCredentials();
            Prompts.showUTorrentConfigurationDialog(config.username, config.password, config.port.ToString());
        }

        private void uTorrentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
