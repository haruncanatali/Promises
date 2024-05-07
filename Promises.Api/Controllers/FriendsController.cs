using MediatR;
using Microsoft.AspNetCore.Mvc;
using Promises.Application.Common.Models;
using Promises.Application.Friends.Commands.Approve;
using Promises.Application.Friends.Commands.Create;
using Promises.Application.Friends.Commands.CreateBlock;
using Promises.Application.Friends.Commands.Delete;
using Promises.Application.Friends.Commands.DeleteBlock;
using Promises.Application.Friends.Queries.GetBlockedFriends;
using Promises.Application.Friends.Queries.GetFriends;
using Promises.Application.Friends.Queries.GetNotFriends;

namespace Promises.Api.Controllers
{
    public class FriendsController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<BaseResponseModel<GetFriendsVm>>> List([FromQuery] bool approved, string? fullName)
        {
            return Ok(await Mediator.Send(new GetFriendsQuery
            {
                Approved = approved,
                FullName = fullName
            }));
        }

        [HttpGet("NotFriends")]
        public async Task<ActionResult<BaseResponseModel<GetNotFriendsVm>>> GetNotFriends([FromQuery] string? fullName)
        {
            return Ok(await Mediator.Send(new GetNotFriendsQuery { UserFullName = fullName}));
        }

        [HttpGet("BlockedUsers")]
        public async Task<ActionResult<BaseResponseModel<GetNotFriendsVm>>> GetBlockedUsers([FromQuery] string? fullName)
        {
            return Ok(await Mediator.Send(new GetBlockedFriendsQuery { UserFullName = fullName }));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<BaseResponseModel<Unit>>> Create([FromForm] CreateFriendCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost("CreateBlock")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<BaseResponseModel<Unit>>> CreateBlock([FromForm] CreateBlockCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<BaseResponseModel<Unit>>> Update([FromForm] ApproveFriendCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut("DeleteBlock")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<BaseResponseModel<Unit>>> Update([FromForm] DeleteBlockCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseResponseModel<Unit>>> Delete([FromQuery]long friendId, long userId)
        {
            await Mediator.Send(new DeleteFriendCommand { FriendId = friendId, UserId = userId});
            return NoContent();
        }
    }
}
