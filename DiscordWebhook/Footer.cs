using Newtonsoft.Json;

namespace DiscordWebhook
{
    public class Footer
    {
        public string text;
        [JsonProperty("icon_url")]
        public string? iconUrl;
        [JsonProperty("proxy_icon_url")]
        public string? proxy_icon_url;

        public Footer(string _text)
        {
            text = _text;
        }
    }
}
