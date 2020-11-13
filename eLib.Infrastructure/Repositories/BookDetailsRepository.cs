using eLib.Entities;
using eLib.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Infrastructure.Repositories
{
    public class BookDetailsRepository : BaseRepository<BookDetails,int>, IBookDetailsRepository
    {
        public BookDetailsRepository(AppDbContext context) : base(context)
        {
        }
    }
}
