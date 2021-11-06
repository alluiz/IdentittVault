using IdentittVault.Entities;
using IdentittVault.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentittVault.Services
{
    public class AccountService : CrudService<Account, ICrudRepository<Account>>
    {
        public AccountService(ICrudRepository<Account> repository) : base(repository)
        {
        }

        protected override void OnCreate(Account user)
        {
            
        }
    }
}
