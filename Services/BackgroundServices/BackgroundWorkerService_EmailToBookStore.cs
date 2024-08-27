using StudentAPI.Services.StudentService;
using System.Net.Mail;
using System.Net;
using StudentAPI.Services.EmailToBookStoreService;
using StudentAPI.Services.BookService;
using StudentAPI.Models;
using Microsoft.IdentityModel.Tokens;

namespace StudentAPI.Services.BackgroundServices
{
    public class BackgroundWorkerService_EmailToBookStore : BackgroundService
    {

        private readonly ILogger<BackgroundWorkerService_EmailToBookStore> _logger;
        private readonly IServiceProvider _serviceProvider;
        public BackgroundWorkerService_EmailToBookStore (ILogger<BackgroundWorkerService_EmailToBookStore> logger, IServiceProvider serviceProvider)

        {
            _logger = logger;
            _serviceProvider = serviceProvider;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentTime = DateTime.Now;
                var desiredDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 27, 15, 31, 00);
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
                    var book = scope.ServiceProvider.GetRequiredService<IBookService>();
                    var email = scope.ServiceProvider.GetRequiredService<IEmailToBookStoreService>();
                    
                    var date = await email.GetDateOfLastEmail();
                    var enrolments = await book.EnrollmentsWithBookAfterADate(date);
                    
                    if (enrolments.IsNullOrEmpty())
                    {
                        continue;
                    }

                    string myEmail = "email@outlook.com";
                    string myPassword = "password";

                    SmtpClient client = new SmtpClient("smtp-mail.outlook.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential(myEmail, myPassword),
                        EnableSsl = true,
                    };

                    string body = "This is an automated message to inform you that we have the following order: ";
                    foreach (var enrollment in enrolments)
                    {
                        var book1 = enrollment.book;
                        var student = enrollment.student;
                        body =  $"{body}\n{book1.Title} {book1.Author} for " +
                            $"{student.FirstName} {student.LastName} with id = {student.Id}.";
                    }

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(myEmail),
                        Subject = "Book Order",
                        Body = body,
                    };
                    mailMessage.To.Add("email@gmail.com");
                    try
                    {
                        // Send the email
                        client.Send(mailMessage);
                        Console.WriteLine("Email sent successfully.");
                        await email.NewEmaillToBookStore(desiredDateTime);
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
