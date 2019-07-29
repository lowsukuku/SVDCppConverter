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
            string path = @"D:\Dev\Keil\ARM\PACK\Keil\STM32L1xx_DFP\1.2.0\SVD\STM32L1xx.svd";
            XmlSerializer serializer = new XmlSerializer(typeof(Device));
            Device device;
            using (FileStream fs = File.OpenRead(path))
            {
                device = (Device)serializer.Deserialize(fs);

            }
        }
    }
}
