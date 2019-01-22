using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWBF2.Serialization;
using System.IO;

namespace SWBF2.UnitTests
{
    /// <summary>
    /// Tests the Serialization of objects. All objects are tested using the COR world.
    /// </summary>
    [TestClass]
    [DeploymentItem("Data")]
    public class TerrainTests
    {
        private ITypedFormatter<Terrain> formatter = new TerrainFormatter();

        [TestMethod]
        public void TerrainAAATest()
        {
            DeserializeSerializeTest("AAA.TER");
        }

        [TestMethod]
        public void TerrainCorTest()
        {
            DeserializeSerializeTest("assets/worlds/COR/world1/cor1.TER");
        }

        [TestMethod]
        public void TerrainDagTest()
        {
            DeserializeSerializeTest("assets/worlds/DAG/world1/dag1.TER");
        }

        [TestMethod]
        public void TerrainDeaTest()
        {
            DeserializeSerializeTest("assets/worlds/DEA/world1/dea1.TER");
        }

        [TestMethod]
        public void TerrainEndorTest()
        {
            DeserializeSerializeTest("assets/worlds/END/world1/end1.TER");
        }

        [TestMethod]
        public void TerrainFelTest()
        {
            DeserializeSerializeTest("assets/worlds/FEL/world1/fel1.TER");
        }

        [TestMethod]
        public void TerrainGeoTest()
        {
            DeserializeSerializeTest("assets/worlds/GEO/world1/geo1.TER");
        }

        [TestMethod]
        public void TerrainHothTest()
        {
            DeserializeSerializeTest("assets/worlds/HOT/world1/hoth.TER");
        }

        [TestMethod]
        public void TerrainKamTest()
        {
            DeserializeSerializeTest("assets/worlds/KAM/world1/kamino1.TER");
        }

        [TestMethod]
        public void TerrainKasTest()
        {
            DeserializeSerializeTest("assets/worlds/KAS/world2/kas2.TER");
        }

        [TestMethod]
        public void TerrainMusTest()
        {
            DeserializeSerializeTest("assets/worlds/MUS/world1/mus1.TER");
        }

        [TestMethod]
        public void TerrainMygTest()
        {
            DeserializeSerializeTest("assets/worlds/MYG/world1/myg1.TER");
        }

        [TestMethod]
        public void TerrainNabTest()
        {
            DeserializeSerializeTest("assets/worlds/NAB/world2/naboo2.TER");
        }

        [TestMethod]
        public void TerrainPolTest()
        {
            DeserializeSerializeTest("assets/worlds/POL/world1/pol1.TER");
        }

        [TestMethod]
        public void TerrainTanTest()
        {
            DeserializeSerializeTest("assets/worlds/TAN/world1/tan1.TER");
        }

        [TestMethod]
        public void TerrainTat2Test()
        {
            DeserializeSerializeTest("assets/worlds/TAT/world2/tat2.TER");
        }

        [TestMethod]
        public void TerrainTat3Test()
        {
            DeserializeSerializeTest("assets/worlds/TAT/world3/tat3.TER");
        }

        [TestMethod]
        public void TerrainUtaTest()
        {
            DeserializeSerializeTest("assets/worlds/UTA/world/uta1.TER");
        }

        [TestMethod]
        public void TerrainYavin1Test()
        {
            DeserializeSerializeTest("assets/worlds/YAV/world1/yavin1.TER");
        }

        [TestMethod]
        public void TerrainYavin2Test()
        {
            DeserializeSerializeTest("assets/worlds/YAV/world2/yav2.TER");
        }

        private void DeserializeSerializeTest(string terrainFileName)
        {
            Terrain original = null;
            using (var fs = new FileStream(terrainFileName, FileMode.Open))
            {
                original = formatter.Deserialize(fs);
            }

            using (var fs = new FileStream(terrainFileName + ".new", FileMode.CreateNew))
            {
                formatter.Serialize(fs, original);
            }

            byte[] expected = File.ReadAllBytes(terrainFileName);
            byte[] actual = File.ReadAllBytes(terrainFileName + ".new");

            var max = expected.Length > actual.Length ? actual.Length : expected.Length;

            for (int i = 0; i < max; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }

            Assert.AreEqual(expected.Length, actual.Length);
        }
    }
}