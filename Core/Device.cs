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
        [XmlArray("registers"), XmlArrayItem("register")]
        public List<Register> Registers;
    }

    public class Register
    {
        [XmlElement(ElementName = "name")]
        public string Name;
    }
}
