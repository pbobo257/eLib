using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Infrastructure.Abstractions
{
    public interface IRepository<T,TKey>
    {
        T GetById(TKey id);
        IEnumerable<T> GetAll();
        T Insert(T item);
        void Update(T item);
        void Delete(T item);
        void SaveChanges();
    }
}
