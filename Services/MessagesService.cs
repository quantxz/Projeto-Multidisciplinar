using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
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

        // Lista todas as conversas de um usuário
        public async Task<List<ConversationDto>> GetConversations(Guid userId)
        {
            var conversations = await _context.Conversations
                .Where(c =>
                    c.User1Id == userId ||
                    c.User2Id == userId
                )
                .ToListAsync();

            var result = new List<ConversationDto>();

            foreach (var conversation in conversations)
            {
                var otherUserId =
                    conversation.User1Id == userId
                        ? conversation.User2Id
                        : conversation.User1Id;

                var otherUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.ID == otherUserId);

                if (otherUser == null)
                    continue;

                var lastMessage = await _context.Messages
                    .Where(m =>
                        m.ConversationId == conversation.ID
                    )
                    .OrderByDescending(m => m.SentAt)
                    .FirstOrDefaultAsync();

                result.Add(new ConversationDto
                {
                    ID = conversation.ID,

                    OtherUser = new UserConversationDto
                    {
                        ID = otherUser.ID,
                        Name = otherUser.Name,
                        PhotoUrl = otherUser.PhotoUrl
                    },

                    LastMessage = lastMessage == null
                        ? null
                        : new MessagesDto
                        {
                            ID = lastMessage.ID,
                            AuthorId = lastMessage.AuthorId,
                            ConversationId = lastMessage.ConversationId,
                            Content = lastMessage.Content,
                            SentAt = lastMessage.SentAt,
                            FileUrl = lastMessage.fileUrl
                        }
                });
            }

            return result;
        }

        public async Task<ConversationsModel?> GetConversation(
            Guid conversationId)
        {
            return await _context.Conversations
                .FirstOrDefaultAsync(
                    c => c.ID == conversationId
                );
        }

        public async Task<List<MessagesDto>> GetMessages(
            Guid conversationId)
        {
            return await _context.Messages
                .Where(m =>
                    m.ConversationId == conversationId
                )
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
    }
}