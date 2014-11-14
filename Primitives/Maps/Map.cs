using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Primitives
{
    class MapData
    {
        /*Contains:
         * Map data for building generation INPUT
         * Collision data for AI stuff EXTRAPOLATE
         * Colour data for making buildings individual INPUT
         * Name and number for indexing 
         * More?
         */

        #region MVar

        string name;
        string[] tileData;
        Color[][] colorData;

        public string[] TileData { get { return tileData; } }
        public Color[][] ColorData { get { return colorData; } }

        #endregion

        #region Make, Load

        private MapData() {}
        
        public MapData(string mapName, string[] map, Color[][] color)
        {
            name = mapName;
            tileData = map;
            colorData = color;
        }

        #endregion
    }
    
    static class Map
    {
        #region MVars

        static List<MapData> maps = new List<MapData>();
        static int mapsI;
        public static MapData CurrentMap { get { return maps[mapsI]; } }
        static int mapsL;

        static List<Tile> tileList;

        #endregion

        #region Make/Break

        public static void Init()
        {
            tileList = new List<Tile>();

            //Blank Map
            maps.Add(new MapData(
                "Base",

                new string[]{
                "        ",
                "        ",
                "        ",
                "        ",
                "        ",
                "        ",
                "        ",
                "        "
            },

                new Color[9][]{
                    new Color[4]{Color.White, Color.White, Color.White, Color.White},
                    new Color[4]{Color.White, Color.White, Color.White, Color.White},
                    new Color[4]{Color.White, Color.White, Color.White, Color.White},
                    new Color[4]{Color.White, Color.White, Color.White, Color.White},
                    new Color[4]{Color.White, Color.White, Color.White, Color.White},
                    new Color[4]{Color.White, Color.White, Color.White, Color.White},
                    new Color[4]{Color.White, Color.White, Color.White, Color.White},
                    new Color[4]{Color.White, Color.White, Color.White, Color.White},
                    new Color[4]{Color.White, Color.White, Color.White, Color.White},
                }
                ));

            //Kencity
            maps.Add(new MapData(
                "KenCity",

                new string[]{
                    "╬═╦═╦═════╗",
                    "║1║3║11133║",
                    "║2║2╠═════╣",
                    "╠═╩═╣12321║",
                    "║231║23132║",
                    "╠═╦═╬═════╣",
                    "║3║5║11321║",
                    "║3╚═╬══╗26║",
                    "║343║66║44║",
                    "╚═══╩══╩══╝"
                },

                new Color[9][]{
                    new Color[4]{Color.Red, Color.Black, Color.White, Color.White},
                    new Color[4]{Color.Red, Color.Blue, Color.White, Color.White},
                    new Color[4]{Color.Red, Color.Yellow, Color.White, Color.White},
                    new Color[4]{Color.Red, Color.Pink, Color.White, Color.White},
                    new Color[4]{Color.Red, Color.Orange, Color.White, Color.White},
                    new Color[4]{Color.Red, Color.Brown, Color.White, Color.White},
                    new Color[4]{Color.Red, Color.White, Color.White, Color.White},
                    new Color[4]{Color.Red, Color.White, Color.White, Color.White},
                    new Color[4]{Color.Red, Color.White, Color.White, Color.White},
                }
                ));

            //Building Demo
            maps.Add(new MapData(
                "Buildings",

            new String[] {
                "╬═════════╗",
                "║123456789║",
                "╚═════════╝"
            },

            new Color[9][]{
                new Color[4]{Color.Green, Color.Black, Color.Yellow, Color.White},
                new Color[4]{Color.Gray, Color.Yellow, Color.White, Color.White},
                new Color[4]{Color.Red, Color.Orange, Color.Cyan, Color.White},
                new Color[4]{Color.Blue, Color.White, Color.Cyan, Color.White},
                new Color[4]{Color.Green, Color.White, Color.Gray, Color.White},
                new Color[4]{Color.Red, Color.Pink, Color.Purple, Color.Black},
                new Color[4]{Color.Blue, Color.Black, Color.Red, Color.Black},
                new Color[4]{Color.Gray, Color.White, Color.White, Color.White},
                new Color[4]{Color.Gray, Color.White, Color.White, Color.White},
                }
                ));

            mapsL = maps.Count - 1;
        }

        #endregion

        #region Maps

        static public void NextMap()
        {
            if (mapsI + 1 <= mapsL)
                mapsI++;
        }

        static public void PrevMap()
        {
            if (mapsI - 1 >= 0)
                mapsI--;
        }

        static public void GetMap(int index)
        {
            if (index <= mapsL && index >= 0)
                mapsI = index;
        }

        static public void LoadCurrentMap()
        {
            tileList.Clear();

            TileFactory.ChangeMap(CurrentMap);
            for (int z = 0; z < CurrentMap.TileData.Length; z++)
            {
                for (int x = 0; x < CurrentMap.TileData[z].Length; x++)
                {
                    {
                        tileList.Add(TileFactory.MakeTile(CurrentMap.TileData[z][x], new Vector3(x, 0, z)));
                    }
                }
            }
        }

        #endregion

        #region Update

        public static void Update(Matrix proj, Matrix view, GameTime gameTime)
        {
            foreach (Tile t in tileList)
            {
                t.Update(proj, view, gameTime);
            }
        }

        public static void Draw()
        {
            foreach (Tile t in tileList)
            {
                t.Draw();
            }
        }

        public static void DrawTrans()
        {
            foreach (Tile t in tileList)
            {
                t.DrawTransparent();
            }
        }

#endregion

    }
}
