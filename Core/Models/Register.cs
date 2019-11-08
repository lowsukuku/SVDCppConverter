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
        public string OffsetBytesString
        {
            get => string.Concat("0x", Offset.Bytes.ToString("X8"));
            set => Offset = Offset.FromBytes(Convert.ToInt32(value, 16));
        }

        [XmlIgnore]
        public Offset Offset { get; set; }

        [XmlElement(ElementName = "size")]
        public string WidthBitsString
        {
            get => string.Concat("0x", Width.Bits.ToString("X8"));
            set => Width = Width.FromBits(Convert.ToInt32(value, 16));
        }

        [XmlIgnore]
        public Width Width { get; set; }

        [XmlArray("fields"), XmlArrayItem("field")]
        public List<Field> Fields;

        public override string ToString() => string.IsNullOrWhiteSpace(Description) ? $"{Name}" : $"{Name}: {Description}";

        public string GenerateRegisterMask()
        {
            if (!string.IsNullOrWhiteSpace(Name))
            {
                var sb = new StringBuilder();
                sb.AppendLine($"            enum class {Name}Mask : u{Width.Bits} {{");

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

        public string GenerateClassCode(string parentPeripheralName)
        {
            return $"            using {Name} = Core::Register<u{Width.Bits}, Core::RegisterMasks::{parentPeripheralName}::{Name}Mask>; // {Description}";
        }

        public string GenerateFieldsCode(string parentPeripheralName)
        {
            return $"            Registers::{parentPeripheralName}::{Name} {Name}; // {Description}";
        }

        public static Register GetDummy(Width width, Offset offset)
        {
            return new Register
            {
                Description = string.Empty,
                Name = string.Empty,
                Width = width,
                Offset = offset,
                Fields = new List<Field>()
            };
        }

        private string GenerateFunctions()
        {
            var sb = new StringBuilder();
            sb.AppendLine(
                    $"            constexpr {Name}Mask operator&({Name}Mask left, {Name}Mask right) {{")
                .AppendLine(
                    $"                return ({Name}Mask)((u{Width.Bits})left & (u{Width.Bits})right);")
                .AppendLine("            }")
                .AppendLine(
                    $"            constexpr {Name}Mask operator|({Name}Mask left, {Name}Mask right) {{")
                .AppendLine(
                    $"                return ({Name}Mask)((u{Width.Bits})left | (u{Width.Bits})right);")
                .AppendLine("            }")
                .AppendLine(
                    $"            constexpr {Name}Mask operator~({Name}Mask mask) {{")
                .AppendLine(
                    $"                return ({Name}Mask)(~((u{Width.Bits})mask));")
                .AppendLine("            }")
                .AppendLine();
            return sb.ToString();
        }
    }
}
