using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Promises.Application.Common.Interfaces;
using Promises.Application.Common.Models;
using Promises.Domain.Enums;

namespace Promises.Application.Notifications.Commands.CommonTimeNotification
{
    public class CommonTimeNotificationCommand : IRequest<BaseResponseModel<Unit>>
    {
        public NotificationFrequency NotificationFrequency { get; set; }

        public class Handler : IRequestHandler<CommonTimeNotificationCommand, BaseResponseModel<Unit>>
        {
            private readonly IApplicationContext _context;
            private readonly IHostingEnvironment _hostingEnvironment;
            private readonly FireFileConfigs _fileConfigs;

            public Handler(IApplicationContext context, IHostingEnvironment hostingEnvironment, IOptions<FireFileConfigs> options)
            {
                _context = context;
                _hostingEnvironment = hostingEnvironment;
                _fileConfigs = options.Value;
            }

            public async Task<BaseResponseModel<Unit>> Handle(CommonTimeNotificationCommand request, CancellationToken cancellationToken)
            {
                List<BlankAgreementModel> agreements = await _context.Agreements
                    .Where(c =>
                        (c.EntityStatus == EntityStatus.Waiting) &&
                        (c.Approved) &&
                        (c.HasNotification) &&
                        (c.NotificationFrequency == request.NotificationFrequency) &&
                        (c.Date.Date >= DateTime.Now.Date))
                    .Select(c => new BlankAgreementModel
                    {
                        Id = c.Id,
                        Title = c.Title,
                        Date = c.Date.Date.ToString("dd/MM/yyyy")
                    })
                    .ToListAsync(cancellationToken);

                if (agreements is { Count: > 0 })
                {
                    List<MessageModel> domMessageModels = new List<MessageModel>();
                    agreements.ForEach(async c =>
                    {

                        var userIds = await _context
                            .AgreementUsers
                            .Where(x => x.AgreementId == c.Id)
                            .Select(x => new
                            {
                                UserId1 = x.PromisedUserId,
                                UserId2 = x.PromiserUserId
                            })
                            .FirstAsync(cancellationToken);

                        var userDeviceTokens = await _context
                            .Users
                            .Where(x => x.Id == userIds.UserId1 || x.Id == userIds.UserId2)
                            .Select(x => x.DeviceToken)
                            .ToListAsync(cancellationToken);

                        domMessageModels.Add(new MessageModel
                        {
                            AgreementId = c.Id,
                            Title = c.Title,
                            Message = c.Date + " tarihli söz için hatırlatma bildiriminiz vardır.",
                            UserDeviceId1 = userDeviceTokens[0],
                            UserDeviceId2 = userDeviceTokens[1]
                        });
                    });

                    FirebaseApp.Create(new AppOptions
                    {
                        Credential =
                            GoogleCredential.FromFile(Path.Combine(_hostingEnvironment.ContentRootPath, _fileConfigs.Parent, _fileConfigs.Directory, _fileConfigs.File))
                    });

                    domMessageModels.ForEach(async c =>
                    {
                        Message message = new Message
                        {
                            Notification = new Notification
                            {
                                Title = c.Title,
                                Body = c.Message
                            },
                            Token = c.UserDeviceId1
                        };

                        FirebaseMessaging messaging = FirebaseMessaging.DefaultInstance;
                        await messaging.SendAsync(message, cancellationToken);

                        message = new Message
                        {
                            Notification = new Notification
                            {
                                Title = c.Title,
                                Body = c.Message
                            },
                            Token = c.UserDeviceId2
                        };
                        
                        messaging = FirebaseMessaging.DefaultInstance;
                        await messaging.SendAsync(message, cancellationToken);
                    });

                    return BaseResponseModel<Unit>.Success(Unit.Value, "Bildirimler başarıyla gönderildi.");
                }

                return BaseResponseModel<Unit>.Success(Unit.Value, "Bildirim göndermek için gerekli sözler bulunamadı.");
            }
        }
    }

    public class BlankAgreementModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
    }

    public class MessageModel
    {
        public long AgreementId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string UserDeviceId1 { get; set; }
        public string UserDeviceId2 { get; set; }
    }
}
