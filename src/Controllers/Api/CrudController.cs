using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using bom.Data.Repository;
using bom.Models;

namespace src.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CrudController<T,TKey> : Controller where T : class, IIdentifiable<TKey>
    {        
        private readonly IRepository<T, TKey> _repository;

        public CrudController(IRepository<T, TKey> repository)
        {
            _repository = repository;
        }

        // GET: api/[controller]
        [HttpGet]
        public virtual IEnumerable<T> Get()
        {
            return _repository.All;
        }

        // GET: api/[controller]/5
        [HttpGet("{id}", Name = "Get")]
        public virtual T Get(TKey id)
        {
            return _repository.Find(id);
        }

        // POST: api/[controller]
        [HttpPost]
        public virtual void Post(T value)
        {
            _repository.Insert(value);
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        public virtual void Put(TKey id, T value)
        {
            T entity = _repository.Find(id);
            if (entity != null)
            {
                value.Id = id;

                // Patch
                // ...

                _repository.Update(value);
            } else
            {
                NotFound(value);
            }              
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public virtual void Delete(TKey id)
        {
            _repository.Delete(id);
        }
    }
}
