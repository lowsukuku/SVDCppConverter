using System;
using System.Collections.Generic;
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
        public string AddressOffsetString
        {
            get => string.Concat("0x", AddressOffset.ToString("X8"));
            set => AddressOffset = Convert.ToUInt32(value, 16);
        }

        [XmlIgnore]
        public uint AddressOffset { get; set; }

        [XmlArray("fields"), XmlArrayItem("field")]
        public List<Field> Fields;

        public override string ToString() => string.IsNullOrWhiteSpace(Description) ? $"{Name}" : $"{Name}: {Description}";
    }
}
