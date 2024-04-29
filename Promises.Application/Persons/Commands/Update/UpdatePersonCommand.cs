using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Managers;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;
using Promises.Domain.Enums;

namespace Promises.Application.Persons.Commands.Update;

public class UpdatePersonCommand : IRequest<BaseResponseModel<Unit>>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public IFormFile? Photo { get; set; }
    
    public class Handler : IRequestHandler<UpdatePersonCommand, BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;
        private readonly FileManager _fileManager;

        public Handler(IApplicationContext context, FileManager fileManager)
        {
            _context = context;
            _fileManager = fileManager;
        }

        public async Task<BaseResponseModel<Unit>> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            Person? person = await _context.Persons
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (person == null)
                throw new BadRequestException("Güncellenecek kişi bulunamadı.");

            person.Name = request.Name;
            person.Surname = request.Surname;
            person.Age = request.Age;
            person.Photo = _fileManager.Upload(request.Photo, FileRoot.PersonProfile);

            _context.Persons.Update(person);
            await _context.SaveChangesAsync(cancellationToken);
            
            return BaseResponseModel<Unit>.Success(Unit.Value, "Kişi başarıyla güncellendi.");
        }
    }
}