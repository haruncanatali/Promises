using MediatR;
using Promises.Application.Notifications.Commands.CommonTimeNotification;
using Promises.Domain.Enums;
using Quartz;

namespace Promises.Api.SchedulerServices;

public class DailyAgreementReminderNotificationBackgroundService : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DailyAgreementReminderNotificationBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await DailyAgreementReminder();
    }

    public async Task DailyAgreementReminder()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        await mediator.Send(new CommonTimeNotificationCommand
        {
            NotificationFrequency = NotificationFrequency.Day
        });
    }
}