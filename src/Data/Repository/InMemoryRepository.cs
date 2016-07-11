using bom.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bom.Data.Repository
{
    public class InMemoryRepository<T, TKey>: IRepository<T, TKey> where T: class, IIdentifiable<TKey>
    {
        private List<T> _list;
        private IMemoryCache _memoryCache;        

        public InMemoryRepository(IMemoryCache memoryCache) {
                _memoryCache = memoryCache;
                _list = ((IEnumerable<T>)_memoryCache).ToList();                        
            }

        private List<T> Load()
        {
            var key = $"{nameof(T)}-cache";
            List<T> _res;            
            if (!_memoryCache.TryGetValue(key, out _res))
            { // save                                
                //_res = ... TODO;                                    
                _memoryCache.Set(key, _res,new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(60)));
            }
            else
            {   //retrive
                _res = _memoryCache.Get(key) as List<T>;                
            }
            return _res;
        }

        public IQueryable<T> All
        {
            get
            {
                return _list.AsQueryable();
            }
        }

        public void Delete(TKey id)
        {
            _list.Remove(Find(id));            
        }

        public T Find(TKey id)
        {
            return _list.SingleOrDefault(_ => _.Id.Equals(id));
        }

        public void Insert(T entity)
        {
            _list.Add(entity);                  
        }

        public void Update(T entity)
        {
            var t = Find(entity.Id);
            t = entity;            
        }
    }
}
