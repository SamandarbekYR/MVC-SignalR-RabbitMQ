using MVCLearn.Configurations;
using MVCLearn.DataAcess.Interfaces.Messages;
using MVCLearn.DataAcess.Repositories.Messages;
using MVCLearn.Interfaces.Messages;
using MVCLearn.Models;
using MVCLearn.Services.Messages;
using MVCLearn.Services.RabbitMQ;
using ReceiverApp.WebApi.Services.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
builder.Services.AddHostedService<RabbitMQConsumerService>();
builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddCustomDbContext(builder.Configuration);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
