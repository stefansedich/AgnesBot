using System;

namespace AgnesBot.Modules.QuoteModule.Domain
{
    public class Quote
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
