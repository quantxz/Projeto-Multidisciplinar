using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoMultidiciplinar.Models;

namespace ProjetoMultidiciplinar.DTOs
{
    public class PhotosDto
    {
        public Guid ID { get; set; }

        public string Url { get; set; } = string.Empty;

        public Guid ProductId { get; set; }

        public ProductsModel Product { get; set; } = null!;
    }
}