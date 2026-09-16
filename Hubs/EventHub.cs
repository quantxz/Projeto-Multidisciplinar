using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ProjetoMultidiciplinar.DTOs;
using ProjetoMultidiciplinar.Services;

namespace ProjetoMultidiciplinar.Hubs
{
    [Authorize]
    public class EventHub : Hub
    {
        private readonly MessagesService _messagesService;

        public EventHub(MessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        public async Task SendMessage(
            Guid conversationID,
            string message,
            string? fileUrl = null,
            string? fileName = null)
        {
            if (string.IsNullOrWhiteSpace(message) &&
                string.IsNullOrWhiteSpace(fileUrl))
            {
                throw new HubException(
                    "A mensagem precisa conter texto ou arquivo."
                );
            }

            var userId = Context.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userId, out var authorId))
                throw new HubException("Usuário não autenticado.");

            var userName = Context.User?
                .FindFirst(ClaimTypes.Name)?.Value;

            var conversation = await _messagesService
                .GetConversation(conversationID);

            if (conversation == null)
                throw new HubException("Conversa não encontrada.");



            Guid receiverId;

            if (conversation.User1Id == authorId)
                receiverId = conversation.User2Id;
            else if (conversation.User2Id == authorId)
                receiverId = conversation.User1Id;
            else
                throw new HubException(
                    "Você não participa dessa conversa."
                );

            var newMessage = new MessagesDto
            {
                ID = Guid.NewGuid(),
                AuthorId = authorId,
                ConversationId = conversationID,
                Content = message,
                SentAt = DateTime.UtcNow,
                UserName = userName,
                FileUrl = fileUrl
            };

            // Salva
            await _messagesService.SaveMessages(newMessage);

            // Envia para o outro usuário
            await Clients.User(receiverId.ToString())
                .SendAsync("ReceiveMessage", newMessage);

            // Opcional: envia para o próprio remetente
            await Clients.Caller
                .SendAsync("ReceiveMessage", newMessage);
        }
    }
}