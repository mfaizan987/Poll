using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poll.Application.DTOs.Poll;
using Poll.Application.IRepositories;
using Poll.Application.Polls.Queries.GetPollById;

public class GetPollByIdQueryHandler : IRequestHandler<GetPollByIdQuery, PollDto?>
{
    private readonly IPollRepository _pollRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetPollByIdQueryHandler(IPollRepository pollRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _pollRepository = pollRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PollDto?> Handle(GetPollByIdQuery request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var workspaceId = GetGuidFromHeader(httpContext, "x-workspace-id");
        var stationId = GetGuidFromHeader(httpContext, "x-station-id");
        var companyId = GetGuidFromHeader(httpContext, "x-company-id");

        var pollEntity = await _pollRepository.GetPollByIdAsync(request.PollId, cancellationToken);

        return pollEntity != null ? _mapper.Map<PollDto>(pollEntity) : null;
    }

    private Guid? GetGuidFromHeader(HttpContext? httpContext, string headerName)
    {
        var headerValue = httpContext?.Request.Headers[headerName].FirstOrDefault();
        return Guid.TryParse(headerValue, out var parsed) ? parsed : null;
    }
}
