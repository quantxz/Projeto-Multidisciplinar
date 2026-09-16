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

        public String Description { get; set; }

        public String State { get; set; }

        public int Quantity { get; set; }

        public ProductCategory Category { get; set; }

        public UsersModel Author { get; set; }

        public Guid AuthorId { get; set; }

        public ICollection<PhotosModel> Photos { get; set; }
    }

    public class AnnouceDto
    {
        public Guid ID { get; set; }

        public String Title { get; set; }

        public String Location { get; set; }

        public String? Description { get; set; }

        public String State { get; set; }

        public int Quantity { get; set; }

        public ProductCategory Category { get; set; }

        public List<IFormFile>? Images { get; set; }
    }

    public enum ProductCategory
    {
        Eletronicos,
        Moveis,
        Roupas,
        Alimentos,
        MateriaisEscolares,
        Outros
    }

}