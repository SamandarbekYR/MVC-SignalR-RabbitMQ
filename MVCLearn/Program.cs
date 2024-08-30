using MVCLearn.Common;
using MVCLearn.Configurations;
using MVCLearn.DataAcess.Interfaces.Messages;
using MVCLearn.DataAcess.Interfaces.Users;
using MVCLearn.DataAcess.Interfaces.UsersMessages;
using MVCLearn.DataAcess.Repositories.Messages;
using MVCLearn.DataAcess.Repositories.Users;
using MVCLearn.DataAcess.Repositories.UsersMessages;
using MVCLearn.Hubs;
using MVCLearn.Interfaces.Messages;
using MVCLearn.Interfaces.RabbitMQ;
using MVCLearn.Models;
using MVCLearn.Services.Messages;
using MVCLearn.Services.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddTransient<IUserRepository, UsersRepository>();
builder.Services.AddTransient<IMessageRepository, MessageRepository>();
builder.Services.AddTransient<IUsersMessagesRepository, UsersMessagesRepository>();
builder.Services.AddTransient<IRabbitMQProducerService, RabbitMQProducerService>();
builder.Services.AddCustomControllers();
builder.Services.AddSignalR();
builder.Services.AddSignalR().AddHubOptions<NotificationHub>(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(6); // mijoz bilan aloqani yo'qotish uchun vaqt
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session saqlash vaqti
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession(); // Session middleware-ni qo'shish

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accaunt}/{action=Login}");

app.MapHub<NotificationHub>("/notificationHub");
app.Run();