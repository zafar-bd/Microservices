using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using NCrontab;

namespace Application.Cron.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        private readonly CrontabSchedule _schedule;
        private DateTime _nextRun;
        private static string Schedule => "* * * * * *"; //every 10 min https://crontab.guru/
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _schedule = CrontabSchedule.Parse(Schedule,
                new CrontabSchedule.ParseOptions
                {
                    IncludingSeconds = true
                });
            _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                await Task.CompletedTask;
                var now = DateTime.Now;
                if (now > _nextRun)
                {
                    Process();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.Now);
                }
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        private void Process()
        {
            Console.WriteLine($"scheduled job every {Schedule} hello world {DateTime.Now:F}");
            _logger.LogInformation($"scheduled job every {Schedule} hello world {DateTime.Now:F}");
        }
    }
}
