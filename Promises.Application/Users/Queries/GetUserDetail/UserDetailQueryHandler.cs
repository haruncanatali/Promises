using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;

namespace Promises.Application.Users.Queries.GetUserDetail
{
    public class UserDetailQueryHandler : IRequestHandler<UserDetailQuery, UserDetailDto>
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public UserDetailQueryHandler(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDetailDto> Handle(UserDetailQuery request, CancellationToken cancellationToken)
        {
            UserDetailDto? user = await _context.Users
                .ProjectTo<UserDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (user == null)
            {
                throw new BadRequestException($"(TKGG-0) Kullanıcı bulunamadı. ID:{request.Id}");
            }
            return user;
        }
    }
}