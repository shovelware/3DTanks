using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class Tetrahedron : Primitive
    {
        public Tetrahedron(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        {
            typeName = "Tetrahedron";

            noVertices = 4;
            noFaces = 4;
            noTris = 4;
            noWires = 6;

            sideDivs = 2;
            sideMults = 1;
            correctionMatrix = Matrix.Identity;//CreateTranslation(new Vector3(-sizeX / 2, (-sizeY /2), +sizeZ / 2));// Ken is this right
        }

        protected override void InitialiseVertices()
        {
            //Check these
            //vertices[00].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            //vertices[01].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], +sizesZ[0, 0]);
            //vertices[02].Position = new Vector3(+sizesX[2, 1], +sizesY[0, 0], -sizesZ[1, 1]);
            //vertices[03].Position = new Vector3(+sizesX[2, 1], +sizesY[1, 1], -sizesZ[2, 1]);
            //(1,1,1), (1,−1,−1), (−1,1,−1), (−1,−1,1)
            vertices[00].Position = new Vector3(-sizesX[1, 1], -sizesY[1, 1], +sizesZ[1, 1]);
            vertices[01].Position = new Vector3(+sizesX[1, 1], -sizesY[1, 1], -sizesZ[1, 1]);
            vertices[02].Position = new Vector3(-sizesX[1, 1], +sizesY[1, 1], -sizesZ[1, 1]);
            vertices[03].Position = new Vector3(+sizesX[1, 1], +sizesY[1, 1], +sizesZ[1, 1]);
        }

        protected override void InitialiseIndices()
        {
            AssignLinedTri(00, 02, 03);
            AssignLinedTri(00, 03, 01);
            AssignLinedTri(00, 01, 02);
            AssignLinedTri(03, 02, 01);
        }

        protected override void InitialiseEdges()
        {
        }
    }
}
