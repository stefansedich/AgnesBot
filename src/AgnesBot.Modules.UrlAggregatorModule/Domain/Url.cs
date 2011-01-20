using System;

namespace AgnesBot.Modules.UrlAggregatorModule.Domain
{
    public class Url
    {
        public string Id { get; set; }
        public string Link { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Nsfw { get; set; }

        public string SafeLink
        {
            get
            {
                if (Nsfw)
                    return "[NSFW] " + Link;

                return Link;
            }
        }
    }
}