using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWBF2.Serialization;

namespace SWBF2.UnitTests.Serialization
{
    [TestClass]
    public class RegionTests : BaseFormatterTest<Region>
    {
        public RegionTests() : base(new RegionFormatter(), "RGN")
        {
        }

        [TestMethod]
        public void CorTest()
        {
            DeserializeSerializeTest("COR");
        }

        [TestMethod]
        public void DagTest()
        {
            DeserializeSerializeTest("DAG");
        }

        [TestMethod]
        public void DeaTest()
        {
            DeserializeSerializeTest("DEA");
        }

        [TestMethod]
        public void EndorTest()
        {
            DeserializeSerializeTest("END");
        }

        [TestMethod]
        public void FelTest()
        {
            DeserializeSerializeTest("FEL");
        }

        [TestMethod]
        public void GeoTest()
        {
            DeserializeSerializeTest("GEO");
        }

        [TestMethod]
        public void HothTest()
        {
            DeserializeSerializeTest("HOT");
        }

        [TestMethod]
        public void KamTest()
        {
            DeserializeSerializeTest("KAM");
        }

        [TestMethod]
        public void KasTest()
        {
            DeserializeSerializeTest("KAS");
        }

        [TestMethod]
        public void MusTest()
        {
            DeserializeSerializeTest("MUS");
        }

        [TestMethod]
        public void MygTest()
        {
            DeserializeSerializeTest("MYG");
        }

        [TestMethod]
        public void NabTest()
        {
            DeserializeSerializeTest("NAB");
        }

        [TestMethod]
        public void PolTest()
        {
            DeserializeSerializeTest("POL");
        }

        [TestMethod]
        public void TanTest()
        {
            DeserializeSerializeTest("TAN");
        }

        [TestMethod]
        public void TatTest()
        {
            DeserializeSerializeTest("TAT");
        }

        [TestMethod]
        public void UtaTest()
        {
            DeserializeSerializeTest("UTA");
        }

        [TestMethod]
        public void Yavin1Test()
        {
            DeserializeSerializeTest("YAV");
        }
    }
}