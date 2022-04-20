using Newtonsoft.Json;

namespace DiscordWebhook
{
    public class Webhook
    {
        [JsonIgnore]
        private readonly string webhookUrl;

        public Webhook(long id, string token)
        {
            webhookUrl = $"https://discord.com/api/webhooks/{id}/{token}";
        }

        public string? content;
        public string? username;
        [JsonProperty("avatar_url")]
        public string? avatarUrl;
        public bool? tts;

        #region embeds
        [JsonIgnore]
        private List<Embed> _embeds = new List<Embed>(10);
        public Embed[]? embeds
        {
            get { return _embeds.Count() != 0 ? _embeds.ToArray() : null; }
            set { _embeds = value != null ? value.ToList() : new List<Embed>(10); }
        }

        public void AddEmbed(Embed embed)
        {
            if (_embeds.Count == 10) { throw new Exception("Maximum number of embeds added."); }
            _embeds.Add(embed);
        }

        public void RemoveEmbed(Embed embed)
        {
            int index = _embeds.IndexOf(embed);
            if (index == -1) { throw new Exception("Embed not found."); }
            _embeds.RemoveAt(index);
        }
        #endregion

        [JsonProperty("allowed_mentions")]
        public Mention? allowedMentions;
        //public Components? components; //Requires an application-owned webhook.

        #region files
        [JsonIgnore]
        private List<KeyValuePair<string, byte[]>> _files = new List<KeyValuePair<string, byte[]>>(10);
        [JsonIgnore]
        public KeyValuePair<string, byte[]>[] files
        {
            get { return _files.ToArray(); }
            set
            {
                List<KeyValuePair<string, byte[]>> list = value.ToList();
                List<string> processedKeys = new List<string>();
                foreach (KeyValuePair<string, byte[]> kv in list)
                {
                    if (processedKeys.Contains(kv.Key)) { throw new Exception("Duplicate key found. IDs must be unique."); }
                    processedKeys.Add(kv.Key);
                }
                if (list.Count > 10) { throw new Exception("Maximum number of entries allowed are 10."); }
                _files = list;
            }
        }

        public async Task<string> AddFile(string filePath, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(filePath)) { throw new FileNotFoundException(); }
            return AddFile(await File.ReadAllBytesAsync(filePath, cancellationToken), Path.GetExtension(filePath));
        }

        public string AddFile(byte[] fileBytes, string? fileExtension = null)
        {
            if (_files.Count >= 10) { throw new Exception("Maximum number of files added."); }

            fileExtension = fileExtension != null && !fileExtension.StartsWith('.') ? $".{fileExtension}" : fileExtension;

            string id;
            Random random = new Random();
            do { id = new string(Enumerable.Repeat("1234567890", 10).Select(s => s[random.Next(s.Length)]).ToArray()) + fileExtension; }
            while (_files.Exists(kv => kv.Key == id));

            _files.Add(new KeyValuePair<string, byte[]>(id, fileBytes));

            return id;
        }

        public void RemoveFile(string id)
        {
            int index = _files.FindIndex(kv => kv.Key == id);
            if (index == -1) { throw new KeyNotFoundException(); }
            _files.RemoveAt(index);
        }

        public static string GetAttatchmentURLForFile(string id)
        {
            return $"attachment://{id}";
        }
        #endregion

        #region attatchments
        [JsonIgnore]
        private List<Attatchment> _attatchments = new List<Attatchment>(10);

        public Attatchment[]? attatchments
        {
            get { return _attatchments.Count() != 0 ? _attatchments.ToArray() : null; }
            set { _attatchments = value != null ? value.ToList() : new List<Attatchment>(10); }
        }

        public void AddAttatchment(Attatchment attatchment)
        {
            if (_attatchments.Count == 10) { throw new Exception("Maximum number of attatchments added."); }
            _attatchments.Add(attatchment);
        }

        public void RemoveAttatchment(Attatchment attatchment)
        {
            int index = _attatchments.IndexOf(attatchment);
            if (index == -1) { throw new Exception("Attatchment not found."); }
            _attatchments.RemoveAt(index);
        }
        #endregion

        public int? flags;

        public async Task<HttpResponseMessage> Send(CancellationToken cancellationToken = default)
        {
            if (content == null
                && _embeds.Count() == 0
                && _files.Count() == 0
            ) { throw new Exception("You must provide a value for at least one of the following: content, embeds, or file."); }

            HttpClient httpClient = new HttpClient();
            string payloadJson = JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            HttpResponseMessage response;

            if (_files.Count() > 0 || _attatchments.Count() > 0)
            {
                MultipartFormDataContent form = new MultipartFormDataContent();
                form.Add(new StringContent(payloadJson), "payload_json");
                if (_attatchments.Count() > 0) { form.Add(new StringContent(JsonConvert.SerializeObject(_attatchments)), "attatchments"); }
                for (int i = 0; i < files.Count(); i++)
                {
                    form.Add(new ByteArrayContent(files[i].Value, 0, files[i].Value.Length), $"file{i + 1}", files[i].Key);
                }
                response = await httpClient.PostAsync(webhookUrl, form, cancellationToken);
            }
            else
            {
                response = await httpClient.PostAsync(
                    webhookUrl,
                    new StringContent(payloadJson, System.Text.Encoding.UTF8, "application/json"),
                    cancellationToken
                );
            }
            httpClient.Dispose();

            return response;
        }
    }
}
