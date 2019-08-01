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

        public override string ToString() => Name;
    }

    public class Peripheral
    {
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlElement(ElementName = "description")]
        public string Description;
        [XmlArray("registers"), XmlArrayItem("register")]
        public List<Register> Registers;

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Description))
                return $"{Name}";

            return $"{Name}: {Description}";
        }
    }

    public class Register
    {
        [XmlElement(ElementName = "name")]
        public string Name;
        [XmlElement(ElementName = "description")]
        public string Description;
        [XmlArray("fields"), XmlArrayItem("field")]
        public List<Bitfield> Bitfields;

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
