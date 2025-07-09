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
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("Id")?.Value;
                return Guid.TryParse(userIdClaim, out var id) ? id : Guid.Empty;
            }
        }

        public string UserName
        {
            get
            {
                var nameClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("Username")?.Value;
                return nameClaim ?? string.Empty;
            }
        }
        public Guid? WorkSpaceId
        {
            get
            {
                var workspaceClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("WorkSpaceId")?.Value;
                return Guid.TryParse(workspaceClaim, out var id) ? id : null;
            }
        }

        public Guid? StationId
        {
            get
            {
                var stationClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("StationId")?.Value;
                return Guid.TryParse(stationClaim, out var id) ? id : null;
            }
        }

        public Guid? CompanyId
        {
            get
            {
                var companyClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId")?.Value;
                return Guid.TryParse(companyClaim, out var id) ? id : null;
            }
        }


        public Guid CreatedById => UserId;

        public Guid UpdatedById => UserId;

        private Guid? GetGuidFromClaimOrHeader(string claimName, string headerName)
        {
            var claimValue = _httpContextAccessor.HttpContext?.User?.FindFirst(claimName)?.Value;
            if (Guid.TryParse(claimValue, out var fromClaim))
                return fromClaim;

            var headerValue = _httpContextAccessor.HttpContext?.Request?.Headers[headerName].FirstOrDefault();
            if (Guid.TryParse(headerValue, out var fromHeader))
                return fromHeader;

            return null;
        }
    }
}
