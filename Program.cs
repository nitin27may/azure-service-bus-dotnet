using ServiceBusAPI.Messages.Consumer;
using ServiceBusAPI.Messages.Sender;
using ServiceBusAPI.Models;
using ServiceBusAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddTransient<INotificationService, NotificationService>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddSingleton<IServicebusSenderService, ServicebusSenderService>();

builder.Services.AddSingleton<IServiceBusConsumer, ServiceBusConsumer>();

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


var busSubscription = app.Services.GetService<IServiceBusConsumer>();
busSubscription.RegisterOnMessageHandlerAndReceiveMessages().GetAwaiter().GetResult();


app.Run();
