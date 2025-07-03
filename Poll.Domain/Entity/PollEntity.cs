using System;
using System.Collections.Generic;

namespace Poll.Domain.Entity
{
    public class PollEntity : BaseEntity
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Question { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool AllowMultipleAnswers { get; set; }
        public ICollection<PollOption> Options { get; set; } = new List<PollOption>();
    }
}
