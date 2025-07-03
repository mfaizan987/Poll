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
    }
}
