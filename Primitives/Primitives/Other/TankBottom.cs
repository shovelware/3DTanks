using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class TankBottom : Primitive
    {
        public TankBottom(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        { 
            typeName = "TankBottom"; 
            noVertices = 8;
            noFaces = 6;
            noTris = 12;
            noWires = 12;
            sideDivs = 2;
            sideMults = 1;
            correctionMatrix = Matrix.CreateTranslation(new Vector3(-sizeX / 2, -sizeY / 2, -sizeZ / 2));
        }

        protected override void InitialiseVertices()
        {
            float tanOfSixtyDeg = (float)(sizesY[1, 1] / Math.Tan(MathHelper.ToRadians(60)));
            float tanOfSixtyMin = sizesZ[1, 1] - tanOfSixtyDeg;
            //Port Face
            vertices[00].Position = new Vector3(+sizesX[0, 0], +sizesY[1, 1], tanOfSixtyDeg);
            vertices[01].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[02].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], tanOfSixtyMin);
            vertices[03].Position = new Vector3(+sizesX[0, 0], +sizesY[1, 1], +sizesZ[1, 1]);
            //Starboard Face                                                  
            vertices[04].Position = new Vector3(+sizesX[1, 1], +sizesY[1, 1], +sizesZ[1, 1]);
            vertices[05].Position = new Vector3(+sizesX[1, 1], +sizesY[1, 1], tanOfSixtyDeg);
            vertices[06].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[07].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], tanOfSixtyMin);
        }

        protected override void InitialiseIndices()
        {
            //Port
            AssignTri(00, 02, 01);
            AssignTri(00, 03, 02);
            //Deck
            AssignTri(00, 04, 03);
            AssignTri(00, 05, 04);
            //Starboard
            AssignTri(04, 06, 07);
            AssignTri(04, 05, 06);
            //Bow
            AssignTri(00, 06, 05);
            AssignTri(00, 01, 06);
            //Stern
            AssignTri(04, 02, 03);
            AssignTri(04, 07, 02);
            //Keel
            AssignTri(02, 06, 01);
            AssignTri(02, 07, 06);
        }

        protected override void InitialiseEdges()
        {
            short[] port = new short[4]{00, 01, 02, 03};
            short[] star = new short[4]{05, 06, 07, 04};

            AssignLinedFace(port);
            AssignLinedFace(star);
            AssignLinedFacesJoints(port, star);
        }
    }
}
