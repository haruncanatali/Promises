using MediatR;
using Promises.Application.Notifications.Commands.CommonTimeNotification;
using Promises.Domain.Enums;
using Quartz;

namespace Promises.Api.SchedulerServices;

public class MonthlyAgreementReminderNotificationBackgroundService : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MonthlyAgreementReminderNotificationBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await MonthlyAgreementReminder();
    }

    public async Task MonthlyAgreementReminder()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        await mediator.Send(new CommonTimeNotificationCommand
        {
            NotificationFrequency = NotificationFrequency.Month
        });
    }
}