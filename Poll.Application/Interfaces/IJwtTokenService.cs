using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string userName, IEnumerable<string> roles);
    }
}
