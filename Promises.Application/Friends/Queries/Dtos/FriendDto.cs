using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Promises.Application.Common.Mappings;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;

namespace Promises.Application.Friends.Queries.Dtos
{
    public class FriendDto
    {
        public long UserId { get; set; }
        public string UserFullName { get; set; }
        public DateTime FriendshipDuration { get; set; }
    }
}
