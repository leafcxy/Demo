using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadPoolDemo
{
    internal class Program
    {
        static object _lock = new object();
        delegate void MyAsyncDelegate();

        static void Main(string[] args)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            ThreadPool.QueueUserWorkItem(func3, null);
            ThreadPool.QueueUserWorkItem(func3, null);
            ThreadPool.QueueUserWorkItem(func3, null);
            Console.ReadKey();
        }

        static void func(object obj)
        {
            lock (_lock)
            {
                //Thread.Sleep(1000);
                Console.WriteLine("func:" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + " - " + Thread.CurrentThread.ManagedThreadId);
            }
        }

        static void func2(object obj)
        {
            new Action(() =>
            {
                Console.WriteLine("func:" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + " - " + Thread.CurrentThread.ManagedThreadId);
            }).Invoke();
        }

        static void func3(object obj)
        {
            // 创建异步委托实例
            MyAsyncDelegate asyncDelegate = new MyAsyncDelegate(MyAsyncMethod);

            // 异步调用委托
            asyncDelegate.BeginInvoke(MyAsyncCallback, null);

            // 继续执行其他代码
            Console.WriteLine("异步调用进行中");
        }
        static void MyAsyncMethod()
        {
            // 异步操作
            Thread.Sleep(1000);
            Console.WriteLine("func:" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + " - " + Thread.CurrentThread.ManagedThreadId);
        }

        static void MyAsyncCallback(IAsyncResult result)
        {
            // 异步操作完成后的代码
            Console.WriteLine("异步操作完成");
        }

    }
}
