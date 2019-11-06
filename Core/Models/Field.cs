using System;
using System.Text;
using System.Xml.Serialization;

namespace Core.Models
{
    public class Field
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

        public string GenerateMasks()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"                {Name} = {Math.Pow(2, Width) - 1}U << {Offset}, // {Description}");
            if (Width > 1)
            {
                for (int i = 0; i < Width; i++)
                {
                    sb.AppendLine($"                {Name}_{i} = 1U << {Offset + i},");
                }
            }

            return sb.ToString();
        }
    }
}
