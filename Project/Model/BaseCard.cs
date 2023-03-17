using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Project.Model
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

        [JsonIgnore]
        public Uri BigImage
        {
            get
            {
                return new Uri($"https://images.pokemontcg.io/{Id.Replace('-', '/')}_hires.png", UriKind.Absolute);
            }
        }

        [JsonIgnore]
        public Uri SmallImage
        {
            get
            {
                return new Uri($"https://images.pokemontcg.io/{Id.Replace('-', '/')}.png", UriKind.Absolute);
            }
        }

    }
}
