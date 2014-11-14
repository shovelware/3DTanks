using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class RoadStraight : Tile
    {
        private Primitive roadNoSo { get { return primitives[1]; } set { primitives[1] = value; } }
        private Primitive lineNoSo { get { return primitives[2]; } set { primitives[2] = value; } }

        public RoadStraight(Game g, BasicEffect b,  Vector3 position, float tileSize, Color[] faces, Color[] wires) : base (g, b, position, tileSize, faces, wires) { }

        protected override void InitialiseBuilding()
        {
            typeName = "Tile Straight";
            primitives = new Primitive[3];

            vSize = new Vector3(8, 0, 8) * sU;

            Vector3 roadRise = new Vector3(0, 0.4f, 0);
            Vector3 roadSize = new Vector3(4, 0, 8) * sU;
            Vector3 lineSize = new Vector3(0, 0, 8) * sU;


            rootPrim = new PrismSquare(game, effect, vOrigin, null, vSize, FaceTone(1, 0), WireTone(2, 0), shadeMode, drawMode);
            roadNoSo = new PrismSquare(game, effect, vOrigin + roadRise, null, roadSize, FaceTone(2, 1), WireTone(3, 1), shadeMode, Primitive.Drawing.Face);
            lineNoSo = new Line(game, effect, vOrigin + roadRise, null, lineSize, WireTone(3, 2));


        }

        public override void Update(Matrix proj, Matrix view, GameTime gameTime)
        {
            primitives[0].Update(proj, view, Matrix.Identity, gameTime);
            primitives[1].Update(proj, view, rootPrim.World, gameTime);
            primitives[2].Update(proj, view, rootPrim.World, gameTime);
        }

    }
}
