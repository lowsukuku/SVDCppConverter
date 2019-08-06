using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using Core;

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
                Device device;
                using (FileStream fs = File.OpenRead(filename))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Device));
                    device = (Device)serializer.Deserialize(fs);
                }

                device.FillPeripheralDerivatives();

                string writepath = $"{device.Name}.h";
                using (StreamWriter sw = File.CreateText(writepath))
                {
                    sw.WriteLine(device.GenerateCppHeader());
                }
            }
        }
    }
}
