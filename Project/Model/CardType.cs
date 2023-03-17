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

        public Type ActualType
        {
            get
            {
                return Type.GetType($"Project.Model.{Class}Card");
            }
        }
    }
}
