using MediatR;
using Promises.Application.Agreements.Commands.UpdateAgreementStatus;
using Quartz;

namespace Promises.Api.SchedulerServices;

public class DailyAgreementUpdateStatusBackgroundService : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DailyAgreementUpdateStatusBackgroundService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await DailyAgreementUpdateStatus();
    }

    public async Task DailyAgreementUpdateStatus()
    {
        using var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetService<IMediator>();
        await mediator.Send(new UpdateAgreementStatusCommand());
    }
}