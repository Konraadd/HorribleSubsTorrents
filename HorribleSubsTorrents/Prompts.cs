using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Windows.Forms;


namespace HorribleSubsTorrents
{
    public static class Prompts
    {
        public static void showNewAnimeDialog()
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 250,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Information about new anime",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Anime URL from horriblesubs.info" };
            TextBox textBoxUrl = new TextBox() { Left = 50, Top = 40, Width = 400 };
            Label textLabel2 = new Label() { Left = 50, Top = 80, Text = "Latest downloaded episode" };
            TextBox textBoxLatestDownloadedEpisode = new TextBox() { Left = 50, Top = 100, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 150, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBoxUrl);
            prompt.Controls.Add(textBoxLatestDownloadedEpisode);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textLabel2);
            prompt.AcceptButton = confirmation;
            DialogResult result = prompt.ShowDialog();
            if (result == DialogResult.OK)
            {
                int latest_dowloaded;
                try
                {
                    latest_dowloaded = int.Parse(textBoxLatestDownloadedEpisode.Text);
                }
                catch(FormatException)
                {
                    MessageBox.Show("Incorrect number for latest downloaded episode!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    WebRequest.Create(textBoxUrl.Text);

                }
                catch (UriFormatException) {
                    MessageBox.Show("Incorrect Url!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (int.TryParse(textBoxLatestDownloadedEpisode.Text, out latest_dowloaded)) {
                    AnimeInfo info = new AnimeInfo(textBoxUrl.Text, latest_dowloaded, DateTime.MinValue);
                    AnimeManager.AddAnime(info);
                }
            }
            return;
        }

        public static void showUTorrentConfigurationDialog(string username = "", string password = "", string port = "8080")
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 300,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "uTorrent Configuration",
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label()           { Left = 50, Top = 20, Text = "Username" };
            TextBox textBoxUsername = new TextBox() { Left = 50, Top = 40, Width = 400, Text=username };
            Label textLabel2 = new Label()          { Left = 50, Top = 80, Text = "Password" };
            TextBox textBoxPassword = new TextBox() { Left = 50, Top = 100, Width = 400, Text=password };
            Label textLabel3 = new Label()          { Left = 50, Top = 140, Text = "Port" };
            TextBox textBoxPort = new TextBox()     { Left = 50, Top = 160, Width = 400, Text = port };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 200, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBoxUsername);
            prompt.Controls.Add(textBoxPassword);
            prompt.Controls.Add(textBoxPort);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textLabel2);
            prompt.Controls.Add(textLabel3);
            prompt.AcceptButton = confirmation;
            DialogResult result = prompt.ShowDialog();
            if (result == DialogResult.OK)
            {
                int port_int = 8080;
                try
                {
                    port_int = int.Parse(textBoxPort.Text);
                }
                catch (Exception ex)
                {
                    // komunikat o niepoprawnym porcie i jeszcze raz dialog
                    return;
                }
                UTorrentConfiguration.saveCredentials(textBoxUsername.Text, textBoxPassword.Text, port_int);
            }
            return;
        }
    }
}
