using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleSocket;

namespace ExampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatMessage message = new ChatMessage();
            message.From = "我是路人甲";
            message.To = "我是路人乙";
            message.Message = "这些年你过得可好?";

            SimpleClient client = new SimpleClient("127.0.0.1", 8081);
            client.Connect();
            client.OnSocketReceive += client_OnSocketReceive;
            client.Send(LitJson.JsonMapper.ToJson(message));

            while (true)
            {
                client.Receive();
                client.Send(Console.ReadLine());
            }
        }

        private static void client_OnSocketReceive(SocketReceiveEventArgs e)
        {
            Console.WriteLine(e.Content);
        }
    }
}
