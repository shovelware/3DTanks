using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{


    class Silo : Tile
    {
        private Primitive octaPrim { get { return primitives[1]; } set { primitives[1] = value; } }
        private Primitive roofPrim { get { return primitives[2]; } set { primitives[2] = value; } }
        private Primitive doorNoSo { get { return primitives[3]; } set { primitives[3] = value; } }
        private Primitive doorEaWe { get { return primitives[4]; } set { primitives[4] = value; } }


        public Silo(Game g, BasicEffect b,  Vector3 position, float tileSize, Color[] faces, Color[] wires) : base (g, b, position, tileSize, faces, wires) { }

        protected override void InitialiseBuilding()
        {
            typeName = "Silo";
            primitives = new Primitive[5];

            vSize = new Vector3(8, 8, 8) * sU;
            Vector3 sizeRedu = new Vector3(0, 4, 0) * sU;

            Vector3 octaRise = new Vector3(0, 4, 0) * sU;
            
            Vector3 roofSize = new Vector3(8, 2, 8) * sU;
            Vector3 roofRise = new Vector3(0, 7f, 0) * sU;

            Vector3 doorSize = new Vector3(8, 3, 2) * sU;
            Vector3 doorDrop = new Vector3(0, 0.5f, 0) * sU;

            rootPrim = new PrismSquare(game, effect, vOrigin, null, vSize - sizeRedu - rootCont, FaceTone(2, 0), WireTone(3, 0), shadeMode, drawMode);
            octaPrim = new TankTurret(game, effect, vOrigin + octaRise, null, vSize - sizeRedu , FaceTone(2, 0), WireTone(3, 0), shadeMode, drawMode);
            roofPrim = new PyramidOctagon(game, effect, vOrigin + roofRise, null, roofSize, FaceTone(3, 2), WireTone(4, 2), shadeMode, drawMode);
            doorNoSo = new PrismSquare(game, effect, vOrigin - doorDrop, rY90, doorSize, FaceTone(3, 1), WireTone(4, 1), shadeMode, drawMode);            
            doorEaWe = new PrismSquare(game, effect, vOrigin - doorDrop, null, doorSize, FaceTone(3, 1), WireTone(4, 1), shadeMode, drawMode);

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
