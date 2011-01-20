using System.Net;

namespace AgnesBot.Modules.TinyUrlModule.Services
{
    public class TinyUrlService : ITinyUrlService
    {
        private const string API_URL = "http://tinyurl.com/api-create.php?url={0}";

        public string ShortenUrl(string url)
        {
            var client = new WebClient();

            return client.DownloadString(string.Format(API_URL, url));
        }
    }
}