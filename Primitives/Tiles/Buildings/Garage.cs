using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{


    class Garage : Tile
    {
        private Primitive doorNSWe { get { return primitives[1]; } set { primitives[1] = value; } }
        private Primitive doorNSEa { get { return primitives[2]; } set { primitives[2] = value; } }
        private Primitive doorEWNo { get { return primitives[3]; } set { primitives[3] = value; } }
        private Primitive doorEWSo { get { return primitives[4]; } set { primitives[4] = value; } }
        private Primitive windFull { get { return primitives[5]; } set { primitives[5] = value; } }

        public Garage(Game g, BasicEffect b,  Vector3 position, float tileSize, Color[] faces, Color[] wires) : base (g, b, position, tileSize, faces, wires) { }

        protected override void InitialiseBuilding()
        {
            typeName = "Garage";
            primitives = new Primitive[6];

            vSize = new Vector3(8, 6, 8) * sU;

            Vector3 doorSize = new Vector3(2, 3, 8) * sU;
            Vector3 doorDrop = new Vector3(0, 1.5f, 0) * sU;
            Vector3 doorNSTr = new Vector3(0, 0, 2) * sU;
            Vector3 doorEWTr = new Vector3(2, 0, 0) * sU;

            Vector3 windSize = new Vector3(8, 1, 8) * sU;
            Vector3 windRise = new Vector3(0, 1.5f, 0) * sU;

            rootPrim = new PrismSquare(game, effect, vOrigin, null, vSize - rootCont, FaceTone(2, 0), WireTone(3, 0), shadeMode, drawMode);
            doorNSWe = new PrismSquare(game, effect, vOrigin - doorDrop - doorEWTr, null, doorSize, FaceTone(3, 1), WireTone(5, 1), shadeMode, drawMode);
            doorNSEa = new PrismSquare(game, effect, vOrigin - doorDrop + doorEWTr, null, doorSize, FaceTone(3, 1), WireTone(5, 1), shadeMode, drawMode);
            doorEWNo = new PrismSquare(game, effect, vOrigin - doorDrop - doorNSTr, rY90, doorSize, FaceTone(3, 1), WireTone(5, 1), shadeMode, drawMode);
            doorEWSo = new PrismSquare(game, effect, vOrigin - doorDrop + doorNSTr, rY90, doorSize, FaceTone(3, 1), WireTone(5, 1), shadeMode, drawMode);
            windFull = new PrismSquare(game, effect, vOrigin + windRise, null, windSize, FaceTone(4, 2), WireTone(5, 2), shadeMode, drawMode);
        }

        public override void Update(Matrix proj, Matrix view, GameTime gameTime)
        {
            primitives[0].Update(proj, view, Matrix.Identity, gameTime);
            primitives[1].Update(proj, view, rootPrim.World, gameTime);
            primitives[2].Update(proj, view, rootPrim.World, gameTime);
            primitives[3].Update(proj, view, rootPrim.World, gameTime);
            primitives[4].Update(proj, view, rootPrim.World, gameTime);
            primitives[5].Update(proj, view, rootPrim.World, gameTime);

        }

    }
}
