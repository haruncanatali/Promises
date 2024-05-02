using MediatR;
using Microsoft.EntityFrameworkCore;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Application.Notifications.Commands.CreateFailedPromiseNotification;
using Promises.Domain.Entities;
using Promises.Domain.Enums;

namespace Promises.Application.Agreements.Commands.UpdateAgreementStatus;

public class UpdateAgreementStatusCommand : IRequest<BaseResponseModel<Unit>>
{
    public class Handler : IRequestHandler<UpdateAgreementStatusCommand, BaseResponseModel<Unit>>
    {
        private readonly IApplicationContext _context;
        private readonly IMediator _mediator;

        public Handler(IApplicationContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<BaseResponseModel<Unit>> Handle(UpdateAgreementStatusCommand request, CancellationToken cancellationToken)
        {
            List<Agreement> agreements = await _context.Agreements
                .Where(c => 
                            c.Date.Date > DateTime.Now.Date && 
                            c.EntityStatus == EntityStatus.Waiting && 
                            c.Approved)
                .Include(c=>c.AgreementUsers)
                .ToListAsync(cancellationToken);
            
            agreements.ForEach(async c =>
            {
                c.EntityStatus = EntityStatus.Failed;
                
                var promiser = await _context.Users
                    .Where(x => x.Id == c.AgreementUsers.First().PromiserUserId)
                    .Select(x => new
                    {
                        DeviceToken = x.DeviceToken,
                        FullName = x.FullName
                    })
                    .FirstAsync(cancellationToken);
                
                var promised = await _context.Users
                    .Where(x => x.Id == c.AgreementUsers.First().PromisedUserId)
                    .Select(x => new
                    {
                        DeviceToken = x.DeviceToken,
                        FullName = x.FullName
                    })
                    .FirstAsync(cancellationToken);
                
                await _mediator.Send(new CreateFailedPromiseNotificationCommand
                {
                    AgreementDate = c.Date,
                    PromiserFullName = promiser.FullName,
                    PromiserDeviceId = promiser.DeviceToken,
                    PromisedFullName = promised.FullName,
                    PromisedDeviceId = promised.DeviceToken
                }, cancellationToken);
            });
            
            _context.Agreements.UpdateRange(agreements);
            await _context.SaveChangesAsync(cancellationToken);
            
            return BaseResponseModel<Unit>.Success(Unit.Value, "Söz durumları başarıyla güncellendi.");
        }
    }
}