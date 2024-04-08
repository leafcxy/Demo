using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

namespace PipeDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 创建命名管道
            using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("MyPipe", PipeDirection.InOut))
            {
                Console.WriteLine("等待客户端连接...");

                // 等待客户端连接
                pipeServer.WaitForConnection();

                Console.WriteLine("客户端已连接。");

                // 从管道中读取数据
                byte[] buffer = new byte[100];
                int bytesRead = pipeServer.Read(buffer, 0, buffer.Length);

                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                Console.WriteLine("从管道中读取到的数据：{0}", message);

                // 向管道中写入数据
                string response = "Hello, pipe client!";
                byte[] responseBuffer = Encoding.ASCII.GetBytes(response);
                pipeServer.Write(responseBuffer, 0, responseBuffer.Length);

                Console.WriteLine("数据已写入管道。");

                // 断开连接并关闭管道
                pipeServer.Disconnect();
            }

            Console.ReadLine();

        }
    }
}
