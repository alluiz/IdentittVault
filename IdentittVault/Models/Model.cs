using IdentittVault.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentittVault.Models
{
    public abstract class Model
    {
        public Guid Id { get; set; }

        [Required, StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Name { get; set; }

        public abstract Entity ToEntity();
    }
}
