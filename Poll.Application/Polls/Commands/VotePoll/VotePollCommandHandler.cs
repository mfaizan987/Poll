using AutoMapper;
using MediatR;
using Poll.Application.DTOs.Poll;
using Poll.Application.IRepositories;
using Poll.Application.Interfaces;
using Poll.Domain.Entity;

namespace Poll.Application.Polls.Commands.VotePoll
{
    public class VotePollCommandHandler : IRequestHandler<VotePollCommand, PollDto>
    {
        private readonly IPollRepository _pollRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public VotePollCommandHandler(
            IPollRepository pollRepository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _pollRepository = pollRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<PollDto> Handle(VotePollCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId == Guid.Empty)
                throw new UnauthorizedAccessException("Invalid token - user id missing.");

            var option = await _pollRepository.GetPollOptionByIdAsync(request.PollVote.OptionId, cancellationToken);

            if (option == null)
                throw new Exception("Option not found.");

            var vote = new PollVote
            {
                PollOptionId = option.Id,
                UserId = userId,
                Created_At = DateTime.UtcNow
            };

            await _pollRepository.VotePollAsync(vote, cancellationToken);

            var poll = await _pollRepository.GetPollByIdAsync(option.PollId, cancellationToken);
            if (poll == null)
                throw new Exception("Poll not found.");

            return _mapper.Map<PollDto>(poll);
        }
    }
}
