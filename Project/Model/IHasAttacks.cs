using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public interface IHasAttacks
    {
        List<Attack> Attacks { get; set; }
        List<Ability> Abilities { get; set; }
    }
}
