using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validator;

namespace MowSessionValidatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] newArgs = new string[] {
                @"C:\git\MobileWeb\Website\m.alaskaair.com\bin",
                @"C:\git\MobileWebShopping\Shopping\bin",
                @"C:\git\MobileWebCheckOut\Booking\bin"
            };
            var main = new Main(args.Length < 2 ? newArgs : args);
            var info = main.Run();
            if (info.AnyDiscrepancies)
            {
                Console.WriteLine("{0} discrepancies found.", info.Runs.Sum(run => run.Discrepancies.Count()));
                Console.WriteLine("Source Directory: {0}", info.SourceDirectory);
                foreach (var run in info.Runs)
                {
                    Console.WriteLine("\t Comparison Directory: {0}", run.ComparisonDirectory);
                    run.Discrepancies.ForEach(discrepancy => Console.WriteLine("\t\t{0}", discrepancy));
                }
                Environment.Exit(1);
            }
            Console.WriteLine("Everything Okay. No discrepancies found.");
            Environment.Exit(0);
        }
    }
}
