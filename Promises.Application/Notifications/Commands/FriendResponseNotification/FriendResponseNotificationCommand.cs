using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Promises.Application.Common.Models;

namespace Promises.Application.Notifications.Commands.FriendResponseNotification;

public class FriendResponseNotificationCommand : IRequest<BaseResponseModel<Unit>>
{
    public string ReceiverDeviceId { get; set; }
    public string SenderFullName { get; set; }
    public bool Approved { get; set; }
    
    public class Handler : IRequestHandler<FriendResponseNotificationCommand, BaseResponseModel<Unit>>
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly FireFileConfigs _fileConfigs;

        public Handler(IHostingEnvironment hostingEnvironment, FireFileConfigs fileConfigs)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileConfigs = fileConfigs;
        }

        public async Task<BaseResponseModel<Unit>> Handle(FriendResponseNotificationCommand request, CancellationToken cancellationToken)
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
                    Title = $"Arkadaşlık isteği yanıtın işlendi!",
                    Body = $"{request.SenderFullName} sana arkadaşlık isteğinizi {(request.Approved ? "kabul etti." : "kabul etmedi.")}."
                },
                Token = request.ReceiverDeviceId
            };

            FirebaseMessaging messaging = FirebaseMessaging.DefaultInstance;
            await messaging.SendAsync(message, cancellationToken);
            
            return BaseResponseModel<Unit>.Success(Unit.Value, "Bildirim başarıyla gönderildi.");
        }
    }
}