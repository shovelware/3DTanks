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
         * Enquire to ken about how quaternion rotation works and how to set it
         */

        #region MVars

        #region Vertex Data & Helpers, Refs

        protected string typeName; public string TypeName { get { return typeName; } }
        protected VertexPositionColorNormal[] vertices;
        protected VertexPositionColorNormal[] wires;

        protected short[] faceIndices;
        protected short[] wireIndices;
        protected short facesI = 0;
        protected short wiresI = 0;

        protected short noVertices, noFaces, noTris, noWires;
        protected Matrix correctionMatrix;

        protected Color colFace, colWire,
                        colFaceTar, colWireTar;

        float colFaceLerp, colWireLerp;

        public Color ColFace { get { return colFace; } set { colFace = value; SetColFace(value); } }
        public Color ColWire { get { return colWire; } set { colWire = value; SetColWire(value); } }
        public Color ColFaceTar { get { return colFaceTar; } set { colFaceTar = value; } }
        public Color ColWireTar { get { return colWireTar; } set { colWireTar = value; } }
        public float ColFaceLerp { get { return colFaceLerp; } set { colFaceLerp = value; } }
        public float ColWireLerp { get { return colWireLerp; } set { colWireLerp = value; } }
        public bool ColFaceLerping { get { return colFace != colFaceTar; } }
        public bool ColWireLerping { get { return colWire != colWireTar; } }

        protected int sideDivs;
        protected int sideMults;

        protected float sizeX, sizeY, sizeZ;
        protected float[,] sizesX, sizesY, sizesZ;

        public float SizeX { get { return sizeX; }/* set { sizeW[1, 1] = value; } */} //Redo divisions after setting?
        public float SizeY { get { return sizeY; }/* set { sizeH[1, 1] = value; } */} //Even allow setting?
        public float SizeZ { get { return sizeZ; }/* set { sizeL[1, 1] = value; } */}

        Game game;

        #endregion

        #region Drawing

        protected BasicEffect effect;
        protected GraphicsDeviceManager gdm;

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

        #region Orientation Matrices &+

        Vector3 vScl, vOrbPos, vPos,
                IvScl, IvOrbPos, IvPos;
        Quaternion qRot, qOrbRot, qRotParent, qOrbRotParent,
                   IqRot, IqOrbRot;

        Matrix mWorld, mScl, mRot, mOrbRot, mOrbPos, mPos;

        List<Matrix> mList = new List<Matrix>();

        #region Get & Set

        public Matrix World { get { return mWorld; } set { mWorld = value; } }
        public Matrix Projection { get { return effect.Projection; } set { effect.Projection = value; } }
        public Matrix View { get { return effect.View; } set { effect.View = value; } }

        public Vector3 vScale { get { return vScl; } set { vScl = value; } }
        public Vector3 vOrbitPosition { get { return vOrbPos; } set { vOrbPos = value; } }
        public Vector3 vPosition { get { return vPos; } }

        public Quaternion qRotation { get { return qRot; } set { qRot = value; } }
        public Quaternion qParentRotation { get { return qRotParent; } set { qRotParent = value; } }
        public Quaternion qOrbitRotation { get { return qOrbRot; } set { qOrbRot = value; } }
        public Quaternion qOrbitParentRotation { get { return qOrbRotParent; } set { qOrbRotParent = value; } }

        public Matrix mScale { get { return mScl; } set { mScl = value; } }
        public Matrix mRotation { get { return mRot; } }

        public Matrix mOrbitRotation { get { return mOrbRot; } }
        public Matrix mOrbitPositon { get { return mOrbPos; } set { mOrbPos = value; } }

        public Matrix mPosition { get { return mPos; } set { mPos = value; } }
        
        #endregion

        #endregion

        #endregion //MVars

        #region Make, Break, Load

        public Primitive(Game g, BasicEffect b, Vector3? position, Quaternion? preRot, Vector3 sizeXYZ, Color? face, Color? wire, Shading shade, Drawing draw)
        {
            game = g;
            gdm = (GraphicsDeviceManager)g.Services.GetService(typeof(IGraphicsDeviceManager));
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

            colFaceTar = colFace = face ?? new Color(255, 178, 064, 255);
            colWireTar = colWire = wire ?? new Color(128, 128, 128, 255);
            colFaceLerp = colWireLerp = 0.05f;

            shadeMode = shade;
            drawMode = draw;

            InitialisePrimitive();
            ResetPrimitive();
        }

        protected virtual void InitialisePrimitive()
        {
            typeName = "BasicPrim";
            noVertices = 8;
            noFaces = 6;
            noTris = 12;
            noWires = 12;
            sideDivs = 2;
            sideMults = 1;
            correctionMatrix = Matrix.CreateTranslation(new Vector3(-sizeX / 2, -sizeY / 2, -sizeZ / 2));
        }

        protected void ResetPrimitive()
        {
            if (shadeMode == Shading.Flat)
            {
                vertices = new VertexPositionColorNormal[noVertices * noTris != 0 ? noVertices * noTris : noVertices];
                wires = new VertexPositionColorNormal[noVertices];
                faceIndices = new short[noTris * 3 != 0 ? noTris * 3 : noVertices];
                wireIndices = new short[noWires * 2];
            }

            else if (shadeMode == Shading.Smooth)
            {
                vertices = new VertexPositionColorNormal[noVertices];
                wires = new VertexPositionColorNormal[noVertices];
                faceIndices = new short[noTris * 3 != 0 ? noTris * 3 : noVertices];
                wireIndices = new short[noWires * 2];
            }

            RotReset();
            InitialiseSides();
            ClearVertices();
            ClearEdges();
            ClearFaceIndices();
            ClearEdgeIndices();
            SetColFace(colFace);
            SetColWire(colWire);
            //Inherited
            InitialiseVertices();
            InitialiseIndices();
            InitialiseEdges();
            //
            TransformVerts(correctionMatrix);
            CopyEdges();

            CalculateNormals();

        }

        #endregion

        #region Vertex Data (Overridden)
        /// <summary>
        /// Define Vertex Position, Colour, Normal Data
        /// If you wanna manually override any colours here's where to do it
        /// 
        /// </summary>
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

        protected void ClearEdges()
        {
            for (int i = wires.Length - 1; i >= 0; i--)
                wires[i].Color = new Color(255, 255, 255);
        }

        protected void ClearFaceIndices()
        {
            for (int i = faceIndices.Length - 1; i >= 0; i--)
                faceIndices[i] = -1;
        }

        protected void ClearEdgeIndices()
        {
            for (int i = wireIndices.Length - 1; i >= 0; i--)
                wireIndices[i] = -1;
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
            foreach (short s in faceIndices)
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

        protected void CopyEdges()
        {
            for (int f = 0; f <= noVertices - 1; f++)
            {
                wires[f].Position = vertices[f].Position;
            }
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
            if (faceIndices.Contains<short>(vertex))//if the vertex is already in indices
            {
                short nextVert = GetFirstUnindexedVertex();
                CopyPos(vertex, nextVert); //copy vertex position to next unindexed vertex
                if (facesI <= faceIndices.Count())
                faceIndices[index] = nextVert; //perform regular assignment with new copied vertex
            }

            else faceIndices[index] = vertex; //otherwise assign normally
        }

        protected void AssignEdgeIndex(short vertex, short index)
        {
            if (wiresI <= wireIndices.Count())
                wireIndices[index] = vertex;
        }

        protected void AssignTri(short v0, short v1, short v2)
        {
            if (shadeMode == Shading.Flat)
            {
                AssignVertexIndex(v0, facesI++);
                AssignVertexIndex(v1, facesI++);
                AssignVertexIndex(v2, facesI++);
            }

            else if (shadeMode == Shading.Smooth)
            {
                faceIndices[facesI++] = v0;
                faceIndices[facesI++] = v1;
                faceIndices[facesI++] = v2;
            }
        }

        protected void AssignLine(short v0, short v1)
        {
            bool ass = true;

            //if the line isn't already there, assign it
            for (short e = 0; e < wireIndices.Count(); e += 2)
            {
                if ((wireIndices[e] == v0 && wireIndices[e + 1] == v1) || (wireIndices[e] == v1 && wireIndices[e + 1] == v0))
                {
                    ass = false;

                    if (wireIndices[e] == -1 && wireIndices[e + 1] == -1)
                        break;
                }
            }

            if (ass)
            {
                AssignEdgeIndex(v0, wiresI++);
                AssignEdgeIndex(v1, wiresI++);
            }
        }

        protected void AssignLinedTri(short v0, short v1, short v2)
        {
            if (shadeMode == Shading.Flat)
            {
                AssignLine(v0, v1);
                AssignLine(v1, v2);
                AssignLine(v2, v0);
                AssignVertexIndex(v0, facesI++);
                AssignVertexIndex(v1, facesI++);
                AssignVertexIndex(v2, facesI++);
            }

            else if (shadeMode == Shading.Smooth)
            {
                AssignLine(v0, v1);
                AssignLine(v1, v2);
                AssignLine(v2, v0);
                faceIndices[facesI++] = v0;
                faceIndices[facesI++] = v1;
                faceIndices[facesI++] = v2;
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

            for (short i = 0; i < faceIndices.Length / 3; i++)
            {
                short index0 = faceIndices[i * 3];
                short index1 = faceIndices[i * 3 + 1];
                short index2 = faceIndices[i * 3 + 2];

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

        protected void SetColFace(Color newCol)
        {
            for (int i = vertices.Length - 1; i >= 0; i--)
                vertices[i].Color = newCol;
        }

        protected void SetColWire(Color newCol)
        {
            for (int i = wires.Length - 1; i >= 0; i--)
                wires[i].Color = newCol;
        }

        protected void LerpColFace()
        {
            if (colFace != colFaceTar)
            {
                Color oldFace = colFace;
                colFace = Color.Lerp(colFace, colFaceTar, colFaceLerp);
                if (colFace == oldFace)
                    colFace = colFaceTar;

                SetColFace(colFace);
            }
        }

        protected void LerpColWire()
        {
            if (colWire != colWireTar)
            {
                Color oldWire = colWire;
                colWire = Color.Lerp(colWire, colWireTar, colWireLerp);
                if (colWire == oldWire)
                    colWire = colWireTar;

                SetColWire(colWire);
            }
        }

        #endregion

        #region Scale

        public void ScaleSet(Vector3 amount)
        {
            vScl = amount;
        }

        public void ScaleAdd(Vector3 scaleXYZ)
        {
            vScl += scaleXYZ;
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

        public void PositionSet(Vector3 posXYZ)
        {
            vPos = posXYZ;
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
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qX));
        }

        public void RotateY(float rotation)
        {
            Quaternion qY = Quaternion.CreateFromAxisAngle(mRot.Up, rotation);
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qY));
        }

        public void RotateZ(float rotation)
        {
            Quaternion qZ = Quaternion.CreateFromAxisAngle(mRot.Forward, rotation);
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qZ));
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
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qWorldX));
        }

        public void RotateWorldY(float rotation)
        {
            Quaternion qWorldY = Quaternion.CreateFromAxisAngle(Vector3.Up, rotation);
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qWorldY));
        }

        public void RotateWorldZ(float rotation)
        {
            Quaternion qWorldZ = Quaternion.CreateFromAxisAngle(Vector3.Forward, rotation);
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qWorldZ));
        }

        #endregion

        #region Orbit

        public void RotateOrbitX(float rotation)
        {
            Quaternion qOrbX = Quaternion.CreateFromAxisAngle(mRot.Right, rotation);
            qOrbRot = Quaternion.Normalize(Quaternion.Concatenate(qOrbRot, qOrbX));
        }

        public void RotateOrbitY(float rotation)
        {
            Quaternion qOrbY = Quaternion.CreateFromAxisAngle(mRot.Up, rotation);
            qOrbRot = Quaternion.Normalize(Quaternion.Concatenate(qOrbRot, qOrbY));
        }

        public void RotateOrbitZ(float rotation)
        {
            Quaternion qOrbZ = Quaternion.CreateFromAxisAngle(mRot.Forward, rotation);
            qOrbRot = Quaternion.Normalize(Quaternion.Concatenate(qOrbRot, qOrbZ));
        }

        public void RotationOrbitReset()
        {
            qOrbRot = IqOrbRot;
        }

        #endregion

        #region Points

        public void RotateVectorX(Vector3 point, float rotation)
        {
            //add translation to list, then do and add rotation
            Quaternion qVecX = Quaternion.CreateFromAxisAngle(mRot.Right, rotation);
            //qVecRot = Quaternion.Normalize(Quaternion.Concatenate(qOrbRot, qVecX));
        }

        public void RotateVectorY(Vector3 point, float rotation)
        {
            Quaternion qVecY = Quaternion.CreateFromAxisAngle(mRot.Up, rotation);
            //qVecRot = Quaternion.Normalize(Quaternion.Concatenate(qOrbRot, qVecY));
        }

        public void RotateVectorZ(Vector3 point, float rotation)
        {
            Quaternion qVecZ = Quaternion.CreateFromAxisAngle(mRot.Forward, rotation);
            //qVecRot = Quaternion.Normalize(Quaternion.Concatenate(qOrbRot, qVecZ));
        }

        #endregion

        public void RotateArbitrary(float rotation, Vector3 axis)
        {
            Quaternion qr = Quaternion.CreateFromAxisAngle(axis, rotation);
            qRot = Quaternion.Normalize(Quaternion.Concatenate(qRot, qr));
        }

        #endregion

        #region U&D

        #region Non-List

        private void UpdateMatrices(Matrix world)
        {
            mScl = Matrix.CreateScale(vScl);
            mRot = Matrix.CreateFromQuaternion(qRot);
            mPos = Matrix.CreateTranslation(vPos);

            mWorld = mScl * mRot * mPos * world;
        }

        public void Update(Matrix proj, Matrix view, Matrix world, GameTime gameTime)
        {
            UpdateMatrices(world);
            effect.Projection = proj;
            effect.View = view;

            GenericUpdate(gameTime);
        }

        #endregion

        #region Listed
        
        public void AddList(Matrix m)
        {
            mList.Add(m);
        }     

        private void UpdateMatricesList()
        {
            mWorld = Matrix.Identity;

            mList.Reverse();
            foreach (Matrix m in mList)
            {
                mWorld *= m;
            }

            mList.Clear();
        }

        /// <summary>
        /// Reverses the Matrix List, multiplies the base world by each list
        /// Don't forget to do things in order or shit will break.
        /// ISROT!
        /// </summary>
        public void UpdateList(Matrix proj, Matrix view, GameTime gameTime)
        {
            UpdateMatricesList();
            effect.Projection = proj;
            effect.View = view;

            GenericUpdate(gameTime);
        }

        #endregion

        private void GenericUpdate(GameTime gameTime)
        {
            LerpColFace();
            LerpColWire();
        }

        public void Draw()
        {
            effect.World = mWorld;

            if (drawMode == Drawing.Face || drawMode == Drawing.WireFace || drawMode == Drawing.FacePoint)
            {
                if (lightMode == Lighting.Face || lightMode == Lighting.WireFace)
                    effect.LightingEnabled = true;
                else effect.LightingEnabled = false;

                //Face draw
                effect.CurrentTechnique.Passes[0].Apply();
                gdm.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, vertices.Length, faceIndices, 0, faceIndices.Length / 3, VertexPositionColorNormal.VertexDeclaration);
            }

            if (drawMode == Drawing.Wire || drawMode == Drawing.WireFace)
            {
                if (lightMode == Lighting.Wire || lightMode == Lighting.WireFace)
                    effect.LightingEnabled = true;
                else effect.LightingEnabled = false;

                //Wireframe draw
                effect.CurrentTechnique.Passes[0].Apply();
                gdm.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, wires, 0, wires.Length, wireIndices, 0, wireIndices.Length / 2, VertexPositionColorNormal.VertexDeclaration);
            }

            //Point Drawing
            if (drawMode == Drawing.Point || drawMode == Drawing.FacePoint)
            {
                if (lightMode == Lighting.Point || lightMode == Lighting.FacePoint)
                    effect.LightingEnabled = true;
                else effect.LightingEnabled = false;

                effect.CurrentTechnique.Passes[0].Apply();
                gdm.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, wires, 0, wires.Length, wireIndices, 0, wireIndices.Length / 2, VertexPositionColorNormal.VertexDeclaration);

            }

        }

        #endregion
    }
}
