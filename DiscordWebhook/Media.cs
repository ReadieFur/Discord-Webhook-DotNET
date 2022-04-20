using Newtonsoft.Json;

namespace DiscordWebhook
{
    public class Media
    {
        public string url;
        [JsonProperty("proxy_url")]
        public string? proxyUrl;
        public uint? height;
        public uint? width;

        public Media(string _url)
        {
            url = _url;
        }
    }
}
