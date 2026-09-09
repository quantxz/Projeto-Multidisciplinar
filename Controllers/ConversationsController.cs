using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoMultidiciplinar.Services;

namespace ProjetoMultidiciplinar.Controllers
{
    [ApiController]
    [Route("conversation")]
    public class ConversationsController : ControllerBase
    {
        private readonly MessagesService _messageService;
        public ConversationsController(MessagesService messagesService)
        {
            _messageService = messagesService;
        }


        [Authorize]
        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessages(Guid conversationId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdString, out var userId))
                return Unauthorized();

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
    }
}