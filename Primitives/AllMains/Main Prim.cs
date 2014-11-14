using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Primitives
{
    /// <summary>
    /// Primitives Main
    /// </summary>
    public class MainP : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputManager input = new InputManager();

        BasicEffect basicEffect;
        public BasicEffect BasicEffect { get { return basicEffect; } }

        float rotSpeed = 0.05f, movSpeed = 5f;

        //List and controls
        List<Primitive> primList = new List<Primitive>();
        int primListI = 0;
        Primitive currentPrim { get { return primList[primListI];  } }

        string[] titles = new string[4];
        int titlesI = 1;
        AxesDual axes0;

        public MainP()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //BasicEffect Initialise
            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, 500), new Vector3(0, 0, 0), Vector3.Up);
            basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 2.0f, (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height, 0.1f, 100000);
            basicEffect.LightingEnabled = true;

            basicEffect.DirectionalLight0.Direction = Vector3.Backward;
            basicEffect.DirectionalLight1.Direction = Vector3.Up + Vector3.Backward;
            basicEffect.DirectionalLight2.Direction = Vector3.Down + Vector3.Backward;

            primList.Add(new Tetrahedron(this, basicEffect, null, null, new Vector3(128, 128, 128), new Color(128, 50, 200, 128), Color.Green, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new Hexahedron(this, basicEffect, null, null, new Vector3(128), Color.Red, Color.Orange, Primitive.Shading.Smooth, Primitive.Drawing.Face));
            primList.Add(new PrismOctagon(this, basicEffect, null, null, new Vector3(256), Color.Green, Color.Gray, Primitive.Shading.Smooth, Primitive.Drawing.Face));
            primList.Add(new PrismSquare(this, basicEffect, null, null, new Vector3(256, 128, 0), Color.Purple, Color.Green, Primitive.Shading.Smooth, Primitive.Drawing.WireFace));
            primList.Add(new TankBottom(this, basicEffect, null, null, new Vector3(256, 128, 512), Color.Yellow, Color.Magenta, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new PrismOctagon(this, basicEffect, null, null, new Vector3(256, 256, 64), Color.LimeGreen, Color.Red, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new PrismOctagon(this, basicEffect, null, null, new Vector3(128, 128, 256), Color.Black, Color.Crimson, Primitive.Shading.Flat, Primitive.Drawing.Wire));
            primList.Add(new Inheritor(this, basicEffect, null, null, new Vector3(128, 128, 256), Color.AliceBlue, Color.Violet, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new Tetrahedron(this, basicEffect, null, null, new Vector3(128, 128, 128), Color.Turquoise, Color.Red, Primitive.Shading.Flat, Primitive.Drawing.Wire));
            primList.Add(new Octahedron(this, basicEffect, null, null, new Vector3(128), Color.Aqua, Color.LightGoldenrodYellow, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new Hexahedron(this, basicEffect, null, null, new Vector3(128), Color.LightYellow, Color.Magenta, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new Icosahedron(this, basicEffect, null, null, new Vector3(128), Color.LightGreen, Color.Black, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new PyramidOctagon(this, basicEffect, null, null, new Vector3(128, 128, 128), Color.MediumVioletRed, Color.MediumSlateBlue, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new BipyramidOctagon(this, basicEffect, null, null, new Vector3(512, 256, 128), Color.DarkOrange, Color.RosyBrown, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new PyramidSquare(this, basicEffect, null, null, new Vector3(128), Color.Purple, Color.PowderBlue, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new BipyramidSquare(this, basicEffect, null, null, new Vector3(128), Color.Purple, Color.PowderBlue, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new Plane(this, basicEffect, null, null, new Vector3(128), Color.Aqua, Color.LawnGreen, Primitive.Shading.Flat, Primitive.Drawing.WireFace));
            primList.Add(new Line(this, basicEffect, null, null, new Vector3(128,0,0), Color.LawnGreen));

            axes0 = new AxesDual(this, basicEffect, new Vector3(-600, -300, 0), Quaternion.Identity, new Vector3(128));

            base.Initialize();
            titles[0] = "Info: WASDQE: Rotate, R: Reset, O/P: Cycle Prim, L: Lighting, K: Ambient, Numpad 0/1/2: Direction Lights. X: Cycle Info";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        private void CheckInput(GameTime gameTime)
        {
            if (input.CheckKey(Keys.W) > 0)
                currentPrim.RotateX(-rotSpeed);
            if (input.CheckKey(Keys.S) > 0)
                currentPrim.RotateX(+rotSpeed);
            if (input.CheckKey(Keys.D) > 0)
                currentPrim.RotateY(+rotSpeed);
            if (input.CheckKey(Keys.A) > 0)
                currentPrim.RotateY(-rotSpeed);
            if (input.CheckKey(Keys.Q) > 0)
                currentPrim.RotateZ(+rotSpeed);
            if (input.CheckKey(Keys.E) > 0)
                currentPrim.RotateZ(-rotSpeed);
            
            if (input.CheckKey(Keys.R) > 0)
            {
                currentPrim.RotationResetAll();
            }

            if (input.CheckKey(Keys.T) > 0)
            {
                currentPrim.vPosition = Vector3.Zero;
            }

            if (input.CheckKey(Keys.G) == 1)
            {
                graphics.PreferMultiSampling = !graphics.PreferMultiSampling;
            }

            //////
            if (input.CheckKey(Keys.Space) == 1)
            {
            }


            if (input.CheckKey(Keys.K) > 0)
            {
                if (!currentPrim.ColFaceLerping)
                {
                    Random rng = new Random();
                    currentPrim.ColFaceTar = new Color(rng.Next(1, 256), rng.Next(1, 256), rng.Next(1, 256));
                }
            }

            if (input.CheckKey(Keys.L) > 0)
            {
                if (!currentPrim.ColWireLerping)
                {
                    Random rng = new Random();
                    currentPrim.ColWireTar = new Color(rng.Next(1, 256), rng.Next(1, 256), rng.Next(1, 256));
                }
            }

            if (input.CheckKey(Keys.LeftShift) > 0)
            {
                rotSpeed = 0.1f;
                movSpeed = 10;
            }

            else if (input.CheckKey(Keys.LeftShift) == -1)
            {
                rotSpeed = 0.05f;
                movSpeed = 5;
            }

            if (input.CheckKey(Keys.O) == 1)
            {
                primListI = CrementIndex(primListI, primList.Count, false);
            }

            if (input.CheckKey(Keys.P) == 1)
            {
                primListI = CrementIndex(primListI, primList.Count, true);
            }
            
            //Movement

            if (input.CheckKey(Keys.Up) > 0)
            {
                currentPrim.PositionAdd(new Vector3(0, movSpeed, 0));
            }

            if (input.CheckKey(Keys.Down) > 0)
            {
                currentPrim.PositionAdd(new Vector3(0, -movSpeed, 0));
            }

            if (input.CheckKey(Keys.Left) > 0)
            {
                currentPrim.PositionAdd(new Vector3(-movSpeed, 0, 0));
            }

            if (input.CheckKey(Keys.Right) > 0)
            {
                currentPrim.PositionAdd(new Vector3(movSpeed, 0 ,0));
            }

            //Other
            if (input.CheckKey(Keys.X) == 1)
            {
                titlesI = CrementIndex(titlesI, titles.Count(), true);
            }

            //Lighting
            //if (input.CheckKey(Keys.L) == 1)
            //    basicEffect.LightingEnabled = !basicEffect.LightingEnabled;
            //
            //if (input.CheckKey(Keys.K) == 1)
            //{
            //    Vector3 increment = new Vector3(0.25f, 0.25f, 0.25f);
            //    Vector3 maximum = new Vector3(1, 1, 1);
            //
            //    if (basicEffect.AmbientLightColor.X + increment.X <= maximum.X)
            //    {
            //        basicEffect.AmbientLightColor += increment;
            //    }
            //
            //    else
            //    {
            //        basicEffect.AmbientLightColor = Vector3.Zero;
            //    }
            //}

            if (input.CheckKey(Keys.NumPad0) == 1)
                basicEffect.DirectionalLight0.Enabled = !basicEffect.DirectionalLight0.Enabled;

            if (input.CheckKey(Keys.NumPad1) == 1)
                basicEffect.DirectionalLight1.Enabled = !basicEffect.DirectionalLight1.Enabled;

            if (input.CheckKey(Keys.NumPad2) == 1)
                basicEffect.DirectionalLight2.Enabled = !basicEffect.DirectionalLight2.Enabled;

            if (input.CheckMouseLeft() > 1)
            {
                currentPrim.RotateWorldY((input.MousePosCurAbs.X - input.MousePosPrvAbs.X) / 100);
                currentPrim.RotateWorldX((input.MousePosCurAbs.Y - input.MousePosPrvAbs.Y) / 100);
            }

            if (input.CheckMouseRight() > 1)
            {
                currentPrim.PositionAdd(new Vector3((input.MousePosCurAbs.X - input.MousePosPrvAbs.X), (input.MousePosCurAbs.Y - input.MousePosPrvAbs.Y) * -1, 0));
            }

            if (input.CheckScrollDir() == 1)
            {
                primListI = CrementIndex(primListI, primList.Count, true);
            }

            if (input.CheckScrollDir() == -1)
            {
                primListI = CrementIndex(primListI, primList.Count, false);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            CheckInput(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            //Wireframe();
            primList[primListI].Update(basicEffect.Projection, basicEffect.View, Matrix.Identity, gameTime);
            primList[primListI].Draw();
            
            axes0.qRotation = currentPrim.qRotation;
            axes0.Update(basicEffect.Projection, basicEffect.View, Matrix.Identity, gameTime);
            axes0.Draw();

            titles[1] = "Primitive Info: [" + primListI + "] " + primList[primListI].TypeName + " | Shading: " + primList[primListI].ShadeMode + " | Drawing: " + primList[primListI].DrawMode + " | AA: " + toStr(graphics.PreferMultiSampling) + "";
            titles[2] = "Color Info: | FaceLerp: " + toStr(currentPrim.ColFaceLerping) + " | Face: " + toStr(currentPrim.ColFace) + " | FaceTar: " + toStr(currentPrim.ColFaceTar) + "";
            titles[3] = "Lighting Info: Lighting: " + toStr(basicEffect.LightingEnabled) + " | Ambience: " + basicEffect.AmbientLightColor.X + " | Light0: " + toStr(basicEffect.DirectionalLight0.Enabled) + ", " + toStr(basicEffect.DirectionalLight0.Direction) + " | Light1: " + toStr(basicEffect.DirectionalLight1.Enabled) + ": " + toStr(basicEffect.DirectionalLight1.Direction) + " | Light2: " + toStr(basicEffect.DirectionalLight2.Enabled) + ": " + toStr(basicEffect.DirectionalLight2.Direction) + ""; ;
            this.Window.Title = titles[titlesI];

            base.Draw(gameTime);
        }

        #region Misc


        private void Wireframe()
        {
            RasterizerState rs = new RasterizerState();
            rs.FillMode = FillMode.WireFrame;
            GraphicsDevice.RasterizerState = rs;
        }

        private int CrementIndex(int index, int collectioncount, bool direction)
        {
            int retindex = index;

            if (direction)
            {
                if (++index <= collectioncount - 1)
                    retindex++;
                else retindex = 0;
            }

            else if (!direction)
            {
                if (--index >= 0)
                    retindex--;
                else retindex = collectioncount - 1;
            }

            return retindex;
        }

        private string toStr(Vector3 input)
        {
            return "(" + input.X + " " + input.Y + " " + input.Z + ")";
        }

        private string toStr(bool input)
        {
            if (input)
                return "T";
            else return "F";
        }

        private string toStr(Color input)
        {
            return "" + input.R + ", " + input.G + ", " + input.B + ", " + input.A + "";
        }

        #endregion
    }
}
