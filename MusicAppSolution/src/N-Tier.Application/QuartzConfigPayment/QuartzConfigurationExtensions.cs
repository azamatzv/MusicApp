using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Tier.Application.QuartzConfigPayment;

public static class QuartzConfigurationExtensions
{
    public static void AddQuartzJobs(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            var monthlyPaymentJob = JobKey.Create(nameof(MonthlyPaymentJob));
            q.AddJob<MonthlyPaymentJob>(opts => opts.WithIdentity(monthlyPaymentJob));
            q.AddTrigger(opts => opts
                .ForJob(monthlyPaymentJob)
                .WithIdentity($"{nameof(MonthlyPaymentJob)}-trigger")
                .WithCronSchedule("0 0 0 L * ?"));  // Runs at midnight on the last day of every month

            // Low Balance Notification Job - Separate triggers for each day
            var lowBalanceJob = JobKey.Create(nameof(LowBalanceNotificationJob));
            q.AddJob<LowBalanceNotificationJob>(opts => opts.WithIdentity(lowBalanceJob));

            // Trigger for L-2 (2nd to last day of month)
            q.AddTrigger(opts => opts
                .ForJob(lowBalanceJob)
                .WithIdentity($"{nameof(LowBalanceNotificationJob)}-trigger-L-2")
                .WithCronSchedule("0 0 9 28 * ?")); // Adjust dynamically

            // Trigger for L-1 (1st to last day of month)
            q.AddTrigger(opts => opts
                .ForJob(lowBalanceJob)
                .WithIdentity($"{nameof(LowBalanceNotificationJob)}-trigger-L-1")
                .WithCronSchedule("0 0 9 29 * ?")); // Adjust dynamically

            // Trigger for L (last day of month)
            q.AddTrigger(opts => opts
                .ForJob(lowBalanceJob)
                .WithIdentity($"{nameof(LowBalanceNotificationJob)}-trigger-L")
                .WithCronSchedule("0 0 9 L * ?"));  // Runs at 9 AM on the last day of every month
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }
}
