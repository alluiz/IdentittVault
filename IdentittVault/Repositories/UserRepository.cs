using IdentittVault.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentittVault.Repositories
{
    public class UserRepository : CrudRepository<User>, IUserRepository
    {
        public UserRepository(IdentittVaultContext context) : base(context)
        {
        }

        public IEnumerable<User> ReadByEmail(string email, ReadOperation operation)
        {
            return operation switch
            {
                ReadOperation.Equal => DbSet.AsNoTracking().Where(i => i.Email.Equals(email)),
                ReadOperation.Like => DbSet.AsNoTracking().Where(i => EF.Functions.Like(i.Email, $"%{email}%")),
                _ => throw new ArgumentOutOfRangeException(nameof(operation), "The operation must be valid."),
            };
        }
    }
}
