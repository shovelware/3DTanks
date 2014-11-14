using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{


    class Bunker : Tile
    {
        private Primitive doorNoSo { get { return primitives[1]; } set { primitives[1] = value; } }
        private Primitive doorEaWe { get { return primitives[2]; } set { primitives[2] = value; } }

        
        public Bunker(Game g, BasicEffect b,  Vector3 position, float tileSize, Color[] faces, Color[] wires) : base (g, b, position, tileSize, faces, wires) { }

        protected override void InitialiseBuilding()
        {
            typeName = "Bunker";
            primitives = new Primitive[3];

            vSize = new Vector3(8, 4, 8) * sU;

            Vector3 doorSize = new Vector3(6, 3, 8) * sU;
            Vector3 doorDrop = new Vector3(0, 0.5f, 0) * sU;

            rootPrim = new PrismSquare(game, effect, vOrigin, null, vSize - rootCont, FaceTone(2, 0), WireTone(3, 0), shadeMode, drawMode);
            doorNoSo = new PrismSquare(game, effect, vOrigin - doorDrop, null, doorSize, FaceTone(4, 1), WireTone(5, 1), shadeMode, drawMode);
            doorEaWe = new PrismSquare(game, effect, vOrigin - doorDrop, rY90, doorSize, FaceTone(4, 1), WireTone(5, 1), shadeMode, drawMode);
        }

        public override void Update(Matrix proj, Matrix view, GameTime gameTime)
        {
            primitives[0].Update(proj, view, Matrix.Identity, gameTime);
            primitives[1].Update(proj, view, rootPrim.World, gameTime);
            primitives[2].Update(proj, view, rootPrim.World, gameTime);
        }

    }
}
