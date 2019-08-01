using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Core
{
    [XmlRoot("device")]
    public class Device
    {
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlArray("peripherals"),XmlArrayItem("peripheral")]
        public List<Peripheral> Peripherals;
    }

    public class Peripheral
    {
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlElement(ElementName = "description")]
        public string Description;
        [XmlArray("registers"), XmlArrayItem("register")]
        public List<Register> Registers;
    }

    public class Register
    {
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlElement(ElementName = "description")]
        public string Description;
        [XmlArray("fields"), XmlArrayItem("field")]
        public List<Bitfield> Bitfields;
    }

    public class Bitfield
    {
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlElement(ElementName = "description")]
        public string Description;
        [XmlElement(ElementName = "bitWidth")]
        public string Width;
        [XmlElement(ElementName = "bitOffset")]
        public string Offset;
    }
}
