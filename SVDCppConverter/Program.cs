using Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SVDCppConverter
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            string svdFile = string.Empty;
            string outputFolder = string.Empty;

            using var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (_, __) => cts.Cancel();

            if (args.Length == 2)
            {
                svdFile = args[0];
                outputFolder = args[1];
            }
            else if (args.Length == 1)
            {
                svdFile = args[0];
            }
            else
            {
                Console.WriteLine("Usage: svdcppconverter <svd1> [output folder]");
                Environment.Exit(-1);
            }

            if (!string.IsNullOrWhiteSpace(outputFolder))
            {
                if (!outputFolder.EndsWith("/") && !outputFolder.EndsWith("\\"))
                    outputFolder += "/";
                outputFolder = Path.GetFullPath(outputFolder);
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);
            }

            try
            {
                Device device = Device.FromXmlFile(svdFile);
                if (!string.IsNullOrWhiteSpace(outputFolder))
                    Directory.SetCurrentDirectory(outputFolder);
                
                await device.GenerateCppHeaderAsync(cts.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
