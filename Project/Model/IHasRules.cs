using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model
{
    public interface IHasRules
    {
        List<string> Rules { get; set; }
    }
}
