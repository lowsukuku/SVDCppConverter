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

                FillPeripheralDerivatives(device);
                AddBitFieldsDummies(device);

                string writepath = $"{device.Name}.h";
                using (StreamWriter sw = File.CreateText(writepath))
                {
                    sw.WriteLine(device.GenerateCppHeader());
                }
            }
        }

        private static void AddBitFieldsDummies(Device device)
        {
            foreach (var peripheral in device.Peripherals)
            {
                foreach (var register in peripheral.Registers)
                {
                    if (register.Bitfields.Any())
                    {
                        var bitfields = register.Bitfields.OrderByDescending(b => b.Offset).ToList();

                        var pairs = bitfields.Zip(bitfields.Skip(1), (b1, b2) => new { b1, b2 }).ToList();

                        var dummies = new List<Bitfield>();
                        foreach (var pair in pairs)
                        {
                            var nextBitfieldOffset = pair.b2.Offset + pair.b2.Width;
                            if (nextBitfieldOffset != pair.b1.Offset)
                            {
                                dummies.Add(new Bitfield
                                {
                                    Description = string.Empty,
                                    Name = string.Empty,
                                    Offset = nextBitfieldOffset,
                                    Width = pair.b1.Offset - nextBitfieldOffset
                            });
                            }
                        }

                        bitfields.AddRange(dummies);

                        var orderedBitfields = bitfields.OrderBy(b => b.Offset).ToList();

                        var firstBitfield = orderedBitfields.FirstOrDefault();
                        if (!(firstBitfield is null) && firstBitfield.Offset != 0)
                        {
                            orderedBitfields.Insert(0, new Bitfield
                            {
                                Description = string.Empty,
                                Name = string.Empty,
                                Offset = 0,
                                Width = firstBitfield.Offset
                            });
                        }

                        register.Bitfields = orderedBitfields;
                    }
                }
            }
        }

        private static void FillPeripheralDerivatives(Device device)
        {
            var derivativePeripherals = device.Peripherals.Where(p => !string.IsNullOrEmpty(p.BasePeripheralName));
            foreach (var peripheral in derivativePeripherals)
            {
                peripheral.Registers = device.Peripherals
                    .First(p => p.Name.Equals(peripheral.BasePeripheralName))
                    .Registers;
            }
        }
    }
}
