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
    }
}