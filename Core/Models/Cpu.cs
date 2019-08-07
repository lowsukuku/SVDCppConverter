using System.Xml.Serialization;

namespace Core.Models
{
    public class Cpu
    {
        [XmlElement(ElementName = "name")]
        public string Name;

        [XmlElement(ElementName = "revision")]
        public string Revision;

        [XmlElement(ElementName = "endian")]
        public string Endianess;

        [XmlElement(ElementName = "mpuPresent")]
        public bool MpuPresent;

        [XmlElement(ElementName = "fpuPresent")]
        public bool FpuPresent;

        [XmlElement(ElementName = "nvicPrioBits")]
        public int NvicPriorityBits;

        [XmlElement(ElementName = "vendorSystickConfig")]
        public bool VendorSystickConfig;
    }
}
