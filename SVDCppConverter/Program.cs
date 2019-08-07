using Core.Models;
using System;
using System.Linq;

namespace SVDCppConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Console.WriteLine("Usage: svdcppconverter <svd1> <svd2> ... <svdN>");
                return;
            }

            foreach (var filename in args)
            {
                try
                {
                    Device device = Device.FromXmlFile(filename);
                    //device.GenerateCppHeader();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
