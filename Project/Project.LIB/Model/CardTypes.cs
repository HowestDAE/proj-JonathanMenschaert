using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.LIB.Model
{
    public class CardType
    {
        public string Class { get; set; }

        public Type ActualType
        {
            get
            {
                return Type.GetType($"Project.LIB.Model.{Class}Card");
            }
        }
    }
}
