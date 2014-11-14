using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    class Inheritor : Primitive
    {
        public Inheritor(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw) : base(g, b, position, preRot, sizeXYZ, face, wire, shade, draw) { }

        protected override void InitialisePrimitive()
        { 
            //Name your primitive type
            typeName = "Inheritor";
            //Make sure this data is correct for vertex calculations
            noVertices = 4;
            //This is just cause, but it could come in handy
            noFaces = 1;
            //Flat shading calculations use this
            noTris = 2;
            //Wireframe drawing needs this
            noWires = 4;
            //Divisions and multiplications for handiness in construction
            //Make sure these are consistent with what you try to access in InitialiseVertices()
            //Also they're explained over there
            sideDivs = 1;
            sideMults = 1;
            //Centering all points, if you build your primitive off-origin
            //This is the translation to move the center of your primitive to the origin, figure it out yourself
            correctionMatrix = Matrix.CreateTranslation(new Vector3(-sizeX / 2, -sizeY / 2, 0)); 
        }

        protected override void InitialiseVertices()
        {
            //These are your vertex declarations
            //sizesX, Y and Z work off sideDivs and sideMults
            //[div, mult] = (size / div) * mult
            //[2, 1] is a half
            //[8, 3] is three eighths
            //[1, 4] is four times the size
            vertices[00].Position = new Vector3(+sizesX[0, 0], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[01].Position = new Vector3(+sizesX[1, 1], +sizesY[0, 0], +sizesZ[0, 0]);
            vertices[02].Position = new Vector3(+sizesX[1, 1], +sizesY[1, 1], +sizesZ[0, 0]);
            vertices[03].Position = new Vector3(+sizesX[0, 0], +sizesY[1, 1], +sizesZ[0, 0]);
        }

        protected override void InitialiseIndices()
        {
            //Assigns a triangle to next available indices.
            //Counter Clockwise winding order, unless you're doing something weird
            AssignTri(00, 02, 01);
            AssignTri(00, 03, 02);
        }

        protected override void InitialiseEdges()
        {
            //Draw the edges around your primitive
            //This is to avoid non-triangular faces splitting into tris while drawing with wireframe
            //This is much easier if you logically assign your vertices
            AssignLine(00, 01);
            AssignLine(01, 02);
            AssignLine(02, 03);
            AssignLine(03, 00);
        }
    }
}
