using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SimpleSocket
{
    public class SimpleClient:SocketBase
    {
        /// <summary>
        /// 服务端Socket
        /// </summary>
        private Socket m_handler;

        /// <summary>
        /// 监听IP
        /// </summary>
        public string IP { get; private set; }

        /// <summary>
        /// 监听端口
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// 监听IP
        /// </summary>
        public IPAddress IPAddress { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleClient(IPAddress ipAddress, int port)
        {
            this.IPAddress = ipAddress;
            this.IP = this.IPAddress.ToString();
            this.Port = port;
        }

        public SimpleClient(string ip, int port)
            : this(IPAddress.Parse(ip), port){ }

        public SimpleClient(int port)
            : this(IPAddress.Any, port) { }
        /// <summary>
        /// 连接到服务器
        /// </summary>
        public void Connect()
        {
            m_handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_handler.Connect(new IPEndPoint(this.IPAddress, this.Port));
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        public void Send(string message)
        {
            SocketSender sender = new SocketSender(m_handler);
            sender.Send(message);
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        public void Receive()
        {
            StateObject state = new StateObject();
            state.WorkSocket = m_handler;

            try{
                m_handler.BeginReceive(state.Data, 0, StateObject.DataLength, SocketFlags.None,this.ReceiveCallBack, state);
            }catch (Exception){
                m_handler.Shutdown(SocketShutdown.Both);
                m_handler.Close();
            }
        }
    }
}
