using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Core.Models
{
    public class Register
    {
        [XmlElement(ElementName = "name")]
        public string Name;

        [XmlElement(ElementName = "description")]
        public string Description;

        [XmlElement(ElementName = "addressOffset")]
        public string AddressOffsetBytesString
        {
            get => string.Concat("0x", AddressOffsetBytes.ToString("X8"));
            set => AddressOffsetBytes = Convert.ToUInt32(value, 16);
        }

        [XmlIgnore]
        public uint AddressOffsetBytes { get; set; }

        [XmlElement(ElementName = "size")]
        public string WidthString
        {
            get => string.Concat("0x", Width.ToString("X8"));
            set => Width = Convert.ToUInt32(value, 16);
        }

        [XmlIgnore]
        public uint Width { get; set; }

        [XmlArray("fields"), XmlArrayItem("field")]
        public List<Field> Fields;

        public override string ToString() => string.IsNullOrWhiteSpace(Description) ? $"{Name}" : $"{Name}: {Description}";

        public string GenerateRegisterMask()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var sb = new StringBuilder();
                sb.AppendLine($"            enum class {Name}Mask : u{Width} {{");

                foreach (Field field in Fields)
                {
                    sb.Append(field.GenerateMasks());
                }

                sb.AppendLine("            };")
                    .Append(GenerateFunctions());

                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private string GenerateFunctions()
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                    $"            constexpr {Name}Mask operator&({Name}Mask left, {Name}Mask right) {{")
                .AppendLine(
                    $"                return ({Name}Mask)((u{Width})left & (u{Width})right);")
                .AppendLine("            }")
                .AppendLine(
                    $"            constexpr {Name}Mask operator|({Name}Mask left, {Name}Mask right) {{")
                .AppendLine(
                    $"                return ({Name}Mask)((u{Width})left | (u{Width})right);")
                .AppendLine("            }")
                .AppendLine(
                    $"            constexpr {Name}Mask operator~({Name}Mask mask) {{")
                .AppendLine(
                    $"                return ({Name}Mask)(~((u{Width})mask));")
                .AppendLine("            }")
                .AppendLine();
            return sb.ToString();
        }

        public string GenerateClassCode(string parentPeripheralName)
        {
            return $"            using {Name} = Core::Register<u{Width}, Core::RegisterMasks::{parentPeripheralName}::{Name}Mask>; // {Description}";
        }

        public string GenerateFieldsCode(string parentPeripheralName)
        {
            return $"            Registers::{parentPeripheralName}::{Name} {Name}; // {Description}";
        }

        public static Register GetDummy(uint widthBits, uint offsetBytes)
        {
            return new Register
            {
                Description = string.Empty,
                Name = string.Empty,
                Width = widthBits,
                AddressOffsetBytes = offsetBytes
            };
        }
    }
}
