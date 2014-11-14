using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class BipyramidOctagon : Primitive
    {
        public BipyramidOctagon(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        {
            typeName = "BipyramidOctagon";
            noVertices = 10;
            noFaces = 16;
            noTris = 16;
            noWires = 24;
            sideDivs = 3;
            sideMults = 2;
            correctionMatrix = Matrix.CreateTranslation(new Vector3(-sizeX / 2, -sizeY / 2, +sizeZ / 2));
        }

        protected override void InitialiseVertices()
        {
            //Point
            vertices[00].Position = new Vector3(+sizesX[2, 1], +sizesY[1, 1], -sizesZ[2, 1]);
            //Middle Rim
            vertices[01].Position = new Vector3(+sizesX[3, 1], +sizesY[2, 1], -sizesZ[1, 1]);
            vertices[02].Position = new Vector3(+sizesX[0, 1], +sizesY[2, 1], -sizesZ[3, 2]);
            vertices[03].Position = new Vector3(+sizesX[0, 1], +sizesY[2, 1], -sizesZ[3, 1]);
            vertices[04].Position = new Vector3(+sizesX[3, 1], +sizesY[2, 1], +sizesZ[0, 1]);
            vertices[05].Position = new Vector3(+sizesX[3, 2], +sizesY[2, 1], +sizesZ[0, 1]);
            vertices[06].Position = new Vector3(+sizesX[1, 1], +sizesY[2, 1], -sizesZ[3, 1]);
            vertices[07].Position = new Vector3(+sizesX[1, 1], +sizesY[2, 1], -sizesZ[3, 2]);
            vertices[08].Position = new Vector3(+sizesX[3, 2], +sizesY[2, 1], -sizesZ[1, 1]);
            //Point
            vertices[09].Position = new Vector3(+sizesX[2, 1], +sizesY[0, 0], -sizesZ[2, 1]);

        }

        protected override void InitialiseIndices()
        {
            //Front face
            AssignLinedTri(00, 02, 01);
            AssignLinedTri(00, 03, 02);
            AssignLinedTri(00, 04, 03);
            AssignLinedTri(00, 05, 04);
            AssignLinedTri(00, 06, 05);
            AssignLinedTri(00, 07, 06);
            AssignLinedTri(00, 08, 07);
            AssignLinedTri(00, 01, 08);
            //Point
            AssignLinedTri(09, 01, 02);
            AssignLinedTri(09, 02, 03);
            AssignLinedTri(09, 03, 04);
            AssignLinedTri(09, 04, 05);
            AssignLinedTri(09, 05, 06);
            AssignLinedTri(09, 06, 07);
            AssignLinedTri(09, 07, 08);
            AssignLinedTri(09, 08, 01);
        }

        protected override void InitialiseEdges()
        {
        }
    }
}
