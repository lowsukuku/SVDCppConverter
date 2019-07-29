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
        public string name;
        [XmlArrayItem("peripheral")]
        public List<Peripheral> peripherals;
    }

    public class Peripheral
    {
        public string name;
    }
}
