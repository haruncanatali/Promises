using MediatR;
using Microsoft.AspNetCore.Mvc;
using Promises.Application.Agreements.Commands.Create;
using Promises.Application.Agreements.Commands.Delete;
using Promises.Application.Agreements.Commands.Update;
using Promises.Application.Agreements.Queries.GetAgreement;
using Promises.Application.Agreements.Queries.GetAgreements;
using Promises.Application.Common.Models;

namespace Promises.Api.Controllers;

public class AgreementsController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<BaseResponseModel<GetAgreementsVm>>> List([FromQuery] long? personId, long? userId, DateTime? date)
    {
        return Ok(await Mediator.Send(new GetAgreementsQuery
        {
            PersonId = personId,
            UserId = userId,
            Date = date
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponseModel<GetAgreementVm>>> GetById(long id)
    {
        return Ok(await Mediator.Send(new GetAgreementQuery { Id = id }));
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<BaseResponseModel<Unit>>> Create([FromForm] CreateAgreementCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<BaseResponseModel<Unit>>> Update([FromForm] UpdateAgreementCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BaseResponseModel<Unit>>> Delete(long id)
    {
        await Mediator.Send(new DeleteAgreementCommand { Id = id });
        return NoContent();
    }
}