using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace platformer
{
    public class LevelLoader
    {
        private ContentManager contentManager;
        private List<Collectible> collectibles;

        public LevelLoader(IServiceProvider serviceProvider)
        {
            contentManager = new ContentManager(serviceProvider, "Content");
        }

        public Level Load(int index)
        {
            List<string> lines = ReadLevel(index);
            Tile[,] tiles = CreateTiles(lines);

            return new Level(tiles, collectibles, contentManager);
        }

        private List<string> ReadLevel(int index)
        {
            string filepath = contentManager.RootDirectory +
                "/levels/level" + index + ".txt";

            List<string> lines = new List<string>();

            using (StreamReader streamReader = new StreamReader(filepath))
            {
                string line = streamReader.ReadLine();
                int lineLength = line.Length;

                while (line != null)
                {
                    if (line.Length != lineLength)
                        throw new Exception("Line lengths are inconsistent");

                    lines.Add(line);
                    line = streamReader.ReadLine();
                }
            }

            return lines;
        }

        private Tile[,] CreateTiles(List<string> lines)
        {
            int levelWidth = lines[0].Length;
            int levelHeight = lines.Count;
            Tile[,] tiles = new Tile[levelHeight, levelWidth];

            for (int y = 0; y < levelHeight; y++)
            {
                for (int x = 0; x < levelWidth; x++)
                {
                    char tileType = lines[y][x];
                    tiles[y, x] = CreateTile(tileType, x, y);
                }
            }

            return tiles;
        }

        private Tile CreateTile(char type, int x, int y)
        {
            switch(type)
            {
                case '.':
                    return new Tile(true);

                case '#':
                    return new Tile(false);

                case 'C':
                    CreateCollectible(x, y);
                    return new Tile(true);

                default:
                    throw new Exception("The tile type is unsupported");
            }
        }

        private void CreateCollectible(int x, int y)
        {
            Collectible collectible = new Collectible(); //TODO load txtre
            float collectibleX = x * Tile.Size + 0.5f * Tile.Size;
            float collectibleY = y * Tile.Size + 0.5f * Tile.Size;
            collectible.Position = new Vector2(collectibleX, collectibleY);
        }
    }
}
