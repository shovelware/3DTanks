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
    /// This is the main type for your game
    /// </summary>
    public class MainT : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputManager input = new InputManager();

        BasicEffect basicEffect;
        public BasicEffect blankEffect;
        public BasicEffect BasicEffect { get { return basicEffect; } }

        float rotSpeed = 0.05f, movSpeed = 5f;

        string[] titles = new string[3];
        int titlesI = 1;

        AxesDual axes0, axes1, axes2;
        Tank tunk;

        public MainT()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //BasicEffect Initialise
            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, 500), new Vector3(0, 0, 0), Vector3.Up);
            basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 2.0f, (float)graphics.GraphicsDevice.Viewport.Width / (float)graphics.GraphicsDevice.Viewport.Height, 0.1f, 100000);

            basicEffect.LightingEnabled = true;
            blankEffect = basicEffect;

            basicEffect.DirectionalLight0.Direction = Vector3.Backward;
            basicEffect.DirectionalLight1.Direction = Vector3.Up + Vector3.Backward;
            basicEffect.DirectionalLight2.Direction = Vector3.Down + Vector3.Backward;


            //tunk = new Tank(this, basicEffect, new Vector3(0, -100, 0));
            axes0 = new AxesDual(this, basicEffect, new Vector3(-600, -300, 0), Quaternion.Identity, new Vector3(128));
            axes1 = new AxesDual(this, basicEffect, new Vector3(0, -300, 0), Quaternion.Identity, new Vector3(128));
            axes2 = new AxesDual(this, basicEffect, new Vector3(+600, -300, 0), Quaternion.Identity, new Vector3(128));

            base.Initialize();
            titles[0] = "Info: WASDQE: Rotate, R: Reset, O/P: Cycle Prim, L: Lighting, K: Ambient, Numpad 0/1/2: Direction Lights. X: Cycle Info";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        private void CheckInput(GameTime gameTime)
        {
            if (input.CheckKey(Keys.D) > 0)
                tunk.RotateBottomRight();
            if (input.CheckKey(Keys.A) > 0) 
                tunk.RotateBottomLeft();
            if (input.CheckKey(Keys.Q) > 0)
                tunk.RotateTurretLeft();
            if (input.CheckKey(Keys.E) > 0)
                tunk.RotateTurretRight();
            if (input.CheckKey(Keys.F) > 0)
                tunk.SpinBarrelCW();






            if (input.CheckKey(Keys.R) > 0)
            {
                tunk.RotReset();
            }

            if (input.CheckKey(Keys.T) > 0)
            {
                tunk.PositionSet(Vector3.Zero);
            }

            if (input.CheckKey(Keys.G) == 1)
            {
                graphics.PreferMultiSampling = !graphics.PreferMultiSampling;
            }

            //////
            if (input.CheckKey(Keys.Space) == 1)
            {
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
            }

            if (input.CheckKey(Keys.P) == 1)
            {
            }


            
            //Movement

            if (input.CheckKey(Keys.Up) > 0)
            {
            }

            if (input.CheckKey(Keys.Down) > 0)
            {
            }

            if (input.CheckKey(Keys.Left) > 0)
            {
                tunk.PositionAdd(new Vector3(-movSpeed, 0, 0));
            }

            if (input.CheckKey(Keys.Right) > 0)
            {
                tunk.PositionAdd(new Vector3(movSpeed, 0 ,0));
            }

            //Other
            if (input.CheckKey(Keys.X) == 1)
            {
                titlesI = CrementIndex(titlesI, titles.Count(), true);
            }

            //Lighting
            if (input.CheckKey(Keys.L) == 1)
                basicEffect.LightingEnabled = !basicEffect.LightingEnabled;

            if (input.CheckKey(Keys.K) == 1)
            {
                Vector3 increment = new Vector3(0.25f, 0.25f, 0.25f);
                Vector3 maximum = new Vector3(1, 1, 1);

                if (basicEffect.AmbientLightColor.X + increment.X <= maximum.X)
                {
                    basicEffect.AmbientLightColor += increment;
                }

                else
                {
                    basicEffect.AmbientLightColor = Vector3.Zero;
                }
            }

            if (input.CheckKey(Keys.NumPad0) == 1)
                basicEffect.DirectionalLight0.Enabled = !basicEffect.DirectionalLight0.Enabled;

            if (input.CheckKey(Keys.NumPad1) == 1)
                basicEffect.DirectionalLight1.Enabled = !basicEffect.DirectionalLight1.Enabled;

            if (input.CheckKey(Keys.NumPad2) == 1)
                basicEffect.DirectionalLight2.Enabled = !basicEffect.DirectionalLight2.Enabled;

            if (input.CheckMouseLeft() > 1)
            {
                tunk.Bottom.RotateY((input.MousePosCurAbs.X - input.MousePosPrvAbs.X) / 100);
                tunk.Bottom.RotateX((input.MousePosCurAbs.Y - input.MousePosPrvAbs.Y) / 100);
            }

            if (input.CheckMouseRight() > 1)
            {
                tunk.PositionAdd(new Vector3((input.MousePosCurAbs.X - input.MousePosPrvAbs.X), (input.MousePosCurAbs.Y - input.MousePosPrvAbs.Y) * -1, 0));
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.Update(gameTime);
            CheckInput(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            //primList[primListI].Update(basicEffect.Projection, basicEffect.View, gameTime);         
            //primList[primListI].Draw(graphics);

            tunk.Update(basicEffect.Projection, basicEffect.View, gameTime);
            tunk.Draw();


            axes0.qRotation = tunk.Bottom.qRotation;
            axes0.Update(basicEffect.Projection, basicEffect.View, Matrix.Identity, gameTime);
            axes0.Draw();

            axes1.qRotation = tunk.Turret.qRotation;
            axes1.Update(basicEffect.Projection, basicEffect.View, Matrix.Identity, gameTime);
            axes1.Draw();

            axes2.qRotation = tunk.Barrel.qRotation;
            axes2.Update(basicEffect.Projection, basicEffect.View, Matrix.Identity, gameTime);
            axes2.Draw();

            //titles[1] = "Primitive Info: [" + primListI + "] " + primList[primListI].TypeName + " | Shading: " + primList[primListI].ShadeMode + " | Drawing: " + primList[primListI].DrawMode + "| AA: " + toStr(graphics.PreferMultiSampling) + "";
            titles[2] = "Lighting Info: Lighting: " + toStr(basicEffect.LightingEnabled) + " | Ambience: " + basicEffect.AmbientLightColor.X + " | Light0: " + toStr(basicEffect.DirectionalLight0.Enabled) + ", " + toStr(basicEffect.DirectionalLight0.Direction) + " | Light1: " + toStr(basicEffect.DirectionalLight1.Enabled) + ": " + toStr(basicEffect.DirectionalLight1.Direction) + " | Light2: " + toStr(basicEffect.DirectionalLight2.Enabled) + ": " + toStr(basicEffect.DirectionalLight2.Direction) + "";;
            titles[1] = "Bottom Pos: " + toStr(tunk.Bottom.vPosition) + " Turret Pos: " + toStr(tunk.Turret.vPosition) + " Barrel Pos:  " + toStr(tunk.Barrel.vPosition) + "";
            this.Window.Title = titles[titlesI];

            base.Draw(gameTime);
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

    }
}
