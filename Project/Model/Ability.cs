using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class Ability
    {
        [JsonProperty(PropertyName = "name")]
        public string AbilityName { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
