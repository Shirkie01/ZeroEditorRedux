using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace SWBF2.Serialization
{
    /// <summary>
    /// Writes data in the SWBF/SWBF2 TER file format.
    /// </summary>
    internal class TerrainWriter : BinaryWriter
    {
        public TerrainWriter(Stream stream) : base(stream)
        {
        }

        public void Write(Terrain terrain)
        {
            Write(terrain.Header);

            if (BaseStream.Position != 2821)
            {
                throw new InvalidOperationException("Header is invalid");
            }

            var gridSize = terrain.Header.GridSize;
            var gridSizeSquared = gridSize * gridSize;

            // Heights
            var expectedPosition = BaseStream.Position + gridSizeSquared * 2;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                Write(terrain.Blocks[x, y].Height);
            });

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidOperationException("Heights are invalid");
            }

            // Colors
            expectedPosition += gridSizeSquared * 4;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                Write(terrain.Blocks[x, y].ForegroundColor);
            });

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidOperationException("Colors are invalid");
            }

            // More Colors
            expectedPosition += gridSizeSquared * 4;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                Write(terrain.Blocks[x, y].BackgroundColor);
            });
            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidOperationException("Colors are invalid");
            }

            // Texture alphas
            expectedPosition += gridSizeSquared * 16;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                var block = terrain.Blocks[x, y];
                for (int i = 0; i < block.TextureAlphas.Length; i++)
                {
                    Write(block.TextureAlphas[i]);
                }
            });

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidOperationException("TextureAlphas are invalid");
            }

            // Write the first blend height of every 4x4 block
            expectedPosition += gridSizeSquared / 8;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                Write(terrain.Blocks[x, y].BlendHeight1);
            }, 4);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidOperationException("BlendHeight1 values are invalid");
            }

            // Write the second blend height of every 4x4 block
            expectedPosition += gridSizeSquared / 8;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                Write(terrain.Blocks[x, y].BlendHeight2);
            }, 4);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidOperationException("BlendHeight2 values are invalid");
            }

            // Write the water id info. Fixed size
            expectedPosition += gridSizeSquared / 4;
            TerrainUtil.ForEveryPoint(gridSize, (x, y) =>
            {
                var block = terrain.Blocks[x, y];

                byte[] bytes = MapTextureLayerIdsToTextureBytes(block.TextureLayerIds);

                Write(bytes[0]);
                Write(bytes[1]);
                Write(block.WaterLayerId);
                Write((byte)0);
            }, 4);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidOperationException("WaterIds are invalid");
            }

            // Foliage is a fixed size
            expectedPosition += 1024 * 1024 / 8;
            WriteFoliage(1024, terrain.Blocks);

            if (BaseStream.Position != expectedPosition)
            {
                throw new InvalidOperationException("Foliage is invalid");
            }

            WriteEndOfFile(terrain);
        }

        /// <summary>
        /// Writes the TER header using the provided writer
        /// </summary>
        /// <param name="writer">The writer</param>
        /// <param name="header">The terrain header</param>
        private void Write(TerrainHeader header)
        {
            Write(Encoding.UTF8.GetBytes("TERR"));
            Write((int)header.Version);

            // Write Extents
            Write(header.Extents.MinX);
            Write(header.Extents.MinY);
            Write(header.Extents.MaxX);
            Write(header.Extents.MaxY);

            Write(header.Unknown1);

            foreach (var layer in header.TextureLayers)
            {
                Write(1 / layer.TileRange);
            }

            foreach (var layer in header.TextureLayers)
            {
                Write(layer.MappingType);
            }

            foreach (var layer in header.TextureLayers)
            {
                Write(layer.Rotation);
            }

            Write(header.MapHeightMultiplier);

            Write(header.GridScale);

            // There's a 1 here. No idea why. Can't remove it though!
            Write(header.Unknown2);

            Write(header.GridSize);

            Write((int)header.InGameOptions);

            // There's a value here, but only on SWBF2 terrains. No idea what it means
            if (header.Version == TerrainVersion.SWBF2)
            {
                Write(header.SWBF2Byte);
            }

            // Write the texture-layer texture names
            foreach (var layer in header.TextureLayers)
            {
                Write(layer.DiffuseTexture);
                Write(layer.DetailTexture);
            }

            // Write the water layers
            foreach (var waterLayer in header.WaterLayers)
            {
                Write(waterLayer);
            }

            // Write the road decal texture names
            foreach (var decalTexName in header.DecalTextureNames)
            {
                Write(decalTexName);
            }

            Write(header.DecalLength * 176);

            // unknown
            Write(header.UnknownBytes);
        }

        /// <summary>
        /// Writes a water layer
        /// </summary>
        /// <param name="layer">The water layer to write</param>
        public void Write(WaterLayer layer)
        {
            // Height is stored twice. May have been intended for future use. One of these may have
            // been meant to be damage.
            Write(layer.Height1);
            Write(layer.Height2);

            Write(layer.Unknown1);
            Write(layer.Unknown2);

            Write(layer.UVAnimation.Velocity.X);
            Write(layer.UVAnimation.Velocity.Y);
            Write(layer.UVAnimation.Repeat.X);
            Write(layer.UVAnimation.Repeat.Y);
            Write(layer.Color);
            Write(layer.TextureName);
        }

        /// <summary>
        /// Writes the color to the TER file. Stored as BGRA
        /// </summary>
        /// <param name="color">The color</param>
        public void Write(Color color)
        {
            Write(new byte[] { color.B, color.G, color.R, color.A });
        }

        /// <summary>
        /// Write a string to the TER file. Stored as 32 bytes, zero terminated.
        /// </summary>
        /// <param name="s">The string</param>
        public override void Write(string s)
        {
            Write(Encoding.UTF8.GetBytes(s.PadRight(32, '\0')));
        }

        /// <summary>
        /// Maps the list of texture layer ids to the required byte fields
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>
        /// A two-byte array, with the ids split into masked enum values 0-7 = 2^n, 8-15 = 2^(n-7)
        /// </returns>
        private byte[] MapTextureLayerIdsToTextureBytes(bool[] ids)
        {
            byte[] bytes = new byte[2];

            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];

                if (id)
                {
                    if (i > 7)
                    {
                        bytes[1] += (byte)(Math.Pow(2, i - 8));
                    }
                    else
                    {
                        bytes[0] += (byte)(Math.Pow(2, i));
                    }
                }
            }

            return bytes;
        }

        /// <summary>
        /// Writes the foliage byte for the terrain file
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="terrain"></param>
        private void WriteFoliage(int gridSize, TerrainBlock[,] blocks)
        {
            // The TER file stores foliage as a nibble for every 4 vertices. As such we don't need to
            // write the value of every vertex, just every other one in both directions
            StringBuilder valuesToWrite = new StringBuilder();
            for (int x = 0; x < gridSize; x += 2)
            {
                valuesToWrite.Clear();

                for (int y = 0; y < gridSize; y += 2)
                {
                    // get the int value of the enum (0-15) and convert it to hex
                    var mask = (int)blocks[x, y].FoliageTypes;
                    valuesToWrite.Append(mask.ToString("X"));
                }

                if (valuesToWrite.Length % 2 != 0)
                {
                    throw new InvalidOperationException("The foliage values must be a multiple of 2");
                }

                var hex = valuesToWrite.ToString();

                //if (hex.Length != size / 2) // ( size / 4 (size of foliage info) * 2 (size of byte)
                //    throw new ArgumentException("Error setting up for writing foliage");

                var array = Enumerable.Range(0, hex.Length)
                 .Where(i => i % 2 == 0)
                 .Select(i => Convert.ToByte(hex.Substring(i, 2), 16))
                 .ToArray();

                Write(array);
            }
        }

        private void WriteEndOfFile(Terrain terrain)
        {
            // Both unknowns below may be related to Decal Edit mode (F11) in the editor. (3 floats
            // every fourth block:scale, x, y, and rotation?)

            // More detail: These blocks are almost definitely related to objects which cut the terrain.

            // Unknown. Appears to be related to gridsize
            Write(terrain.UnknownBytes1);

            // Unknown. Appears to be related to gridsize
            Write(terrain.UnknownBytes2);

            Write(terrain.EndOfFileLength);

            // Ending bytes
            //Write(terrain.EndOfFileBytes);
        }
    }
}