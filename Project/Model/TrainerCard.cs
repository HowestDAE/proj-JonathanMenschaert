using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class TrainerCard : BaseCard, IHasRules
    {
        [JsonProperty(PropertyName = "rules")]
        public List<string> Rules { get; set; }
    }
}
