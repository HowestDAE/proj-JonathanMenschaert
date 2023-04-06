using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public class CardType
    {
        public string Class { get; set; }

        [JsonIgnore]
        public Type ActualType
        {
            get
            {
                return Type.GetType($"Project.Model.{Class.Replace("é", "e")}Card");
            }
        }
    }
}
