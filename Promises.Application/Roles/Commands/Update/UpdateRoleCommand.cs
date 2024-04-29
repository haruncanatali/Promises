using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Models;
using Promises.Domain.Identity;

namespace Promises.Application.Roles.Commands.Update;

public class UpdateRoleCommand : IRequest<BaseResponseModel<Unit>>
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    public class Handler : IRequestHandler<UpdateRoleCommand, BaseResponseModel<Unit>>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<UpdateRoleCommand> _logger;

        public Handler(RoleManager<Role> roleManager, ILogger<UpdateRoleCommand> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<BaseResponseModel<Unit>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            Role? role = await _roleManager.FindByIdAsync(request.Id.ToString());

            if (role == null)
            {
                _logger.LogCritical($"Güncellenmek istenen rol bulunamadı. ID:{request.Id}");
                throw new BadRequestException($"Güncellenmek istenen rol bulunamadı. ID:{request.Id}");
            }

            role.Name = request.Name;
            await _roleManager.UpdateAsync(role);
            
            _logger.LogCritical($"Rol başarıyla güncellendi. ID:{request.Id}");
            return BaseResponseModel<Unit>.Success(Unit.Value,$"Rol başarıyla güncellendi. ID:{request.Id}");
        }
    }
}