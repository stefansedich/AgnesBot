using System;

namespace AgnesBot.Modules.UrlAggregatorModule.Domain
{
    public class Url
    {
        public string Id { get; set; }
        public string Link { get; set; }
        public DateTime Timestamp { get; set; }
        public bool NSFW { get; set; }

        public string SafeUrl
        {
            get
            {
                if (NSFW)
                    return "[NSFW] " + Link;

                return Link;
            }
        }
    }
}