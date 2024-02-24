using API.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class EventsHub : Hub<IEventsClient>
    {
        public async Task JoinLogin(string login)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, login);
        }
    }
}
