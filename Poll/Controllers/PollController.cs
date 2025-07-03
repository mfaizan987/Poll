using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poll.Application.DTOs.Poll;
using Poll.Application.Interfaces;
using Poll.Application.Polls.Commands.CreatePoll;
using Poll.Application.Polls.Commands.VotePoll;
using Poll.Application.Polls.Queries.GetAllPolls;
using Poll.Application.Polls.Queries.GetPollById;

namespace Poll.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PollsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public PollsController(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> CreatePoll([FromBody] PollDto dto)
        {
            if (_currentUserService.UserId == Guid.Empty)
                return Unauthorized("Invalid token - user id missing.");

            dto.UserId = _currentUserService.UserId;
            dto.UserName = _currentUserService.UserName;

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

        [HttpGet("getById{id}")]
        public async Task<IActionResult> GetPollById(Guid id)
        {
            var query = new GetPollByIdQuery(id);
            var poll = await _mediator.Send(query);
            return Ok(poll);
        }

        [HttpPost("Vote")]
        public async Task<IActionResult> Vote([FromBody] VotePollCommand command)
        {
            var updatedPoll = await _mediator.Send(command);
            return Ok(updatedPoll);
        }


    }
}
