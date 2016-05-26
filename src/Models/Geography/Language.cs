using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bom.Models.Geography
{
    public class Language : Identifiable<string>
    {
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
