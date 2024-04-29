using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;

namespace Promises.Application.Persons.Commands.Delete;

public class DeletePersonCommand : IRequest<BaseResponseModel<Unit>>
{
    public long Id { get; set; }
    
    public class handler : IRequestHandler<DeletePersonCommand, BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;

        public handler(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<BaseResponseModel<Unit>> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            Person? person = await _context.Persons
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (person == null)
                throw new BadRequestException("Silinecek kişi bulunamadı.");

            List<Agreement> agreements = await _context.Agreements
                .Where(c => c.PersonId == request.Id)
                .ToListAsync(cancellationToken);

            if (agreements.Count > 0)
            {
                List<long> agreementsIds = agreements.Select(c => c.Id).ToList();
                List<EventPhoto> eventPhotos = await _context.EventPhotos
                    .Where(c => agreementsIds.Contains(c.AgreementId))
                    .ToListAsync(cancellationToken);

                if (eventPhotos.Count > 0)
                {
                    _context.EventPhotos.RemoveRange(eventPhotos);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                
                _context.Agreements.RemoveRange(agreements);
                await _context.SaveChangesAsync(cancellationToken);
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync(cancellationToken);
            
            return BaseResponseModel<Unit>.Success(Unit.Value, "Kişi başarıyla silindi.");
        }
    }
}