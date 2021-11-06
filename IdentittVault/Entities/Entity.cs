using IdentittVault.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentittVault.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        [Required, StringLength(maximumLength: 50, MinimumLength = 3)]
        public string Name { get; set; }

        public abstract Model ToModel(bool expand = false);
    }
}