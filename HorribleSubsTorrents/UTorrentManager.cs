using System;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Windows.Forms;


namespace HorribleSubsTorrents
{
    class UTorrentManager
    {
        string utorrent_api_url, token, cookie;
        NetworkCredential credentials;


        public UTorrentManager()
        {
            UTorrentConfiguration config = new UTorrentConfiguration();
            config.readCredentials();
            this.utorrent_api_url = "http://localhost:" + config.port.ToString() +"/gui";
            this.credentials = new NetworkCredential(config.username, config.password);
            this.setTokenAndCookie();
        }

        private void setTokenAndCookie()
        {
            string url = this.utorrent_api_url + "/token.html";
            string token = null, cookie = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = credentials;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    string httpResponse = reader.ReadToEnd();
                    Regex rgx = new Regex(@">(?!<).+<\/d");
                    token = rgx.Match(httpResponse).Value;
                    token = token.Substring(1, token.Length - 4);
                    Regex cookie_rgx = new Regex(@"GUID=.+;");
                    cookie = cookie_rgx.Match(response.Headers.ToString()).Value;
                    cookie = cookie.Substring(5, cookie.Length - 6);
                }
                this.cookie = cookie;
                this.token = token;
            } catch (WebException)
            {
                MessageBox.Show("Can't connect to uTorrent API!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AddTorrent(string torrentUrl)
        {
            torrentUrl = WebUtility.UrlEncode(torrentUrl);
            string url = this.utorrent_api_url + "/?token=" + this.token + "&action=add-url&s=" + torrentUrl;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Credentials = this.credentials;
            CookieContainer container = new CookieContainer();
            container.Add(new Cookie("GUID", cookie) { Domain = "localhost" });
            request.CookieContainer = container;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string httpResponse = reader.ReadToEnd();
                Console.WriteLine(httpResponse);
            }
        }


    }
}
