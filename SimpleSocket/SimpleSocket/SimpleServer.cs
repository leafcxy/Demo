using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;

namespace SimpleSocket
{
    public class SimpleServer:SocketBase
    {
        /// <summary>
        /// 多线程标志位
        /// </summary>
        private readonly ManualResetEvent m_acceptSignal = new ManualResetEvent(false);

        /// <summary>
        /// 客户端列表
        /// </summary>
        private List<Socket> m_handlerList = new List<Socket>();

        /// <summary>
        /// 服务端Socket
        /// </summary>
        private Socket m_listener;

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
        /// 监听数目
        /// </summary>
        public int ListenCount { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleServer(IPAddress ipAddress, int port, int listenCount)
        {
            this.IPAddress = ipAddress;
            this.IP = this.IPAddress.ToString();
            this.Port = port;
            this.ListenCount = listenCount;
        }

        public SimpleServer(string ipAddress, int port, int listenConunt)
            : this(IPAddress.Parse(ipAddress), port, listenConunt) { }

        public SimpleServer(IPAddress ipAddress, int port)
            : this(ipAddress, port, 100) { }

        public SimpleServer(string ipAddress, int port)
            : this(IPAddress.Parse(ipAddress), port, 100) { }

        public SimpleServer(int port)
            : this(IPAddress.Any, port, 100) { }

        public SimpleServer(int port, int listenCount)
            : this(IPAddress.Any, port, listenCount) { }

        public SimpleServer(IPEndPoint localPoint, int listenCount)
            : this(localPoint.Address, localPoint.Port, listenCount) { }

        public SimpleServer(IPEndPoint localPoint)
            : this(localPoint.Address, localPoint.Port, 100) { }


        /// <summary>
        /// 开始监听
        /// </summary>
        public void Listen()
        {
            //初始化服务端Socket
            m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_listener.Bind(new IPEndPoint(this.IPAddress, this.Port));
            m_listener.Listen(this.ListenCount);

            //连接客户端
            while(true)
            {
                m_acceptSignal.Reset();
                m_listener.BeginAccept(AcceptCallBack, m_listener);
                m_acceptSignal.WaitOne();
            }
        }

        /// <summary>
        /// BeginAccept的回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallBack(IAsyncResult ar)
        {
             m_acceptSignal.Set();
            //获取服务端和客户端Socket
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            //将客户端添加至客户端列表
            m_handlerList.Add(handler);
            
            //向客户端广播消息
            Console.WriteLine(string.Format("客户端{0}已上线", handler.RemoteEndPoint));

            StateObject state = new StateObject();
            state.WorkSocket = handler;

            //从客户端接收消息
            try{
                handler.BeginReceive(state.Data, 0, StateObject.DataLength, SocketFlags.None, ReceiveCallBack, state);
            }
            catch (Exception e)
            {
                Broadcast(string.Format("客户端{0}下线", handler.RemoteEndPoint));
                m_handlerList.Remove(handler);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        }

        /// <summary>
        /// 向所有的客户端广播消息
        /// </summary>
        public void Broadcast(string message)
        {
            if(m_handlerList == null)
                return;

            if(m_handlerList.Count <= 0)
                return;

            //遍历客户端列表并发送消息
            foreach(Socket handler in m_handlerList)
            {
                SocketSender sender = new SocketSender(handler);
                sender.Send(message);
            }
        }
    }
}
