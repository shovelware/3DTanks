using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class Line : Primitive
    {
        public Line(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? wire) : base(g, b, position, preRot, sizeXYZ, Color.Black, wire, Shading.Flat, Drawing.Wire) { }

        protected override void InitialisePrimitive()
        { 
            typeName = "Line";
            noVertices = 2;
            noFaces = 0;
            noTris = 0;
            noWires = 1;
            sideDivs = 2;
            sideMults = 1;
            correctionMatrix = Matrix.Identity; 
        }

        protected override void InitialiseVertices()
        {
            vertices[00].Position = new Vector3(+sizesX[2, 1], +sizesY[2, 1], +sizesZ[2, 1]);
            vertices[01].Position = new Vector3(-sizesX[2, 1], -sizesY[2, 1], -sizesZ[2, 1]);
        }

        protected override void InitialiseIndices()
        {
        }

        protected override void InitialiseEdges()
        {
            AssignLine(00, 01);
        }
    }
}
