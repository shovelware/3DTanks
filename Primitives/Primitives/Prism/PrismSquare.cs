using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class PrismSquare : Primitive
    {
        public PrismSquare(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        { 
            typeName = "PrismSquare"; 
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
            //Left
            vertices[00].Position = new Vector3(+sizesX[0, 0], +sizesY[1, 1], +sizesZ[0, 0]);
            vertices[01].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[02].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[1, 1]);
            vertices[03].Position = new Vector3(+sizesX[0, 0], +sizesY[1, 1], +sizesZ[1, 1]);
            //Right                                                  
            vertices[04].Position = new Vector3(+sizesX[1, 1], +sizesY[1, 1], +sizesZ[1, 1]);
            vertices[05].Position = new Vector3(+sizesX[1, 1], +sizesY[1, 1], +sizesZ[0, 0]);
            vertices[06].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[07].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], +sizesZ[1, 1]);
        }

        protected override void InitialiseIndices()
        {
            //Laft
            AssignTri(00, 02, 01);
            AssignTri(00, 03, 02);
            //Top
            AssignTri(00, 04, 03);
            AssignTri(00, 05, 04);
            //Right
            AssignTri(04, 06, 07);
            AssignTri(04, 05, 06);
            //Front
            AssignTri(00, 06, 05);
            AssignTri(00, 01, 06);
            //Back
            AssignTri(04, 02, 03);
            AssignTri(04, 07, 02);
            //Bottom
            AssignTri(02, 06, 01);
            AssignTri(02, 07, 06);
        }

        protected override void InitialiseEdges()
        {
            //Left
            AssignLine(00, 01);
            AssignLine(01, 02);
            AssignLine(02, 03);
            AssignLine(03, 00);
            //Right
            AssignLine(04, 05);
            AssignLine(05, 06);
            AssignLine(06, 07);
            AssignLine(07, 04);
            //Front
            AssignLine(00, 05);
            AssignLine(01, 06);
            //Back
            AssignLine(03, 04);
            AssignLine(02, 07);
        }
    }
}
