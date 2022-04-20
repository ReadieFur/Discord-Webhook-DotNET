using Newtonsoft.Json;

namespace DiscordWebhook
{
    public class Mention
    {
        #region parse
        private List<string> _parse = new List<string>(3);
        public string[] parse
        {
            get { return _parse.ToArray(); }
            set
            {
                List<string> list = value.ToList();
                if (list.Count != list.Distinct().Count()) { throw new Exception("Duplicate found. Entries must be unique."); }
                else if (list.Count > 100) { throw new Exception("Maximum number of entries allowed are 100."); }
                _parse = list;
            }
        }

        public enum EParseOptions
        {
            roles,
            users,
            everyone
        }

        private string GetParseString(EParseOptions option)
        {
            string parseString;
            switch (option)
            {
                case EParseOptions.roles:
                    parseString = "roles";
                    break;
                case EParseOptions.users:
                    parseString = "users";
                    break;
                case EParseOptions.everyone:
                    parseString = "everyone";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return parseString;
        }

        public void AddParse(EParseOptions option)
        {
            string parseString = GetParseString(option);
            if (_parse.Contains(parseString)) { throw new Exception($"Parse option '{parseString}' already exists."); }
            _parse.Add(parseString);
        }

        public void RemoveParse(EParseOptions option)
        {
            string parseString = GetParseString(option);
            int index = _parse.IndexOf(parseString);
            if (index == -1) { throw new Exception($"Option '{parseString}' not set."); }
            _parse.RemoveAt(index);
        }

        public string[] GetParse()
        {
            return _parse.ToArray();
        }
        #endregion

        #region roles
        [JsonIgnore]
        private List<long> _roles = new List<long>(100);
        public long[] roles
        {
            get { return _roles.ToArray(); }
            set
            {
                List<long> list = value.ToList();
                if (list.Count != list.Distinct().Count()) { throw new Exception("Duplicate found. Entries must be unique."); }
                else if (list.Count > 100) { throw new Exception("Maximum number of entries allowed are 100."); }
                _roles = list;
            }
        }

        public void AddRole(long id)
        {
            if (_roles.Count() >= 100) { throw new Exception("Maximum number of IDs added."); }
            else if (_roles.Contains(id)) { throw new Exception($"ID '{id}' already exists in list."); }
            _roles.Add(id);
        }

        public void RemoveRole(long id)
        {
            int index = _roles.IndexOf(id);
            if (index == -1) { throw new Exception($"ID {id} not set."); }
            _roles.RemoveAt(index);
        }

        public long[] GetRoles()
        {
            return _roles.ToArray();
        }
        #endregion

        #region users
        [JsonIgnore]
        private List<long> _users = new List<long>(100);
        public long[] users
        {
            get { return _users.ToArray(); }
            set
            {
                List<long> list = value.ToList();
                if (list.Count != list.Distinct().Count()) { throw new Exception("Duplicate found. Entries must be unique."); }
                else if (list.Count > 100) { throw new Exception("Maximum number of entries allowed are 100."); }
                _users = list;
            }
        }

        public void AddUser(long id)
        {
            if (_users.Count() >= 100) { throw new Exception("Maximum number of IDs added."); }
            else if (_users.Contains(id)) { throw new Exception($"ID '{id}' already exists in list."); }
            _users.Add(id);
        }

        public void RemoveUser(long id)
        {
            int index = _users.IndexOf(id);
            if (index == -1) { throw new Exception($"ID {id} not set."); }
            _users.RemoveAt(index);
        }
        #endregion

        [JsonProperty("replied_user")]
        public bool repliedUser = false;
    }
}
