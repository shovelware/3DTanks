using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class TankBarrel : Primitive
    {
        public TankBarrel(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        {
            typeName = "TankBarrel";
            noVertices = 18;
            noFaces = 10;
            noTris = 32;
            noWires = 24;
            sideDivs = 3;
            sideMults = 2;
            correctionMatrix = Matrix.CreateTranslation(new Vector3(-sizeX / 2, +sizeY / 2, 0));
        }

        protected override void InitialiseVertices()
        {
            //Front
            vertices[00].Position = new Vector3(+sizesX[2, 1], -sizesY[2, 1], +sizesZ[2, 1]);
            vertices[01].Position = new Vector3(+sizesX[3, 1], -sizesY[1, 1], +sizesZ[2, 1]);
            vertices[02].Position = new Vector3(+sizesX[0, 1], -sizesY[3, 2], +sizesZ[2, 1]);
            vertices[03].Position = new Vector3(+sizesX[0, 1], -sizesY[3, 1], +sizesZ[2, 1]);
            vertices[04].Position = new Vector3(+sizesX[3, 1], +sizesY[0, 1], +sizesZ[2, 1]);
            vertices[05].Position = new Vector3(+sizesX[3, 2], +sizesY[0, 1], +sizesZ[2, 1]);
            vertices[06].Position = new Vector3(+sizesX[1, 1], -sizesY[3, 1], +sizesZ[2, 1]);
            vertices[07].Position = new Vector3(+sizesX[1, 1], -sizesY[3, 2], +sizesZ[2, 1]);
            vertices[08].Position = new Vector3(+sizesX[3, 2], -sizesY[1, 1], +sizesZ[2, 1]);
            //Back
            vertices[09].Position = new Vector3(+sizesX[2, 1], -sizesY[2, 1], -sizesZ[2, 1]);
            vertices[10].Position = new Vector3(+sizesX[3, 1], -sizesY[1, 1], -sizesZ[2, 1]);
            vertices[11].Position = new Vector3(+sizesX[3, 2], -sizesY[1, 1], -sizesZ[2, 1]);
            vertices[12].Position = new Vector3(+sizesX[1, 1], -sizesY[3, 2], -sizesZ[2, 1]);
            vertices[13].Position = new Vector3(+sizesX[1, 1], -sizesY[3, 1], -sizesZ[2, 1]);
            vertices[14].Position = new Vector3(+sizesX[3, 2], +sizesY[0, 1], -sizesZ[2, 1]);
            vertices[15].Position = new Vector3(+sizesX[3, 1], +sizesY[0, 1], -sizesZ[2, 1]);
            vertices[16].Position = new Vector3(+sizesX[0, 1], -sizesY[3, 1], -sizesZ[2, 1]);
            vertices[17].Position = new Vector3(+sizesX[0, 1], -sizesY[3, 2], -sizesZ[2, 1]);
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
            //Rear face
            AssignTri(09, 10, 11);
            AssignTri(09, 11, 12);
            AssignTri(09, 12, 13);
            AssignTri(09, 13, 14);
            AssignTri(09, 14, 15);
            AssignTri(09, 15, 16);
            AssignTri(09, 16, 17);
            AssignTri(09, 17, 10);
            //Rim
            AssignTri(01, 10, 17);
            AssignTri(01, 17, 02);
            AssignTri(03, 17, 16);
            AssignTri(03, 02, 17);
            AssignTri(03, 16, 15);
            AssignTri(03, 15, 04);
            AssignTri(05, 15, 14);
            AssignTri(05, 04, 15);
            AssignTri(05, 14, 13);
            AssignTri(05, 13, 06);
            AssignTri(07, 13, 12);
            AssignTri(07, 06, 13);
            AssignTri(07, 12, 11);
            AssignTri(07, 11, 08);
            AssignTri(01, 11, 10);
            AssignTri(01, 08, 11);
        }

        protected override void InitialiseEdges()
        {
            short[] fron = new short[8] { 01, 02, 03, 04, 05, 06, 07, 08 };
            short[] back = new short[8] { 10, 17, 16, 15, 14, 13, 12, 11 };

            AssignLinedFace(fron);
            AssignLinedFace(back);
            AssignLinedFacesJoints(fron, back);
        }
    }
}
