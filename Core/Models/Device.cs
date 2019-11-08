﻿using Core.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        public string ResetValueString
        {
            get => string.Concat("0x", ResetValue.ToString("X8"));
            set => ResetValue = Convert.ToUInt32(value, 16);
        }

        [XmlIgnore]
        public uint ResetValue { get; set; }

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
            using FileStream fs = File.OpenRead(path);
            XmlSerializer serializer = new XmlSerializer(typeof(Device));
            var device = (Device)serializer.Deserialize(fs);

            device.NormalizeDescriptions();
            device.Peripherals = device.Peripherals.OrderBy(p => p.BaseAddress).ToList();

            foreach (var peripheral in device.Peripherals)
            {
                peripheral.Registers = peripheral.Registers.OrderBy(r => r.Offset.Bytes).ToList();
                foreach (Register register in peripheral.Registers)
                {
                    register.Fields = register.Fields.OrderBy(f => f.Offset).ToList();
                }
            }
            device.AddDummyRegisters();
            device.FillPeripheralDerivatives();
            return device;
        }

        private void AddDummyRegisters()
        {
            foreach (Peripheral peripheral in Peripherals)
            {
                peripheral.AddDummyRegisters();
            }
        }

        public async Task GenerateCppHeaderAsync(CancellationToken ct)
        {
            var sb = new StringBuilder();
            sb.AppendLine("// This file is autogenerated")
                .AppendLine()
                .AppendLine($"// {Name}")
                .AppendLine($"// {Description}")
                .AppendLine()
                .AppendLine("#pragma once")
                .AppendLine();

            foreach (Peripheral peripheral in Peripherals)
            {
                sb.AppendLine($"#include \"{peripheral.Name}.h\"");
                await peripheral.GenerateCppHeaderAsync(ct);
            }

            await using var file = File.Create($"{Name}.h");
            await using var header = new StreamWriter(file, Encoding.UTF8);
            await header.WriteAsync(sb, ct);

            var assembly = typeof(Device).Assembly;
            await using var registerHeader = File.Create("Register.h");
            using var resourceReader = new StreamReader(assembly.GetManifestResourceStream("Core.Templates.Register.h"));
            await resourceReader.BaseStream.CopyToAsync(registerHeader, ct);
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
