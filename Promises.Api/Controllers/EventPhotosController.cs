using MediatR;
using Microsoft.AspNetCore.Mvc;
using Promises.Application.Common.Models;
using Promises.Application.EventPhotos.Commands.Create;
using Promises.Application.EventPhotos.Commands.Delete;
using Promises.Application.EventPhotos.Commands.Update;
using Promises.Application.EventPhotos.Queries.GetEventPhoto;
using Promises.Application.EventPhotos.Queries.GetEventPhotos;

namespace Promises.Api.Controllers;

public class EventPhotosController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<BaseResponseModel<GetEventPhotosVm>>> List([FromQuery] long? agreementId)
    {
        return Ok(await Mediator.Send(new GetEventPhotosQuery()
        {
            AgreementId = agreementId
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponseModel<GetEventPhotoVm>>> GetById(long id)
    {
        return Ok(await Mediator.Send(new GetEventPhotoQuery() { Id = id }));
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<BaseResponseModel<Unit>>> Create([FromForm] CreateEventPhotoCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<BaseResponseModel<Unit>>> Update([FromForm] UpdateEventPhotoCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BaseResponseModel<Unit>>> Delete(long id)
    {
        await Mediator.Send(new DeleteEventPhotoCommand { Id = id });
        return NoContent();
    }
}