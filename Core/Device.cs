﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core
{
    [XmlRoot("device")]
    public class Device : ICppHeaderItem
    {
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlArray("peripherals"),XmlArrayItem("peripheral")]
        public List<Peripheral> Peripherals;

        public override string ToString() => Name;

        public string GenerateCppHeader(int indentation)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"// THIS FILE IS AUTOGENERATED")
                .AppendLine($"// {Name}")
                .AppendLine();

            foreach (var peripheral in Peripherals)
            {
                sb.Append(peripheral.GenerateCppHeader(0));
            }

            return sb.ToString();
        }
    }

    public class Peripheral : ICppHeaderItem
    {
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlElement(ElementName = "description")]
        public string Description;
        [XmlArray("registers"), XmlArrayItem("register")]
        public List<Register> Registers;

        public string GenerateCppHeader(int indentation)
        {
            var sb = new StringBuilder();
            var comment = string.Join(' ', Description?.Split(' ', '\r', '\n'), string.Empty);

            sb.AppendLine()
                .AppendLine($"// {comment}")
                .AppendLine($"class {Name}")
                .AppendLine("{");

            foreach (var register in Registers)
            {
                sb.Append(register.GenerateCppHeader(4));
            }

            sb.AppendLine("};")
                .AppendLine();

            return sb.ToString();
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Description))
                return $"{Name}";

            return $"{Name}: {Description}";
        }
    }

    public class Register : ICppHeaderItem
    {
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlElement(ElementName = "description")]
        public string Description;
        [XmlArray("fields"), XmlArrayItem("field")]
        public List<Bitfield> Bitfields;

        public string GenerateCppHeader(int indentation)
        {
            var sb = new StringBuilder();

            var comment = string.Join(' ', Description?.Split(' ', '\r', '\n'));
            sb.Append(' ', indentation)
                .AppendLine($"volatile unsigned int {Name}; // {comment}");

            return sb.ToString();
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Description))
                return $"{Name}";

            return $"{Name}: {Description}";
        }
    }

    public class Bitfield
    {
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlElement(ElementName = "description")]
        public string Description;
        [XmlElement(ElementName = "bitWidth")]
        public int Width;
        [XmlElement(ElementName = "bitOffset")]
        public int Offset;

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Description))
                return $"{Name}; Offset {Offset}; Width {Width}";

            return $"{Name}: {Description}; Offset {Offset}; Width {Width}";
        }
    }
}
