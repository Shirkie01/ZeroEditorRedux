using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class LightFormatter : ITypedFormatter<IList<Light>>
    {
        public IList<Light> Deserialize(Stream serializationStream)
        {
            IList<Light> lights = new List<Light>();
            using (var reader = new StreamReader(serializationStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var startIndex = line.IndexOf("\"");
                    var lightName = line.Substring(startIndex, line.IndexOf("\"") - startIndex);

                    startIndex = line.IndexOf(", ");
                    var lightId = int.Parse(line.Substring(startIndex, line.LastIndexOf(")") - startIndex));

                    Light light = new Light(lightName, lightId);

                    // {
                    reader.ReadLine();

                    line = reader.ReadLine();
                    light.Rotation = Quaternion.Parse(line);

                    line = reader.ReadLine();
                    light.Position = Vector3.Parse(line);

                    line = reader.ReadLine();
                    startIndex = line.IndexOf("(");
                    light.Type = (LightType)int.Parse(line.Substring(startIndex, line.LastIndexOf(")") - startIndex));

                    line = reader.ReadLine();

                    //light.Color = ;

                    // }
                    reader.ReadLine();
                }
            }
            return lights;
        }

        public void Serialize(Stream serializationStream, IList<Light> lights)
        {
            using (var writer = new StreamWriter(serializationStream))
            {
                foreach (var light in lights)
                {
                    writer.WriteLine(string.Format("Light(\"{0}\", {1})", light.Name, light.Id));
                    writer.WriteLine("{");
                    writer.WriteLine(string.Format("\tRotation({0});", light.Rotation.ToString()));
                    writer.WriteLine(string.Format("\tPosition({0});", light.Position.ToString()));
                    writer.WriteLine(string.Format("\tType({0});", light.Type));
                    writer.WriteLine(string.Format("\tColor({0});", light.Color));

                    if ((int)light.Type == 1)
                    {
                        if (light.CastShadow)
                        {
                            writer.WriteLine("\tCastShadow();");
                        }

                        if (light.CastSpecular)
                        {
                            writer.WriteLine("\tCastSpecular(1);");
                        }

                        if (light.Static)
                        {
                            writer.WriteLine("\tStatic();");
                        }

                        if (light.Region != null)
                        {
                            writer.WriteLine(string.Format("\tRegion(\"{0}\")", light.Region.Name));
                        }

                        writer.WriteLine(string.Format("\tPS2BlendMode({0})", light.ps2BlendMode));

                        writer.WriteLine(string.Format("\tTileUV({0}, {1})", light.TileUV.X, light.TileUV.Y));
                        writer.WriteLine(string.Format("\tOffsetUV({0}, {1})", light.OffsetUV.X, light.OffsetUV.Y));
                    }
                    else if ((int)light.Type == 2)
                    {
                        writer.WriteLine(string.Format("\tRange({0})", light.Range));
                    }

                    writer.WriteLine("}");
                }
            }
        }
    }
}