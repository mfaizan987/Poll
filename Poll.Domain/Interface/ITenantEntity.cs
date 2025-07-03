using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Domain.Interface
{
    public interface ITenantEntity
    {
        Guid? WorkSpaceId { get; set; }
        Guid? StationId { get; set; }
        Guid? CompanyId { get; set; }
        bool IsDeleted { get; set; }

    }
}
