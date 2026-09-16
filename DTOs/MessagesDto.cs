using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoMultidiciplinar.Models;

namespace ProjetoMultidiciplinar.DTOs
{
    public class MessagesDto
    {
        public Guid ID {get; set;}

        public Guid AuthorId { get; set; }

        public string Content { get; set; }

        public DateTime SentAt  { get; set; }

        public Guid ConversationId { get; set; }

        public String? FileUrl;
        public String? FileName;

        public String? UserName {get; set;}

    }
}