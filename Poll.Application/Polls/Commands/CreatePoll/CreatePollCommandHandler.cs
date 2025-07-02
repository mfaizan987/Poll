using AutoMapper;
using MediatR;
using Poll.Application.DTOs.Poll;
using Poll.Application.Interfaces;
using Poll.Application.IRepositories;
using Poll.Domain.Entity;

namespace Poll.Application.Polls.Commands.CreatePoll
{
    public class CreatePollCommandHandler : IRequestHandler<CreatePollCommand, PollDto>
    {
        private readonly IPollRepository _pollRepository;
        private readonly IMapper _mapper;

        public CreatePollCommandHandler(IPollRepository pollRepository, IMapper mapper)
        {
            _pollRepository = pollRepository;
            _mapper = mapper;
        }

        public async Task<PollDto> Handle(CreatePollCommand request, CancellationToken cancellationToken)
        {
            var poll = _mapper.Map<PollEntity>(request.PollDto);

            var createdPoll = await _pollRepository.CreatePollAsync(poll, cancellationToken);

            var resultDto = _mapper.Map<PollDto>(createdPoll);

            return resultDto;
        }
    }
}
