using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class Icosahedron : Primitive
    {
        public Icosahedron(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        { 
            typeName = "Icosahedron";

            noVertices = 12;
            noFaces = 20;
            noTris = 20;
            noWires = 30;

            sideDivs = 2;
            sideMults = 1;
            correctionMatrix = Matrix.Identity;
        }

        protected override void InitialiseVertices()
        {
            vertices[00].Position = new Vector3(+sizesX[0, 0], +sizesY[1, 1], +sizesZ[2, 1]);
            vertices[01].Position = new Vector3(+sizesX[0, 0], +sizesY[1, 1], -sizesZ[2, 1]);
            vertices[02].Position = new Vector3(-sizesX[1, 1], +sizesY[2, 1], +sizesZ[0, 0]);
            vertices[03].Position = new Vector3(-sizesX[1, 1], -sizesY[2, 1], +sizesZ[0, 0]);
            vertices[04].Position = new Vector3(-sizesX[2, 1], +sizesY[0, 0], +sizesZ[1, 1]);
            vertices[05].Position = new Vector3(-sizesX[2, 1], +sizesY[0, 0], -sizesZ[1, 1]);
            vertices[06].Position = new Vector3(+sizesX[0, 0], -sizesY[1, 1], +sizesZ[2, 1]);
            vertices[07].Position = new Vector3(+sizesX[0, 0], -sizesY[1, 1], -sizesZ[2, 1]);
            vertices[08].Position = new Vector3(+sizesX[1, 1], -sizesY[2, 1], +sizesZ[0, 0]);
            vertices[09].Position = new Vector3(+sizesX[1, 1], +sizesY[2, 1], +sizesZ[0, 0]);
            vertices[10].Position = new Vector3(+sizesX[2, 1], +sizesY[0, 0], +sizesZ[1, 1]);
            vertices[11].Position = new Vector3(+sizesX[2, 1], +sizesY[0, 0], -sizesZ[1, 1]);
        }

        protected override void InitialiseIndices()
        {
            AssignLinedTri(00, 02, 01);
            AssignLinedTri(00, 01, 09);
            AssignLinedTri(00, 10, 04);
            AssignLinedTri(06, 04, 10);
            AssignLinedTri(00, 04, 02);
            AssignLinedTri(00, 09, 10);
            AssignLinedTri(03, 04, 06);
            AssignLinedTri(02, 04, 03);
            AssignLinedTri(09, 08, 10);
            AssignLinedTri(06, 10, 08);

            AssignLinedTri(07, 06, 08);
            AssignLinedTri(07, 03, 06);
            AssignLinedTri(05, 01, 02);
            AssignLinedTri(05, 02, 03);
            AssignLinedTri(01, 05, 11);
            AssignLinedTri(01, 11, 09);
            AssignLinedTri(09, 11, 08);
            AssignLinedTri(11, 07, 08);
            AssignLinedTri(05, 07, 11);
            AssignLinedTri(05, 03, 07);
        }

        protected override void InitialiseEdges()
        {

        }
    }
}
