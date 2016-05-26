using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bom.Models.Geography
{
    public class Currency: Identifiable<string>
    {
        public string Name { get; set; }
    }
}
