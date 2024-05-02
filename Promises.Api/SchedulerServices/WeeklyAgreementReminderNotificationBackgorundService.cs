using MediatR;
using Promises.Application.Notifications.Commands.CommonTimeNotification;
using Promises.Domain.Enums;
using Quartz;

namespace Promises.Api.SchedulerServices;

public class WeeklyAgreementReminderNotificationBackgorundService : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public WeeklyAgreementReminderNotificationBackgorundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await WeeklyAgreementReminder();
    }

    public async Task WeeklyAgreementReminder()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        await mediator.Send(new CommonTimeNotificationCommand
        {
            NotificationFrequency = NotificationFrequency.Week
        });
    }
}