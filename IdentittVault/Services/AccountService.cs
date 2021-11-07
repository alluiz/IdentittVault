using IdentittVault.Entities;
using IdentittVault.Repositories;
using IdentittVault.System.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentittVault.Services
{
    public class AccountService : CrudService<Account, ICrudRepository<Account>>
    {
        private readonly IUserRepository userRepository;

        public AccountService(ICrudRepository<Account> repository, IUserRepository userRepository, IdentittVaultSecure secure) : base(repository, secure)
        {
            this.userRepository = userRepository;
        }

        protected async override void OnCreate(Account account)
        {
            User user = await userRepository.ReadAsync(account.UserId);

            byte[] publicKey = user.PublicKeyPlain;

            account.CypherPassword = secure.EncryptionWithRSAPublicKey(account.Password, publicKey, false);
        }

        protected async override void OnRead(Account account)
        {
            User user = await userRepository.ReadAsync(account.UserId);

            byte[] privateKey = secure.DecryptWithAES(user.PrivateKeyCrypt);

            account.Password = secure.DecryptionWithRSAPrivateKey(account.CypherPassword, privateKey, false);
        }
    }
}
