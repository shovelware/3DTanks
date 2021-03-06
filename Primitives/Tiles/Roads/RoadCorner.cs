﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class RoadCorner : Tile
    {
        private Primitive roadNort { get { return primitives[1]; } set { primitives[1] = value; } }
        private Primitive roadWest { get { return primitives[2]; } set { primitives[2] = value; } }
        private Primitive lineNort { get { return primitives[3]; } set { primitives[3] = value; } }
        private Primitive lineWest { get { return primitives[4]; } set { primitives[4] = value; } }

        public RoadCorner(Game g, BasicEffect b, Vector3 position, float tileSize, Color[] faces, Color[] wires) : base(g, b, position, tileSize, faces, wires) { }

        protected override void InitialiseBuilding()
        {
            typeName = "Tile Corner";
            primitives = new Primitive[5];

            vSize = new Vector3(8, 0, 8) * sU;

            Vector3 roadRise = new Vector3(0, 0.4f, 0);
            Vector3 roadNoSi = new Vector3(4, 0, 6) * sU;
            Vector3 roadNoSh = new Vector3(0, 0, 1) * sU;

            Vector3 roadWeSi = new Vector3(4, 0, 4) * sU;
            Vector3 roadWeSh = new Vector3(2, 0, 0) * sU;

            Vector3 lineNoSi = new Vector3(0, 0, 4) * sU;
            Vector3 lineWeSi = new Vector3(4, 0, 0) * sU;

            rootPrim = new PrismSquare(game, effect, vOrigin, null, vSize, FaceTone(1, 0), WireTone(2, 0), shadeMode, drawMode);
            roadNort = new PrismSquare(game, effect, vOrigin + roadRise - roadNoSh, null, roadNoSi, FaceTone(2, 1), WireTone(3, 1), shadeMode, Primitive.Drawing.Face);
            roadWest = new PrismSquare(game, effect, vOrigin + roadRise - roadWeSh, null, roadWeSi, FaceTone(2, 1), WireTone(3, 1), shadeMode, Primitive.Drawing.Face);
            lineNort = new Line(game, effect, vOrigin + roadRise - roadNoSh * 2, null, lineNoSi, WireTone(3, 2));
            lineWest = new Line(game, effect, vOrigin + roadRise - roadWeSh, null, lineWeSi, WireTone(3, 2));
        }

        public override void Update(Matrix proj, Matrix view, GameTime gameTime)
        {
            primitives[0].Update(proj, view, Matrix.Identity, gameTime);
            primitives[1].Update(proj, view, rootPrim.World, gameTime);
            primitives[2].Update(proj, view, rootPrim.World, gameTime);
            primitives[3].Update(proj, view, rootPrim.World, gameTime);
            primitives[4].Update(proj, view, rootPrim.World, gameTime);
        }

    }
}
