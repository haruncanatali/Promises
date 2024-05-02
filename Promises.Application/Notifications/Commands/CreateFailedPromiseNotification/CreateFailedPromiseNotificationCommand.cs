using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Promises.Application.Common.Models;

namespace Promises.Application.Notifications.Commands.CreateFailedPromiseNotification;

public class CreateFailedPromiseNotificationCommand : IRequest<BaseResponseModel<Unit>>
{
    public string PromiserDeviceId { get; set; }
    public string PromisedDeviceId { get; set; }
    public string PromiserFullName { get; set; }
    public string PromisedFullName { get; set; }
    public DateTime AgreementDate { get; set; }

    public class Handler : IRequestHandler<CreateFailedPromiseNotificationCommand, BaseResponseModel<Unit>>
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly FireFileConfigs _fileConfigs;

        public Handler(IHostingEnvironment hostingEnvironment, IOptions<FireFileConfigs> options)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileConfigs = options.Value;
        }

        public async Task<BaseResponseModel<Unit>> Handle(CreateFailedPromiseNotificationCommand request, CancellationToken cancellationToken)
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
                    Title = $"Söz tutulamadı!",
                    Body = $"{request.PromisedFullName} arkadaşına {request.AgreementDate:dd/MM/yyyy} tarihli verdiğin sözü tutamadın."
                },
                Token = request.PromiserDeviceId
            };

            FirebaseMessaging messaging = FirebaseMessaging.DefaultInstance;
            await messaging.SendAsync(message, cancellationToken);

            message = new Message
            {
                Notification = new Notification
                {
                    Title = $"Sözünü tutmadı!",
                    Body = $"{request.PromiserFullName} arkadaşın {request.AgreementDate:dd/MM/yyyy} tarihli verdiği sözü tutamadı."
                },
                Token = request.PromisedDeviceId
            };

            messaging = FirebaseMessaging.DefaultInstance;
            await messaging.SendAsync(message, cancellationToken);

            return BaseResponseModel<Unit>.Success(Unit.Value, "Bildirimler başarıyla gönderildi.");
        }
    }
}