using IdentittVault.Entities;
using System.Collections.Generic;

namespace IdentittVault.Repositories
{
    public interface IUserRepository: ICrudRepository<User>
    {
        IEnumerable<User> ReadByEmail(string email, ReadOperation operation);
    }
}