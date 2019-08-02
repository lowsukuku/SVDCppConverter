﻿using System;
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

                var bitfields = device.Peripherals[49].Registers[1].Bitfields;

                {
                    var pairs = bitfields.Zip(bitfields.Skip(1), (b1, b2) => new {b1, b2}).ToList();

                    var dummies = new List<Bitfield>();
                    foreach (var pair in pairs)
                    {
                        if (pair.b2.Offset + pair.b2.Width != pair.b1.Offset)
                        {
                            var description = "// Dummy";
                            var name = $"Dummy{dummies.Count}";
                            var offset = pair.b2.Offset + pair.b2.Width;
                            var width = pair.b1.Offset - offset;

                            dummies.Add(new Bitfield
                            {
                                Description = description,
                                Name = name,
                                Offset = offset,
                                Width = width
                            });
                        }
                    }

                    bitfields.AddRange(dummies);
                }

                bitfields = bitfields.OrderByDescending(b => b.Offset).ToList();

                string writepath = $"{device.Name}.h";
                using (StreamWriter sw = File.CreateText(writepath))
                {
                    sw.WriteLine(device.GenerateCppHeader(0));
                }
            }
        }
    }
}
