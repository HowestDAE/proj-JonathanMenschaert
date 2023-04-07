using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class PokemonCard : BaseCard, IHasAttacks, IHasType
    {
        [JsonProperty(PropertyName = "attacks")]
        public List<Attack> Attacks { get; set; }

        [JsonProperty(PropertyName = "abilities")]
        public List<Ability> Abilities { get; set; }

        [JsonProperty(PropertyName = "types")]
        public List<string> Types { get; set; }
    }
}
