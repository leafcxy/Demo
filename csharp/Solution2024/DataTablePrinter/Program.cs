using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTablePrinter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataTable table = new DataTable("Prescriptions");

            table.Columns.Add("Dosage", typeof(Int32));
            table.Columns.Add("Drug", typeof(String));
            table.Columns.Add("Patient", typeof(String));
            table.Columns.Add("Date", typeof(DateTime));

            table.Rows.Add(25, "Indocin", "David", DateTime.Parse("5/1/2008 12:30:52 PM"));
            table.Rows.Add(50, "Enebrel", "Sam", DateTime.Parse("9/11/2008 8:42:30 AM"));
            table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Parse("12/12/2019 9:15:00 AM"));
            table.Rows.Add(21, "Combivent", "Janet", DateTime.Parse("12/8/2018 3:00:00 PM"));
            table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Parse("9/14/2020 4:44:44 PM"));

            Console.WriteLine(table.ToPrettyPrintedString());
            Console.ReadKey();
        }
    }
}
