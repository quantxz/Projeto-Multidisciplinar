using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoMultidiciplinar.Models;

namespace ProjetoMultidiciplinar.DTOs
{
    public class ProductsDto
    {
        public Guid ID { get; set; }

        public String Title { get; set; }

        public String Location { get; set; }

        public UsersModel Author { get; set; }

        public Guid AuthorId { get; set; }

        public ICollection<PhotosModel> Photos { get; set; }
    }

    public class AnnouceDto
    {
        public Guid ID { get; set; }
        public String Title { get; set; }
        public String Location { get; set; }

        public List<IFormFile>? Images { get; set; }
    }
}