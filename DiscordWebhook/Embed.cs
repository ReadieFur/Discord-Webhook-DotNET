using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace DiscordWebhook
{
    public class Embed
    {
        public string? title;
        public const string type = "rich";
        public string? description;
        public string? url;
        #region timestamp
        [JsonIgnore]
        private string? _timestamp;
        public string? timestamp
        {
            get { return _timestamp; }
            set
            {
                if (value == null) { _timestamp = null; }
                //https://stackoverflow.com/questions/12756159/regex-and-iso8601-formatted-datetime
                if (Regex.Match(value, @"^\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(\.\d+)?(([+-]\d\d:\d\d)|Z)?$", RegexOptions.IgnoreCase).Captures.Count == 0)
                { throw new Exception("Must be an ISO8601 timestamp."); }
                _timestamp = value;
            }
        }

        public void SetTimestampFromDateTime(DateTime dateTime)
        {
            timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
        }
        #endregion
        public uint? color;
        public Footer? footer;
        public Media? image;
        public Media? thumbnail;
        public Media? video;
        public Provider? provider;
        public Author? author;
        #region fields
        [JsonIgnore]
        private List<Field> _fields = new List<Field>();
        public List<Field>? fields
        {
            get { return _fields.Count != 0 ? _fields : null; }
            set { _fields = value; }
        }
        #endregion
    }
}
