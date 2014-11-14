using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class Inheritorz : Primitive
    {
        public Inheritorz(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        { 
            typeName = "";

            noVertices = 4;
            noFaces = 1;
            noTris = 2;
            noWires = 4;

            sideDivs = 1;
            sideMults = 1;
            correctionMatrix = Matrix.CreateTranslation(new Vector3(-sizeX / 2, -sizeY / 2, -sizeZ / 2)); 
        }

        protected override void InitialiseVertices()
        {
            vertices[00].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[01].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[02].Position = new Vector3(+sizesX[1, 1], +sizesY[1, 1], +sizesZ[0, 0]);
            vertices[03].Position = new Vector3(+sizesX[0, 0], +sizesY[1, 1], +sizesZ[0, 0]);
        }

        protected override void InitialiseIndices()
        {
            AssignTri(00, 02, 01);
            AssignTri(00, 03, 02);
        }

        protected override void InitialiseEdges()
        {
            AssignLine(00, 01);
            AssignLine(01, 02);
            AssignLine(02, 03);
            AssignLine(03, 00);
        }
    }
}
