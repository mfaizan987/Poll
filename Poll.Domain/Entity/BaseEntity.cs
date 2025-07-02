using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poll.Domain.Entity
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime Created_At { get; set; } = DateTime.UtcNow;
        public DateTime Updated_At { get; set; } = DateTime.UtcNow;
        public Guid? CreatedById { get; set; }
        public Guid? UpdatedById { get; set; }
        public bool IsDeleted { get; set; } = false;
        public Guid? WorkSpaceId { get; set; }
        public Guid? StationId { get; set; }
        public Guid? CompanyId { get; set; }



    }
}
