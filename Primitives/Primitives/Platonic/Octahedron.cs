using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class Octahedron : Primitive
    {
        public Octahedron(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        {
            typeName = "Octahedron";

            noVertices = 6;
            noFaces = 8;
            noTris = 8;
            noWires = 12;

            sideDivs = 1;
            sideMults = 1;
            correctionMatrix = Matrix.Identity;
        }

        protected override void InitialiseVertices()
        {
            

            vertices[00].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[1, 1]);
            vertices[01].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[02].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], -sizesZ[1, 1]);
            vertices[03].Position = new Vector3(-sizesX[1, 1], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[04].Position = new Vector3(+sizesX[0, 0], +sizesY[1, 1], +sizesZ[0, 0]);
            vertices[05].Position = new Vector3(+sizesX[0, 0], -sizesY[1, 1], +sizesZ[0, 0]);
        }

        protected override void InitialiseIndices()
        {
            AssignLinedTri(01, 00, 04);
            AssignLinedTri(02, 01, 04);
            AssignLinedTri(03, 02, 04);
            AssignLinedTri(00, 03, 04);
            AssignLinedTri(03, 00, 05);
            AssignLinedTri(02, 03, 05);
            AssignLinedTri(01, 02, 05);
            AssignLinedTri(00, 01, 05);
        }

        protected override void InitialiseEdges()
        {
        }
    }
}
