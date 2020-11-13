using eLib.Entities;
using eLib.Infrastructure.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eLib.Infrastructure.Repositories
{
    public class AccountRepository : BaseRepository<Account, int>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context)
        {

        }

        public Account GetByLogin(string login)
        {
            return _dbSet.FirstOrDefault(x => x.Login.Equals(login));
        }
    }
}
