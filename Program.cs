global using StudentAPI.Models;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Data;
using StudentAPI.Services.BackgroundServices;
using StudentAPI.Services.BookService;
using StudentAPI.Services.EmailToBookStoreService;
using StudentAPI.Services.RequestService;
using StudentAPI.Services.StudentService;
using StudentAPI.Services.SubjectService;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddHostedService<BackgroundWorkerService_EmailToBookStore>();
builder.Services.AddHostedService<BackgroundWorkerService_StudentFailled>();
builder.Services.AddHostedService<BackgroundWorkerService_Email>();
builder.Services.AddHostedService<BackgroundWorkerService_MeanGrade>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IStudentService, StudentService2>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IEmailToBookStoreService, EmailToBookStoreService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
