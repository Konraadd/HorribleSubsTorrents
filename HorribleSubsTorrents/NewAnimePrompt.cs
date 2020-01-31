using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace HorribleSubsTorrents
{
    public static class NewAnimePrompt
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
                if (int.TryParse(textBoxLatestDownloadedEpisode.Text, out latest_dowloaded)) {
                    AnimeInfo info = new AnimeInfo(textBoxUrl.Text, latest_dowloaded);
                    AnimeManager.AddAnime(info);
                }
            }
            return;
        }
    }
}
