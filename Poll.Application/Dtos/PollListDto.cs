﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Application.Dtos
{
    public class PollListDto
    {
        public Guid Id { get; set; }
        public string Question { get; set; } = "";
        public string? Description { get; set; }
        public bool HasVoted { get; set; }
        public List<Guid> UserVotedOptionId { get; set; } = new();
        public List<string> UserVotedOptionText { get; set; } = new();

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool AllowMultipleAnswers { get; set; }
        public List<PollOptionDto> Options { get; set; } = new();
    }

}
