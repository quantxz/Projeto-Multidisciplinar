using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoMultidiciplinar.Data;
using ProjetoMultidiciplinar.DTOs;
using ProjetoMultidiciplinar.Models;

namespace ProjetoMultidiciplinar.Services
{
    public class MessagesService
    {
        private readonly AppDbContext _context;
        public MessagesService(AppDbContext context)
        {
            _context = context;
        }



        public async Task SaveMessages(MessagesDto messageData)
        {

            var message = new MessagesModel
            {
                ID = messageData.ID,
                AuthorId = messageData.AuthorId,
                Content = messageData.Content,
                SentAt = messageData.SentAt,
                RoomId = messageData.RoomId
            };

            _context.Messages.Add(message);

            await _context.SaveChangesAsync();
        }

    }
}