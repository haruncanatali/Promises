using FirebaseAdmin.Messaging;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Promises.Application.Common.Models;

namespace Promises.Application.Notifications.Commands.CreateSuccessPromiseNotification;

public class CreateSuccessPromiseNotificationCommand : IRequest<BaseResponseModel<Unit>>
{
    public string PromiserDeviceId { get; set; }
    public string PromisedDeviceId { get; set; }
    public string PromiserFullName { get; set; }
    public string PromisedFullName { get; set; }
    public DateTime AgreementDate { get; set; }

    public class Handler : IRequestHandler<CreateSuccessPromiseNotificationCommand, BaseResponseModel<Unit>>
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Handler(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<BaseResponseModel<Unit>> Handle(CreateSuccessPromiseNotificationCommand request, CancellationToken cancellationToken)
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
                    Title = $"Söz tutuldu!",
                    Body = $"{request.PromisedFullName} arkadaşına {request.AgreementDate:dd/MM/yyyy} tarihli verdiğin sözü tuttun."
                },
                Token = request.PromiserDeviceId
            };

            FirebaseMessaging messaging = FirebaseMessaging.DefaultInstance;
            await messaging.SendAsync(message, cancellationToken);

            message = new Message
            {
                Notification = new Notification
                {
                    Title = $"Sözünü tuttu!",
                    Body = $"{request.PromiserFullName} arkadaşın {request.AgreementDate:dd/MM/yyyy} tarihli verdiği sözü tuttu."
                },
                Token = request.PromisedDeviceId
            };

            messaging = FirebaseMessaging.DefaultInstance;
            await messaging.SendAsync(message, cancellationToken);

            return BaseResponseModel<Unit>.Success(Unit.Value, "Bildirimler başarıyla gönderildi.");
        }
    }
}