using IdentittVault.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentittVault.Entities
{
    public class Account : Entity
    {
        public User User { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [Required]
        public string CypherPassword { get; set; }

        public override AccountModel ToModel(bool expand = false)
        {
            AccountModel model = new();
            model.Id = this.Id;
            model.UserId = this.UserId;
            
            if (expand)
            {
                model.Username = this.Username;
                model.Name = this.Name;
                model.Password = this.Password;
            }

            return model;
        }
    }
}
