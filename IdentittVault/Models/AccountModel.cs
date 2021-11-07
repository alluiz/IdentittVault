using IdentittVault.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentittVault.Models
{
    public class AccountModel: Model
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }

        public override Account ToEntity()
        {
            Account entity = new()
            {
                Id = this.Id,
                Name = this.Name,
                UserId = this.UserId,
                Username = this.Username,
                Password = this.Password
            };

            return entity;
        }
    }
}
