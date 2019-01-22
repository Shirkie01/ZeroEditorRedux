using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace SWBF2.Serialization
{
    /// <summary>
    /// Reads data from the SWBF/SWBF2 TER file format.
    /// </summary>
    internal class TerrainReader : BinaryReader
    {
        public TerrainReader(Stream stream) : base(stream)
        {
        }

        public Terrain ReadTerrain()
        {
            Terrain terrain = new Terrain();
            terrain.Header = ReadHeader();
            if (terrain.Header.DecalLength != 0)
            {
                ReadRoadDecals();
            }
            terrain.Blocks = ReadTerrainBlocks(terrain.Header.GridSize);
            ReadEndOfFile(terrain);
            return terrain;
        }

        /// <summary>
        /// Reads the header of the terrain file.
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <param name="terrain">The terrain</param>
        private TerrainHeader ReadHeader()
        {
            // Check file validity
            if (!"TERR".Equals(Encoding.UTF8.GetString(ReadBytes(4))))
            {
                throw new ArgumentException("Invalid terrain file. Terrain files must start with TERR");
            }

            var header = new TerrainHeader();

            // Read version
            header.Version = (TerrainVersion)ReadInt32();

            // Read terrain extents
            header.Extents = new TerrainExtents(ReadInt16(), ReadInt16(), ReadInt16(), ReadInt16());

            // Unknown ('164' for yavin1.TER. MaxZ?)
            header.Unknown1 = ReadInt32();

            //16 floats follow, describing the Tile Range of each Texture Layer
            for (int i = 0; i < header.TextureLayers.Length; i++)
            {
                //tile range is stored as reciprocal (1/X), e.g. 0.03125 for actually 32
                //to get the true tile size, we have ge the reciprocal again
                // (1 / 32) = 0.03125     (1 / 0.03125) = 32
                var layer = new TextureLayer();
                layer.TileRange = 1 / ReadSingle();
                header.TextureLayers[i] = layer;
            }

            //16 bytes follow, describing mapping type for each layer
            foreach (var layer in header.TextureLayers)
            {
                layer.MappingType = ReadByte();
            }

            // Assumed to be layer rotation values (float)
            foreach (var layer in header.TextureLayers)
            {
                layer.Rotation = ReadSingle();
            }

            header.MapHeightMultiplier = ReadSingle();
            header.GridScale = ReadSingle();

            //unknown int32 ('1' for yavin1.TER)
            header.Unknown2 = ReadInt32();

            //Full Map Size (e.g. 256 for a 256x256 Map)
            header.GridSize = ReadInt32();

            header.InGameOptions = (InGameOptions)ReadInt32();

            //SWBF2 Versions have a byte exactly on this point. purpose unknown ('15' for yavin1.TER)
            if (header.Version == TerrainVersion.SWBF2)
            {
                header.SWBF2Byte = ReadByte();
            }

            //16 Texture Layers follow (total length of 1024)
            //char[32]	Diffuse texture name.
            //char[32]  Detail texture name.
            foreach (var layer in header.TextureLayers)
            {
                layer.DiffuseTexture = ReadString();
                layer.DetailTexture = ReadString();
            }

            // Read water layers
            for (int i = 0; i < header.WaterLayers.Length; i++)
            {
                header.WaterLayers[i] = ReadWaterLayer();
            }

            for (int i = 0; i < header.DecalTextureNames.Length; i++)
            {
                header.DecalTextureNames[i] = ReadString();
            }

            header.DecalLength = ReadInt32();

            //unknown bytes
            header.UnknownBytes = ReadBytes(8);

            return header;
        }

        private void ReadRoadDecals()
        {
            throw new NotImplementedException("Road decals are not implemented.");
        }

        private WaterLayer ReadWaterLayer()
        {
            var water = new WaterLayer();

            // Height is listed twice. Possibly for two layers bordering another, or maybe one was
            // meant to be damage (based on disassembly).
            water.Height1 = ReadSingle();
            water.Height2 = ReadSingle();

            // two unknowns follow. Always zero?
            water.Unknown1 = ReadSingle();
            water.Unknown2 = ReadSingle();

            water.UVAnimation.Velocity = new PointF(ReadSingle(), ReadSingle());
            water.UVAnimation.Repeat = new PointF(ReadSingle(), ReadSingle());
            water.Color = ReadColor();
            water.TextureName = ReadString();

            return water;
        }

        /// <summary>
        /// Reads the color from the TER file. Stored as BGRA
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <returns>A color</returns>
        public Color ReadColor()
        {
            byte[] bytes = ReadBytes(4);
            return Color.FromArgb(bytes[3], bytes[2], bytes[1], bytes[0]);
        }

        /// <summary>
        /// Reads a string from the TER file.
        /// </summary>
        /// <returns>A string</returns>
        public override string ReadString()
        {
            return Encoding.UTF8.GetString(ReadBytes(32)).Trim('\0');
        }

        private TerrainBlock[,] ReadTerrainBlocks(int gridSize)
        {
            TerrainBlock[,] blocks = new TerrainBlock[1024, 1024];

            // Init
            TerrainUtil.ForEveryPoint(1024, (x, y) =>
            {
                blocks[x, y] = new TerrainBlock();
            });

            var gridSizeSquared = gridSize * gridSize;
            var expectedPosition = BaseStream.Position + gridSizeSquared * 2;

            // Heights
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                blocks[x, y].Height = ReadInt16();
            });

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidDataException("Stream in wrong position: " + BaseStream.Position);
            }

            // Color1 (uses gridsize)
            expectedPosition += gridSizeSquared * 4;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                blocks[x, y].ForegroundColor = ReadColor();
            });

            // Color2
            expectedPosition += gridSizeSquared * 4;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                blocks[x, y].BackgroundColor = ReadColor();
            });

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidDataException("Stream in wrong position: " + BaseStream.Position);
            }

            // Texture alphas
            expectedPosition += gridSizeSquared * 16;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                var block = blocks[x, y];
                for (int i = 0; i < block.TextureAlphas.Length; i++)
                {
                    block.TextureAlphas[i] = ReadByte();
                }
            });

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidDataException("Stream in wrong position: " + BaseStream.Position);
            }

            // Read the last-used heights for the (lower-left?) corner of the blend tool for each 4x4
            // block, (but only when it crosses into another 4x4 block). Why is this even in the TER file?
            expectedPosition += gridSizeSquared / 8;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                var blendHeight = ReadInt16();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        blocks[x + i, y + j].BlendHeight1 = blendHeight;
                    }
                }
            }, 4);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidDataException("Stream in wrong position: " + BaseStream.Position);
            }

            // Read the last-used heights for the (upper-right?) corner of the blend tool for each
            // 4x4 block, (but only when it crosses into another 4x4 block). Why is this even in the
            // TER file?
            expectedPosition += gridSizeSquared / 8;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                var blendHeight = ReadInt16();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        blocks[x + i, y + j].BlendHeight2 = blendHeight;
                    }
                }
            }, 4);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidDataException("Stream in wrong position: " + BaseStream.Position);
            }

            expectedPosition += gridSizeSquared / 4;
            // Water
            ReadWaterLayerIds(gridSize, blocks);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidDataException("Stream in wrong position: " + BaseStream.Position);
            }

            // THIS IS FIXED. DO NOT CHANGE. Foliage is stored as a masked enum with 1 byte for every
            // 4 blocks, and always 1024*1024.
            expectedPosition += 1024 * 1024 / 8;
            // Foliage
            ReadFoliage(1024, blocks);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidDataException("Stream in wrong position: " + BaseStream.Position);
            }

            return blocks;
        }

        /// <summary>
        /// Read the water layer ids for each block.
        /// </summary>
        /// <param name="gridSize">The size of the grid</param>
        /// <param name="blocks">The terrain blocks to write the water layer ids to.</param>
        private void ReadWaterLayerIds(int gridSize, TerrainBlock[,] blocks)
        {
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                // First two bytes are masked enums for the texture layer ids present on this tile.
                // 0-7 => 2^n, 8-15 = 2^(n-7)
                var texIds = MapTextureBytesToTextureLayerId(ReadByte(), ReadByte());

                // Water Layer Id (0-15)
                var waterLayerId = ReadByte();

                // Unknown byte - reserved for future additional water layers?
                ReadByte();

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        var block = blocks[x + i, y + j];
                        block.WaterLayerId = waterLayerId;
                        block.TextureLayerIds = texIds;
                    }
                }
            }, 4);
        }

        /// <summary>
        /// Read the foliage data for each block.
        /// </summary>
        /// <param name="gridSize">The size of the grid</param>
        /// <param name="blocks">The terrain blocks to write the foliage data to</param>
        private void ReadFoliage(int gridSize, TerrainBlock[,] blocks)
        {
            // Foliage is stored as a list of bytes. If there are less than 1024 bytes, the file is
            // filled with zeros to reach 1024. The bytes stored in the file expand outward to fill
            // this 1024 byte area. A 32x32 terrain will be: 00 00 00 00 11 11 11 11 00 00 00 00,
            // while a filled 64x64 terrain will have bytes: 00 00 11 11 11 11 11 11 11 11 00 00
            //
            // Update: It's a list of nibbles, not bytes. A terrain containing foliage for every
            // other vertex will look like 01 01 01 01 01

            for (int x = 0; x < gridSize; x += 2)
            {
                // Read the entire row of bytes
                byte[] bytes = ReadBytes(gridSize / 4);
                string hex = BitConverter.ToString(bytes).Replace("-", string.Empty);

                for (int y = 0; y < gridSize; y += 2)
                {
                    // Convert the nibble to hex
                    string v = hex.Substring(y / 2, 1);
                    int value = int.Parse(v, System.Globalization.NumberStyles.HexNumber);

                    if (value < 16)
                    {
                        blocks[x, y].FoliageTypes = (FoliageType)value;
                        blocks[x + 1, y].FoliageTypes = (FoliageType)value;
                        blocks[x, y + 1].FoliageTypes = (FoliageType)value;
                        blocks[x + 1, y + 1].FoliageTypes = (FoliageType)value;
                    }
                    else
                    {
                        throw new InvalidDataException(string.Format("Could not convert '{0}' to FoliageType. Valid values are 1,2,4,8, or any sum up to 15", v));
                    }
                }
            }
        }

        /// <summary>
        /// Maps the Texture bytes to the Texture layer Id.
        /// </summary>
        /// <param name="first">The first byte of the texture layer ids (0-7)</param>
        /// <param name="second">The second byte to the texture layer ids (8-15)</param>
        /// <returns>
        /// Array of 16 bools, specifying whether the texture id is applied to the current block.
        /// </returns>
        private bool[] MapTextureBytesToTextureLayerId(byte first, byte second)
        {
            bool[] textureLayerIds = new bool[16];

            for (int i = 7; i >= 0; i--)
            {
                var val = (byte)Math.Pow(2, i);
                if (first >= val)
                {
                    textureLayerIds[i] = true;
                    first -= val;
                }

                if (second >= val)
                {
                    textureLayerIds[i + 8] = true;
                    second -= val;
                }
            }

            return textureLayerIds;
        }

        public void ReadEndOfFile(Terrain terrain)
        {
            // Unknown. Appears to be related to gridsize
            terrain.UnknownBytes1 = ReadBytes(terrain.UnknownBytes1.Length);

            // Unknown. Appears to be related to gridsize
            terrain.UnknownBytes2 = ReadBytes(terrain.UnknownBytes2.Length);

            terrain.EndOfFileLength = ReadInt32();
            terrain.NumberOfItems = ReadInt32();

            for (int i = 0; i < terrain.NumberOfItems; i++)
            {
                int numSubItems = ReadInt32();
                float[] subItems = new float[numSubItems];

                int j = 0;
                while (j < numSubItems)
                {
                    subItems[j++] = ReadSingle();
                }

                terrain.EndOfFileItems.Add(subItems);
            }

            // Ending bytes
            //terrain.EndOfFileBytes = ReadBytes(terrain.EndOfFileLength);
        }
    }
}