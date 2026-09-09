using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMultidiciplinar.Models
{
    public class RoomsModel
    {
        [Required]
        public Guid Id { get; set; }

        public string Name { get; set; }

    }
}