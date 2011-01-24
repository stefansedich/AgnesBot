using Newtonsoft.Json;

namespace AgnesBot.Modules.GitHubModule.Domain
{
    public class Repository
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}