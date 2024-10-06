using System;
using System.Threading;
using System.Windows.Forms;

namespace VOX_NS
{
    internal static class Program
    {
        private const string MutexName = "VOX_NS";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(true, MutexName, out bool createdNew))
            {
                if (!createdNew)
                {
                    // 另一个实例正在运行
                    MessageBox.Show("应用程序已经在运行了！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }

                //捕获全局未处理异常
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmMain());
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            Logs.Write(ex);
            MessageBox.Show(ex.Message.ToString());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            Logs.Write(ex);
            MessageBox.Show(ex.Message.ToString());
        }
    }
}
