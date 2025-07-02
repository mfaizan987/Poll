using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Domain.Entity
{
    public class PollEntity : BaseEntity
    {
        public string Question { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool AllowMultipleAnswers { get; set; }
        public ICollection<PollOption> Options { get; set; } = new List<PollOption>();
    }
}
