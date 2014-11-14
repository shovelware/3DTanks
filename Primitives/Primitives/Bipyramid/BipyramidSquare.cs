using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class BipyramidSquare : Primitive
    {
        public BipyramidSquare(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        { 
            typeName = "BipyramidSquare"; 
            noVertices = 6;
            noFaces = 8;
            noTris = 12;
            noWires = 12;
            sideDivs = 2;
            sideMults = 1;
            correctionMatrix = Matrix.CreateTranslation(new Vector3(-sizeX / 2, 0, +sizeZ / 2));
        }

        protected override void InitialiseVertices()
        {
            //Base
            vertices[00].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[01].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[02].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], -sizesZ[1, 1]);
            vertices[03].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], -sizesZ[1, 1]);
            //Point
            vertices[04].Position = new Vector3(+sizesX[2, 1], +sizesY[2, 1], -sizesZ[2, 1]);
            vertices[05].Position = new Vector3(+sizesX[2, 1], -sizesY[2, 1], -sizesZ[2, 1]);                  
        }

        protected override void InitialiseIndices()
        {
            //Top
            AssignLinedTri(04, 01, 00);
            AssignLinedTri(04, 02, 01);
            AssignLinedTri(04, 03, 02);
            AssignLinedTri(04, 00, 03);
            //Bottom
            AssignLinedTri(05, 00, 01);
            AssignLinedTri(05, 01, 02);
            AssignLinedTri(05, 02, 03);
            AssignLinedTri(05, 03, 00);
        }

        protected override void InitialiseEdges()
        {
        }
    }
}
