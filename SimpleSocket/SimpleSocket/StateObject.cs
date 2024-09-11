using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace SimpleSocket
{
    public class StateObject
    {
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        public const int DataLength = 1024;

        /// <summary>
        /// 当前工作的Sokcet
        /// </summary>
        public Socket WorkSocket { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public byte[] Data { get; set; }

        public StateObject()
        {
            this.WorkSocket = null;
            this.Data = new byte[DataLength];
        }
    }
}
