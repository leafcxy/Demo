using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MdbHelperDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MdbInstance.ExecuteNonQuery("select * from test;");
        }
    }
}
