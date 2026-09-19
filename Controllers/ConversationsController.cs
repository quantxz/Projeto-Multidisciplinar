using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ProjetoMultidiciplinar.Services;
using ProjetoMultidiciplinar.DTOs;
using ProjetoMultidiciplinar.Data;
using ProjetoMultidiciplinar.Models;

namespace ProjetoMultidiciplinar.Controllers
{
    [Authorize]
    [ApiController]
    [Route("conversation")]
    public class ConversationsController : ControllerBase
    {

        private readonly MessagesService _messageService;

        private readonly FilesService _fileService;

        private readonly AppDbContext _context;

        public ConversationsController(MessagesService messagesService, FilesService fileService, AppDbContext context)
        {
            _messageService = messagesService;
            _fileService = fileService;
            _context = context;
        }

        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessages(Guid conversationId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var userName = User.FindFirstValue(ClaimTypes.Name);

            var conversation = await _messageService
                .GetConversation(conversationId);

            if (conversation == null)
                return NotFound();

            if (conversation.User1Id != userId &&
                conversation.User2Id != userId)
            {
                return Forbid();
            }

            var messages = await _messageService
                .GetMessages(conversationId);

            return Ok(messages);
        }

        [HttpPost("{conversationId}/file")]
        public async Task<IActionResult> SaveFileInMessage(
            Guid conversationId,
            [FromForm] IFormFile file)
        {
            var userIdString =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

            var conversation =
                await _messageService.GetConversation(conversationId);

            if (conversation == null)
                return NotFound();

            if (conversation.User1Id != userId &&
                conversation.User2Id != userId)
            {
                return Forbid();
            }

            var result = await _fileService.SaveFile(file);
            Console.WriteLine(result);
            return Ok(new
            {
                fileName = result.FileName,
                url = result.Url
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateConversation([FromBody] CreateConversationsDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized();

            if (userId == dto.UserId)
                return BadRequest("Você não pode iniciar uma conversa consigo mesmo.");

            var otherUser = await _context.Users
                .FirstOrDefaultAsync(u => u.ID == dto.UserId);

            if (otherUser == null)
                return NotFound("Usuário não encontrado.");

            var conversation = await _context.Conversations
                .FirstOrDefaultAsync(c =>
                    (c.User1Id == userId && c.User2Id == dto.UserId) ||
                    (c.User1Id == dto.UserId && c.User2Id == userId)
                );

            if (conversation == null)
            {
                conversation = new ConversationsModel
                {
                    ID = Guid.NewGuid(),
                    User1Id = userId,
                    User2Id = dto.UserId
                };

                _context.Conversations.Add(conversation);

                await _context.SaveChangesAsync();
            }

            return Ok(conversation);
        }
    }
}