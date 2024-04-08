using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogsDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Convert.ToInt32("A");
            }
            catch (Exception ex)
            {
                Logs.WriteToEmail(ex);
            }

            Console.ReadKey();
        }
    }
}
