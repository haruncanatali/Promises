using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Promises.Domain.Base;

namespace Promises.Domain.Entities
{
    public class BlockedFriends : BaseEntity
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
    }
}
