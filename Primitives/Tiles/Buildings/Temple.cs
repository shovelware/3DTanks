using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{


    class Temple : Tile
    {
        private Primitive corrNoSo { get { return primitives[1]; } set { primitives[1] = value; } }
        private Primitive corrEaWe { get { return primitives[2]; } set { primitives[2] = value; } }
        private Primitive doorNoSo { get { return primitives[3]; } set { primitives[3] = value; } }
        private Primitive doorEaWe { get { return primitives[4]; } set { primitives[4] = value; } }
        private Primitive pillPrim { get { return primitives[5]; } set { primitives[5] = value; } }

        public Temple(Game g, BasicEffect b,  Vector3 position, float tileSize, Color[] faces, Color[] wires) : base (g, b, position, tileSize, faces, wires) { }

        protected override void InitialiseBuilding()
        {
            typeName = "Temple";
            primitives = new Primitive[6];

            vSize = new Vector3(8, 8, 8) * sU;

            Vector3 corrSize = new Vector3(8, 4, 4) * sU;
            Vector3 corrDrop = new Vector3(0, 2f, 0) * sU;
            Vector3 corrCont = new Vector3(0.5f, 0, 0.5f);
            
            Vector3 doorSize = new Vector3(8, 3, 2) * sU;
            Vector3 doorDrop = new Vector3(0, 2.5f, 0) * sU;

            Vector3 pillSize = new Vector3(1, 2, 1) * sU;
            Vector3 pillRise = new Vector3(0, 6f, 0) * sU;

            rootPrim = new PyramidSquare(game, effect, vOrigin, null, vSize - rootCont, FaceTone(2, 0), WireTone(4, 0), shadeMode, drawMode);
            corrNoSo = new PrismSquare(game, effect, vOrigin - corrDrop, null, corrSize - corrCont, FaceTone(4, 2), WireTone(5, 2), shadeMode, drawMode);
            corrEaWe = new PrismSquare(game, effect, vOrigin - corrDrop, rY90, corrSize - corrCont, FaceTone(4, 2), WireTone(5, 2), shadeMode, drawMode);
            doorNoSo = new PrismSquare(game, effect, vOrigin - doorDrop, null, doorSize, FaceTone(5, 1), WireTone(6, 1), shadeMode, drawMode);
            doorEaWe = new PrismSquare(game, effect, vOrigin - doorDrop, rY90, doorSize, FaceTone(5, 1), WireTone(6, 1), shadeMode, drawMode);
            pillPrim = new PrismSquare(game, effect, vOrigin + pillRise, null, pillSize, FaceTone(1, 3, 128), WireTone(7, 3), shadeMode, drawMode);
        }

        public override void Update(Matrix proj, Matrix view, GameTime gameTime)
        {
            primitives[0].Update(proj, view, Matrix.Identity, gameTime);
            primitives[1].Update(proj, view, rootPrim.World, gameTime);
            primitives[2].Update(proj, view, rootPrim.World, gameTime);
            primitives[3].Update(proj, view, rootPrim.World, gameTime);
            primitives[4].Update(proj, view, rootPrim.World, gameTime);
            pillPrim.RotateY(0.05f);
            primitives[5].Update(proj, view, rootPrim.World, gameTime);
        }

    }
}
