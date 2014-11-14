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
    /// Customised Streets Demo
    /// </summary>
    public class MainD : Game
    {
        GraphicsDeviceManager graphics;

        Tank tunk;

        BasicEffect basicEffect;
        Camera3D camera;

        enum CameraState { FIRSTPERSON, THIRDPERSON, FREECAM, COUNT };
        CameraState cameraState, prvCameraState;

        List<Projectile> proj = new List<Projectile>();

        InputManager input;

        public MainD()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferMultiSampling = true;
        }

        #region Init/Content

        protected override void Initialize()
        {
            input = new InputManager();
            GraphicsDevice.BlendState = BlendState.AlphaBlend;

            basicEffect = new BasicEffect(graphics.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = true;
            basicEffect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);

            //basicEffect.DirectionalLight0.Enabled = true;
            //basicEffect.DirectionalLight0.Direction = Vector3.Up;

            //basicEffect.FogEnabled = true;
            //basicEffect.FogStart = 256;
            //basicEffect.FogEnd = 512;

            TileFactory.Init(this, 256, basicEffect);
            Map.Init();
            ResetGame();
        }

        private void ResetGame()
        {
            tunk = new Tank(this, basicEffect, new Vector3(0, 0, 0), Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(-90)), 128, Color.Yellow, Color.Magenta);
            camera = new Camera3D(new Vector3(0, 10, 0), new Vector3(50, 10, 50), Vector3.Up, 1f, graphics.GraphicsDevice.Viewport.AspectRatio, 1, 5120);
            Map.GetMap(1);
            Map.LoadCurrentMap();

        }

        protected override void LoadContent() { }

        protected override void UnloadContent() { }

        #endregion

        #region Tanks

        private void SwitchCamState()
        {        
            if (cameraState < CameraState.COUNT)
                cameraState++;
            if (cameraState == CameraState.COUNT)
                cameraState = CameraState.FIRSTPERSON;
        }

        #endregion

        private void CheckInput()
        {

            #region Camera

            #region Movement

            if (input.CheckKey(Keys.NumPad8) > 0)
                camera.MoveFore();
            if (input.CheckKey(Keys.NumPad2) > 0)
                camera.MoveBack();

            if (input.CheckKey(Keys.NumPad4) > 0)
                camera.MoveLeft();
            if (input.CheckKey(Keys.NumPad6) > 0)
                camera.MoveRight();

            if (input.CheckKey(Keys.NumPad9) > 0)
                camera.MoveUp();
            if (input.CheckKey(Keys.NumPad7) > 0)
                camera.MoveDown();

            if (input.CheckKey(Keys.NumPad1) > 0)
                camera.MoveFast();
            if (input.CheckKey(Keys.NumPad3) > 0)
                camera.MoveSlow();

            #endregion

            #region Rotation

            if (input.CheckKey(Keys.J) > 0)
                camera.RotateWorldY(0.1f);
            if (input.CheckKey(Keys.L) > 0)
                camera.RotateWorldY(-0.1f);

            if (input.CheckKey(Keys.I) > 0)
                camera.RotateX(0.1f);
            if (input.CheckKey(Keys.K) > 0)
                camera.RotateX(-0.1f);

            if (input.CheckKey(Keys.U) > 0)
                camera.RotateZ(-0.1f);
            if (input.CheckKey(Keys.O) > 0)
                camera.RotateZ(0.1f);

            #endregion

            if (input.CheckKey(Keys.NumPad5) == 1)
                SwitchCamState();

            if (input.CheckKey(Keys.R) > 0)
            {
                camera.Reset();
            }

            //Juice
            if (input.CheckKey(Keys.OemComma) > 0)
            {
                camera.AspectTarget = 4f;
            }
            if (input.CheckKey(Keys.OemComma) <= 0)
            {
                camera.AspectReset();
            }

            if (input.CheckKey(Keys.OemPeriod) > 0)
            {
                camera.FoVTarget = 0.1f;
            }
            if (input.CheckKey(Keys.OemPeriod) <= 0)
            {
                camera.FoVReset();
            }

            #endregion

            #region Tank

            if (input.CheckKey(Keys.W) > 0)
                tunk.MoveFore();
            if (input.CheckKey(Keys.S) > 0)
                tunk.MoveBack();
            if (input.CheckKey(Keys.A) > 0)
                tunk.RotateBottomLeft();
            if (input.CheckKey(Keys.D) > 0)
                tunk.RotateBottomRight();
            if (input.CheckKey(Keys.Q) > 0)
                tunk.RotateTurretLeft();
            if (input.CheckKey(Keys.E) > 0)
                tunk.RotateTurretRight();
            if (input.CheckKey(Keys.F) > 0)
                tunk.SpinBarrelCW();

            if (input.CheckKey(Keys.C) > 0)
            {
                Random rng = new Random();
                tunk.SetColFaceTar(Color.Purple);
                tunk.SetColWireTar(Color.CornflowerBlue);
            }

            if (input.CheckKey(Keys.V) > 0)
            {
                Random rng = new Random();
                tunk.SetColFaceTar(new Color(rng.Next(0, 256), rng.Next(0, 256), rng.Next(0, 256)));
                tunk.SetColWireTar(new Color(rng.Next(0, 256), rng.Next(0, 256), rng.Next(0, 256)));
            }

            if (input.CheckKey(Keys.V) > 0)
            {
                Random rng = new Random();
                tunk.SetColFaceTar(Color.Gray);
                tunk.SetColWireTar(Color.Cyan);
            }

            if (input.CheckKey(Keys.Space) > 0)
            {
                if (tunk.Fire())
                {
                    proj.Add(new Bullet(this, basicEffect, tunk.Barrel.World, 32, Color.Red, Color.Blue));
                }
            }

            #endregion

            #region Game

            if (input.CheckKey(Keys.F5) > 0)
            {
                ResetGame();
            }


            #endregion

        }

        protected override void Update(GameTime gameTime)
        {
            FPS.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            input.Update(gameTime);
            CheckInput();

            Map.Update(camera.Projection, camera.View, gameTime);

            switch (cameraState)
            {
                case CameraState.FIRSTPERSON:
                    camera.Position = tunk.vCam1P;
                    camera.Lookat(tunk.Barrel.World.Forward);
                    break;
                case CameraState.THIRDPERSON:
                    camera.Position = tunk.vCam3P;
                    camera.Lookat(tunk.Barrel.World.Forward);
                    break;
                case CameraState.FREECAM:
                    break;
            }

            camera.Update(gameTime);

            tunk.Update(camera.Projection, camera.View, gameTime);
            
            foreach (Projectile p in proj)
            {
                p.Update(camera.Projection, camera.View, gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //this.Window.Title = "AR: " + camera.Aspect + " FoV: " + camera.FoV + "Pos" + (camera.Position - tunk.vPosition) + "";
            this.Window.Title = "FPS: " + FPS.frameRate + "Pos:" + tunk.vPosition + "Vel:" + tunk.vVelocity + "";
            ////

            Map.Draw();
            tunk.Draw();
            foreach (Projectile p in proj)
            {
                p.Draw();
            }

            //Transparent objects
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            Map.DrawTrans();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            ////
            FPS.Draw();
        }
    }
}
