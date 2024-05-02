using MediatR;
using Promises.Application.Notifications.Commands.CommonTimeNotification;
using Promises.Domain.Enums;
using Quartz;

namespace Promises.Api.SchedulerServices;

public class YearlyAgreementReminderNotificationBackgroundService : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public YearlyAgreementReminderNotificationBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await YearlyAgreementReminder();
    }

    public async Task YearlyAgreementReminder()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        await mediator.Send(new CommonTimeNotificationCommand
        {
            NotificationFrequency = NotificationFrequency.Year
        });
    }
}