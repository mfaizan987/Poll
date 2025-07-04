using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Application.Dtos
{
    public class CreatePollOptionDto : BaseDto
    {
        public string OptionText { get; set; } = string.Empty;

    }
}
