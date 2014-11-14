using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class Plane : Primitive
    {
        public Plane(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        { 
            typeName = "Plane"; 
            noVertices = 4;
            noFaces = 1;
            noTris = 2;
            noWires = 4;
            sideDivs = 1;
            sideMults = 1;
            correctionMatrix = Matrix.CreateTranslation(new Vector3(-sizeX / 2, 0, -sizeZ / 2));
        }

        protected override void InitialiseVertices()
        {
            vertices[00].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[01].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[02].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], +sizesZ[1, 1]);
            vertices[03].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[1, 1]);
        }

        protected override void InitialiseIndices()
        {
            AssignTri(00, 01, 02);
            AssignTri(00, 02, 03);
        }

        protected override void InitialiseEdges()
        {
            short[] face = new short[] { 00, 01, 02, 03 };

            AssignLinedFace(face);
        }
    }
}
