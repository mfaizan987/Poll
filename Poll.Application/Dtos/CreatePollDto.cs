using Poll.Application.Dtos;

public class CreatePollDto : BaseDto
{
    public string Question { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool AllowMultipleAnswers { get; set; }
    public List<CreatePollOptionDto> Options { get; set; } = new();
}
