using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace LogsDemo
{
    /// <summary>
    /// 日志策略接口
    /// </summary>
    public interface ILogStrategy
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message">消息</param>
        void Write(string message);
    }

    /// <summary>
    /// 日志操作管理类
    /// </summary>
    public class Logs
    {
        private static ILogStrategy _ilogstrategy = new LogStrategy();//日志策略
        private static ILogStrategy _iemailstrategy = new EmailStrategy();//日志策略

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message">消息</param>
        public static void Write(string message)
        {
            _ilogstrategy.Write(message);
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="ex">异常对象</param>
        public static void Write(Exception ex)
        {
            _ilogstrategy.Write(string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="ex">异常对象</param>
        public static void WriteToEmail(Exception ex)
        {
            _ilogstrategy.Write(string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));
            _iemailstrategy.Write(string.Format("{0}{1}{2}", ex.Message, Environment.NewLine, ex.StackTrace));
        }
    }

    /// <summary>
    /// 基于txt文件的日志策略
    /// </summary>
    public class LogStrategy : ILogStrategy
    {
        private static object _locker = new object();//锁对象

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message">消息</param>
        public void Write(string message)
        {
            lock (_locker)
            {
                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\logs\log" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                    FileInfo fileInfo = new FileInfo(fileName);
                    if (!fileInfo.Directory.Exists)
                    {
                        fileInfo.Directory.Create();
                    }
                    if (!fileInfo.Exists)
                    {
                        fileInfo.Create().Close();
                    }
                    else if (fileInfo.Length > 2048 * 1000)
                    {
                        fileInfo.Delete();
                    }

                    fs = fileInfo.OpenWrite();
                    sw = new StreamWriter(fs);
                    sw.BaseStream.Seek(0, SeekOrigin.End);

                    sw.Write("{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                    sw.Write(Environment.NewLine);

                    StackFrame stackFrame = FindStackFrame();
                    MethodBase methodBase = GetCallingMethodBase(stackFrame);
                    string callingClass = methodBase.ReflectedType.FullName;
                    string callingMethod = methodBase.Name;
                    sw.Write("{0}.{1}", callingClass, callingMethod);
                    sw.Write(Environment.NewLine);

                    sw.Write(message);
                    sw.Write(Environment.NewLine);
                    sw.Write(Environment.NewLine);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Flush();
                        sw.Close();
                    }
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
        }

        private static StackFrame FindStackFrame()
        {
            StackTrace stackTrace = new StackTrace();
            int i = 0;
            foreach (StackFrame item in stackTrace.GetFrames())
            {
                MethodBase methodBase = item.GetMethod();
                string name = MethodBase.GetCurrentMethod().Name;
                if (!methodBase.Name.Equals("Write") && !methodBase.Name.Equals(name))
                    return new StackFrame(i, false);
                i++;
            }
            return null;
        }

        private static MethodBase GetCallingMethodBase(StackFrame stackFrame)
        {
            return stackFrame == null
                ? MethodBase.GetCurrentMethod() : stackFrame.GetMethod();
        }
    }

    /// <summary>
    /// 基于Email文件的日志策略
    /// </summary>
    public class EmailStrategy : ILogStrategy
    {
        private static object _locker = new object();//锁对象
        private static string _emailsubject = "ICT Upload Procedure Error Log";//主题
        private static string _emailFrom = "t-mics@trio-prc.com";//发件人
        private static string _emailTo = "it_cxyu@triopy.com";//收件人
        private static SmtpClient _smtpClient = new SmtpClient("mail.trio-prc.com");

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message">消息</param>
        public void Write(string message)
        {
            lock (_locker)
            {
                string hostName = Dns.GetHostName();
                string addressIp = GetIPAddressByHostName(hostName);

                StringBuilder sb = new StringBuilder();
                sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                sb.Append(" " + "⌈" + addressIp + "⌋");
                sb.Append(Environment.NewLine);

                StackFrame stackFrame = FindStackFrame();
                MethodBase methodBase = GetCallingMethodBase(stackFrame);
                string callingClass = methodBase.ReflectedType.FullName;
                string callingMethod = methodBase.Name;
                sb.Append(callingClass + "." + callingMethod);
                sb.Append(Environment.NewLine);

                sb.Append(message);
                sb.Append(Environment.NewLine);

                MailMessage mailMessage = new MailMessage();
                mailMessage.IsBodyHtml = false;
                mailMessage.SubjectEncoding = Encoding.UTF8;
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.From = new MailAddress(_emailFrom);
                mailMessage.To.Add(_emailTo);
                mailMessage.Subject = "[" + hostName + "]" + _emailsubject;
                mailMessage.Body = sb.ToString();

                try
                {
                    _smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    if (mailMessage != null)
                    {
                        mailMessage.Dispose();
                    }
                }
            }
        }

        private static string GetIPAddressByHostName(string hostName)
        {
            string addressIp = string.Empty;
            foreach (IPAddress item in Dns.GetHostEntry(hostName).AddressList)
            {
                if (item.AddressFamily.ToString() == "InterNetwork")
                    addressIp = item.ToString();
            }
            return addressIp;
        }

        private static StackFrame FindStackFrame()
        {
            StackTrace stackTrace = new StackTrace();
            int i = 0;
            foreach (StackFrame item in stackTrace.GetFrames())
            {
                MethodBase methodBase = item.GetMethod();
                string name = MethodBase.GetCurrentMethod().Name;
                if (!methodBase.Name.Equals("WriteToEmail") && !methodBase.Name.Equals("Write") && !methodBase.Name.Equals(name))
                    return new StackFrame(i, false);
                i++;
            }
            return null;
        }

        private static MethodBase GetCallingMethodBase(StackFrame stackFrame)
        {
            return stackFrame == null
                ? MethodBase.GetCurrentMethod() : stackFrame.GetMethod();
        }
    }
}
