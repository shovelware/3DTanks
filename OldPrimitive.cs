using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Primitives
{
    public struct VertexPositionColorNormal
    {
        public Vector3 Position;
        public Color Color;
        public Vector3 Normal;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Color, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float) * 3 + 4, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0)
        );
    }

    abstract class Primitive
    {
        /*
         * TODO
         * Rotate about point
         * Attach to nodes?
         */

        #region MVars
        //Initialisation
        protected string typeName; public string TypeName { get { return typeName; } }
        protected VertexPositionColorNormal[] vertices;
        protected short[] indices;
        protected short[] edges;

        protected Color colFace, colWire;
        protected short scale;

        protected int sideDivs;
        protected int sideMults;

        protected short noVertices, noFaces, noTris, noEdges;
        protected Matrix correctionMatrix;

        protected short indicesI = 0;
        protected short edgesI = 0;

        protected float sizeX, sizeY, sizeZ;
        protected float[,] sizesX, sizesY, sizesZ;

        protected int iVert0, iVertL, iVertC,
                      iFlat0, iFlatL, iFlatC,
                      iEdge0, iEdgeL, iEdgeC,
                      iv0, ivL;

        public Color ColFace { get { return colFace; } set { colFace = value; SetFaceCol(value); } }
        public Color ColWire { get { return colWire; } set { colWire = value; SetWireCol(value); } }

        public float SizeX { get { return sizeX; }/* set { sizeW[1, 1] = value; } */} //Redo divisions after setting?
        public float SizeY { get { return sizeY; }/* set { sizeH[1, 1] = value; } */} //Even allow setting?
        public float SizeZ { get { return sizeZ; }/* set { sizeL[1, 1] = value; } */}

        //Matrices

        static Game gaem;

        #region Drawing

        protected BasicEffect effect;
        public enum Shading { Smooth, Flat, Count };
        public enum Drawing { Wire, Face, Point, WireFace, FacePoint, Count };
        public enum Lighting { Wire, Face, Point, WireFace, FacePoint, Count }; //Need to finish this

        protected Drawing drawMode = Drawing.WireFace;
        protected Shading shadeMode = Shading.Flat;
        protected Lighting lightMode = Lighting.Face;

        public Drawing DrawMode { get { return drawMode; } }
        public Shading ShadeMode { get { return shadeMode; } }
        public Lighting LightMode { get { return lightMode; } }

        #endregion

        #region Orientation
        Vector3 vScl, vOrbPos, vPos,
                IvScl, IvOrbPos, IvPos;
        Quaternion qRot, qOrbRot, qRotParent, qOrbRotParent,
                   IqRot, IqOrbRot;

        Matrix mScl, mRot, mOrbRot, mOrbPos, mPos;

        #region Get & Set

        public Matrix World { get { return effect.World; } set { effect.World = value; } }
        public Matrix Projection { get { return effect.Projection; } set { effect.Projection = value; } }
        public Matrix View { get { return effect.View; } set { effect.View = value; } }

        public Vector3 vScale { get { return vScl; } set { vScl = value; } }
        public Vector3 vOrbitPosition { get { return vOrbPos; } set { vOrbPos = value; } }
        public Vector3 vPosition { get { return vPos; } set { vPos = value; } }

        public Quaternion qRotation { get { return qRot; } set { qRot = value; } }
        public Quaternion qParentRotation { get { return qRotParent; } set { qRotParent = value; } }
        public Quaternion qOrbitRotation { get { return qOrbRot; } set { qOrbRot = value; } }
        public Quaternion qOrbitParentRotation { get { return qOrbRotParent; } set { qOrbRotParent = value; } }

        public Matrix mScale { get { return mScl; } set { mScl = value; } }
        public Matrix mRotation { get { return mRot; } }

        public Matrix mOrbitRotation { get { return mOrbRot; } }
        public Matrix mOrbitPositon { get { return mOrbPos; } set { mOrbPos = value; } }

        public Matrix mPositon { get { return mPos; } set { mPos = value; } }
        
        #endregion

        #endregion

        #endregion MVars

        #region Make, Break, Load

        public Primitive(BasicEffect b)
        {
            effect = b;

            float sizeF = 128;
            sizeX = sizeF;
            sizeY = sizeF;
            sizeZ = sizeF;

            scale = 1;
            colFace = new Color(000, 255, 000, 255);
            colWire = new Color(255, 255, 000, 255);

            InitialisePrimitive();
            ResetPrimitive();
        }

        public Primitive(BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw)
        {
            effect = b;

            IvPos = vPos = position ?? Vector3.Zero;
            IqRot = qRot = preRot ?? Quaternion.Identity;
            qRotParent = Quaternion.Identity;
            qOrbRotParent = Quaternion.Identity;

            vScl = new Vector3(1, 1, 1);
            qOrbRot = Quaternion.Identity;
            vOrbPos = Vector3.Zero;

            sizeX = sizeXYZ.X;
            sizeY = sizeXYZ.Y;
            sizeZ = sizeXYZ.Z;

            scale = 1;
            colFace = face ?? new Color(255, 178, 064, 255);
            colWire = wire ?? new Color(128, 128, 128, 255);

            shadeMode = shade;
            drawMode = draw;

            InitialisePrimitive();
            ResetPrimitive();
            gaem = new Game();
        }

        protected virtual void InitialisePrimitive()
        {
            typeName = "BasicPrim";
            noVertices = 8;
            noFaces = 6;
            noTris = 12;
            noEdges = 12;
            sideDivs = 2;
            sideMults = 1;
            correctionMatrix = Matrix.CreateTranslation(new Vector3(-sizeX / 2, -sizeY / 2, -sizeZ / 2));
        }

        protected void ResetPrimitive()
        {
            if (shadeMode == Shading.Flat)
            {
                //condition ? first_expression : second_expression;
                vertices = new VertexPositionColorNormal[noVertices * noTris != 0 ? noVertices * noTris : noVertices];
                indices = new short[noTris * 3 != 0 ? noTris * 3 : noVertices];
                edges = new short[noEdges * 2];
            }

            else if (shadeMode == Shading.Smooth)
            {
                vertices = new VertexPositionColorNormal[noVertices];
                indices = new short[noTris * 3 != 0 ? noTris * 3 : noVertices];
                edges = new short[noEdges * 2];
            }

            qRot = Quaternion.Identity;
            

            InitialiseSides();
            ClearVertices();
            ClearIndices();
            ClearEdges();
            SetCol(colFace);
            //Inherited
            InitialiseVertices();
            InitialiseIndices();
            InitialiseEdges();
            //
            TransformVerts(correctionMatrix);
            CalculateNormals();
        }

        #endregion

        #region Vertex Data (Overridden)

        protected virtual void InitialiseVertices()
        {
            //Insert Vertex Position Date
        }

        protected virtual void InitialiseIndices()
        {
            //Insert Index Data
        }

        protected virtual void InitialiseEdges()
        {
            //Insert Edge Data
        }

        #endregion

        #region Vertex Tools

        protected void ClearVertices()
        {
            for (int i = vertices.Length - 1; i >= 0; i--)
                vertices[i].Color = new Color(255, 255, 255);
        }

        protected void ClearIndices()
        {
            for (int i = indices.Length - 1; i >= 0; i--)
                indices[i] = -1;
        }

        protected void ClearEdges()
        {
            for (int i = edges.Length - 1; i >= 0; i--)
                edges[i] = -1;
        }

        protected void InitialiseSides()
        {
            sizesX = new float[sideDivs + 1, sideMults + 1];
            sizesY = new float[sideDivs + 1, sideMults + 1];
            sizesZ = new float[sideDivs + 1, sideMults + 1];

            sizesX[1, 1] = sizeX;
            sizesY[1, 1] = sizeY;
            sizesZ[1, 1] = sizeZ;

            DivideSides(sideDivs, sideMults);
        }

        protected void DivideSides(int divisions, int multiplis)
        {
            for (int d = 0; d <= divisions; d++)
            {
                for (int m = 0; m <= multiplis; m++)
                {
                    if (d != 0) //Avoid divZero errors
                    {
                        sizesX[d, m] = (sizesX[1, 1] / d) * m;
                        sizesY[d, m] = (sizesY[1, 1] / d) * m;
                        sizesZ[d, m] = (sizesZ[1, 1] / d) * m;
                    }

                    else
                    {
                        sizesX[d, m] = 0;
                        sizesY[d, m] = 0;
                        sizesZ[d, m] = 0;
                    }
                }
            }
        }

        protected short GetFirstUnindexedVertex()
        {
            short largest = -1;
            foreach (short s in indices)
            {
                if (s > largest)
                    largest = s;
                if (s == -1)
                    break;
            }

            if (largest < noVertices - 1)
            {
                largest = noVertices;
            }

            return (short)(largest + 1);
        }

        protected void CopyPos(short src, short cpy)
        {
            vertices[cpy].Position = vertices[src].Position;
        }

        protected void TransformVerts(Matrix tm)
        {
            for (int i = vertices.Length - 1; i >= 0; i--)
                vertices[i].Position = Vector3.Transform(vertices[i].Position, tm);
        }

        #endregion

        #region Vertex Assignment

        protected void AssignVertexIndex(short vertex, short index)
        {
            if (indices.Contains<short>(vertex))//if the vertex is already in indices
            {
                short nextVert = GetFirstUnindexedVertex();
                CopyPos(vertex, nextVert); //copy vertex position to next unindexed vertex
                if (curVertexIndex <= indices.Count())
                indices[index] = nextVert; //perform regular assignment with new copied vertex
            }

            else indices[index] = vertex; //otherwise assign normally
        }

        protected void AssignEdgeIndex(short vertex, short index)
        {
            if (curEdgeIndex <= edges.Count())
                edges[index] = vertex;
        }

        protected void AssignTri(short v0, short v1, short v2)
        {
            if (shadeMode == Shading.Flat)
            {
                AssignVertexIndex(v0, curVertexIndex++);
                AssignVertexIndex(v1, curVertexIndex++);
                AssignVertexIndex(v2, curVertexIndex++);
            }

            else if (shadeMode == Shading.Smooth)
            {
                indices[curVertexIndex++] = v0;
                indices[curVertexIndex++] = v1;
                indices[curVertexIndex++] = v2;
            }
        }

        protected void AssignLine(short v0, short v1)
        {
            bool ass = true;

            //if the line isn't already there, assign it
            for (short e = 0; e < edges.Count(); e += 2)
            {
                if ((edges[e] == v0 && edges[e + 1] == v1) || (edges[e] == v1 && edges[e + 1] == v0))
                {
                    ass = false;

                    if (edges[e] == -1 && edges[e + 1] == -1)
                        break;
                }
            }

            if (ass)
            {
                AssignEdgeIndex(v0, curEdgeIndex++);
                AssignEdgeIndex(v1, curEdgeIndex++);
            }
        }

        protected void AssignLinedTri(short v0, short v1, short v2)
        {
            if (shadeMode == Shading.Flat)
            {
                AssignLine(v0, v1);
                AssignLine(v1, v2);
                AssignLine(v2, v0);
                AssignVertexIndex(v0, curVertexIndex++);
                AssignVertexIndex(v1, curVertexIndex++);
                AssignVertexIndex(v2, curVertexIndex++);
            }

            else if (shadeMode == Shading.Smooth)
            {
                AssignLine(v0, v1);
                AssignLine(v1, v2);
                AssignLine(v2, v0);
                indices[curVertexIndex++] = v0;
                indices[curVertexIndex++] = v1;
                indices[curVertexIndex++] = v2;
            }

        }

        protected void AssignLinedFace(short[] verts)
        {
            for (short e = 0; e < verts.Count() - 1; e++)
            {
                AssignLine(verts[e], verts[e + 1]);
            }

            AssignLine(verts[verts.Length - 1], verts[0]);
        }

        protected void AssignLinedFacesJoints(short[] faceUno, short[] faceDuo)
        {
            if (faceUno.Count() == faceDuo.Count())
            {
                for (int s = faceUno.Count() - 1; s >= 0; s--)
                {
                    AssignLine(faceUno[s], faceDuo[s]);
                }
            }
        }

        protected void AssignLinedFaceToPoint(short[] face, short point)
        {
            foreach (short v in face)
            {
                AssignLine(v, point);
            }
        }

        #endregion

        #region Normals

        protected void CalculateNormals()
        {
            //for every 3 indices, calculate a face normal and assign it to all three;

            for (short i = 0; i < indices.Length / 3; i++)
            {
                short index0 = indices[i * 3];
                short index1 = indices[i * 3 + 1];
                short index2 = indices[i * 3 + 2];

                if (index0 != -1 && index1 != -1 && index2 != -1)//Checking if indices have been assigned
                {
                    Vector3 side0 = vertices[index0].Position - vertices[index1].Position;
                    Vector3 side1 = vertices[index0].Position - vertices[index2].Position;
                    Vector3 normal = Vector3.Cross(side0, side1);

                    vertices[index0].Normal += normal;
                    vertices[index1].Normal += normal;
                    vertices[index2].Normal += normal;
                }
            }

            for (int i = 0; i < vertices.Length; i++)
                vertices[i].Normal.Normalize();
        }

        #endregion

        #region Colours

        public void SetCol(Color newCol)
        {
            for (int i = vertices.Length - 1; i >= 0; i--)
                vertices[i].Color = newCol;
        }

        public void RandomiseVertexCol() // Doesn't work with Wire drawing because setCol()
        {
            Random rng = new Random();

            for (int i = vertices.Length - 1; i >= 0; i--)
            {
                vertices[i].Color = new Color(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255));
            }
        }

        #endregion

        #region Tweaks

        //All scaling is kind of broke

        public void ChangeScale(short newscale)
        {
            scale = newscale;
            for (int i = vertices.Length - 1; i > 0; i--)
                vertices[i].Position *= scale;
        }

        public void CrementScale(bool dir)
        {
            sbyte direction = 1;
            if (!dir)
                direction *= -1;

            for (int i = vertices.Length - 1; i >= 0; i--)
                vertices[i].Position *= (scale + direction);
        }

        public void ResetScale()
        {
            ResetPrimitive();
        }

        #endregion

        #region Movement

        public void PositionAdd(Vector3 posXYZ)
        {
            vPos += posXYZ;
        }

        public void PositionReset()
        {
            vPos = IvPos;
        }
        #endregion

        #region Rotation

        #region Local Axes

        public void RotateX(float rotation)
        {
            Quaternion qX = Quaternion.CreateFromAxisAngle(mRot.Right, rotation);
            qRot = Quaternion.Concatenate(qRot, qX);
        }

        public void RotateY(float rotation)
        {
            Quaternion qY = Quaternion.CreateFromAxisAngle(mRot.Up, rotation);
            qRot = Quaternion.Concatenate(qRot, qY);
        }

        public void RotateZ(float rotation)
        {
            Quaternion qZ = Quaternion.CreateFromAxisAngle(mRot.Forward, rotation);
            qRot = Quaternion.Concatenate(qRot, qZ);
        }

        public void RotReset()
        {
            qRot = IqRot;
        }

        #endregion

        #region World Axes

        public void RotateWorldX(float rotation)
        {
            Quaternion qWorldX = Quaternion.CreateFromAxisAngle(Vector3.Right, rotation);
            qRot = Quaternion.Concatenate(qRot, qWorldX);
        }

        public void RotateWorldY(float rotation)
        {
            Quaternion qWorldY = Quaternion.CreateFromAxisAngle(Vector3.Up, rotation);
            qRot = Quaternion.Concatenate(qRot, qWorldY);
        }

        public void RotateWorldZ(float rotation)
        {
            Quaternion qWorldZ = Quaternion.CreateFromAxisAngle(Vector3.Forward, rotation);
            qRot = Quaternion.Concatenate(qRot, qWorldZ);
        }

        #endregion

        #region Orbit

        public void RotateOrbitX(float rotation)
        {
            Quaternion qOrbX = Quaternion.CreateFromAxisAngle(mRot.Right, rotation);
            qOrbRot = Quaternion.Concatenate(qOrbRot, qOrbX);
        }

        public void RotateOrbitY(float rotation)
        {
            Quaternion qOrbY = Quaternion.CreateFromAxisAngle(mRot.Up, rotation);
            qOrbRot = Quaternion.Concatenate(qOrbRot, qOrbY);
        }

        public void RotateOrbitZ(float rotation)
        {
            Quaternion qOrbZ = Quaternion.CreateFromAxisAngle(mRot.Forward, rotation);
            qOrbRot = Quaternion.Concatenate(qOrbRot, qOrbZ);
        }

        public void RotationOrbitReset()
        {
            qOrbRot = IqOrbRot;
        }

        #endregion

        #endregion

        #region U&D

        public void Update(Matrix proj, Matrix view, Matrix world, GameTime gameTime)
        {
            NormalizeQuats();
            UpdateMatrices(world);
            effect.Projection = proj;
            effect.View = view;
        }

        private void NormalizeQuats()
        {
            qRot.Normalize();
            qRotParent.Normalize();
            qOrbRot.Normalize();
            qOrbRotParent.Normalize();

        }

        private void UpdateMatrices(Matrix world)
        {
            mScl = Matrix.CreateScale(vScl);

            //mRot = Matrix.CreateFromQuaternion(Quaternion.Concatenate(qParent, qRot));
            mRot = Matrix.CreateFromQuaternion(qRot);//

            //mOrbPos = Matrix.CreateTranslation(vOrbPos);

            //mOrbRot = Matrix.CreateFromQuaternion(Quaternion.Concatenate(qOrbRot, qOrbParent));
            //mOrbRot = Matrix.CreateFromQuaternion(qOrbRot);//

            mPos = Matrix.CreateTranslation(vPos);

            //effect.World = world * mScl * mRot * mOrbPos * mOrbRot * mPos;
            effect.World = world * mScl * mRot * mPos;
        }

        public void Draw(GraphicsDeviceManager g)
        {
            if (drawMode == Drawing.Face || drawMode == Drawing.WireFace || drawMode == Drawing.FacePoint)
            {
                if (lightMode == Lighting.Face || Lighting.WireFace)
                    effect.LightingEnabled = true;
                else effect.LightingEnabled = false;
                

                //gdm = (GraphicsDeviceManager)gaem.Services.GetService(typeof(IGraphicsDeviceManager));
                //Ken how do I use this properly

                //Face draw
                effect.CurrentTechnique.Passes[0].Apply();
                g.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, indices, 0, indices.Length / 3, VertexPositionColorNormal.VertexDeclaration);
            }

            if (drawMode == Drawing.Wire || drawMode == Drawing.WireFace)
            {

                if (lightMode == Lighting.Wire || Lighting.WireFace)
                    effect.LightingEnabled = true;
                else effect.LightingEnabled = false;



                                //store RasteriserState, add depth bias, turn on
                //RasterizerState rsOld = g.GraphicsDevice.RasterizerState;
                //RasterizerState rs = new RasterizerState();
                //rs.DepthBias = 1;
                //g.GraphicsDevice.RasterizerState = rs;

                //flip to wire color, draw wires, flip back to face color
                //set col to wireframe
                SetCol(colWire);

                //Wireframe draw
                effect.CurrentTechnique.Passes[0].Apply();
                g.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, vertices, 0, vertices.Length, edges, 0, edges.Length / 2, VertexPositionColorNormal.VertexDeclaration);//Ken make this better

                //reset stuff to old values
                SetCol(colFace);
                effect.LightingEnabled = lighting;
                //g.GraphicsDevice.RasterizerState = rsOld;

            }

            if (drawMode == Drawing.Point || drawMode == Drawing.FacePoint)
            {
                if (lightMode == Lighting.Point || Lighting.FacePoint)
                    effect.LightingEnabled = true;
                else effect.LightingEnabled = false;

                effect.CurrentTechnique.Passes[0].Apply();

                g.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, vertices, 0, vertices.Length, edges, 0, edges.Length / 2, VertexPositionColorNormal.VertexDeclaration);

            }

        #endregion
        }
    }
}
