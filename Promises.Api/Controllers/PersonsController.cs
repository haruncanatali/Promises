using MediatR;
using Microsoft.AspNetCore.Mvc;
using Promises.Application.Common.Models;
using Promises.Application.Persons.Commands.Create;
using Promises.Application.Persons.Commands.Delete;
using Promises.Application.Persons.Commands.Update;
using Promises.Application.Persons.Queries.GetPerson;
using Promises.Application.Persons.Queries.GetPersons;

namespace Promises.Api.Controllers;

public class PersonsController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<BaseResponseModel<GetPersonsVm>>> List([FromQuery] string? name, string? surname)
    {
        return Ok(await Mediator.Send(new GetPersonsQuery
        {
            Name = name,
            Surname = surname
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BaseResponseModel<GetPersonVm>>> GetById(long id)
    {
        return Ok(await Mediator.Send(new GetPersonQuery { Id = id }));
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<BaseResponseModel<Unit>>> Create([FromForm] CreatePersonCommand command)
    {
        return Ok(await Mediator.Send(command));
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<BaseResponseModel<Unit>>> Update([FromForm] UpdatePersonCommand command)
    {
        return Ok(await Mediator.Send(command));
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BaseResponseModel<Unit>>> Delete(long id)
    {
        await Mediator.Send(new DeletePersonCommand { Id = id });
        return NoContent();
    }
}