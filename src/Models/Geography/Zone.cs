using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bom.Models.Geography
{
    public class Zone: Identifiable<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
