using IdentittVault.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace IdentittVault.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(PrivateKeyPlainHash), IsUnique = true)]
    [Index(nameof(PublicKeyPlainHash), IsUnique = true)]
    public class User: Entity
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string HashPassword { get; set; }
        
        [NotMapped]
        public string Password { get; set; }

        public List<Account> Accounts { get; set; }

        [Required]
        public byte[] PrivateKeyCrypt { get; set; }

        [Required]
        public byte[] PublicKeyPlain { get; set; }
        
        [Required]
        public string PrivateKeyPlainHash { get; set; }
        
        [Required]
        public string PublicKeyPlainHash { get; set; }

        public override UserModel ToModel(bool expand = false)
        {
            UserModel model = new UserModel();
            model.Id = this.Id;

            if (expand)
            {
                model.Email = this.Email;
                model.Name = this.Name;
            }

            return model;
        }
    }
}
