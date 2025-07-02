using MediatR;
using Microsoft.AspNetCore.Mvc;
using Poll.Application.DTOs.Poll;
using Poll.Application.Polls.Commands.CreatePoll;
using Poll.Application.Polls.Commands.VotePoll;
using Poll.Application.Polls.Queries.GetAllPolls;
using Poll.Application.Polls.Queries.GetPollById;

namespace Poll.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PollsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePoll([FromBody] PollDto dto)
        {
            var command = new CreatePollCommand { PollDto = dto };
            var createdPoll = await _mediator.Send(command);
            return Ok(createdPoll);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllPolls()
        {
            var query = new GetAllPollsQuery();
            var polls = await _mediator.Send(query);
            return Ok(polls);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPollById(Guid id)
        {
            var query = new GetPollByIdQuery(id);
            var poll = await _mediator.Send(query);
            return Ok(poll);
        }



        // vote
        [HttpPost("vote")]
        public async Task<IActionResult> Vote([FromBody] VotePollCommand command)
        {
            await _mediator.Send(command);
            var updatedPoll = await _mediator.Send(new GetPollByIdQuery(command.PollId));
            return Ok(updatedPoll);
        }

    }
}
