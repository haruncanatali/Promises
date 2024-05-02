using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Promises.Application.Common.Models;

namespace Promises.Application.Notifications.Commands.CreateRequestApprovalForPromiseNotification;

public class CreateRequestApprovalForPromiseNotificationCommand : IRequest<BaseResponseModel<Unit>>
{
    public string SenderFullName { get; set; }
    public string ReceiverDeviceToken { get; set; }

    public class Handler : IRequestHandler<CreateRequestApprovalForPromiseNotificationCommand, BaseResponseModel<Unit>>
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly FireFileConfigs _fileConfigs;

        public Handler(IHostingEnvironment hostingEnvironment, IOptions<FireFileConfigs> options)
        {
            _hostingEnvironment = hostingEnvironment;
            _fileConfigs = options.Value;
        }

        public async Task<BaseResponseModel<Unit>> Handle(CreateRequestApprovalForPromiseNotificationCommand request, CancellationToken cancellationToken)
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