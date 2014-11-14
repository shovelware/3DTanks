using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{


    class Antenna : Tile
    {
        private Primitive wallNort { get { return primitives[1]; } set { primitives[1] = value; } }
        private Primitive wallEast { get { return primitives[2]; } set { primitives[2] = value; } }
        private Primitive wallSout { get { return primitives[3]; } set { primitives[3] = value; } }
        private Primitive wallWest { get { return primitives[4]; } set { primitives[4] = value; } }
        private Primitive basePrim { get { return primitives[5]; } set { primitives[5] = value; } }
        private Primitive casePrim { get { return primitives[6]; } set { primitives[6] = value; } }

        public Antenna(Game g, BasicEffect b,  Vector3 position, float tileSize, Color[] faces, Color[] wires) : base (g, b, position, tileSize, faces, wires) { }

        protected override void InitialiseBuilding()
        {
            typeName = "Antenna";
            primitives = new Primitive[7];

            vSize = new Vector3(8, 10, 8) * sU;

            Vector3 baseSize = new Vector3(8, 0, 8) * sU;
            Vector3 baseShuf = new Vector3(0, 5, 0) * sU;

            Vector3 wallNSSz = new Vector3(8, 4, 0) * sU;
            Vector3 wallEWSz = new Vector3(0, 4, 8) * sU;
            Vector3 wallNSSh = new Vector3(0, 0, 4) * sU;
            Vector3 wallEWSh = new Vector3(4, 0, 0) * sU;
            
            Vector3 wallRise = new Vector3(0, 2, 0) * sU;

            Vector3 poleSize = new Vector3(0.5f, 10, 0.5f) * sU;

            Vector3 caseShuf = new Vector3(0, 2, 0) * sU;
            Vector3 caseSize = new Vector3(1, 6, 1) * sU;

            rootPrim = new PrismSquare(game, effect, vOrigin, null, poleSize - rootCont, FaceTone(3, 2), WireTone(7, 2), shadeMode, drawMode);
            wallNort = new PrismSquare(game, effect, vOrigin - baseShuf - wallNSSh + wallRise, null, wallNSSz, FaceTone(2, 0, 196), WireTone(6, 0), shadeMode, drawMode);
            wallEast = new PrismSquare(game, effect, vOrigin - baseShuf + wallEWSh + wallRise, null, wallEWSz, FaceTone(2, 0, 196), WireTone(6, 0), shadeMode, drawMode);
            wallSout = new PrismSquare(game, effect, vOrigin - baseShuf + wallNSSh + wallRise, null, wallNSSz, FaceTone(2, 0, 196), WireTone(6, 0), shadeMode, drawMode);
            wallWest = new PrismSquare(game, effect, vOrigin - baseShuf - wallEWSh + wallRise, null, wallEWSz, FaceTone(2, 0, 196), WireTone(6, 0), shadeMode, drawMode);

            basePrim = new PrismSquare(game, effect, vOrigin - baseShuf, null, baseSize, FaceTone(1, 0), WireTone(2, 0), shadeMode, drawMode);
            casePrim = new PrismSquare(game, effect, vOrigin - caseShuf, null, caseSize, FaceTone(1, 1), WireTone(2, 1), shadeMode, drawMode);
        }

        public override void Update(Matrix proj, Matrix view, GameTime gameTime)
        {
            LerpInside();
            primitives[0].Update(proj, view, Matrix.Identity, gameTime);
            primitives[1].Update(proj, view, rootPrim.World, gameTime);
            primitives[2].Update(proj, view, rootPrim.World, gameTime);
            primitives[3].Update(proj, view, rootPrim.World, gameTime);
            primitives[4].Update(proj, view, rootPrim.World, gameTime);
            primitives[5].Update(proj, view, rootPrim.World, gameTime);
            primitives[6].Update(proj, view, rootPrim.World, gameTime);
        }

        private void LerpInside()
        {

            if (!primitives[0].ColFaceLerping)
            {
                if (primitives[0].ColFace == colFaces[2])
                {
                    primitives[0].ColFaceTar = colFaces[3];
                }

                else primitives[0].ColFaceTar = colFaces[2];
            }

            if (!primitives[0].ColWireLerping)
            {
                if (primitives[0].ColWire == colWires[2])
                {
                    primitives[0].ColWireTar = colWires[3];
                }

                else primitives[0].ColWireTar = colWires[2];
            }
        }

    }
}
