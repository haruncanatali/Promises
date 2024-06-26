﻿using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Promises.Application.Common.Models;
using Microsoft.Extensions.Options;

namespace Promises.Application.Notifications.Commands.CreateResponseApprovalForPromise;

public class CreateResponseApprovalForPromiseNotificationCommand : IRequest<BaseResponseModel<Unit>>
{
    public string ReceiverDeviceToken { get; set; }
    public DateTime AgreementDate { get; set; }
    public string SenderFullName { get; set; }
    public bool State { get; set; }

    public class Handler : IRequestHandler<CreateResponseApprovalForPromiseNotificationCommand, BaseResponseModel<Unit>>
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly FireFileConfigs _fileConfigs;

        public Handler(IHostingEnvironment hostingEnvironment, IOptions<FireFileConfigs> options)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileConfigs = options.Value;
        }

        public async Task<BaseResponseModel<Unit>> Handle(CreateResponseApprovalForPromiseNotificationCommand request, CancellationToken cancellationToken)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential =
                            GoogleCredential.FromFile(Path.Combine(_hostingEnvironment.ContentRootPath, _fileConfigs.Parent, _fileConfigs.Directory, _fileConfigs.File))
            });

            Message message = new Message
            {
                Notification = new Notification
                {
                    Title = request.State ? "Söz isteğin kabul edildi!" : "Söz isteğin kabul edilmedi!",
                    Body = $"{request.SenderFullName} {request.AgreementDate:dd/MM/yyyy} tarihli söz isteğini kabul {(request.State ? "etti.":"etmedi.")}."
                },
                Token = request.ReceiverDeviceToken
            };

            FirebaseMessaging messaging = FirebaseMessaging.DefaultInstance;
            await messaging.SendAsync(message, cancellationToken);

            return BaseResponseModel<Unit>.Success(Unit.Value, "Bildirim başarıyla gönderildi.");
        }
    }
}