using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using eLib.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eLib.Infrastructure.Repositories
{
    public abstract class BaseRepository<T, TKey> : IRepository<T, TKey> where T : class, Entities.IEntity<TKey>
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual T GetById(TKey id)
        {
            return _dbSet.FirstOrDefault(x => x.Id.Equals(id));
        }

        public virtual void Delete(T item)
        {
            _dbSet.Remove(item);
        }

        public virtual T Insert(T item)
        {
            var insertedItem = _dbSet.Add(item).Entity;
            return insertedItem;
        }

        public virtual void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
