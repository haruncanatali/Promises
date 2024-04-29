using Promises.Api.SchedulerServices;
using Quartz;

namespace Promises.Api.Configs;

public static class SchedulerConfig
{
    public static IServiceCollection AddSchedulerConfig(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            var jobKey = new JobKey("SchedulerWorkerList");
            q.AddJob<AgreementNotificationBackgroundService>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("AgreementNotification-trigger")
                .WithCronSchedule("0 1 20 ? * *")
            );
        });
        
        services.AddTransient<AgreementNotificationBackgroundService>();
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}
