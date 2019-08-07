using Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Core.Models
{
    [XmlRoot("device")]
    public class Device
    {
        [XmlElement(ElementName = "name")]
        public string Name;

        [XmlElement(ElementName = "version")]
        public string Version { get; set; }

        [XmlElement(ElementName = "description")]
        public string Description;

        [XmlElement(ElementName = "cpu")]
        public Cpu Cpu { get; set; }

        [XmlElement(ElementName = "addressUnitBits")]
        public int MinimumBitsAddressable { get; set; }

        [XmlElement(ElementName = "width")]
        public int MaximumSingleTransferDataWidthBits { get; set; }

        [XmlElement(ElementName = "size")]
        public string DefaultRegisterWidthBitsString
        {
            get => string.Concat("0x", DefaultRegisterWidthBits.ToString("X2"));
            set => DefaultRegisterWidthBits = Convert.ToUInt32(value, 16);
        }

        [XmlIgnore]
        public uint DefaultRegisterWidthBits { get; set; }

        [XmlElement(ElementName = "resetValue")]
        public string DefaultResetValueString
        {
            get => string.Concat("0x", DefaultResetValue.ToString("X8"));
            set => DefaultResetValue = Convert.ToUInt32(value, 16);
        }

        [XmlIgnore]
        public uint DefaultResetValue { get; set; }

        [XmlElement(ElementName = "resetMask")]
        public string DefaultResetMaskString
        {
            get => string.Concat("0x", DefaultResetMask.ToString("X8"));
            set => DefaultResetMask = Convert.ToUInt32(value, 16);
        }

        [XmlIgnore]
        public uint DefaultResetMask { get; set; }

        [XmlArray("peripherals"), XmlArrayItem("peripheral")]
        public List<Peripheral> Peripherals;

        public static Device FromXmlFile(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Device));
                var device = (Device)serializer.Deserialize(fs);

                device.NormalizeDescriptions();
                device.AddBitFieldsDummies();
                device.Peripherals = device.Peripherals.OrderBy(p => p.BaseAddress).ToList();

                foreach (var peripheral in device.Peripherals)
                {
                    peripheral.Registers = peripheral.Registers.OrderBy(r => r.AddressOffset).ToList();
                }
                device.FillPeripheralDerivatives();
                return device;
            }
        }

        public void GenerateCppHeader()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString() => Name;

        private Device()
        { }

        private void NormalizeDescriptions()
        {
            Description = Description.ManyToOneLine();
            foreach (var peripheral in Peripherals)
            {
                peripheral.Description = peripheral.Description.ManyToOneLine();
                foreach (var register in peripheral.Registers)
                {
                    register.Description = register.Description.ManyToOneLine();
                    foreach (var field in register.Fields)
                    {
                        field.Description = field.Description.ManyToOneLine();
                    }
                }
            }
        }

        private void AddBitFieldsDummies()
        {
            foreach (var peripheral in Peripherals)
            {
                foreach (var register in peripheral.Registers)
                {
                    if (register.Fields.Any())
                    {
                        var fields = register.Fields.OrderByDescending(b => b.Offset).ToList();

                        var fieldPairs = fields.Zip(fields.Skip(1), (b1, b2) => new { b1, b2 }).ToList();

                        var dummies = new List<Field>();
                        foreach (var pair in fieldPairs)
                        {
                            var nextFieldOffset = pair.b2.Offset + pair.b2.Width;
                            if (nextFieldOffset != pair.b1.Offset)
                            {
                                dummies.Add(Field.GetDummy(width: pair.b1.Offset - nextFieldOffset, offset: nextFieldOffset));
                            }
                        }

                        fields.AddRange(dummies);

                        var orderedFields = fields.OrderBy(b => b.Offset).ToList();

                        var firstField = orderedFields.FirstOrDefault();
                        if (!(firstField is null) && firstField.Offset != 0)
                        {
                            orderedFields.Insert(0, Field.GetDummy(width: firstField.Offset, offset: 0));
                        }

                        register.Fields = orderedFields;
                    }
                }
            }
        }

        private void FillPeripheralDerivatives()
        {
            var derivativePeripherals = Peripherals.Where(p => !string.IsNullOrEmpty(p.BasePeripheralName));
            foreach (var peripheral in derivativePeripherals)
            {
                var basePeripheral = Peripherals.First(p => p.Name.Equals(peripheral.BasePeripheralName));
                peripheral.Registers = basePeripheral.Registers;
                peripheral.Description = basePeripheral.Description;
                peripheral.GroupName = basePeripheral.GroupName;
            }
        }
    }
}
