using System;
using Nancy.Hosting.Self;
using Quartz;
using Haketon.Jobs;
using Quartz.Impl;

namespace Haketon
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri =
                new Uri("http://localhost:3579");

            using (var host = new NancyHost(uri))
            {
                host.Start();
                Console.WriteLine("Your application is running on " + uri);

                IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
                scheduler.Start();

                IJobDetail job = JobBuilder.Create<MatchingJob>().Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithSimpleSchedule(x => x
                        .WithIntervalInSeconds(10)
                        .RepeatForever())
                    .Build();

                scheduler.ScheduleJob(job, trigger);

                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
            }

        }
    }
}
