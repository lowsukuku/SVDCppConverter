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

        public static Field GetDummy(int width, int offset)
        {
            return new Field
            {
                Description = string.Empty,
                Name = string.Empty,
                Width = width,
                Offset = offset
            };
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Description))
                return $"{Name}; Offset {Offset}; Width {Width}";

            return $"{Name}: {Description}; Offset {Offset}; Width {Width}";
        }
    }
}
