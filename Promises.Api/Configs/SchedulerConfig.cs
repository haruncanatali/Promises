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
            var jobKey = new JobKey("DailyAgreementReminderNotification");
            q.AddJob<DailyAgreementReminderNotificationBackgroundService>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("DailyAgreementReminderNotification-trigger")
                .WithCronSchedule("0 0 0 * * ?")
            );
        });
        
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            var jobKey = new JobKey("WeeklyAgreementReminderNotification");
            q.AddJob<WeeklyAgreementReminderNotificationBackgorundService>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("WeeklyAgreementReminderNotification-trigger")
                .WithCronSchedule("0 0 0 ? * MON")
            );
        });
        
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            var jobKey = new JobKey("MonthlyAgreementReminderNotification");
            q.AddJob<MonthlyAgreementReminderNotificationBackgroundService>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("MonthlyAgreementReminderNotification-trigger")
                .WithCronSchedule("0 0 0 1 * ?")
            );
        });
        
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            var jobKey = new JobKey("YearlyAgreementReminderNotification");
            q.AddJob<YearlyAgreementReminderNotificationBackgroundService>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("YearlyAgreementReminderNotification-trigger")
                .WithCronSchedule("0 0 0 1 1 ?")
            );
        });
        
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            var jobKey = new JobKey("DailyAgreementUpdateStatusService");
            q.AddJob<DailyAgreementUpdateStatusBackgroundService>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("DailyAgreementUpdateStatusService-trigger")
                .WithCronSchedule("0 0/15 * * * ?")
            );
        });
        
        services.AddTransient<DailyAgreementReminderNotificationBackgroundService>();
        services.AddTransient<WeeklyAgreementReminderNotificationBackgorundService>();
        services.AddTransient<MonthlyAgreementReminderNotificationBackgroundService>();
        services.AddTransient<YearlyAgreementReminderNotificationBackgroundService>();
        services.AddTransient<DailyAgreementUpdateStatusBackgroundService>();
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}
