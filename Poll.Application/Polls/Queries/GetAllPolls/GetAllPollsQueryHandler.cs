using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Poll.Application.Dtos;
using Poll.Application.IRepositories;
using Poll.Application.Polls.Queries.GetAllPolls;

public class GetAllPollsQueryHandler : IRequestHandler<GetAllPollsQuery, List<PollListDto>>
{
    private readonly IPollRepository _pollRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllPollsQueryHandler(IPollRepository pollRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _pollRepository = pollRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<PollListDto>> Handle(GetAllPollsQuery request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var workspaceId = GetGuidFromHeader(httpContext, "x-workspace-id");
        var stationId = GetGuidFromHeader(httpContext, "x-station-id");
        var companyId = GetGuidFromHeader(httpContext, "x-company-id");

        var polls = await _pollRepository.GetAllPollsAsync(cancellationToken);

        return _mapper.Map<List<PollListDto>>(polls);
    }

    private Guid? GetGuidFromHeader(HttpContext? httpContext, string headerName)
    {
        var headerValue = httpContext?.Request.Headers[headerName].FirstOrDefault();
        return Guid.TryParse(headerValue, out var parsed) ? parsed : null;
    }
}
