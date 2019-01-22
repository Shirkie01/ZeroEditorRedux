using log4net;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SWBF2
{
    public class World
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(World));

        public IList<GameMode> GameModes = new List<GameMode>();
        public IList<Layer> Layers = new List<Layer>();
        public Terrain Terrain { get; set; }
        public IList<Barrier> Barriers { get; } = new List<Barrier>();

        public static World LoadFromFile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            string text = "";
            using (StreamReader streamReader = new StreamReader(path, Encoding.ASCII, false))
            {
                text = streamReader.ReadToEnd();
            }

            World world = new World();
            string ldxFilePath = System.IO.Path.Combine(fileInfo.DirectoryName, System.IO.Path.GetFileNameWithoutExtension(fileInfo.Name) + ".ldx");
            LoadLDXFile(ldxFilePath, ref world);

            return world;
        }

        private static void LoadLDXFile(string path, ref World world)
        {
            string ldxFile = string.Empty;
            using (StreamReader sr = new StreamReader(path))
            {
                ldxFile = sr.ReadToEnd();
            }

            string layersMatch = "Layer\\(\"([A-z0-9_-]+)\",\\s*([0-9]+)";
            MatchCollection matchCollection = Regex.Matches(ldxFile, layersMatch, RegexOptions.Singleline);
            foreach (Match match in matchCollection)
            {
                Layer layer = new Layer("DefaultLayer", 0);
                layer.Name = match.Groups[1].Value;
                layer.SaveType = int.Parse(match.Groups[2].Value);
                world.Layers.Add(layer);
            }

            string gameModeMatch = "GameMode\\(\"([A-z0-9_-]+)\"\\)\\s*{\\s*(Layer\\(([0-9]+)\\);\\s*)+\\s*}";

            matchCollection = Regex.Matches(ldxFile, gameModeMatch, RegexOptions.Singleline);
            foreach (Match match in matchCollection)
            {
                GameMode gameMode = new GameMode(match.Groups[1].Value);

                foreach (Capture capture in match.Groups[3].Captures)
                {
                    gameMode.Layers.Add(world.Layers[int.Parse(capture.Value)]);
                }

                world.GameModes.Add(gameMode);
            }
        }
    }
}