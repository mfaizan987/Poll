using Microsoft.AspNetCore.Http;
using Poll.Application.Interfaces;
using System.Security.Claims;

namespace Poll.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;
                return userIdClaim != null ? Guid.Parse(userIdClaim) : Guid.Empty;
            }
        }

        public string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
            }
        }

        public Guid? WorkSpaceId => GetGuidFromHeader("x-workspace-id");

        public Guid? StationId => GetGuidFromHeader("x-station-id");

        public Guid? CompanyId => GetGuidFromHeader("x-company-id");

        private Guid? GetGuidFromHeader(string headerName)
        {
            var headerValue = _httpContextAccessor.HttpContext?.Request?.Headers[headerName].FirstOrDefault();
            if (Guid.TryParse(headerValue, out var parsed))
                return parsed;
            return null;
        }
    }
}
