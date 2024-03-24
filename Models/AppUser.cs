using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    [Table("AppUser")]
    public class AppUser : IdentityUser
    {
        public List<Portfolios> Portfolios { get; set; } = new List<Portfolios>();
    }
}