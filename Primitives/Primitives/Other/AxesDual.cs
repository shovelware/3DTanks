using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class AxesDual : Primitive
    {
        /* TODO
         * Write attach method
         * Give parent
         */

        public AxesDual(Game g, BasicEffect b, Vector3? position, Quaternion preRot, Vector3 sizeXYZ) : base(g, b, position, preRot, sizeXYZ, Color.White, Color.Green, Shading.Flat, Drawing.WireFace) { }

        protected override void InitialisePrimitive()
        { 
            typeName = "AxesDual";

            noVertices = 12;
            noFaces = 0;
            noTris = 0;
            noWires = 6;

            sideDivs = 1;
            sideMults = 1;
            correctionMatrix = Matrix.Identity; 
        }

        protected override void InitialiseVertices()
        {
            //X
            vertices[00].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[01].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], +sizesZ[0, 0]);
            //Y
            vertices[02].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[03].Position = new Vector3(+sizesX[0, 0], +sizesY[1, 1], +sizesZ[0, 0]);
            //Z
            vertices[04].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[05].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[1, 1]);
            //-X
            vertices[06].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[07].Position = new Vector3(-sizesX[1, 1], +sizesY[0, 0], +sizesZ[0, 0]);
            //-Y
            vertices[08].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[09].Position = new Vector3(+sizesX[0, 0], -sizesY[1, 1], +sizesZ[0, 0]);
            //-Z
            vertices[10].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[11].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], -sizesZ[1, 1]);
        }

        protected override void InitialiseIndices()
        {
        }

        protected override void InitialiseEdges()
        {
            AssignLine(00, 01);
            AssignLine(02, 03);
            AssignLine(04, 05);
            AssignLine(06, 07);
            AssignLine(08, 09);
            AssignLine(10, 11);

            //+
            wires[00].Color = new Color(255, 000, 000);
            wires[01].Color = new Color(255, 000, 000);
            wires[02].Color = new Color(000, 255, 000);
            wires[03].Color = new Color(000, 255, 000);
            wires[04].Color = new Color(000, 000, 255);
            wires[05].Color = new Color(000, 000, 255);
            //-
            wires[06].Color = new Color(000, 255, 255);
            wires[07].Color = new Color(000, 255, 255);
            wires[08].Color = new Color(255, 000, 255);
            wires[09].Color = new Color(255, 000, 255);
            wires[10].Color = new Color(255, 255, 000);
            wires[11].Color = new Color(255, 255, 000);

        }
    }
}
