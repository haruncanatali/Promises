using MediatR;
using Microsoft.AspNetCore.Identity;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Models;
using Promises.Domain.Identity;

namespace Promises.Application.Roles.Commands.AddToRole;

public class AddToRoleCommand : IRequest<BaseResponseModel<Unit>>
{
    public long RoleId { get; set; }
    public long UserId { get; set; }
    
    public class Handler : IRequestHandler<AddToRoleCommand, BaseResponseModel<Unit>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public Handler(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<BaseResponseModel<Unit>> Handle(AddToRoleCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userManager.FindByIdAsync(request.UserId.ToString());

            if (user != null)
            {
                Role? role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
                if (role != null)
                {
                    await _userManager.AddToRoleAsync(user, role.Name!);
                }
                else
                {
                    Role? normalRole = await _roleManager.FindByNameAsync("Normal");
                    if (normalRole != null)
                    {
                        await _userManager.AddToRoleAsync(user, "Normal");
                    }
                    else
                    {
                        await _roleManager.CreateAsync(new Role
                        {
                            Name = "Normal"
                        });
                        await _userManager.AddToRoleAsync(user, "Normal");
                    }
                }
                
                return BaseResponseModel<Unit>.Success(Unit.Value, "Success");
            }

            throw new BadRequestException("Invalid UserID.");
        }
    }
}