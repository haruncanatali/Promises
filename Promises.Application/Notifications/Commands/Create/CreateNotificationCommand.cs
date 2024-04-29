using MediatR;
using Promises.Application.Common.Models;

namespace Promises.Application.Notifications.Commands.Create;

public class CreateNotificationCommand : IRequest<BaseResponseModel<Unit>>
{
    public class Handler : IRequestHandler<CreateNotificationCommand, BaseResponseModel<Unit>>
    {
        public async Task<BaseResponseModel<Unit>> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}