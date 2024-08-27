using StudentAPI.Services.StudentService;
using System.Net.Mail;
using System.Net;
using StudentAPI.Services.EmailToBookStoreService;

namespace StudentAPI.Services.BackgroundServices
{
    public class BackgroundWorkerService_Email : BackgroundService
    {

        private readonly ILogger<BackgroundWorkerService_Email> _logger;
        private readonly IServiceProvider _serviceProvider;

        public BackgroundWorkerService_Email(ILogger<BackgroundWorkerService_Email> logger, IServiceProvider serviceProvider)

        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentTime = DateTime.Now;
                var desiredDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 27, 15, 30, 00);
                if (desiredDateTime < currentTime)
                {
                    // If the desired time is in the past, schedule it for tomorrow
                    desiredDateTime = desiredDateTime.AddDays(1);
                }
                _logger.LogInformation($"Next execution scheduled for {desiredDateTime}");
                TimeSpan delay = desiredDateTime - currentTime;
                await Task.Delay(delay, stoppingToken);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var stud = scope.ServiceProvider.GetRequiredService<IStudentService>();
                    var students = await stud.GetAllStudents();

                    string myEmail = "email@outlook.com";
                    string myPassword = "password";

                    SmtpClient client = new SmtpClient("smtp-mail.outlook.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(myEmail, myPassword),
                        EnableSsl = true,
                    };
                    foreach (var student in students)
                    {
                        List<String> subjects = new List<String>();
                        foreach (var enrollment in student.Enrollments)
                        {
                            if (!enrollment.Passed)
                            {
                                subjects.Add(enrollment.SubjName);
                            }
                        }
                        if (!string.IsNullOrEmpty(student.Email))
                        {
                            MailMessage mailMessage = new MailMessage
                            {
                                From = new MailAddress(myEmail),
                                Subject = "subjects",
                                Body = $"This is an automated message to remind you the subject you havent pass.\n The subjects:\n {string.Join("\n", subjects)} ",
                            };
                            mailMessage.To.Add($"{student.Email}");
                            try
                            {
                                // Send the email
                                client.Send(mailMessage);
                                Console.WriteLine("Email sent successfully.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Failed to send email: {ex.Message}");
                            }
                        }
                    }
                }
            }

        }


    }
}
