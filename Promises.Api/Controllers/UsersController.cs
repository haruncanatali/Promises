using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Promises.Application.Common.Models;
using Promises.Application.Users.Commands.CreateUser;
using Promises.Application.Users.Commands.DeleteRoleFromUser;
using Promises.Application.Users.Commands.DeleteUser;
using Promises.Application.Users.Commands.UpdateUser;
using Promises.Application.Users.Queries.GetUserDetail;
using Promises.Application.Users.Queries.GetUsersList;

namespace Promises.Api.Controllers;

public class UsersController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<UserListVm>> GetAll([FromQuery] string? fullName)
    {
        return Ok(await Mediator.Send(new GetUserListQuery
        {
            FullName = fullName
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserListVm>> GetById(long id = 0)
    {
        return Ok(await Mediator.Send(new UserDetailQuery { Id = id }));
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<BaseResponseModel<long>>> Create([FromForm] CreateUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<BaseResponseModel<long>>> Update([FromForm] UpdateUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPut]
    [Route("DeleteRoleFromUser")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<BaseResponseModel<long>>> Update([FromForm] DeleteRoleFromUserCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteUserCommand { Id = id });
        return NoContent();
    }
}