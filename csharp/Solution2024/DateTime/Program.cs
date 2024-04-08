using System;
using System.Globalization;

namespace DateTimeDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Convert.ToDateTime("2023-07-07");
                //Convert.ToDateTime("2023-07-10");
                Console.WriteLine(0.1 + 0.2);

                System.DateTime.ParseExact("2023/07/07", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                System.DateTime.ParseExact("2023/07/10", "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadKey();

        }
    }
}
