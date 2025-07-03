using AutoMapper;
using MediatR;
using Poll.Application.DTOs.Poll;
using Poll.Application.Interfaces;
using Poll.Application.IRepositories;
using Poll.Domain.Entity;
using System.Security.Claims;

namespace Poll.Application.Polls.Commands.CreatePoll
{
    public class CreatePollCommandHandler : IRequestHandler<CreatePollCommand, PollDto>
    {
        private readonly IPollRepository _pollRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreatePollCommandHandler(IPollRepository pollRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PollDto> Handle(CreatePollCommand request, CancellationToken cancellationToken)
        {
            var poll = _mapper.Map<PollEntity>(request.PollDto);

            poll.UserId = _currentUserService.UserId;

            poll.UserName = _currentUserService.UserName;

            var createdPoll = await _pollRepository.CreatePollAsync(poll, cancellationToken);

            var resultDto = _mapper.Map<PollDto>(createdPoll);

            return resultDto;
        }
    }
}
