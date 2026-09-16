using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoMultidiciplinar.Models;

namespace ProjetoMultidiciplinar.DTOs
{
    public class UserDto
    {
        public Guid ID { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

        public String Password { get; set; }

        public String? PhotoUrl { get; set; } = string.Empty;

        public String? Bio { get; set; } = string.Empty;

        public String? Locale { get; set; } = string.Empty;

    }

    public class UserUpdateProflieDto
    {
        public String? Name { get; set; }

        public IFormFile? PhotoUrl { get; set; }

        public String? Bio { get; set; }

        public String? Locale { get; set; }
    }
}