using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMultidiciplinar.Models
{
    public class ConversationsModel
    {
        public Guid ID { get; set; }

        public Guid User1Id { get; set; }
        
        public Guid User2Id { get; set; }

        public ICollection<MessagesModel> Messages { get; set; }
            = new List<MessagesModel>();
    }
}