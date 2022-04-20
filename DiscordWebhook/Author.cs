using Newtonsoft.Json;

namespace DiscordWebhook
{
    public class Author
    {
        public string name;
        public string? url;
        [JsonProperty("icon_url")]
        public string? iconUrl;
        [JsonProperty("proxy_icon_url")]
        public string? proxyIconUrl;

        public Author(string _name)
        {
            name = _name;
        }
    }
}
