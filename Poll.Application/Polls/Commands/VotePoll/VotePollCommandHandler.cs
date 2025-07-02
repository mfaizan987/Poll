using MediatR;
using Poll.Application.Dtos;
using Poll.Application.DTOs.Poll;
using Poll.Application.IRepositories;
using Poll.Domain.Entity;

namespace Poll.Application.Polls.Commands.VotePoll
{
    public class VotePollCommandHandler : IRequestHandler<VotePollCommand, PollDto>
    {
        private readonly IPollRepository _pollRepository;

        public VotePollCommandHandler(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        public async Task<PollDto> Handle(VotePollCommand request, CancellationToken cancellationToken)
        {
            var option = await _pollRepository.GetPollOptionByIdAsync(request.OptionId, cancellationToken);

            if (option == null)
                throw new Exception("Option not found.");

            var vote = new PollVote
            {
                PollOptionId = option.Id,
                Created_At = DateTime.UtcNow
            };

            await _pollRepository.VotePollAsync(vote, cancellationToken);

            var poll = await _pollRepository.GetPollByIdAsync(option.PollId, cancellationToken);

            if (poll == null)
                throw new Exception("Poll not found.");

            var pollDto = new PollDto
            {
                Id = poll.Id,
                Question = poll.Question,
                Options = poll.Options.Select(opt => new PollOptionDto
                {
                    Id = opt.Id,
                    OptionText = opt.OptionText,
                    VoteCount = opt.PollVotes.Count
                }).ToList()
            };

            return pollDto;
        }
    }
}
