using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationProject.DTO
{
    public class UserDetailsDTO
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 5)]
        public string Password { get; set; }

    }
}
