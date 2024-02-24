using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient.Services
{
    public static class ConsoleService
    {
        public static string GetLogin()
        {
            Console.Write("Enter your login: ");
            return Console.ReadLine();
        }

        public static string GetEvent(string login)
        {
            Console.Clear();
            Console.WriteLine($"Login: {login}");
            Console.Write("Enter event name (or exit): ");
            return Console.ReadLine();
        }

        public static void WriteResult(bool success)
        {
            var message = success switch
            {
                true => "Successfully sent :)",
                false => "Failed to send :("
            };

            Console.Clear();
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
