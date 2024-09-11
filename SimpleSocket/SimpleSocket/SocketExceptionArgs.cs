using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SimpleSocket
{
    public class SocketExceptionArgs
    {
        public SocketSender Sender;
        public Exception Exception;

        public SocketExceptionArgs(SocketSender sender, Exception exception)
        {
            this.Sender = sender;
            this.Exception = exception;
        }
    }
}
