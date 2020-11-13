using eLib.Entities;
using eLib.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eLib.Infrastructure.Repositories
{
    public class BookHeaderRepository : BaseRepository<BookHeader, int>, IBookHeaderRepository
    {
        public BookHeaderRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<BookHeader> GetAll()
        {
            return _dbSet.Include(x => x.Details).ToList();
        }

        public BookHeader GetById(int id)
        {
            return _dbSet.Where(x => x.Id.Equals(id)).Include(x => x.Details).FirstOrDefault();
        }
    }
}
