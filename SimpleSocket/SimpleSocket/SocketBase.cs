using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SimpleSocket
{
    /// <summary>
    /// Socket接收到消息的事件委托
    /// </summary>
    public delegate void SocketReceiveEventHandle(SocketReceiveEventArgs e);

    /// <summary>
    /// Socket发生异常时的事件委托
    /// </summary>
    /// <param name="e"></param>
    public delegate void SocketExceptionEventHandle(SocketExceptionArgs e);

    public class SocketBase
    {
        /// <summary>
        /// Socket接收到消息的事件
        /// </summary>
        public event SocketReceiveEventHandle OnSocketReceive;

        /// <summary>
        /// Socket触发异常时的事件
        /// </summary>
        public event SocketExceptionEventHandle OnSocketException;

        /// <summary>
        /// 执行OnSocketReceive
        /// </summary>
        private void ProcessSocketReceive(Socket handler,string data)
        {
            if(OnSocketReceive != null)
            {
                SocketSender sender = new SocketSender(handler);
                OnSocketReceive(new SocketReceiveEventArgs(sender,data));
            }
        }

        /// <summary>
        /// 执行OnSocketException
        /// </summary>
        private void ProcessSocketException(Socket handler,Exception e)
        {
            if(OnSocketException != null)
            {
                SocketSender sender = new SocketSender(handler);
                OnSocketException(new SocketExceptionArgs(sender, e));
            }
        }

        /// <summary>
        /// BeginReceive的回调函数
        /// </summary>
        /// <param name="ar"></param>
        protected void ReceiveCallBack(IAsyncResult ar)
        {
            //获取客户端Socket
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;

            int length = 0;

            //接受客户端消息
            try{
                length = handler.EndReceive(ar);
            }catch(Exception e){
                ProcessSocketException(handler, e);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                return;
            }

            if (length <= 0){
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                return;
            }

            //读取数据
            byte[] data = new byte[length];
            Array.Copy(state.Data, 0, data, 0, length);

            ProcessSocketReceive(handler, Encoding.UTF8.GetString(data));


            //接收数据
            try{
                handler.BeginReceive(state.Data, 0, StateObject.DataLength, SocketFlags.None, ReceiveCallBack, state);
            }catch (Exception e){
                ProcessSocketException(handler, e);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
        } 
    }
}
