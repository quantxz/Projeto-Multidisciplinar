using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ProjetoMultidiciplinar.Data;
using ProjetoMultidiciplinar.DTOs;
using ProjetoMultidiciplinar.Models;

namespace ProjetoMultidiciplinar.Services
{
    [Authorize]
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
                ConversationId = messageData.ConversationId,
                fileUrl = messageData.FileUrl
            };

            _context.Messages.Add(message);

            await _context.SaveChangesAsync();
        }

        public async Task<List<ConversationsModel>> GetConversations(Guid userId)
        {
            return await _context.Conversations
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .ToListAsync();
        }

        public async Task<List<MessagesDto>> GetMessages(Guid conversationId)
        {
            return await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .Join(
                    _context.Users,
                    message => message.AuthorId,
                    user => user.ID,
                    (message, user) => new MessagesDto
                    {
                        ID = message.ID,
                        AuthorId = message.AuthorId,
                        UserName = user.Name,
                        ConversationId = message.ConversationId,
                        Content = message.Content,
                        SentAt = message.SentAt,
                        FileUrl = message.fileUrl
                    }
                )
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<ConversationsModel?> GetConversation(Guid conversationId)
        {
            return await _context.Conversations
                .FirstOrDefaultAsync(c => c.ID == conversationId);
        }
    }
}