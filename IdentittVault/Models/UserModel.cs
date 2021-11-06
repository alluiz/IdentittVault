using IdentittVault.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace IdentittVault.Models
{
    public class UserModel: Model
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public override User ToEntity()
        {
            User entity = new()
            {
                Id = this.Id,
                Name = this.Name,
                Email = this.Email,
                Password = this.Password
            };

            return entity;
        }
    }
}
