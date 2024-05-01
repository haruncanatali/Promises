using MediatR;
using Quartz;

namespace Promises.Api.SchedulerServices;

public class AgreementNotificationBackgroundService : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AgreementNotificationBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await GetAgreementNotifications();
    }

    private async Task GetAgreementNotifications()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        //await mediator.Send(new CreateNotificationCommand());
    }
}