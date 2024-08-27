using StudentAPI.Services.StudentService;
using System.Net.Mail;
using System.Net;

namespace StudentAPI.Services.BackgroundServices
{
    public class BackgroundWorkerService_MeanGrade : BackgroundService
    {


        private readonly ILogger<BackgroundWorkerService_MeanGrade> _logger;
        private readonly IServiceProvider _serviceProvider;

        public BackgroundWorkerService_MeanGrade(ILogger<BackgroundWorkerService_MeanGrade> logger, IServiceProvider serviceProvider)

        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentTime = DateTime.Now;
                var desiredDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 27, 15, 32, 00);
                if (desiredDateTime < currentTime)
                {
                    // If the desired time is in the past, schedule it for tomorrow
                    desiredDateTime = desiredDateTime.AddMonths(1);
                }
                _logger.LogInformation($"Next execution scheduled for {desiredDateTime}");
                TimeSpan delay = desiredDateTime - currentTime;
                await Task.Delay(delay, stoppingToken);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var stud = scope.ServiceProvider.GetRequiredService<IStudentService>();
                    var students = await stud.GetAllStudents();

                    List<string> mean = new List<string>();
                    foreach (var student in students)
                    {
                        var meanGrade = await stud.GetMeanGrade(student.ID);
                        mean.Add($"Student {student.LastName} {student.FirstName} have mean grade = {meanGrade}.");
                    }
                    await Console.Out.WriteLineAsync($"{string.Join("\n", mean)}");
                }
            }

        }
    }
}
