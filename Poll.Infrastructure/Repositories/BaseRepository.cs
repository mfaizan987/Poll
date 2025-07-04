using Microsoft.AspNetCore.Http;
using Poll.Domain.Entity;
using Poll.Infrastructure.Data;

public class BaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly AppDbContext _context;
    private readonly IHttpContextAccessor _http;

    public BaseRepository(AppDbContext context, IHttpContextAccessor http)
    {
        _context = context;
        _http = http;
    }
    protected IQueryable<TEntity> FilteredQuery()
    {
        var workspaceId = GetGuidFromHeader("x-workspace-id");
        var stationId = GetGuidFromHeader("x-station-id");
        var companyId = GetGuidFromHeader("x-company-id");

        return _context.Set<TEntity>().Where(x =>
            !x.IsDeleted &&
            (workspaceId == null || x.WorkSpaceId == workspaceId) &&
            (stationId == null || x.StationId == stationId) &&
            (companyId == null || x.CompanyId == companyId)
        );
    }
    private Guid? GetGuidFromHeader(string headerName)
    {
        var headerValue = _http.HttpContext?.Request.Headers[headerName].FirstOrDefault();
        if (Guid.TryParse(headerValue, out var parsed))
            return parsed;
        return null;
    }
}
