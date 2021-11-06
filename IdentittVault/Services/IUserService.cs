using IdentittVault.Entities;
using IdentittVault.Repositories;
using IdentittVault.System.Security;
using System.Collections.Generic;

namespace IdentittVault.Services
{
    public interface IUserService: ICrudService<User>
    {
        List<User> ReadByEmail(string email, ReadOperation operation);
        PasswordResult ValidatePasswordStrength(string password);
    }
}