using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    static class TileFactory
    {
        public static String tileSymbols = "═║╔ ╗╚ ╝╠ ╣╦ ╩ ╬ 123456";//789 go here
        public static String straightSymbols = "║═";
        public static String cornerSymbols = "╝╗╔╚";
        public static String teeSymbols = "╣╦╠╩";
        public static String crossSymbols = "╬";
        public static String blankSymbols = " ";
        public static String buildingSymbols = "123456"; //and here

        static Game game;
        static float tileSize;
        static BasicEffect basicEffect;
        static MapData currentMap;

        static Quaternion[] quats = new Quaternion[4] { 
            Quaternion.Identity,
            Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(90)),
            Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(180)),
            Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(270)),
        };

        public static void Init(Game g, float ts, BasicEffect effect)
        {
            game = g;
            tileSize = ts;
            basicEffect = effect;
        }

        public static Tile MakeTile(char c, Vector3 position)
        {
            Tile t = new TileBlank(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[0], currentMap.ColorData[0]);

            if (straightSymbols.Contains(c))
            {
                int i = straightSymbols.IndexOf(c);
                t = new RoadStraight(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[0], currentMap.ColorData[0]);
                t.qRotation = quats[i];
            }

            else if (teeSymbols.Contains(c))
            {
                int i = teeSymbols.IndexOf(c);
                t = new RoadTee(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[0], currentMap.ColorData[0]);
                t.qRotation = quats[i];
            }

            else if (cornerSymbols.Contains(c))
            {
                int i = cornerSymbols.IndexOf(c);
                t = new RoadCorner(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[0], currentMap.ColorData[0]);
                t.qRotation = quats[i];
            }

            else if (crossSymbols.Contains(c))
            {
                t = new RoadCross(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[0], currentMap.ColorData[0]);
            }


            else if (buildingSymbols.Contains(c))
            {
                switch (c)
                {
                    case '1':
                        t = new Bunker(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[1], currentMap.ColorData[1]);
                        break;
                    case '2':
                        t = new Garage(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[2], currentMap.ColorData[2]);
                        break;
                    case '3':
                        t = new Apartment(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[3], currentMap.ColorData[3]);
                        break;
                    case '4':
                        t = new Silo(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[4], currentMap.ColorData[4]);
                        break;
                    case '5':
                        t = new Temple(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[5], currentMap.ColorData[5]);
                        break;
                    case '6':
                        t = new Antenna(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[6], currentMap.ColorData[6]);
                        break;
                    case '7':
                        t = new Silo(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[7], currentMap.ColorData[7]);
                        break;
                    case '8':
                        t = new Temple(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[8], currentMap.ColorData[8]);
                        break;
                    case '9':
                        t = new Antenna(game, basicEffect, position * tileSize, tileSize, currentMap.ColorData[9], currentMap.ColorData[9]);
                        break;
                }
            }

            return t;
        }

        public static void ChangeMap(MapData newMap)
        {
            currentMap = newMap;
        }
    }

    abstract class Tile
    {

        #region MVars

        protected Game game;
        protected BasicEffect effect;

        protected Primitive[] primitives;
        protected Color[] colFaces, colWires;

        protected string typeName; public string TypeName { get { return typeName; } }

        protected float baseSize;
        protected float sU;
        protected Vector3 vOrigin, vPos, vAdjust, vSize;

        public Vector3 Size { get { return vSize; } }
        public Primitive rootPrim { get { return primitives[0]; } set { primitives[0] = value; } }

        public Matrix World { get { return rootPrim.World; } set { rootPrim.World = value; } }
        public Vector3 vPosition { get { return rootPrim.vPosition; } set { rootPrim.vPosition = value; } }
        public Quaternion qRotation { get { return rootPrim.qRotation; } set { rootPrim.qRotation = value; } }

        #region Drawing

        protected Primitive.Drawing drawMode = Primitive.Drawing.WireFace;
        protected Primitive.Shading shadeMode = Primitive.Shading.Flat;
        protected Primitive.Lighting lightMode = Primitive.Lighting.Face;

        public Primitive.Drawing DrawMode { get { return drawMode; } }
        public Primitive.Shading ShadeMode { get { return shadeMode; } }
        public Primitive.Lighting LightMode { get { return lightMode; } }

        #endregion

        #region Tones

        protected static Color Tone0 = new Color(000, 000, 000);
        protected static Color Tone1 = new Color(032, 032, 032);
        protected static Color Tone2 = new Color(064, 064, 064);
        protected static Color Tone3 = new Color(096, 096, 096);
        protected static Color Tone4 = new Color(128, 128, 128);
        protected static Color Tone5 = new Color(160, 160, 160);
        protected static Color Tone6 = new Color(192, 192, 192);
        protected static Color Tone7 = new Color(224, 224, 224);
        protected static Color Tone8 = new Color(256, 256, 256);
        protected static Color[] tones = new Color[9] { Tone0, Tone1, Tone2, Tone3, Tone4, Tone5, Tone6, Tone7, Tone8 };

        #endregion

        #region Construction Helpers

        protected static Vector3 rootCont = new Vector3(0.4f, 0.4f, 0.4f);
        protected static Quaternion rY90 = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(90));
        protected static Quaternion rX90 = Quaternion.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(90));
        protected static Quaternion rZ90 = Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.ToRadians(90));

        #endregion

        #endregion //Mvars

        #region Make, Load

        public Tile(Game g, BasicEffect b, Vector3 position, float tileSize, Color[] faces, Color[] wires)
        {
            game = g;
            effect = b;
            vPos = vOrigin = Vector3.Zero;
            baseSize = tileSize;
            colFaces = faces;
            colWires = wires;

            sU = tileSize / 8;

            InitialiseBuilding();

            PositionAdd(position);

            vAdjust = new Vector3(0, rootPrim.SizeY / 2, 0);
        }

        protected virtual void InitialiseBuilding() { }

        #endregion

        #region Movement

        public void PositionAdd(Vector3 posXYZ)
        {
            vPos += posXYZ;
        }

        public void PositionReset()
        {
            rootPrim.PositionReset();
            vPos = rootPrim.vPosition;
        }

        #endregion

        #region Rotation

        #region Local Axes

        public void RotateX(float rotation)
        {
            rootPrim.RotateX(rotation);
        }

        public void RotateY(float rotation)
        {
            rootPrim.RotateY(rotation);
        }

        public void RotateZ(float rotation)
        {
            rootPrim.RotateZ(rotation);
        }

        public void RotReset()
        {
            rootPrim.RotationResetAll();
        }

        #endregion

        #region World Axes

        public void RotateWorldX(float rotation)
        {
            rootPrim.RotateWorldX(rotation);
        }

        public void RotateWorldY(float rotation)
        {
            rootPrim.RotateWorldY(rotation);
        }

        public void RotateWorldZ(float rotation)
        {
            rootPrim.RotateWorldZ(rotation);
        }

        #endregion

        #endregion

        #region Colors

        protected Color FaceTone(int tone, int index)
        {
            try
            {
                return Color.Lerp(colFaces[index], tones[tone], 0.5f);
            }

            catch (NullReferenceException) { return tones[tone]; }
            catch (IndexOutOfRangeException) { return tones[tone]; }
        }

        protected Color WireTone(int tone, int index)
        {
            try
            {
                return Color.Lerp(colWires[index], tones[tone], 0.5f);
            }

            catch (NullReferenceException) { return tones[tone]; }
            catch (IndexOutOfRangeException) { return tones[tone]; }
        }

        protected Color FaceTone(int tone, int index, byte alpha)
        {
            try
            {
                Color returncol = Color.Lerp(colFaces[index], tones[tone], 0.5f);
                returncol.A = alpha;
                return returncol;
            }

            catch (NullReferenceException) { return tones[tone]; }
            catch (IndexOutOfRangeException) { return tones[tone]; }
        }

        protected Color WireTone(int tone, int index, byte alpha)
        {
            try
            {
                Color returncol = Color.Lerp(colWires[index], tones[tone], 0.5f);
                returncol.A = alpha;
                return returncol;
            }

            catch (NullReferenceException) { return tones[tone]; }
            catch (IndexOutOfRangeException) { return tones[tone]; }
        }

        #endregion

        #region U&D

        public abstract void Update(Matrix proj, Matrix view, GameTime gametime);

        public virtual void Draw()
        {
            rootPrim.vPosition = vPos + vAdjust;

            foreach (Primitive p in primitives)
            {
                //try
                {
                    if (p.Opaque)
                        p.Draw();
                }

                //catch (NullReferenceException) { }
            }
        }

        public virtual void DrawTransparent()
        {
            rootPrim.vPosition = vPos + vAdjust;

            foreach (Primitive p in primitives)
            {
                //try
                {
                    if (!p.Opaque)
                        p.Draw();
                }

                //catch (NullReferenceException) { }
            }
        }

        #endregion

    }
}