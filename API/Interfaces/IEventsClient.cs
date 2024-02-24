using API.Models;

namespace API.Interfaces
{
    public interface IEventsClient
    {
        public const string Url = "https://localhost:7137/events";

        Task ReceiveEvents(EventMessage message);
        Task JoinLogin(string login);
    }
}
