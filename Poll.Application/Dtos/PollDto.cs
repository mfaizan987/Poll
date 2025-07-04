    using Poll.Application.Dtos;

    namespace Poll.Application.DTOs.Poll
    {
        public class PollDto:BaseDto
        {
            public Guid Id { get; set; }
            public Guid UserId { get; set; }
            public string UserName { get; set; } = string.Empty;
            public string Question { get; set; } = string.Empty;
            public string? Description { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public bool AllowMultipleAnswers { get; set; }
            public List<PollOptionDto> Options { get; set; } = new();
        }
    }
