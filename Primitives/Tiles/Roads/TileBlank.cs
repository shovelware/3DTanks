using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class TileBlank : Tile
    {
        public TileBlank(Game g, BasicEffect b,  Vector3 position, float tileSize, Color[] faces, Color[] wires) : base (g, b, position, tileSize, faces, wires) { }

        protected override void InitialiseBuilding()
        {
            typeName = "Tile Blank";
            primitives = new Primitive[1];

            vSize = new Vector3(8, 0, 8) * sU;

            rootPrim = new PrismSquare(game, effect, vOrigin, null, vSize, FaceTone(1, 0), WireTone(2, 0), shadeMode, drawMode);
        }

        public override void Update(Matrix proj, Matrix view, GameTime gameTime)
        {
            primitives[0].Update(proj, view, Matrix.Identity, gameTime);
        }

    }
}
