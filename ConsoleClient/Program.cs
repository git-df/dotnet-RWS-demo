using API.Models;
using ConsoleClient.Services;


var rabbitMQ = new RabbitMQService();

var message = new EventMessage(ConsoleService.GetLogin()?.ToUpper());

while (message.Event != "exit")
{
    message.Event = ConsoleService.GetEvent(message.Login);
    message.Date = DateTime.Now;
    ConsoleService.WriteResult(rabbitMQ.SendMessage(message));
}