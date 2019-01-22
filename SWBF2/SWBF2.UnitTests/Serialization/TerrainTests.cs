using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWBF2.Serialization;
using System.IO;

namespace SWBF2.UnitTests.Serialization
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
        public void TerrainCorTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/COR/world1/cor1.TER");
        }

        [TestMethod]
        public void TerrainDagTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/DAG/world1/dag1.TER");
        }

        [TestMethod]
        public void TerrainDeaTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/DEA/world1/dea1.TER");
        }

        [TestMethod]
        public void TerrainEndorTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/END/world1/end1.TER");
        }

        [TestMethod]
        public void TerrainFelTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/FEL/world1/fel1.TER");
        }

        [TestMethod]
        public void TerrainGeoTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/GEO/world1/geo1.TER");
        }

        [TestMethod]
        public void TerrainHothTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/HOT/world1/hoth.TER");
        }

        [TestMethod]
        public void TerrainKamTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/KAM/world1/kamino1.TER");
        }

        [TestMethod]
        public void TerrainKasTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/KAS/world2/kas2.TER");
        }

        [TestMethod]
        public void TerrainMusTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/MUS/world1/mus1.TER");
        }

        [TestMethod]
        public void TerrainMygTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/MYG/world1/myg1.TER");
        }

        [TestMethod]
        public void TerrainNabTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/NAB/world2/naboo2.TER");
        }

        [TestMethod]
        public void TerrainPolTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/POL/world1/pol1.TER");
        }

        [TestMethod]
        public void TerrainTanTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/TAN/world1/tan1.TER");
        }

        [TestMethod]
        public void TerrainTat2Test()
        {
            TerrainDeserializeSerializeTest("assets/worlds/TAT/world2/tat2.TER");
        }

        [TestMethod]
        public void TerrainTat3Test()
        {
            TerrainDeserializeSerializeTest("assets/worlds/TAT/world3/tat3.TER");
        }

        [TestMethod]
        public void TerrainUtaTest()
        {
            TerrainDeserializeSerializeTest("assets/worlds/UTA/world/uta1.TER");
        }

        [TestMethod]
        public void TerrainYavin1Test()
        {
            TerrainDeserializeSerializeTest("assets/worlds/YAV/world1/yavin1.TER");
        }

        [TestMethod]
        public void TerrainYavin2Test()
        {
            TerrainDeserializeSerializeTest("assets/worlds/YAV/world2/yav2.TER");
        }

        private void TerrainDeserializeSerializeTest(string terrainFileName)
        {
            //terrainFileName = "C:\\BF2_ModTools\\" + terrainFileName;

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