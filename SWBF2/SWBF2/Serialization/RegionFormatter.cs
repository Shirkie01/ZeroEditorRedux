using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class RegionFormatter : ITypedFormatter<IList<Region>>
    {
        public IList<Region> Deserialize(Stream serializationStream)
        {
            IList<Region> regions = new List<Region>();
            using (var reader = new StreamReader(serializationStream))
            {
                // version
                var line = reader.ReadLine();

                // count
                line = reader.ReadLine();
                int startIndex = line.IndexOf("(") + 1;
                line = line.Substring(startIndex, line.IndexOf(")") - startIndex);
                int regionCount = int.Parse(line);

                reader.ReadLine();

                for (int i = 0; i < regionCount; i++)
                {
                    // region name and id
                    line = reader.ReadLine();

                    startIndex = line.IndexOf("\"") + 1;
                    string regionType = line.Substring(startIndex, line.LastIndexOf("\"") - startIndex);

                    startIndex = line.IndexOf(", ") + 1;
                    line = line.Substring(startIndex, line.LastIndexOf(")") - startIndex);
                    int id = int.Parse(line);

                    Region region = new Region(regionType, id);

                    // {
                    reader.ReadLine();

                    line = reader.ReadLine();
                    if (line.Contains("Layer"))
                    {
                        startIndex = line.IndexOf("(") + 1;
                        region.LayerId = int.Parse(line.Substring(startIndex, line.IndexOf(")") - startIndex));
                        line = reader.ReadLine();
                    }

                    region.Position = Vector3.Parse(line.Substring(line.IndexOf("(")));

                    line = reader.ReadLine();
                    region.Rotation = Quaternion.Parse(line.Substring(line.IndexOf("(")));

                    line = reader.ReadLine();
                    region.Size = Vector3.Parse(line.Substring(line.IndexOf("(")));

                    line = reader.ReadLine();

                    if (line.Contains("Name"))
                    {
                        startIndex = line.IndexOf("\"") + 1;
                        line = line.Substring(startIndex, line.LastIndexOf("\"") - startIndex);
                        region.Name = line;
                        line = reader.ReadLine();
                    }

                    if (line.Contains("NextIsGrouped"))
                    {
                        region.NextIsGrouped = true;
                        line = reader.ReadLine();
                    }

                    // }
                    if ("}" != line)
                    {
                        throw new InvalidDataException($"End of region expected. '{line}'");
                    }

                    // blank line
                    line = reader.ReadLine();

                    regions.Add(region);
                }
            }

            return regions;
        }

        public void Serialize(Stream serializationStream, IList<Region> regions)
        {
            using (var writer = new StreamWriter(serializationStream))
            {
                writer.WriteLine("Version(1);");
                writer.WriteLine($"RegionCount({regions.Count});");

                foreach (var region in regions)
                {
                    writer.WriteLine();
                    writer.WriteLine($"Region(\"{region.RegionType}\", {region.Id})");
                    writer.WriteLine("{");

                    if (region.LayerId != -1)
                    {
                        writer.WriteLine(string.Format("\tLayer({0});", region.LayerId));
                    }

                    writer.WriteLine(string.Format("\tPosition({0});", region.Position));
                    writer.WriteLine(string.Format("\tRotation({0});", region.Rotation));
                    writer.WriteLine(string.Format("\tSize({0});", region.Size));

                    if (!string.IsNullOrEmpty(region.Name))
                    {
                        writer.WriteLine(string.Format("\tName(\"{0}\");", region.Name));
                    }

                    if (region.NextIsGrouped)
                    {
                        writer.WriteLine("\tNextIsGrouped();");
                    }
                    writer.WriteLine("}");
                }
            }
        }
    }
}