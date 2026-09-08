using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMultidiciplinar.Models
{
    public class ProductsModel
    {
        public Guid ID { get; set; }
        
        [Required]
        public String Title { get; set; }

        [Required]
        public String Location { get; set; }

        [Required]
        public Guid AuthorId { get; set; }

        public ICollection<PhotosModel> Photos { get; set; }
            = new List<PhotosModel>();
    }
}