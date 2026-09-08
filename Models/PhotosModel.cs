using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMultidiciplinar.Models
{
    public class PhotosModel
    {
        public Guid ID { get; set; }

        [Required]
        public string Url { get; set; } = string.Empty;

        public Guid ProductId { get; set; }
    }
}