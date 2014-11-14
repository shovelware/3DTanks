using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Primitives
{
    class TileFactory
    {
        public static String streetSymbols = "═║╔ ╗╚ ╝╠ ╣╦ ╩ ╬ 123456";//789 go here
        public static String straightSymbols = "║═";
        public static String cornerSymbols = "╝╗╔╚";
        public static String teeSymbols = "╣╦╠╩";
        public static String crossSymbols = "╬";
        public static String blankSymbols = " ";
        public static String buildingSymbols = "123456"; //and here

        static Game game;
        static int tileSize;
        static BasicEffect basicEffect;
        static MapData currentMap;

        static Quaternion[] quats = new Quaternion[4] { 
            Quaternion.Identity,
            Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(90)),
            Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(180)),
            Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(270)),
        };

        public static Tile makeStreet(char c, Vector3 position, Color[] primaries, Color[] secondaries)
        {
            Tile t = new TileBlank(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);

            if (straightSymbols.Contains(c))
            {
                int i = straightSymbols.IndexOf(c);
                t = new RoadStraight(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                t.qRotation = quats[i];
            }

            else if (teeSymbols.Contains(c))
            {
                int i = teeSymbols.IndexOf(c);
                t = new RoadTee(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                t.qRotation = quats[i];
            }

            else if (cornerSymbols.Contains(c))
            {
                int i = cornerSymbols.IndexOf(c);
                t = new RoadCorner(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                t.qRotation = quats[i];
            }

            else if (crossSymbols.Contains(c))
            {
                t = new RoadCross(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
            }


            else if (buildingSymbols.Contains(c))
            {
                switch (c)
                {
                    case '1':
                        t = new Bunker(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                        break;
                    case '2':
                        t = new Garage(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                        break;
                    case '3':
                        t = new Apartment(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                        break;
                    case '4':
                        t = new Silo(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                        break;
                    case '5':
                        t = new Temple(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                        break;
                    case '6':
                        t = new Antenna(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                        break;
                    case '7':
                        t = new Silo(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                        break;
                    case '8':
                        t = new Temple(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                        break;
                    case '9':
                        t = new Antenna(game, basicEffect, position * tileSize, tileSize, primaries, secondaries);
                        break;
                }
            }

            return t;
        }

        public static void Init(Game g, int ts, BasicEffect effect)
        {
            game = g;
            tileSize = ts;
            basicEffect = effect;
        }

        public static List<Tile> MakeMap(MapData newMap)
        {
            currentMap = newMap;
            List<Tile> tiles = new List<Tile>();

            for (int z = 0; z < currentMap.TileData.Length; z++)
            {
                for (int x = 0; x < currentMap.TileData[z].Length; x++)
                {
                    Color[] cols = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow };

                    char c = currentMap.TileData[z][x];
                    if (TileFactory.streetSymbols.Contains(c))
                    {
                        Tile t = TileFactory.makeStreet(currentMap.TileData[z][x], new Vector3(x, 0, z), cols, cols);
                        tiles.Add(t);
                    }
                }
            }

            return tiles;
        }
    }
}