using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.Models
{
    public class Peripheral
    {
        [XmlAttribute(AttributeName = "derivedFrom")]
        public string BasePeripheralName;

        [XmlElement(ElementName = "name")]
        public string Name;

        [XmlElement(ElementName = "description")]
        public string Description;

        [XmlElement(ElementName = "groupName")]
        public string GroupName { get; set; }

        [XmlElement(ElementName = "baseAddress")]
        public string BaseAddressString
        {
            get => string.Concat("0x", BaseAddress.ToString("X8"));
            set => BaseAddress = Convert.ToUInt32(value, 16);
        }

        [XmlIgnore]
        public uint BaseAddress { get; set; }

        [XmlArray("registers"), XmlArrayItem("register")]
        public List<Register> Registers;

        public override string ToString() => string.IsNullOrWhiteSpace(Description) ? $"{Name}" : $"{Name}: {Description}";
    }
}
