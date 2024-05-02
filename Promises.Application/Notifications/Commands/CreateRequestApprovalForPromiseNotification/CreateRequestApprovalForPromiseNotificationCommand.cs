using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.Extensions.Hosting;
using Promises.Application.Common.Models;

namespace Promises.Application.Notifications.Commands.CreateRequestApprovalForPromiseNotification;

public class CreateRequestApprovalForPromiseNotificationCommand : IRequest<BaseResponseModel<Unit>>
{
    public string SenderFullName { get; set; }
    public string ReceiverDeviceToken { get; set; }

    public class Handler : IRequestHandler<CreateRequestApprovalForPromiseNotificationCommand, BaseResponseModel<Unit>>
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Handler(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<BaseResponseModel<Unit>> Handle(CreateRequestApprovalForPromiseNotificationCommand request, CancellationToken cancellationToken)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = 
                    GoogleCredential.FromFile(Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "CloudSettings", "promises-app-fdc62-firebase-adminsdk-6rs4q-6959bef4e3.json"))
            });

            Message message = new Message
            {
                Notification = new Notification
                {
                    Title = "Söz isteğin var!",
                    Body = $"{request.SenderFullName} sana bir söz isteği gönderip onayına sundu."
                },
                Token = request.ReceiverDeviceToken
            };

            FirebaseMessaging messaging = FirebaseMessaging.DefaultInstance;
            await messaging.SendAsync(message, cancellationToken);

            return BaseResponseModel<Unit>.Success(Unit.Value, "Bildirim başarıyla gönderildi.");
        }
    }
}