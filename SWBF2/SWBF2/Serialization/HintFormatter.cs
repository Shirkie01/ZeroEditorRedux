using System.Collections.Generic;
using System.IO;

namespace SWBF2.Serialization
{
    public class HintFormatter : ITypedFormatter<IList<Hint>>
    {
        public IList<Hint> Deserialize(Stream serializationStream)
        {
            var hints = new List<Hint>();
            using (var reader = new StreamReader(serializationStream))
            {
                // Blank
                var line = reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    // Constructor
                    line = GetSubstringValue(line);
                    line = line.Replace("\"", "").Replace(", ", ",");
                    var cctorValues = line.Split(',');

                    var hint = new Hint(cctorValues[0], (HintType)int.Parse(cctorValues[1]));

                    // Open Brace
                    line = reader.ReadLine();

                    // Position
                    line = reader.ReadLine();
                    hint.Position = Vector3.Parse(line);

                    // Rotation
                    line = reader.ReadLine();
                    hint.Rotation = Quaternion.Parse(line);

                    // Radius if available
                    line = reader.ReadLine();
                    if (line.Contains("Radius"))
                    {
                        hint.Radius = double.Parse(GetSubstringValue(line));
                        line = reader.ReadLine();
                    }

                    // PrimaryStance if available
                    if (line.Contains("PrimaryStance"))
                    {
                        hint.PrimaryStance = (PrimaryStance)(int)(double.Parse(GetSubstringValue(line)));
                        line = reader.ReadLine();
                    }

                    // SecondaryStance if available
                    if (line.Contains("SecondaryStance"))
                    {
                        hint.SecondaryStance = (SecondaryStance)int.Parse(GetSubstringValue(line));
                        line = reader.ReadLine();
                    }

                    hint.HintMode = (HintMode)int.Parse(GetSubstringValue(line));

                    // CommandPost if available, closing brace otherwise
                    line = reader.ReadLine();
                    if (line.Contains("CommandPost"))
                    {
                        hint.CommandPost = GetSubstringValue(line).Replace("\"", "");
                        reader.ReadLine();
                    }

                    hints.Add(hint);

                    // Space
                    reader.ReadLine();
                }

                return hints;
            }

            throw new InvalidDataException("Could not correctly parse hints.");
        }

        private string GetSubstringValue(string s)
        {
            var startIndex = s.IndexOf("(") + 1;
            return s.Substring(startIndex, s.IndexOf(")") - startIndex);
        }

        public void Serialize(Stream serializationStream, IList<Hint> obj)
        {
            using (var writer = new StreamWriter(serializationStream))
            {
                foreach (var hint in obj)
                {
                    writer.WriteLine();
                    writer.WriteLine(string.Format("Hint(\"{0}\", \"{1}\")", hint.Name, (int)hint.Type));
                    writer.WriteLine("{");
                    writer.WriteLine(string.Format("\tPosition({0});", hint.Position));
                    writer.WriteLine(string.Format("\tRotation({0});", hint.Rotation));

                    if (hint.Radius != 0)
                    {
                        writer.WriteLine(string.Format("\tRadius({0:F6});", hint.Radius));
                    }

                    if (hint.PrimaryStance != PrimaryStance.None)
                    {
                        writer.WriteLine(string.Format("\tPrimaryStance({0});", (int)hint.PrimaryStance));
                    }

                    if (hint.SecondaryStance != SecondaryStance.None)
                    {
                        writer.WriteLine(string.Format("\tSecondaryStance({0});", (int)hint.SecondaryStance));
                    }

                    writer.WriteLine(string.Format("\tMode({0});", (int)hint.HintMode));

                    if (!string.IsNullOrWhiteSpace(hint.CommandPost))
                    {
                        writer.WriteLine(string.Format("\tCommandPost(\"{0}\");", hint.CommandPost));
                    }

                    writer.WriteLine("}");
                }
            }
        }
    }
}