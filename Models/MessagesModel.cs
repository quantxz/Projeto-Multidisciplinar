using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMultidiciplinar.Models
{
    public class MessagesModel
    {
        public Guid ID { get; set; }

        [Required]
        public Guid AuthorId { get; set; }
        
        [Required]
        public Guid ConversationId { get; set; }

        [Required]
        public String Content { get; set; }
      
        public String? fileUrl { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }


    }
}