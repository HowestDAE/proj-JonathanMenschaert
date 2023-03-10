using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.LIB
{

    public abstract class BaseCard
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        //[JsonProperty(PropertyName = "subtypes")]
        //public string[] SubTypes { get; set; }

        [JsonProperty(PropertyName = "supertype")]
        public string SuperType { get; set; }

        //[JsonProperty(PropertyName = "level")]
        //public int Level{ get; set; }

        //[JsonProperty(PropertyName = "hp")]
        //public int Hp { get; set; }

        //[JsonProperty(PropertyName = "types")]
        //public string[] Types { get; set; }


    }
}
