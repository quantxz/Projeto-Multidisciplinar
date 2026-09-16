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
            public String Email { get; set; }

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