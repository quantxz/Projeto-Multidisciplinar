using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoMultidiciplinar.Models;

namespace ProjetoMultidiciplinar.DTOs
{
    public class MessagesDto
    {
        public Guid ID { get; set; }

        public Guid AuthorId { get; set; }

        public UsersModel Author { get; set; }

        public String Content { get; set; }
    }
}