using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMultidiciplinar.Models
{
    public class UsersModel
    {

        public Guid ID { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public String Email { get; set; }

        [MaxLength(255)]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,20}$",
            ErrorMessage = "A senha deve ter: 1 maiúscula, 1 minúscula, 1 número e 1 caractere especial.")]
        [Required]
        public String Password { get; set; }

        public String? PhotoUrl { get; set; } = string.Empty;

        public String? Bio { get; set; } = string.Empty;

        public String? Locale { get; set; } = string.Empty;

        public String? Rating { get; set; } = string.Empty;

        public ICollection<MessagesModel> Messages { get; set; }
            = new List<MessagesModel>();

        public ICollection<ProductsModel> Announcements { get; set; }
            = new List<ProductsModel>();

    }
}