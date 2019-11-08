using Core.Models;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateFrom8Bits_OffsetGets8Bits()
        {
            var offset = Offset.FromBits(8);
            Assert.AreEqual(8, offset.Bits);
        }

        [Test]
        public void CreateFrom8Bits_OffsetGets1Byte()
        {
            var offset = Offset.FromBits(8);
            Assert.AreEqual(1, offset.Bytes);
        }

        [Test]
        public void CreateFrom1Byte_OffsetGets8Bits()
        {
            var offset = Offset.FromBytes(1);
            Assert.AreEqual(8, offset.Bits);
        }

        [Test]
        public void CreateFrom1Byte_OffsetGets1Byte()
        {
            var offset = Offset.FromBytes(1);
            Assert.AreEqual(1, offset.Bytes);
        }

        [Test]
        public void CreateFrom1And2Bytes_SumGets3Bytes()
        {
            var left = Offset.FromBytes(1);
            var right = Offset.FromBytes(2);
            var offset = left + right;
            Assert.AreEqual(3, offset.Bytes);
            offset = right + left;
            Assert.AreEqual(3, offset.Bytes);
        }
    }
}
