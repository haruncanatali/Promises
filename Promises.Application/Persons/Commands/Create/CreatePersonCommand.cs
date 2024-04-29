using MediatR;
using Microsoft.AspNetCore.Http;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Managers;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;
using Promises.Domain.Enums;

namespace Promises.Application.Persons.Commands.Create;

public class CreatePersonCommand : IRequest<BaseResponseModel<Unit>>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public IFormFile? Photo { get; set; }
    
    public class Handler : IRequestHandler<CreatePersonCommand, BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;
        private readonly FileManager _fileManager;

        public Handler(IApplicationContext context, FileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<BaseResponseModel<Unit>> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            await _context.Persons.AddAsync(new Person
            {
                Name = request.Name,
                Surname = request.Surname,
                Age = request.Age,
                Photo = _fileManager.Upload(request.Photo, FileRoot.PersonProfile)
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            
            return BaseResponseModel<Unit>.Success(Unit.Value,"Kişi başarıyla eklendi.");
        }
    }
}