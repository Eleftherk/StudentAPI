using StudentAPI.Data;
using StudentAPI.Models;
using StudentAPI.Services.StudentService;
using StudentAPI.Services.SubjectService;

namespace StudentAPI.Services.BackgroundServices
{
    public class BackgroundWorkerService_StudentFailled : BackgroundService
    {

        private readonly ILogger<BackgroundWorkerService_StudentFailled> _logger;
        private readonly IServiceProvider _serviceProvider;

        public BackgroundWorkerService_StudentFailled(ILogger<BackgroundWorkerService_StudentFailled> logger, IServiceProvider serviceProvider)

        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var desiredTimeOfDay = new TimeSpan(15, 33, 00);
                var currentTime = DateTime.Now;
                var desiredDateTime = currentTime.Date + desiredTimeOfDay;

                if (desiredDateTime < currentTime)
                {
                    // If the desired time is in the past, schedule it for tomorrow
                    desiredDateTime = desiredDateTime.AddDays(1);
                }
                _logger.LogInformation($"Next execution scheduled for {desiredDateTime}");
                var delay = desiredDateTime - currentTime;


                await Task.Delay(delay, stoppingToken);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var subj = scope.ServiceProvider.GetRequiredService<ISubjectService>();
                    var subjects = subj.GetAllSubjects();
                    foreach (var subject in await subjects)
                    {
                        await Console.Out.WriteLineAsync(subject.Name);
                        for (int i = 0; i < subject.Enrollments.Count; i++)
                        {
                            if (!subject.Enrollments[i].Passed)
                            {
                                var FirstName = subject.Enrollments[i].StudFirstName;
                                var LastName = subject.Enrollments[i].StudLastName;
                                await Console.Out.WriteLineAsync(FirstName + " " + LastName);
                            }

                        }
                        await Console.Out.WriteLineAsync("\n");
                    }
                }
            }

        }
    }
}
