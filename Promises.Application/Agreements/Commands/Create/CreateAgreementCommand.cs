using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Managers;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;
using Promises.Domain.Enums;

namespace Promises.Application.Agreements.Commands.Create;

public class CreateAgreementCommand : IRequest<BaseResponseModel<Unit>>
{
    public string Description { get; set; }
    public PriorityLevel PriorityLevel { get; set; }
    public DateTime Date { get; set; }
    public CommitmentStatus CommitmentStatus { get; set; }
    public bool HasNotification { get; set; }
    public bool HasMailNotification { get; set; }
    public long PersonId { get; set; }
    public int NotificationFrequency { get; set; }
    public List<IFormFile>? EventPhotos { get; set; }
    
    public class Handler : IRequestHandler<CreateAgreementCommand, BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;
        private readonly FileManager _fileManager;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IApplicationContext context, FileManager fileManager, ICurrentUserService currentUserService)
        {
            _context = context;
            _fileManager = fileManager;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponseModel<Unit>> Handle(CreateAgreementCommand request, CancellationToken cancellationToken)
        {
            EntityEntry<Agreement> result = await _context.Agreements.AddAsync(new Agreement
            {
                Description = request.Description,
                PriorityLevel = request.PriorityLevel,
                Date = request.Date,
                CommitmentStatus = request.CommitmentStatus,
                HasMailNotification = request.HasMailNotification,
                HasNotification = request.HasNotification,
                NotificationFrequency = request.HasNotification ? request.NotificationFrequency : 0,
                PersonId = request.PersonId,
                UserId = _currentUserService.UserId
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            if (request.EventPhotos != null && request.EventPhotos.Count > 0)
            {
                List<EventPhoto> eventPhotos = new List<EventPhoto>();
                
                request.EventPhotos.ForEach(c =>
                {
                    eventPhotos.Add(new EventPhoto
                    {
                        AgreementId = result.Entity.Id,
                        Photo = _fileManager.Upload(c, FileRoot.EventPhotos)
                    });
                });
                
                await _context.EventPhotos.AddRangeAsync(eventPhotos, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            
            return BaseResponseModel<Unit>.Success(Unit.Value, "Söz başarıyla eklendi.");
        }
    }
}