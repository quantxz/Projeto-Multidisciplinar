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
            try
            {
                Console.WriteLine("========== EVENT HUB ==========");
                Console.WriteLine($"Conversation: {conversationID}");
                Console.WriteLine($"Message: {message}");
                Console.WriteLine($"FileUrl: {fileUrl}");

                if (string.IsNullOrWhiteSpace(message) &&
                    string.IsNullOrWhiteSpace(fileUrl))
                {
                    throw new HubException(
                        "A mensagem precisa conter texto ou arquivo."
                    );
                }

                var userId = Context.User?
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                Console.WriteLine($"UserId: {userId}");

                if (!Guid.TryParse(userId, out var authorId))
                {
                    throw new HubException("Usuário não autenticado.");
                }

                var userName = Context.User?
                    .FindFirst(ClaimTypes.Name)?.Value;

                Console.WriteLine($"UserName: {userName}");

                var conversation = await _messagesService
                    .GetConversation(conversationID);

                Console.WriteLine(
                    $"Conversation encontrada: {conversation != null}"
                );

                if (conversation == null)
                {
                    throw new HubException("Conversa não encontrada.");
                }

                Guid receiverId;

                if (conversation.User1Id == authorId)
                {
                    receiverId = conversation.User2Id;
                }
                else if (conversation.User2Id == authorId)
                {
                    receiverId = conversation.User1Id;
                }
                else
                {
                    throw new HubException(
                        "Você não participa dessa conversa."
                    );
                }

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

                Console.WriteLine("Salvando mensagem...");

                await _messagesService.SaveMessages(newMessage);

                Console.WriteLine("Mensagem salva!");

                await Clients.User(receiverId.ToString())
                    .SendAsync("ReceiveMessage", newMessage);

                await Clients.Caller
                    .SendAsync("ReceiveMessage", newMessage);

                Console.WriteLine("Mensagem enviada aos clientes!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("========== ERRO EVENT HUB ==========");
                Console.WriteLine($"Tipo: {ex.GetType().FullName}");
                Console.WriteLine($"Mensagem: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine("========== INNER EXCEPTION ==========");
                    Console.WriteLine(
                        $"Tipo: {ex.InnerException.GetType().FullName}"
                    );
                    Console.WriteLine(
                        $"Mensagem: {ex.InnerException.Message}"
                    );
                    Console.WriteLine(
                        $"StackTrace: {ex.InnerException.StackTrace}"
                    );
                }

                throw;
            }
        }
    }
}