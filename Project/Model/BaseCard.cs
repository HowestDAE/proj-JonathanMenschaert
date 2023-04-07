using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace Project.Model
{
    public abstract class BaseCard : ObservableObject
    {
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "supertype")]
        public string SuperType { get; set; }

        [JsonProperty(PropertyName = "subtypes")]
        public List<string> SubTypes { get; set; }

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
