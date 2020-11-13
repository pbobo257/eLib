using eLib.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace eLib.Infrastructure.Abstractions
{
    public interface IAccountRepository : IRepository<Account,int>
    {
        Account GetByLogin(string login);
    }
}
