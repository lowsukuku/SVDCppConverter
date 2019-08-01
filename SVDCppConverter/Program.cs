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
            string readpath = @"D:\Dev\Keil\ARM\PACK\Keil\STM32L1xx_DFP\1.2.0\SVD\STM32L1xx.svd";
            XmlSerializer serializer = new XmlSerializer(typeof(Device));
            Device device;
            using (FileStream fs = File.OpenRead(readpath))
            {
                device = (Device)serializer.Deserialize(fs);
            }
            string writepath = $"{device.Name}.h";
            using (StreamWriter sw = File.CreateText(writepath))
            {
                foreach (var peripheral in device.Peripherals)
                {
                    sw.WriteLine($"/* {peripheral.Description?.Replace('\n', ' ')} */\r\nclass {peripheral.Name}\r\n{{");
                    foreach (var register in peripheral.Registers)
                    {

                        sw.WriteLine($"\tvolatile unsigned int {register.Name}; //{register.Description?.Replace('\n', ' ')}");
                    }
                    sw.WriteLine("};");
                }
            }
        }
    }
}
