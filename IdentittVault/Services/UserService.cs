using IdentittVault.Entities;
using IdentittVault.Repositories;
using IdentittVault.System.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IdentittVault.Services
{
    public class UserService : CrudService<User, ICrudRepository<User>>, IUserService
    {
        private readonly IdentittVaultSecure secure;
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository,
            IdentittVaultSecure secure) : base(userRepository)
        {
            this.secure = secure;
            this.userRepository = userRepository;
        }

        protected override void OnCreate(User user)
        {
            User existentUser = ReadByEmail(user.Email, ReadOperation.Equal)
                .FirstOrDefault();

            if (existentUser != null)
                ConflictValue(nameof(user.Email), user.Email);

            CreateHashPassword(user);
            CreateKeyPair(user);
        }

        protected override void OnDelete(User entity)
        {
            //entity.Keys = await keysRepository.ReadAsync(entity.KeysId);
            //entity.Keys.PublicKey = await keyRepository.ReadAsync(entity.Keys.PublicKeyId);
            //entity.Keys.PrivateKey = await keyRepository.ReadAsync(entity.Keys.PrivateKeyId);

            //await ke
        }

        private void CreateKeyPair(User user)
        {
            Dictionary<string, byte[]> keys = secure.GenerateRSAKeys();

            user.PublicKeyPlain = keys["public"];
            user.PublicKeyPlainHash = secure.ComputeSha256Hash(user.PublicKeyPlain);

            user.PrivateKeyCrypt = secure.EncryptWithAES(keys["private"]);
            user.PrivateKeyPlainHash = secure.ComputeSha256Hash(keys["private"]);
        }

        public PasswordResult ValidatePasswordStrength(string password)
        {
            return this.secure.ValidatePasswordStrength(password);
        }

        private void CreateHashPassword(User user)
        {
            user.HashPassword = secure.ComputeSha256Hash(user.Password);
            user.Password = null;
        }

        public List<User> ReadByEmail(string email, ReadOperation operation)
        {
            List<User> entities = userRepository.ReadByEmail(email, operation).ToList();
            OnRead(entities);

            return entities;
        }
    }
}
