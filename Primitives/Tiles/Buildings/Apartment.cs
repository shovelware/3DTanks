using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{


    class Apartment : Tile
    {
        private Primitive doorNoSo { get { return primitives[1]; } set { primitives[1] = value; } }
        private Primitive doorEaWe { get { return primitives[2]; } set { primitives[2] = value; } }
        private Primitive windLowe { get { return primitives[3]; } set { primitives[3] = value; } }
        private Primitive windMidl { get { return primitives[4]; } set { primitives[4] = value; } }
        private Primitive windHigh { get { return primitives[5]; } set { primitives[5] = value; } }
        private Primitive windTops { get { return primitives[6]; } set { primitives[6] = value; } }

        public Apartment(Game g, BasicEffect b,  Vector3 position, float tileSize, Color[] faces, Color[] wires) : base (g, b, position, tileSize, faces, wires) { }

        protected override void InitialiseBuilding()
        {
            typeName = "Apartment";
            primitives = new Primitive[7];

            vSize = new Vector3(8, 12, 8) * sU;

            Vector3 doorSize = new Vector3(2, 2, 8) * sU;
            Vector3 doorDrop = new Vector3(0, 5, 0) * sU;

            Vector3 windSize = new Vector3(8, 1, 8) * sU;
            Vector3 windPush = new Vector3(0, 2f, 0) * sU;

            rootPrim = new PrismSquare(game, effect, vOrigin, null, vSize - rootCont, FaceTone(2, 0), WireTone(3, 0), shadeMode, drawMode);
            doorNoSo = new PrismSquare(game, effect, vOrigin - doorDrop, null, doorSize, FaceTone(5, 1), WireTone(6, 1), shadeMode, drawMode);
            doorEaWe = new PrismSquare(game, effect, vOrigin - doorDrop, rY90, doorSize, FaceTone(5, 1), WireTone(6, 1), shadeMode, drawMode);
            windLowe = new PrismSquare(game, effect, vOrigin - windPush, null, windSize, FaceTone(6, 2), WireTone(7, 2), shadeMode, drawMode);
            windMidl = new PrismSquare(game, effect, vOrigin, null, windSize, FaceTone(6, 2), WireTone(7, 2), shadeMode, drawMode);
            windHigh = new PrismSquare(game, effect, vOrigin + windPush, null, windSize, FaceTone(6, 2), WireTone(7, 2), shadeMode, drawMode);
            windTops = new PrismSquare(game, effect, vOrigin + windPush * 2, null, windSize, FaceTone(6, 2), WireTone(7, 2), shadeMode, drawMode);
        }

        public override void Update(Matrix proj, Matrix view, GameTime gameTime)
        {
            primitives[0].Update(proj, view, Matrix.Identity, gameTime);
            primitives[1].Update(proj, view, rootPrim.World, gameTime);
            primitives[2].Update(proj, view, rootPrim.World, gameTime);
            primitives[3].Update(proj, view, rootPrim.World, gameTime);
            primitives[4].Update(proj, view, rootPrim.World, gameTime);
            primitives[5].Update(proj, view, rootPrim.World, gameTime);
            primitives[6].Update(proj, view, rootPrim.World, gameTime);
        }

    }
}
