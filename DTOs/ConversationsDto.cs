using System;

namespace ProjetoMultidiciplinar.DTOs
{
    public class ConversationDto
    {
        public Guid ID { get; set; }

        public UserConversationDto OtherUser { get; set; } = null!;

        public MessagesDto? LastMessage { get; set; }
    }

    public class UserConversationDto
    {
        public Guid ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? PhotoUrl { get; set; }
    }
}