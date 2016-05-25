using bom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bom.Data.Repository
{
    interface IRepository<T, TKey> where T : class, IIdentifiable<TKey>
    {
        IQueryable<T> All { get; }
        //IQueryable<T> AllEager(params Expression<Func<T, object>>[] includes);
        T Find(TKey id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(TKey id);
    }
}
