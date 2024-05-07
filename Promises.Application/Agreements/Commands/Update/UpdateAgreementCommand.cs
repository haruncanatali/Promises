using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Exceptions;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Managers;
using Promises.Application.Common.Models;
using Promises.Domain.Entities;
using Promises.Domain.Enums;

namespace Promises.Application.Agreements.Commands.Update;

public class UpdateAgreementCommand : IRequest<BaseResponseModel<Unit>>
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public PriorityLevel PriorityLevel { get; set; }
    public DateTime Date { get; set; }
    public bool HasNotification { get; set; }
    public bool HasMailNotification { get; set; }
    public long UserId { get; set; }
    public NotificationFrequency NotificationFrequency { get; set; }
    public List<long>? EventPhotosToBeDeleted { get; set; }
    public List<IFormFile>? EventPhotosToBeAdded { get; set; }
    
    public class Handler : IRequestHandler<UpdateAgreementCommand, BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly FileManager _fileManager;

        public Handler(IApplicationContext context, FileManager fileManager, ICurrentUserService currentUserService)
        {
            _context = context;
            _fileManager = fileManager;
            _currentUserService = currentUserService;
        }

        public async Task<BaseResponseModel<Unit>> Handle(UpdateAgreementCommand request, CancellationToken cancellationToken)
        {
            Agreement? agreement = await _context.Agreements
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (agreement == null)
                throw new BadRequestException("Güncellenecek söz bulunamadı.");

            if (DateTime.Now.Date < request.Date.Date)
                throw new BadRequestException("Söz tarihi bugünden küçük olamaz.");

            var blockedIds = await _context.BlockedFriends
                .Where(c => (c.ReceiverId == _currentUserService.UserId || c.SenderId == _currentUserService.UserId))
                .Select(c => c.ReceiverId == _currentUserService.UserId ? c.SenderId : c.ReceiverId)
                .ToListAsync(cancellationToken);

            if (blockedIds.Contains(request.UserId))
                throw new BadRequestException("Bu kişi ile arkadaş olmanız engellenmiştir.");

            agreement.Title = request.Title;
            agreement.Description = request.Description;
            agreement.PriorityLevel = request.PriorityLevel;
            agreement.Date = request.Date;
            agreement.HasNotification = request.HasNotification;
            agreement.HasMailNotification = request.HasMailNotification;
            agreement.NotificationFrequency = request.NotificationFrequency;
            agreement.EntityStatus = EntityStatus.Waiting;

            if (request.EventPhotosToBeDeleted is { Count: > 0 })
            {
                List<EventPhoto> eventPhotos = await _context.EventPhotos
                    .Where(c => request.EventPhotosToBeDeleted.Contains(c.Id))
                    .ToListAsync(cancellationToken);
                
                _context.EventPhotos.RemoveRange(eventPhotos);
                await _context.SaveChangesAsync(cancellationToken);
            }

            if (request.EventPhotosToBeAdded is { Count: > 0 })
            {
                request.EventPhotosToBeAdded.ForEach(c =>
                {
                    agreement.EventPhotos.Add(new EventPhoto
                    {
                        AgreementId = agreement.Id,
                        Photo = _fileManager.Upload(c,FileRoot.EventPhotos)
                    });
                });
            }

            AgreementUsers? agreementUsers = await _context.AgreementUsers
                .FirstOrDefaultAsync(c => c.AgreementId == agreement.Id, cancellationToken);

            if (agreementUsers != null)
            {
                if (agreementUsers.PromisedUserId != request.UserId)
                {
                    agreementUsers.PromisedUserId = request.UserId;
                    _context.AgreementUsers.Update(agreementUsers);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            else
            {
                throw new BadRequestException("Veritabanı hatası meydana geldi. KOD:SVKB01");
            }

            await _context.SaveChangesAsync(cancellationToken);
            
            return BaseResponseModel<Unit>.Success(Unit.Value, "Söz başarıyla güncellendi.");
        }
    }
}