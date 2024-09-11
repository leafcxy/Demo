using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleSocket;

namespace ExampleServer
{
    class Program
    {
        private static SimpleServer server;

        static void Main(string[] args)
        {
            server = new SimpleServer(8081);
            server.OnSocketReceive+=server_OnSocketReceive;
            server.Listen();
        }

        private static void server_OnSocketReceive(SocketReceiveEventArgs e)
        {
            server.Broadcast(string.Format("客户端{0}:{1}",e.Sender.EndPoint,e.Content));
        }
    }
}
