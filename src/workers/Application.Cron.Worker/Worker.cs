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
        private static string Schedule => "*/10 * * * * *"; //Runs every 10 seconds
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
                var now = DateTime.Now;
                var nextrun = _schedule.GetNextOccurrence(now);
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
            _logger.LogInformation($"scheduled job every {Schedule} hello world {DateTime.Now:F}");
        }
    }
}
