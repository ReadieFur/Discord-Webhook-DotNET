using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordWebhook
{
    public class Field
    {
        public string name;
        public string value;
        public bool? inline;

        public Field(string _name, string _value)
        {
            name = _name;
            value = _value;
        }
    }
}
