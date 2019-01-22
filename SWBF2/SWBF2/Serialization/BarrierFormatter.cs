using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class BarrierFormatter : ITypedFormatter<IList<Barrier>>
    {
        public IList<Barrier> Deserialize(Stream serializationStream)
        {
            IList<Barrier> barriers = new List<Barrier>();

            using (var reader = new StreamReader(serializationStream))
            {
                var line = reader.ReadLine();
                var startIndex = line.IndexOf("(") + 1;
                var count = int.Parse(line.Substring(startIndex, line.IndexOf(")") - startIndex));

                // Blank
                reader.ReadLine();

                for (int i = 0; i < count; i++)
                {
                    line = reader.ReadLine();
                    startIndex = line.IndexOf("\"") + 1;
                    var name = line.Substring(startIndex, line.LastIndexOf("\"") - startIndex);
                    var barrier = new Barrier(name);

                    if (!"{".Equals(reader.ReadLine()))
                    {
                        throw new InvalidDataException("File corrupted at " + reader.BaseStream.Position);
                    }

                    for (int j = 0; j < 4; j++)
                    {
                        barrier.Corners.Add(ReadCorner(reader));
                    }

                    line = reader.ReadLine();

                    startIndex = line.IndexOf("(") + 1;
                    barrier.Flag = int.Parse(line.Substring(startIndex, line.IndexOf(")") - startIndex));

                    if (!"}".Equals(reader.ReadLine()))
                    {
                        throw new InvalidDataException("File corrupted at " + reader.BaseStream.Position);
                    }

                    barriers.Add(barrier);

                    reader.ReadLine();
                }
            }

            return barriers;
        }

        /// <summary>
        /// Reads the value of the corner positions
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Vector3 ReadCorner(StreamReader reader)
        {
            var line = reader.ReadLine();
            var startIndex = line.IndexOf("(") + 1;
            line = line.Substring(startIndex, line.IndexOf(")") - startIndex);
            var positions = line.Split(',');
            // Maybe check for negative zero?
            return new Vector3(double.Parse(positions[0].Trim(' ')), double.Parse(positions[1].Trim(' ')), double.Parse(positions[2].Trim(' ')));
        }

        public void Serialize(Stream serializationStream, IList<Barrier> barriers)
        {
            using (var writer = new StreamWriter(serializationStream))
            {
                writer.WriteLine(string.Format("BarrierCount({0});", barriers.Count));
                writer.WriteLine();

                for (int i = 0; i < barriers.Count; i++)
                {
                    var barrier = barriers[i];
                    writer.WriteLine(string.Format("Barrier(\"{0}\")", barrier.Name));
                    writer.WriteLine("{");

                    foreach (var corner in barrier.Corners)
                    {
                        writer.WriteLine(string.Format("\tCorner({0});", corner));
                    }

                    writer.WriteLine(string.Format("\tFlag({0});", barrier.Flag));
                    writer.WriteLine("}");

                    if (i != barriers.Count - 1)
                    {
                        writer.WriteLine();
                    }
                }
            }
        }
    }
}