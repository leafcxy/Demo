using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SimpleSocket
{
    public class SocketSender
    {
        /// <summary>
        /// 发送者Socket
        /// </summary>
        private Socket handler;

        /// <summary>
        /// 终端地址
        /// </summary>
        public EndPoint EndPoint
        {
            get
            {
                if(handler == null)
                    return null;
                return handler.RemoteEndPoint;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SocketSender(Socket handler)
        {
            this.handler = handler;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            if(this.handler == null)
                return;

            //将消息进行编码
            byte[] data = new byte[1024];
            data = Encoding.UTF8.GetBytes(message);
            //发送消息
            this.handler.Send(data);
        }

        /// <summary>
        /// 关闭Socket
        /// </summary>
        public void Close()
        {
            if(this.handler == null)
                return;

            this.handler.Shutdown(SocketShutdown.Both);
            this.handler.Close();
        }
    }
}
