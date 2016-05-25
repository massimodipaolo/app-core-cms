using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bom.Models
{
    interface IIdentifiable<TKey> 
    {
        TKey Id { get; set; }            
    }

    public class Identifiable<TKey> : IIdentifiable<TKey>
    {
        public virtual TKey Id { get; set; }                
    }

}
