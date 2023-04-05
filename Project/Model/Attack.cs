using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class Attack
    {
        [JsonProperty(PropertyName = "name")]
        public string AttackName { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public List<string> Cost { get; set; }

        [JsonProperty(PropertyName = "damage")]
        public string Damage { get; set; }
    }
}
