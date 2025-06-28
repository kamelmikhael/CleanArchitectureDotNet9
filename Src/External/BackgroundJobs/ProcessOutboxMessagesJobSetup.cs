using Microsoft.Extensions.Options;
using Quartz;

namespace BackgroundJobs;

public class ProcessOutboxMessagesJobSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(ProcessOutboxMessagesJob));

        options.AddJob<ProcessOutboxMessagesJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
                 .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(10)
                                    .RepeatForever()));
    }
}
