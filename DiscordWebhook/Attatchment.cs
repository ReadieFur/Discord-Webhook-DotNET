using Newtonsoft.Json;

namespace DiscordWebhook
{
    public class Attatchment
    {
        public long id;
        public string filename;
        public string? description;
        [JsonProperty("content_type")]
        public string? contentType;
        public long size;
        public string url;
        [JsonProperty("proxy_url")]
        public string proxyUrl;
        public uint? height;
        public uint? width;
        public bool? ephemeral;

        public Attatchment(long _id, string _filename, long _size, string _url, string _proxyUrl)
        {
            id = _id;
            filename = _filename;
            size = _size;
            url = _url;
            proxyUrl = _proxyUrl;
        }
    }
}
