using MVCLearn.Common;
using MVCLearn.Configurations;
using MVCLearn.DataAcess.Interfaces.Messages;
using MVCLearn.DataAcess.Interfaces.Users;
using MVCLearn.DataAcess.Repositories.Messages;
using MVCLearn.DataAcess.Repositories.Users;
using MVCLearn.Hubs;
using MVCLearn.Interfaces.Messages;
using MVCLearn.Interfaces.RabbitMQ;
using MVCLearn.Models;
using MVCLearn.Services.Messages;
using MVCLearn.Services.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddTransient<IUserRepository, UsersRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IRabbitMQProducerService, RabbitMQProducerService>();
builder.Services.AddCustomControllers();
builder.Services.AddSignalR();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accaunt}/{action=Login}");

app.MapHub<NotificationHub>("/notificationHub");
app.Run();