using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SimpleSocket
{
    public class SocketReceiveEventArgs:EventArgs
    {
        public SocketSender Sender;
        public string Content;

        public SocketReceiveEventArgs(SocketSender sender,string content)
        {
            this.Sender = sender;
            this.Content = content;
        }
    }
}
