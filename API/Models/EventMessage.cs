using System.Text.Json;

namespace API.Models
{
    public class EventMessage
    {
        public string Login { get; set; }
        public string Event { get; set; }
        public DateTime Date { get; set; }

        public EventMessage(string login)
        {
            Login = login;
            Event = string.Empty;
            Date = DateTime.Now;
        }

        public string ToJson()
            => JsonSerializer.Serialize(this);
        public static EventMessage FromJson(string jsonMessage)
            => JsonSerializer.Deserialize<EventMessage>(jsonMessage);
    }
}
