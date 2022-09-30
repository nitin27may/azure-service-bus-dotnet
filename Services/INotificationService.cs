using ServiceBusAPI.Models;

namespace ServiceBusAPI.Services;

public interface INotificationService
{
    Task SendNotficaiton(Notification note);
}