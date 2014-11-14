using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class BulletPrim : Primitive
    {
        public BulletPrim(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        {
            typeName = "PyramidOctagon";
            noVertices = 10;
            noFaces = 9;
            noTris = 16;
            noWires = 16;
            sideDivs = 3;
            sideMults = 2;
            correctionMatrix = Matrix.CreateTranslation(new Vector3(-sizeX / 2, -sizeY / 2, +sizeZ / 2)) * Matrix.CreateRotationX(MathHelper.ToRadians(-90));
        }

        protected override void InitialiseVertices()
        {
            //Base
            vertices[00].Position = new Vector3(+sizesX[2, 1], +sizesY[0, 1] ,-sizesZ[2, 1]);
            vertices[01].Position = new Vector3(+sizesX[3, 1], +sizesY[0, 1] ,-sizesZ[1, 1]);
            vertices[02].Position = new Vector3(+sizesX[0, 1], +sizesY[0, 1] ,-sizesZ[3, 2]);
            vertices[03].Position = new Vector3(+sizesX[0, 1], +sizesY[0, 1] ,-sizesZ[3, 1]);
            vertices[04].Position = new Vector3(+sizesX[3, 1], +sizesY[0, 1] ,+sizesZ[0, 1]);
            vertices[05].Position = new Vector3(+sizesX[3, 2], +sizesY[0, 1] ,+sizesZ[0, 1]);
            vertices[06].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 1] ,-sizesZ[3, 1]);
            vertices[07].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 1] ,-sizesZ[3, 2]);
            vertices[08].Position = new Vector3(+sizesX[3, 2], +sizesY[0, 1] ,-sizesZ[1, 1]);
            //Point
            vertices[09].Position = new Vector3(+sizesX[2, 1], +sizesY[1, 1], -sizesZ[2, 1]);
        }                                                                        

        protected override void InitialiseIndices()
        {
            //Front face
            AssignTri(00, 01, 02);
            AssignTri(00, 02, 03);
            AssignTri(00, 03, 04);
            AssignTri(00, 04, 05);
            AssignTri(00, 05, 06);
            AssignTri(00, 06, 07);
            AssignTri(00, 07, 08);
            AssignTri(00, 08, 01);
            //Point
            AssignTri(09, 02, 01);
            AssignTri(09, 03, 02);
            AssignTri(09, 04, 03);
            AssignTri(09, 05, 04);
            AssignTri(09, 06, 05);
            AssignTri(09, 07, 06);
            AssignTri(09, 08, 07);
            AssignTri(09, 01, 08);
        }

        protected override void InitialiseEdges()
        {
            short[] fron = new short[8] { 01, 02, 03, 04, 05, 06, 07, 08 };

            AssignLinedFace(fron);
            AssignLinedFaceToPoint(fron, 09);
        }
    }
}
