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

        public async Task EntrarNaSala(Guid roomId)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                roomId.ToString()
            );

            await Clients.Caller.SendAsync(
                "EntrouNaSala",
                roomId
            );
        }

        public async Task SairDaSala(Guid roomId)
        {
            await Groups.RemoveFromGroupAsync(
                Context.ConnectionId,
                roomId.ToString()
            );

            await Clients.Caller.SendAsync(
                "SaiuDaSala",
                roomId
            );
        }
        public async Task SendMessage(Guid roomId, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new HubException("A mensagem não pode estar vazia.");
            }

            var userId = Context.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userId, out var authorId))
            {
                throw new HubException("Usuário não autenticado.");
            }

            var userName = Context.User?
                .FindFirst(ClaimTypes.Name)?.Value;

            var messageDto = new MessagesDto
            {
                ID = Guid.NewGuid(),
                AuthorId = authorId,
                Content = message,
                SentAt = DateTime.UtcNow,
                RoomId = roomId,
                UserName = userName
            };

            await _messagesService.SaveMessages(messageDto);

            await Clients.Group(roomId.ToString()).SendAsync(
                "ReceiveMessage",
                messageDto
            );

        }
    }
}